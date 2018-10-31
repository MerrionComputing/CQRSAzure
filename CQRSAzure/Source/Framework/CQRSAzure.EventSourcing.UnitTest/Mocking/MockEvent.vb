Imports System.IO
Imports System.Runtime.Serialization
Imports System.Security.Permissions
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.UnitTest

Namespace Mocking


    <Serializable()>
    <DomainName("Unit Test")>
    <AggregateName("Mock Aggregate")>
    Public Class MockEventTypeOne
        Implements IEvent(Of MockAggregate)

        Public Property EventOneStringProperty As String

        Public Property EventOneIntegerProperty As Integer

        Public ReadOnly Property Version As UInteger Implements IEvent(Of MockAggregate).Version
            Get
                Return 0
            End Get
        End Property



        Public Sub New()

        End Sub

        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            EventOneStringProperty = info.GetValue("EventOneStringProperty", GetType(String))
            EventOneIntegerProperty = info.GetInt32("EventOneIntegerProperty")

        End Sub

        <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
        Public Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData

            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            info.AddValue("EventOneStringProperty", EventOneStringProperty)
            info.AddValue("EventOneIntegerProperty", EventOneIntegerProperty)

        End Sub

    End Class

    Public Class MockEventTypeOneSerialiser
        Implements IEventSerializer(Of MockEventTypeOne)

        Public ReadOnly Property Capabilities As IEventSerializer.SerialiserCapability Implements IEventSerializer.Capabilities
            Get
                Return (IEventSerializer.SerialiserCapability.Stream Or IEventSerializer.SerialiserCapability.NameValuePairs)
            End Get
        End Property

        Public Function ToNameValuePairs(ByVal evt As MockEventTypeOne) As IDictionary(Of String, Object) Implements IEventSerializer(Of MockEventTypeOne).ToNameValuePairs

            Dim retValues As New Dictionary(Of String, Object)()
            retValues.Add(NameOf(MockEventTypeOne.EventOneIntegerProperty), evt.EventOneIntegerProperty)
            retValues.Add(NameOf(MockEventTypeOne.EventOneStringProperty), evt.EventOneStringProperty)
            Return retValues

        End Function

        Public Function ToNameValuePairs(eventToSerialise As Object) As IDictionary(Of String, Object) Implements IEventSerializer.ToNameValuePairs

            Dim ref As MockEventTypeOne = CType(eventToSerialise, MockEventTypeOne)
            If (ref IsNot Nothing) Then
                Return ToNameValuePairs(ref)
            Else
                Throw New InvalidCastException("Attempt to serialise different unrecognised data type")
            End If

        End Function

        Public Function FromNameValuePairs(valueDictionary As IDictionary(Of String, Object)) As MockEventTypeOne Implements IEventSerializer(Of MockEventTypeOne).FromNameValuePairs

            Dim ret As New MockEventTypeOne()

            If (valueDictionary IsNot Nothing) Then
                If (valueDictionary.ContainsKey(NameOf(MockEventTypeOne.EventOneIntegerProperty))) Then
                    ret.EventOneIntegerProperty = valueDictionary(NameOf(MockEventTypeOne.EventOneIntegerProperty))
                End If
                If (valueDictionary.ContainsKey(NameOf(MockEventTypeOne.EventOneStringProperty))) Then
                    ret.EventOneStringProperty = valueDictionary(NameOf(MockEventTypeOne.EventOneStringProperty))
                End If
            End If

            Return ret

        End Function

        Public Function SaveToStream(streamToWriteTo As Stream, ByVal evt As MockEventTypeOne) As Long Implements IEventSerializer(Of MockEventTypeOne).SaveToStream

            If (streamToWriteTo IsNot Nothing) Then
                Dim startPosition As Long = streamToWriteTo.Position

                Using writer As New System.IO.BinaryWriter(streamToWriteTo, System.Text.UTF32Encoding.UTF32)
                    writer.Seek(startPosition, SeekOrigin.Begin)
                    writer.Write(evt.EventOneIntegerProperty)
                    If Not String.IsNullOrEmpty(evt.EventOneStringProperty) Then
                        writer.Write(evt.EventOneStringProperty)
                    Else
                        writer.Write("")
                    End If
                    Return (writer.BaseStream.Position - startPosition)
                End Using
            End If

            Return Nothing

        End Function

        Public Function SaveToStream(streamToWriteTo As Stream, eventToSerialise As Object) As Long Implements IEventSerializer.SaveToStream

            Dim ref As MockEventTypeOne = CType(eventToSerialise, MockEventTypeOne)
            If (ref IsNot Nothing) Then
                Return SaveToStream(streamToWriteTo, ref)
            Else
                Throw New InvalidCastException("Attempt to serialise different unrecognised data type")
            End If

        End Function

        Public Function FromStream(streamToRead As Stream) As MockEventTypeOne Implements IEventSerializer(Of MockEventTypeOne).FromStream

            Dim ret As New MockEventTypeOne()
            If (streamToRead IsNot Nothing) Then
                Using reader As New System.IO.BinaryReader(streamToRead, System.Text.UTF32Encoding.UTF32)
                    ret.EventOneIntegerProperty = reader.ReadInt32()
                    ret.EventOneStringProperty = reader.ReadString()
                End Using
            End If
            Return ret
        End Function

        Private Function IEventSerializer_FromNameValuePairs(valueDictionary As IDictionary(Of String, Object)) As Object Implements IEventSerializer.FromNameValuePairs

            Return FromNameValuePairs(valueDictionary)

        End Function

        Private Function IEventSerializer_FromStream(streamToRead As Stream) As Object Implements IEventSerializer.FromStream

            Return FromStream(streamToRead)

        End Function
    End Class

    <Serializable()>
    <AggregateName("Mock Aggregate")>
    <DomainName("Unit Test")>
    <EventAsOfDateAttribute(NameOf(MockEventTypeTwo.EventTwoNullableDateProperty))>
    Public Class MockEventTypeTwo
        Implements IEvent(Of MockAggregate)

        Public Property EventTwoStringProperty As String

        Public Property EventTwoDecimalProperty As Decimal

        Public Property EventTwoNullableDateProperty As Nullable(Of DateTime)

        Public ReadOnly Property Version As UInteger Implements IEvent(Of MockAggregate).Version
            Get
                Return 1
            End Get
        End Property

        Public Sub New()

        End Sub

        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            EventTwoStringProperty = info.GetValue("EventTwoStringProperty", GetType(String))
            EventTwoDecimalProperty = info.GetValue("EventTwoDecimalProperty", GetType(Decimal))
            EventTwoNullableDateProperty = info.GetValue("EventTwoNullableDateProperty", GetType(Nullable(Of DateTime)))

        End Sub

        <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
        Public Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData

            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            info.AddValue("EventTwoStringProperty", EventTwoStringProperty)
            info.AddValue("EventTwoDecimalProperty", EventTwoDecimalProperty)
            info.AddValue("EventTwoNullableDateProperty", EventTwoNullableDateProperty)

        End Sub

    End Class



    <Serializable()>
    <AggregateName("Mock Aggregate")>
    <DomainName("Unit Test")>
    <EventAsOfDateAttribute(NameOf(MockEventTypeTwo.EventTwoNullableDateProperty))>
    <EventGetFormatterForStream("MockEventTypeTwoWithSerialser_GetFormatter")>
    <EventDeserializeFromNameValuePairs("MockEventTypeTwoWithSerialser_FromNameValuePairs")>
    <EventSerializeToNameValuePairs("MockEventTypeTwoWithSerialser_ToNameValuePairs")>
    Public Class MockEventTypeTwoWithSerialser
        Implements IEvent(Of MockAggregate)

        Public Property EventTwoStringProperty As String

        Public Property EventTwoDecimalProperty As Decimal

        Public Property EventTwoNullableDateProperty As Nullable(Of DateTime)

        Public ReadOnly Property Version As UInteger Implements IEvent(Of MockAggregate).Version
            Get
                Return 1
            End Get
        End Property

        Public Sub New()

        End Sub

        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            EventTwoStringProperty = info.GetValue("EventTwoStringProperty", GetType(String))
            EventTwoDecimalProperty = info.GetValue("EventTwoDecimalProperty", GetType(Decimal))
            EventTwoNullableDateProperty = info.GetValue("EventTwoNullableDateProperty", GetType(Nullable(Of DateTime)))

        End Sub

        <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
        Public Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData

            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            info.AddValue("EventTwoStringProperty", EventTwoStringProperty)
            info.AddValue("EventTwoDecimalProperty", EventTwoDecimalProperty)
            info.AddValue("EventTwoNullableDateProperty", EventTwoNullableDateProperty)

        End Sub


