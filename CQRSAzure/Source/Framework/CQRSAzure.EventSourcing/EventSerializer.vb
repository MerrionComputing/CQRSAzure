Imports System.IO
Imports System.Reflection
Imports System.Linq.Expressions
Imports CQRSAzure.EventSourcing
Imports System.Runtime.Serialization

Public Class EventSerializer(Of TEvent As {New, IEvent})
    Inherits EventSerializer
    Implements IEventSerializer(Of TEvent)

#Region "Delegate definitions"
    ''' <summary>
    ''' Save the given event type to a stream (binary, JSON etc)
    ''' </summary>
    Public Delegate Function SaveToStream() As Func(Of TEvent, System.IO.Stream)

    ''' <summary>
    ''' Read the given event data from a binary stream
    ''' </summary>
    ''' <returns>
    ''' The created event object populated with the data from the stream
    ''' </returns>
    Public Delegate Function FromStream() As Func(Of System.IO.Stream, TEvent)


    ''' <summary>
    ''' Serialise the event to a dictionary of name:value pairs to be saved
    ''' </summary>
    Public Delegate Function ToNameValuePairs() As Func(Of TEvent, IDictionary(Of String, Object))

    ''' <summary>
    ''' Deserialise the event data from name:value pairs
    ''' </summary>
    Public Delegate Function FromNameValuePairs() As Func(Of IDictionary(Of String, Object), TEvent)


#End Region



    Private m_getStreamFormatter As Func(Of IFormatter)

    Private m_saveNameValuePairs As Func(Of TEvent, IDictionary(Of String, Object))
    Private m_readNameValuePairs As Func(Of IDictionary(Of String, Object), TEvent)

    ''' <summary>
    ''' What functions can this serialiser perform
    ''' </summary>
    Public ReadOnly Property Capabilities As IEventSerializer.SerialiserCapability Implements IEventSerializer.Capabilities
        Get
            Dim ret As IEventSerializer.SerialiserCapability = IEventSerializer.SerialiserCapability.None
            If (m_getStreamFormatter IsNot Nothing) Then
                ret = ret Or IEventSerializer.SerialiserCapability.Stream
            End If
            If (m_saveNameValuePairs IsNot Nothing) AndAlso (m_readNameValuePairs IsNot Nothing) Then
                ret = ret Or IEventSerializer.SerialiserCapability.NameValuePairs
            End If
            Return ret
        End Get
    End Property

    Public Sub New(Optional ByVal getStreamFormatter As Func(Of IFormatter) = Nothing,
                    Optional ByVal saveNameValuePairsFunction As Func(Of TEvent, IDictionary(Of String, Object)) = Nothing,
                    Optional ByVal readNameValuePairsFunction As Func(Of IDictionary(Of String, Object), TEvent) = Nothing)

        If (getStreamFormatter IsNot Nothing) Then
            m_getStreamFormatter = getStreamFormatter
        Else
            'make one for the event type..
            m_getStreamFormatter = EventSerializerFactory.CreateDefaultStreamFormatterFunction(Of TEvent)()
        End If



        If (saveNameValuePairsFunction IsNot Nothing) Then
            m_saveNameValuePairs = saveNameValuePairsFunction
        Else
            'make one for the event type
            m_saveNameValuePairs = EventSerializerFactory.CreateDefaultSaveNameValuePairsFunction(Of TEvent)()
        End If

        If (readNameValuePairsFunction IsNot Nothing) Then
            m_readNameValuePairs = readNameValuePairsFunction
        Else
            'make one for the event type
            m_readNameValuePairs = CreateDefaultReadNameValuePairsFunction(Of TEvent)()
        End If

    End Sub


    Public Overloads Shared Function Create(Optional ByVal saveStreamFunction As Func(Of IFormatter) = Nothing,
                    Optional ByVal saveNameValuePairsFunction As Func(Of TEvent, IDictionary(Of String, Object)) = Nothing,
                    Optional ByVal readNameValuePairsFunction As Func(Of IDictionary(Of String, Object), TEvent) = Nothing) As EventSerializer(Of TEvent)

        Return New EventSerializer(Of TEvent)(saveStreamFunction,
                                              saveNameValuePairsFunction,
                                              readNameValuePairsFunction)

    End Function

    Private Function TypedEventSerializer_ToNameValuePairs(ByVal evt As TEvent) As IDictionary(Of String, Object) Implements IEventSerializer(Of TEvent).ToNameValuePairs

        If (m_saveNameValuePairs IsNot Nothing) Then
            Return m_saveNameValuePairs.Invoke(evt)
        Else
            Throw New NotImplementedException("No name/value pair serialiser function set")
        End If

    End Function

    Private Function UntypedEventSerializer_ToNameValuePairs(ByVal evt As Object) As IDictionary(Of String, Object) Implements IEventSerializer.ToNameValuePairs

        Dim ref As TEvent = CType(evt, TEvent)
        If (ref IsNot Nothing) Then
            Return TypedEventSerializer_ToNameValuePairs(ref)
        Else
            Throw New InvalidCastException("Attempt to serialise different unrecognised data type")
        End If

    End Function

    Private Function TypedEventSerializer_FromNameValuePairs(valueDictionary As IDictionary(Of String, Object)) As TEvent Implements IEventSerializer(Of TEvent).FromNameValuePairs

        If (m_readNameValuePairs IsNot Nothing) Then
            Return m_readNameValuePairs.Invoke(valueDictionary)
        Else
            Throw New NotImplementedException("No name/value pair serialiser function set")
        End If

    End Function


    Private Function UntypedEventSerializer_FromNameValuePairs(valueDictionary As IDictionary(Of String, Object)) As Object Implements IEventSerializer.FromNameValuePairs

        Return TypedEventSerializer_FromNameValuePairs(valueDictionary)

    End Function

    Private Function TypedEventSerializer_SaveToStream(streamToWriteTo As Stream, ByVal evt As TEvent) As Long Implements IEventSerializer(Of TEvent).SaveToStream

        Return UntypedEventSerializer_SaveToStream(streamToWriteTo, evt)

    End Function


    Private Function UntypedEventSerializer_SaveToStream(streamToWriteTo As Stream, ByVal evt As Object) As Long Implements IEventSerializer.SaveToStream

        If (m_getStreamFormatter IsNot Nothing) Then
            m_getStreamFormatter.Invoke().Serialize(streamToWriteTo, evt)
            Return streamToWriteTo.Position
        Else
            Throw New NotImplementedException("No stream serialiser function set")
        End If


    End Function


    Private Function TypedEventSerializer_FromStream(streamToRead As Stream) As TEvent Implements IEventSerializer(Of TEvent).FromStream

        Return CType((UntypedEventSerializer_FromStream(streamToRead)), TEvent)

    End Function

    Private Function UntypedEventSerializer_FromStream(streamToRead As Stream) As Object Implements IEventSerializer.FromStream

        If (m_getStreamFormatter IsNot Nothing) Then
            Return m_getStreamFormatter.Invoke().Deserialize(streamToRead)
        Else
            Throw New NotImplementedException("No stream serialiser function set")
        End If

    End Function

