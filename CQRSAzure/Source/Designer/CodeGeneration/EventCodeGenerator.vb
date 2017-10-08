Imports System.CodeDom
Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces

Public Class EventCodeGenerator
    Implements IEntityCodeGenerator

    Public Const EVENT_FILENAME_IDENTIFIER = "eventDefinition"

    Private ReadOnly m_event As EventDefinition

    Public ReadOnly Property FilenameBase As String Implements IEntityCodeGenerator.FilenameBase
        Get
            Return ModelCodeGenerator.MakeValidCodeFilenameBase(m_event.Name) & "." & EVENT_FILENAME_IDENTIFIER
        End Get
    End Property

    Public Function GenerateInterface() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateInterface
        'add the imports namespace
        Dim eventInterfaceRet As New CodeCompileUnit()

        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        eventInterfaceRet.Namespaces.Add(aggregateNamespace)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next

        If (Not String.IsNullOrWhiteSpace(m_event.AggregateIdentifier.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_event.AggregateIdentifier.Notes}))
        End If

        ' Add the interface declaration (partial)
        Dim interfaceDeclaration As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration(m_event.Name)
        ' Comment the interface declaration
        If (Not String.IsNullOrWhiteSpace(m_event.Description)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_event.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_event.Notes)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_event.Notes}))
        End If

        ' Make the interface inherit Interface IEvent(Of TAggregate As IAggregationIdentifier)
        Dim parentAggregateInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsInterfaceReference(m_event.AggregateIdentifier.Name)
        Dim genericEventInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("IEvent", {parentAggregateInterface})
        interfaceDeclaration.BaseTypes.Add(genericEventInterface)


        'Add all its properties
        For Each eventProp As EventProperty In m_event.EventProperties
            Dim eventmember As CodeMemberProperty = InterfaceCodeGeneration.SimplePropertyDeclaration(True, eventProp.Name, eventProp.DataType)
            If (eventmember IsNot Nothing) Then

                'Add business meaning comments
                If Not String.IsNullOrEmpty(eventProp.Description) Then
                    eventmember.Comments.AddRange(CommentGeneration.SummaryCommentSection({eventProp.Description}))
                End If
                If Not String.IsNullOrEmpty(eventProp.Notes) Then
                    eventmember.Comments.AddRange(CommentGeneration.RemarksCommentSection({eventProp.Notes}))
                End If
                interfaceDeclaration.Members.Add(eventmember)
            End If
        Next

        aggregateNamespace.Types.Add(interfaceDeclaration)

        Return eventInterfaceRet

    End Function

    Public Function GenerateImplementation() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateImplementation

        'add the imports namespace
        Dim eventClasseRet As New CodeCompileUnit()


        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next

        eventClasseRet.Namespaces.Add(aggregateNamespace)
        If (Not String.IsNullOrWhiteSpace(m_event.AggregateIdentifier.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_event.AggregateIdentifier.Notes}))
        End If

        ' Add the class declaration (partial)
        Dim classDeclaration As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration(m_event.Name)
        If (Not String.IsNullOrWhiteSpace(m_event.Description)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_event.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_event.Notes)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_event.Notes}))
        End If
        'Make the class implement the interface
        classDeclaration.BaseTypes.Add(New CodeTypeReference(GetType(Object)))
        classDeclaration.BaseTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(m_event.Name))

        'and also make it serializable
        classDeclaration.CustomAttributes.Add(AttributeCodeGenerator.SerializableAttribute)

        'Add the model name (DomainNameAttribute)
        If Not String.IsNullOrWhiteSpace(m_event.AggregateIdentifier.CQRSModel.Name) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument(New CodePrimitiveExpression(m_event.AggregateIdentifier.CQRSModel.Name)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.DomainNameAttribute",
                                                              params))
        End If

        'Add the category attribute
        If Not String.IsNullOrWhiteSpace(m_event.Category) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument(New CodePrimitiveExpression(m_event.Category)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.Category",
                                                              params))
        End If

        'Add the EventAsOfDateAttribute
        For Each eventProp In m_event.EventProperties
            If (eventProp.IsEffectiveDate) Then
                'Add an attribute to mark this property as the event effective date provider EventAsOfDateAttribute
                Dim params As New List(Of CodeAttributeArgument)
                params.Add(New CodeAttributeArgument(New CodePrimitiveExpression(EventCodeGenerator.ToMemberName(eventProp.Name, False))))

                classDeclaration.CustomAttributes.Add(
                    AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.EventAsOfDateAttribute", params)
                    )
                Exit For
            End If
        Next

        'add all the backing code..
        ' Version number constant
        Dim versionConst As CodeMemberField = PropertyCodeGeneration.PublicConstant("EVENT_VERSION", New CodeTypeReference(GetType(Int32)), New CodePrimitiveExpression(m_event.Version))
        If (versionConst IsNot Nothing) Then
            versionConst.Comments.Add(New CodeCommentStatement("Version number - always increment this if the event definition changes", False))
            classDeclaration.Members.Add(versionConst)
            'add a property to get the event version for IEvent<>.GetVersion
            Dim propertyVersionMember As CodeMemberProperty = PropertyCodeGeneration.PublicMember("Version", PropertyDataType.PositiveInteger, backingProperty:="EVENT_VERSION")
            If (propertyVersionMember IsNot Nothing) Then
                propertyVersionMember.ImplementationTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(m_event.Name))

                classDeclaration.Members.Add(propertyVersionMember)
            End If
        End If

        '1) private members
        classDeclaration.Members.AddRange(PropertyCodeGeneration.PrivateBackingMembers(m_event.EventProperties.Cast(Of IPropertyEntity).ToList(), True))
        '2) public side of these private members
        For Each eventProp In m_event.EventProperties
            Dim propertyMember As CodeMemberProperty = PropertyCodeGeneration.PublicMember(eventProp)
            If (propertyMember IsNot Nothing) Then
                propertyMember.ImplementationTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(m_event.Name))

                classDeclaration.Members.Add(propertyMember)
            End If
        Next
        '3) add constructors
        Dim emptyConstructor As New CodeConstructor()
        emptyConstructor.Attributes += MemberAttributes.Public
        emptyConstructor.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Empty constructor For serialisation",
                                                                                   "This should be removed If serialisation Is Not needed"}))
        classDeclaration.Members.Add(emptyConstructor)

        If (m_options.ConstructorPreference <> ModelCodegenerationOptionsBase.ConstructorPreferenceSetting.ParametersOnly) Then
            Dim variableName As String = ModelCodeGenerator.MakeImplementationClassName(m_event.Name) & "Init"
            Dim fromInterfaceConstructor As CodeConstructor = ConstructorCodeGenerator.InterfaceBasedConstructor(m_event.EventProperties.Cast(Of IEventPropertyEntity).ToList(), ModelCodeGenerator.MakeInterfaceName(m_event.Name), variableName)
            fromInterfaceConstructor.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Create And populate a New instance Of this Class from the underlying Interface"}))
            fromInterfaceConstructor.Comments.AddRange(CommentGeneration.RemarksCommentSection({"This should be called When the Event Is created from an Event stream"}))
            classDeclaration.Members.Add(fromInterfaceConstructor)
        End If

        If (m_options.ConstructorPreference <> ModelCodegenerationOptionsBase.ConstructorPreferenceSetting.InterfaceOnly) Then
            Dim fromParametersConstructor As CodeConstructor = ConstructorCodeGenerator.ParameterisedConstructor(m_event.EventProperties.Cast(Of IEventPropertyEntity).ToList())
            fromParametersConstructor.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Create And populate a New instance Of this Class from the underlying properties"}))
            fromParametersConstructor.Comments.AddRange(CommentGeneration.RemarksCommentSection({"This should be called When the Event Is created from an Event stream"}))
            fromParametersConstructor.Comments.AddRange(Me.ParameterComments())
            classDeclaration.Members.Add(fromParametersConstructor)


            'Add a static Create factory method
            Dim createParameters As New List(Of CodeParameterDeclarationExpression)
            For Each param As IEventPropertyEntity In m_event.EventProperties.Cast(Of IEventPropertyEntity).ToList()
                createParameters.Add(
                    New CodeParameterDeclarationExpression(
                    EventCodeGenerator.ToMemberType(param.DataType),
                    EventCodeGenerator.ToMemberName(param.Name, False, True))
                    )
            Next

            Dim createFunction As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedFunction("Create", createParameters, makeStatic:=True, returnType:=New CodeTypeReference(ModelCodeGenerator.MakeInterfaceName(m_event.Name)))
            If createFunction IsNot Nothing Then
                createFunction.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Factory method To create an instance Of this Event"}))
                createFunction.Comments.AddRange(Me.ParameterComments())
                ' Call the fromParametersConstructor..e.g. return new ThisEvent(Parameter_1, Parameter_2) 

                createFunction.Statements.Add(New CodeMethodReturnStatement(
                                              ConstructorCodeGenerator.EventConstructor(
                                              New CodeTypeReference(ModelCodeGenerator.MakeImplementationClassName(m_event.Name)),
                                              m_event.EventProperties.Cast(Of IEventPropertyEntity).ToList())))

                classDeclaration.Members.Add(createFunction)
            End If
        End If

        'add a serialization constructor..
        'e.g. Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        Dim serializationConstructor As CodeConstructor = ConstructorCodeGenerator.SerializationConstructor(m_event.EventProperties)
        serializationConstructor.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Create And populate a New instance Of this Class from the serialized data"}))
        serializationConstructor.Comments.AddRange(CommentGeneration.ParamCommentsSection("info", {"The SerializationInfo passed In containing the values Of this Event"}))
        serializationConstructor.Comments.AddRange(CommentGeneration.ParamCommentsSection("context", {"Additional StreamingContext On how the Event Is streamed"}))

        classDeclaration.Members.Add(serializationConstructor)

        'add a Public Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData
        Dim infoParameter As New CodeParameterDeclarationExpression(New CodeTypeReference("SerializationInfo"), "info")
        Dim contextParameter As New CodeParameterDeclarationExpression(New CodeTypeReference("StreamingContext"), "context")

        Dim GetObjectDataSub As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedSub("GetObjectData",
                                                                                              {infoParameter,
                                                                                               contextParameter}
                                                                                                         )

        'Make it implement the ISerializable reference
        Dim implementsISerializable As CodeTypeReference = InterfaceCodeGeneration.ImplementsInterfaceReference("ISerializable")
        If (implementsISerializable IsNot Nothing) Then
            GetObjectDataSub.ImplementationTypes.Add(implementsISerializable)
        End If

        'Add the comments to this ISerializable
        GetObjectDataSub.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Populates a SerializationInfo with the data needed to serialize this event instance"}))
        GetObjectDataSub.Comments.AddRange(CommentGeneration.RemarksCommentSection({"The version number is also to be saved"}))

        'Add If (info Is Nothing) Then Throw New ArgumentNullException("info")

        Dim addValueMethodReference As New CodeMethodReferenceExpression(New CodeVariableReferenceExpression("info"), "AddValue")
        For Each param As IEventPropertyEntity In m_event.EventProperties.Cast(Of IEventPropertyEntity).ToList()
            'add the info.AddValue("[name]", [name])
            Dim addValueMethodInvoke As New CodeMethodInvokeExpression(addValueMethodReference,
                                                                       {New CodePrimitiveExpression(EventCodeGenerator.ToMemberName(param.Name, False, False)),
                                                                       New CodeVariableReferenceExpression(EventCodeGenerator.ToMemberName(param.Name, True, False))
                                                                       }
                                                                       )

            GetObjectDataSub.Statements.Add(addValueMethodInvoke)

        Next

        'add the info.AddValue("EVENT_VERSION", versionNumber)
        Dim addEventVersionMethodInvoke As New CodeMethodInvokeExpression(addValueMethodReference,
                                                                       {New CodePrimitiveExpression("EVENT_VERSION"),
                                                                       New CodeVariableReferenceExpression("EVENT_VERSION")
                                                                       }
                                                                       )

        GetObjectDataSub.Statements.Add(addEventVersionMethodInvoke)

        classDeclaration.Members.Add(GetObjectDataSub)


        'put the built class into the namespace
        aggregateNamespace.Types.Add(classDeclaration)

        Return eventClasseRet

    End Function

    ''' <summary>
    ''' Get the namespace of the aggregate that owns this event
    ''' </summary>
    ''' <remarks>
    ''' This allows different aggregates to have events with the same name without causing code errors
    ''' </remarks>
    Public ReadOnly Property NamespaceHierarchy As IList(Of String) Implements IEntityCodeGenerator.NamespaceHierarchy
        Get
            If (m_event IsNot Nothing) Then
                Return {m_event.AggregateIdentifier.CQRSModel.Name,
                    m_event.AggregateIdentifier.Name,
                    EVENT_FILENAME_IDENTIFIER}
            Else
                Return {}
            End If
        End Get
    End Property

    Public ReadOnly Property RequiredNamespaces As IEnumerable(Of CodeNamespaceImport) Implements IEntityCodeGenerator.RequiredNamespaces
        Get
            Return {
                New CodeNamespaceImport("System"),
                New CodeNamespaceImport("System.Runtime.Serialization"),
                New CodeNamespaceImport("System.Security.Permissions"),
                New CodeNamespaceImport("CQRSAzure.EventSourcing"),
                CodeGenerationUtilities.CreateNamespaceImport({m_event.AggregateIdentifier.CQRSModel.Name,
                    m_event.AggregateIdentifier.Name})
                }
        End Get
    End Property

    Private m_options As IModelCodeGenerationOptions = ModelCodeGenerationOptions.Default()
    Public Sub SetCodeGenerationOptions(options As IModelCodeGenerationOptions) Implements IEntityCodeGenerator.SetCodeGenerationOptions
        m_options = options
    End Sub

    Public Shared Function ToMemberType(ByVal propertyType As PropertyDataType) As CodeTypeReference

        Select Case propertyType
            Case PropertyDataType.Boolean
                Return New CodeTypeReference(GetType(System.Boolean))
            Case PropertyDataType.Date
                Return New CodeTypeReference(GetType(System.DateTime))
            Case PropertyDataType.Decimal
                Return New CodeTypeReference(GetType(System.Decimal))
            Case PropertyDataType.FloatingPointNumber
                Return New CodeTypeReference(GetType(System.Double))
            Case PropertyDataType.Image
                Return New CodeTypeReference(GetType(System.Byte()))
            Case PropertyDataType.Integer
                Return New CodeTypeReference(GetType(System.Int32))
            Case PropertyDataType.PositiveInteger
                Return New CodeTypeReference(GetType(System.UInt32))
            Case PropertyDataType.String
                Return New CodeTypeReference(GetType(System.String))
            Case Else
                Return New CodeTypeReference(GetType(System.Object))
        End Select

    End Function

    Public Shared Function ToMemberTypeName(ByVal propertyType As PropertyDataType) As String

        Select Case propertyType
            Case PropertyDataType.Boolean
                Return "Boolean"
            Case PropertyDataType.Date
                Return "DateTime"
            Case PropertyDataType.Decimal
                Return "Decimal"
            Case PropertyDataType.FloatingPointNumber
                Return "Double"
            Case PropertyDataType.Image
                Return "Byte()"
            Case PropertyDataType.Integer
                Return "Integer"
            Case PropertyDataType.PositiveInteger
                Return "UInt"
            Case PropertyDataType.String
                Return "String"
            Case Else
                Return "Object"
        End Select

    End Function


    ''' <summary>
    ''' Turn the given event property name to a valid code property name
    ''' </summary>
    ''' <param name="propertyName">
    ''' The name of the property as used in the model
    ''' </param>
    ''' <param name="backingField">
    ''' Are we creating the private backing field, rather than the public property
    ''' </param>
    ''' <param name="inputParameter">
    ''' If true, this parameter needs to be given a different name to indicate that it is an input parameter
    ''' </param>
    ''' <returns>
    ''' An appropriate name to use for the code property/field
    ''' </returns>
    Public Shared Function ToMemberName(ByVal propertyName As String, ByVal backingField As Boolean, Optional ByVal inputParameter As Boolean = False) As String

        If (String.IsNullOrWhiteSpace(propertyName)) Then
            Throw New ArgumentException("A Property member name cannot be blank", NameOf(propertyName))
        End If

        Dim invalidCharacters As Char() = " -!, .;':@£$%^&*()-+=/\#~"
        Dim returnMembername As String = String.Join("_", propertyName.Split(invalidCharacters)).Trim()

        If ((String.IsNullOrWhiteSpace(returnMembername)) OrElse (String.IsNullOrWhiteSpace(returnMembername.TrimEnd("_")))) Then
            Throw New ArgumentException("A property member name cannot be only invalid characters", NameOf(propertyName))
        End If

        If (backingField) Then
            returnMembername = "_" & returnMembername
        End If

        returnMembername = returnMembername.TrimEnd("_")

        If (inputParameter) Then
            returnMembername &= "_In"
        End If

        Return returnMembername

    End Function

    Public ReadOnly Property ParameterComments As CodeCommentStatementCollection
        Get
            'Get all of the parameters from this event
            Dim ret As New CodeCommentStatementCollection

            For Each param As IEventPropertyEntity In m_event.EventProperties.Cast(Of IEventPropertyEntity).ToList()
                Dim comments As List(Of String) = New List(Of String)()
                If (Not String.IsNullOrWhiteSpace(param.Description)) Then
                    comments.Add(param.Description)
                End If
                If (Not String.IsNullOrWhiteSpace(param.Notes)) Then
                    If (Not String.IsNullOrWhiteSpace(param.Description)) Then
                        comments.Add("")
                    End If
                    comments.Add(param.Notes)
                End If
                ret.AddRange(CommentGeneration.ParamCommentsSection(param.Name, comments))
            Next

            Return ret
        End Get
    End Property


    Public Sub New(ByVal eventToGenerate As EventDefinition)
        m_event = eventToGenerate
    End Sub

End Class