#Region "Static serialisers"
        ''' <summary>
        ''' Turn a dictionary of name:value pairs into a new instance of MockEventTypeTwoWithSerialiser
        ''' </summary>
        ''' <param name="nameValuePairs">
        ''' Dictionary of values for the properties of the event
        ''' </param>
        Public Shared Function MockEventTypeTwoWithSerialser_FromNameValuePairs(ByVal nameValuePairs As IDictionary(Of String, Object)) As MockEventTypeTwoWithSerialser

            Dim ret As New MockEventTypeTwoWithSerialser()

            If (nameValuePairs IsNot Nothing) Then
                If (nameValuePairs.ContainsKey("Le String")) Then
                    ret.EventTwoStringProperty = nameValuePairs("Le String").ToString().Replace("#", "a")
                End If
                If (nameValuePairs.ContainsKey("Le Decimal")) Then
                    ret.EventTwoDecimalProperty = Convert.ToDecimal(nameValuePairs("Le Decimal"))
                End If
                If (nameValuePairs.ContainsKey("Le Date")) Then
                    ret.EventTwoNullableDateProperty = Convert.ToDateTime(nameValuePairs("Le Date"))
                End If
            End If

            Return ret

        End Function

        ''' <summary>
        ''' Turn an instance of this event into name/value pairs for serialisation
        ''' </summary>
        ''' <param name="eventToSave">
        ''' The event to save as name/value pairs
        ''' </param>
        Public Shared Function MockEventTypeTwoWithSerialser_ToNameValuePairs(ByVal eventToSave As MockEventTypeTwoWithSerialser) As IDictionary(Of String, Object)

            Dim ret As New Dictionary(Of String, Object)

            ret.Add("Le String", eventToSave.EventTwoStringProperty.Replace("a", "#"))
            If (eventToSave.EventTwoNullableDateProperty.HasValue) Then
                ret.Add("Le Date", eventToSave.EventTwoNullableDateProperty.Value)
            End If
            ret.Add("Le Decimal", eventToSave.EventTwoDecimalProperty)

            Return ret

        End Function

        Public Shared Function MockEventTypeTwoWithSerialser_GetFormatter() As IFormatter

            Return New Formatters.Binary.BinaryFormatter()

        End Function