End Class

Public MustInherit Class EventSerializer


    Private ReadOnly m_supportedEvents As Dictionary(Of String, IEnumerable(Of Integer))

    Public Function SupportsEvent(ByVal eventName As String, ByVal eventVersion As Integer) As Boolean

        If (m_supportedEvents Is Nothing) Then
            ' There were no details as to what this serialiser supports so we need to assume that
            ' is already taken care of - for example in a Type::Serialiser dictionary
            Return True
        End If

        If (m_supportedEvents.ContainsKey(eventName)) Then
            If (m_supportedEvents(eventName).Contains(eventVersion)) Then
                Return True
            Else
                'This version is not recognised by the serialiser
                Return False
            End If
        Else
            ' This serializer does not recognise the given event type
            Return False
        End If

    End Function


    Public Shared Function Create(Of TEvent As {New, IEvent})(ByVal evt As TEvent) As EventSerializer(Of TEvent)
        Return Create(Of TEvent)()
    End Function

    Public Shared Function Create(Of TEvent As {New, IEvent})() As EventSerializer(Of TEvent)
        ' Magic happens here...

    End Function

    Public Shared Function Create(ByVal evtType As Type) As EventSerializer
        Dim method As MethodInfo = GetType(EventSerializer).GetMethod("Create", BindingFlags.Public Or BindingFlags.Static, Nothing, Type.EmptyTypes, Nothing)
        Dim result As Object = method.MakeGenericMethod(evtType).Invoke(Nothing, Nothing)
        Return DirectCast(result, EventSerializer)
    End Function

    Protected Friend Sub New(Optional ByVal supportedEvents As Dictionary(Of String, IEnumerable(Of Integer)) = Nothing)

        If (supportedEvents IsNot Nothing) Then
            m_supportedEvents = supportedEvents
        End If

    End Sub