#End Region

    End Class



    Public Class MockEventTypeTwoInstance
        Implements IEventInstance(Of String)

        Private ReadOnly m_EventInstance As MockEventTypeTwo
        Private ReadOnly m_key As String
        Private ReadOnly m_Version As UInteger

        Public ReadOnly Property AggregateKey As String Implements IEventInstance(Of String).AggregateKey
            Get
                Return m_key
            End Get
        End Property

        Public ReadOnly Property Version As UInteger Implements IEventInstance.Version
            Get
                Return m_Version
            End Get
        End Property

        Public ReadOnly Property EventInstance As IEvent Implements IEventInstance.EventInstance
            Get
                Return m_EventInstance
            End Get
        End Property

        Public Sub New(ByVal eventInstanceIn As MockEventTypeTwo,
                       ByVal key As String,
                       ByVal version As UInteger)

            m_EventInstance = eventInstanceIn
            m_key = key
            m_Version = version
        End Sub

    End Class

    <Serializable()>
    <DomainName("UnitTest")>
    <EventAsOfDateAttribute(NameOf(MockEventTypeTwo.EventTwoNullableDateProperty))>
    Public Class MockGuidEventTypeTwo
        Implements IEvent(Of MockGuidAggregate)

        Public Property EventTwoStringProperty As String

        Public Property EventTwoDecimalProperty As Decimal

        Public Property EventTwoNullableDateProperty As Nullable(Of DateTime)

        Public ReadOnly Property Version As UInteger Implements IEvent(Of MockGuidAggregate).Version
            Get
                Return 1
            End Get
        End Property

        Public Sub New()

        End Sub

        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            EventTwoStringProperty = info.GetValue("EventTwoStringProperty", GetType(String))
            EventTwoDecimalProperty = info.GetValue("EventTwoDecimalProperty", GetType(Decimal))
            EventTwoNullableDateProperty = info.GetValue("EventTwoNullableDateProperty", GetType(Nullable(Of DateTime)))

        End Sub

        <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
        Public Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData

            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            info.AddValue("EventTwoStringProperty", EventTwoStringProperty)
            info.AddValue("EventTwoDecimalProperty", EventTwoDecimalProperty)
            info.AddValue("EventTwoNullableDateProperty", EventTwoNullableDateProperty)

        End Sub

    End Class

    <Serializable()>
    <DomainName("UnitTest")>
    Public Class MockEventTypeThree
        Implements IEvent(Of MockAggregate)

        Public Property CountryCode As String

        Public Property InternationalDialingCode As String

        Public ReadOnly Property Version As UInteger Implements IEvent(Of MockAggregate).Version
            Get
                Return 3
            End Get
        End Property

        Public Sub New()

        End Sub

        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            CountryCode = info.GetValue("CountryCode", GetType(String))
            InternationalDialingCode = info.GetValue("InternationalDialingCode", GetType(String))

        End Sub

        <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
        Public Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData

            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            info.AddValue("CountryCode", CountryCode)
            info.AddValue("InternationalDialingCode", InternationalDialingCode)

        End Sub
    End Class
End Namespace