End Class

Partial Public Module EventSerializerFactory


    Public Function Create(Of TEvent As {New, IEvent})(ByVal evt As TEvent) As EventSerializer(Of TEvent)
        Return Create(Of TEvent)()
    End Function

    Public Function Create(Of TEvent As {New, IEvent})() As EventSerializer(Of TEvent)

        Dim getFormatterFunction As Func(Of IFormatter) = Nothing
        Dim saveNameValuePairsFunction As Func(Of TEvent, IDictionary(Of String, Object)) = Nothing
        Dim readNameValuePairsFunction As Func(Of IDictionary(Of String, Object), TEvent) = Nothing

        ' Create the standard serialiser functions
        getFormatterFunction = CreateDefaultStreamFormatterFunction(Of TEvent)()
        saveNameValuePairsFunction = CreateDefaultSaveNameValuePairsFunction(Of TEvent)()
        readNameValuePairsFunction = CreateDefaultReadNameValuePairsFunction(Of TEvent)()



        'Return a serialiser made from them
        Return EventSerializer(Of TEvent).Create(getFormatterFunction,
                                                 saveNameValuePairsFunction,
                                                 readNameValuePairsFunction)


    End Function



    ''' <summary>
    ''' Create a default serialiser to save an event data to name/value pairs 
    ''' </summary>
    ''' <typeparam name="TEvent">
    ''' The type of the object that will be read
    ''' </typeparam>
    ''' <remarks>
    ''' This allows a global swapping out of serialisers if needed
    ''' </remarks>
    Public Function CreateDefaultSaveNameValuePairsFunction(Of TEvent As {New, IEvent})() As Func(Of TEvent, IDictionary(Of String, Object))


        'parameter eventToSave passed in
        Dim eventToSaveParameter As ParameterExpression =
            Expression.Parameter(GetType(TEvent), "eventToSave")

        Dim returnParameter As ParameterExpression =
            Expression.Parameter(GetType(System.Collections.Generic.IDictionary(Of String, Object)), "ret")

        'if the event class has an attribute then return the content of that...
        Dim functionName As String = EventSerializeToNameValuePairsAttribute.GetFunctionName(GetType(TEvent))
        If (Not String.IsNullOrWhiteSpace(functionName)) Then
            'find the static function so named...
            Dim functionMethod = GetType(TEvent).GetMethod(functionName, BindingFlags.Static Or BindingFlags.Public)
            If (functionMethod IsNot Nothing) Then
                'we just want to return that function method...
                Dim saveNameValuePairsFunction As BlockExpression = Expression.Block(
                            New ParameterExpression() {returnParameter},
                            Expression.Assign(returnParameter,
                                Expression.Call(functionMethod, eventToSaveParameter)),
                            returnParameter)


                Dim retNamedLambda As LambdaExpression = Expressions.Expression(Of EventSerializer(Of TEvent).ToNameValuePairs).Lambda(saveNameValuePairsFunction, {eventToSaveParameter})
                Dim retNamedDeletage = retNamedLambda.Compile()

                Return CType(retNamedDeletage, Func(Of TEvent, IDictionary(Of String, Object)))
            End If
        End If

        'otherwise build the function by reflection



        Dim addMethod As MethodInfo = GetType(System.Collections.Generic.IDictionary(Of String, Object)).GetMethod("Add")

        Dim innerBlockExpressions As New List(Of Expression)()

        ' add an entry for each public property of TEvent
        For Each pr As PropertyInfo In GetType(TEvent).GetProperties()
            If (pr.CanRead) Then
                'ret.Add(pr.Name, pr.Value)
                innerBlockExpressions.Add(Expression.Call(returnParameter,
                                                           addMethod, {
                                                               Expression.Constant(pr.Name),
                                                               Expression.Convert(Expression.PropertyOrField(eventToSaveParameter, pr.Name), GetType(Object))
                                                          })
                                                          )
            End If
        Next

        'and also each public field
        For Each fi As FieldInfo In GetType(TEvent).GetFields()
            If (Not fi.IsStatic) Then
                'ret.Add(fi.Name, fi.Value)
                innerBlockExpressions.Add(Expression.Call(returnParameter,
                                                           addMethod, {
                                                               Expression.Constant(fi.Name),
                                                               Expression.Convert(Expression.PropertyOrField(eventToSaveParameter, fi.Name), GetType(Object))
                                                          })
                                                          )
            End If
        Next

        'return void at the end of the inner block
        innerBlockExpressions.Add(Expression.Constant(Nothing))

        'build an expression body to do the serialisation to a dictionary
        Dim innerBlock As BlockExpression = Expression.Block(innerBlockExpressions.AsEnumerable())


        Dim saveNameValuePairsBody As BlockExpression = Expression.Block(
                    New ParameterExpression() {returnParameter},
                    Expression.Assign(returnParameter,
                        Expression.[New](GetType(System.Collections.Generic.Dictionary(Of String, Object)))),
                    innerBlock,
                    returnParameter)


        Dim retLambda As LambdaExpression = Expressions.Expression(Of EventSerializer(Of TEvent).ToNameValuePairs).Lambda(saveNameValuePairsBody, {eventToSaveParameter})
        Dim retDeletage = retLambda.Compile()

        Return CType(retDeletage, Func(Of TEvent, IDictionary(Of String, Object)))

    End Function


    ''' <summary>
    ''' Create a default serialiser function to read from a name-values pair data source
    ''' </summary>
    ''' <typeparam name="TEvent">
    ''' The type of the object that will be read
    ''' </typeparam>
    ''' <remarks> 
    ''' This allows a global swapping out of serialisers if needed
    ''' </remarks>
    Public Function CreateDefaultReadNameValuePairsFunction(Of TEvent As {New, IEvent})() As Func(Of IDictionary(Of String, Object), TEvent)

        'Parameter passed in
        Dim valueParameter As ParameterExpression =
            Expression.Parameter(GetType(System.Collections.Generic.IDictionary(Of String, Object)), "valueDictionary")

        'Type to be populated...
        Dim returnParameter As ParameterExpression =
            Expression.Parameter(GetType(TEvent), "ret")

        'if the event class has an attribute then return the content of that...
        Dim functionName As String = EventDeserializeFromNameValuePairsAttribute.GetFunctionName(GetType(TEvent))
        If (Not String.IsNullOrWhiteSpace(functionName)) Then
            'find the static function so named...
            Dim functionMethod = GetType(TEvent).GetMethod(functionName, BindingFlags.Static Or BindingFlags.Public)
            If (functionMethod IsNot Nothing) Then
                'we just want to return that function method...
                Dim saveNameValuePairsFunction As BlockExpression = Expression.Block(
                            New ParameterExpression() {returnParameter},
                            Expression.Assign(returnParameter,
                                Expression.Call(functionMethod, valueParameter)),
                            returnParameter)


                Dim retNamedLambda As LambdaExpression = Expressions.Expression(Of EventSerializer(Of TEvent).ToNameValuePairs).Lambda(saveNameValuePairsFunction, {valueParameter})
                Dim retNamedDeletage = retNamedLambda.Compile()

                Return CType(retNamedDeletage, Func(Of IDictionary(Of String, Object), TEvent))
            End If
        End If

        'otherwise build the function by reflection


        Dim innerBlockExpressions As New List(Of Expression)()

        Dim itemMethod As MethodInfo = GetType(System.Collections.Generic.IDictionary(Of String, Object)).GetMethod("Item")

        ' Return a function that returns each public property by name and value...
        For Each pr As PropertyInfo In GetType(TEvent).GetProperties(BindingFlags.Public)
            If pr.CanWrite Then
                innerBlockExpressions.Add(Expression.Assign(
                                          Expression.PropertyOrField(returnParameter, pr.Name),
                                          Expression.Call(valueParameter,
                                                          itemMethod,
                                                          Expression.Constant(pr.Name)))
                                                          )
            End If
        Next

        'and also each public field
        For Each fi As FieldInfo In GetType(TEvent).GetFields(BindingFlags.Public)
            innerBlockExpressions.Add(Expression.Assign(
                                      Expression.PropertyOrField(returnParameter, fi.Name),
                                      Expression.Call(valueParameter,
                                                      itemMethod,
                                                      Expression.Constant(fi.Name)))
                                                      )
        Next


        'return void at the end of the inner block
        innerBlockExpressions.Add(Expression.Constant(Nothing))

        'build an expression body to do the de-serialisation from a dictionary
        Dim innerBlock As BlockExpression = Expression.Block(innerBlockExpressions.AsEnumerable())


        Dim fromNameValuePairsBody As BlockExpression = Expression.Block(
                    New ParameterExpression() {returnParameter},
                    Expression.Assign(returnParameter,
                        Expression.[New](GetType(TEvent))),
                    innerBlock,
                    returnParameter)

        Dim retLambda As LambdaExpression = Expressions.Expression(Of EventSerializer(Of TEvent).FromNameValuePairs).Lambda(fromNameValuePairsBody, {valueParameter})
        Dim retDeletage = retLambda.Compile()

        Return CType(retDeletage, Func(Of IDictionary(Of String, Object), TEvent))

    End Function

    Public Function CreateDefaultStreamFormatterFunction(Of TEvent As {New, IEvent})() As Func(Of IFormatter)

        Dim returnParameter As ParameterExpression =
           Expression.Parameter(GetType(IFormatter), "ret")

        'if the event class has an attribute then return the content of that...
        Dim functionName As String = EventGetFormatterForStreamAttribute.GetFunctionName(GetType(TEvent))
        If (Not String.IsNullOrWhiteSpace(functionName)) Then
            'find the static function so named...
            Dim functionMethod = GetType(TEvent).GetMethod(functionName, BindingFlags.Static Or BindingFlags.Public)
            If (functionMethod IsNot Nothing) Then
                'we just want to return that function method...
                Dim getFormatterForStreamFunction As BlockExpression = Expression.Block(
                            New ParameterExpression() {returnParameter},
                            Expression.Assign(returnParameter,
                                Expression.Call(functionMethod)),
                            returnParameter)


                Dim retNamedLambda As LambdaExpression = Expressions.Expression(Of EventSerializer(Of TEvent).ToNameValuePairs).Lambda(getFormatterForStreamFunction, {})
                Dim retNamedDeletage = retNamedLambda.Compile()

                Return CType(retNamedDeletage, Func(Of IFormatter))
            End If
        End If

        'otherwise default to a bog standard binary formatter
        Dim getFormatterForStreamBody As BlockExpression = Expression.Block(
                    New ParameterExpression() {returnParameter},
                    Expression.Assign(returnParameter,
                        Expression.[New](GetType(Formatters.Binary.BinaryFormatter))),
                    returnParameter)

        Dim retLambda As LambdaExpression = Expressions.Expression(Of EventSerializer(Of TEvent).ToNameValuePairs).Lambda(getFormatterForStreamBody, {})
        Dim retDeletage = retLambda.Compile()

        Return CType(retDeletage, Func(Of IFormatter))

    End Function
End Module

#Region "Class Serialization Attributes"

''' <summary>
''' Attribute to tag an event with the name of the (static) function that can be used to serialize that 
''' event to a name/values pair dictionary
''' </summary>
''' <remarks>
''' This is the serialisation method that will be used when the backing storage needs to store an event as property name / value pairs
''' for example if the backing store is a NoSQL table
''' </remarks>
<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
Public NotInheritable Class EventSerializeToNameValuePairsAttribute
    Inherits Attribute


    Private ReadOnly m_functionName As String
    ''' <summary>
    ''' The name of the function that does the serialising to name/value pairs magic
    ''' </summary>
    Public ReadOnly Property FunctionName As String
        Get
            Return m_functionName
        End Get
    End Property

#Region "Constructors"
    Public Sub New(ByVal functionNameIn As String)
        m_functionName = functionNameIn
    End Sub
#End Region

    Public Shared Function GetFunctionName(ByVal eventObjectType As Type) As String

        For Each eventFunctionNameAttr As EventSerializeToNameValuePairsAttribute In eventObjectType.GetCustomAttributes(GetType(EventSerializeToNameValuePairsAttribute), False)
#Region "Tracing"
            EventSourcing.LogVerboseInfo(eventObjectType.ToString() & " has the name/value pair serialisation function name attribute set to " & eventFunctionNameAttr.FunctionName)
#End Region
            Return eventFunctionNameAttr.FunctionName
        Next

#Region "Tracing"
        EventSourcing.LogVerboseInfo(eventObjectType.ToString() & " has no name/value pair serialisation function name attribute set")
#End Region

        ' No function set - return an empty string
        Return String.Empty

    End Function

    Public Shared Function GetFunctionName(ByVal eventObject As Object) As String

        Return GetFunctionName(eventObject.GetType())

    End Function
End Class


''' <summary>
''' Attribute to tag an event with the name of the (static) function that can be used to deserialize that 
''' event from a name/values pair dictionary
''' </summary>
''' <remarks>
''' This is the serialisation method that will be used when the backing storage needs to store an event as property name / value pairs
''' for example if the backing store is a NoSQL table
''' </remarks>
<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
Public NotInheritable Class EventDeserializeFromNameValuePairsAttribute
    Inherits Attribute


    Private ReadOnly m_functionName As String
    ''' <summary>
    ''' The name of the function that does the deserialising from name/value pairs magic
    ''' </summary>
    Public ReadOnly Property FunctionName As String
        Get
            Return m_functionName
        End Get
    End Property

#Region "Constructors"
    Public Sub New(ByVal functionNameIn As String)
        m_functionName = functionNameIn
    End Sub
#End Region

    Public Shared Function GetFunctionName(ByVal eventObjectType As Type) As String

        For Each eventFunctionNameAttr As EventDeserializeFromNameValuePairsAttribute In eventObjectType.GetCustomAttributes(GetType(EventDeserializeFromNameValuePairsAttribute), False)
#Region "Tracing"
            EventSourcing.LogVerboseInfo(eventObjectType.ToString() & " has the name/value pair deserialisation function name attribute set to " & eventFunctionNameAttr.FunctionName)
#End Region
            Return eventFunctionNameAttr.FunctionName
        Next

#Region "Tracing"
        EventSourcing.LogVerboseInfo(eventObjectType.ToString() & " has no name/value pair deserialisation function name attribute set")
#End Region

        ' No function set - return an empty string
        Return String.Empty

    End Function

    Public Shared Function GetFunctionName(ByVal eventObject As Object) As String

        Return GetFunctionName(eventObject.GetType())

    End Function
End Class

''' <summary>
''' Attribute to tag an event with the name of the (static) function that can provide the formatter be used to serialize that 
''' event to or from a stream
''' </summary>
''' <remarks>
''' This is the serialisation method that may be used when the backing storage needs to store an event as raw binary
''' for example if the backing store is a file or blob.
''' </remarks>
<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
Public NotInheritable Class EventGetFormatterForStreamAttribute
    Inherits Attribute


    Private ReadOnly m_functionName As String
    ''' <summary>
    ''' The name of the function that returns the formatter that does the serialising to binary streams magic
    ''' </summary>
    Public ReadOnly Property FunctionName As String
        Get
            Return m_functionName
        End Get
    End Property

#Region "Constructors"
    Public Sub New(ByVal functionNameIn As String)
        m_functionName = functionNameIn
    End Sub
#End Region

    Public Shared Function GetFunctionName(ByVal eventObjectType As Type) As String

        For Each eventFunctionNameAttr As EventGetFormatterForStreamAttribute In eventObjectType.GetCustomAttributes(GetType(EventGetFormatterForStreamAttribute), False)
#Region "Tracing"
            EventSourcing.LogVerboseInfo(eventObjectType.ToString() & " has the binary stream serialisation function name attribute set to " & eventFunctionNameAttr.FunctionName)
#End Region
            Return eventFunctionNameAttr.FunctionName
        Next

#Region "Tracing"
        EventSourcing.LogVerboseInfo(eventObjectType.ToString() & " has no binary stream serialisation function name attribute set")
#End Region

        ' No function set - return an empty string
        Return String.Empty

    End Function

    Public Shared Function GetFunctionName(ByVal eventObject As Object) As String

        Return GetFunctionName(eventObject.GetType())

    End Function


End Class

#End Region