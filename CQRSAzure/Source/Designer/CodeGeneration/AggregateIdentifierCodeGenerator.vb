Imports System.CodeDom
Imports System.CodeDom.Compiler
Imports CQRSAzure.CQRSdsl.CodeGeneration
Imports CQRSAzure.CQRSdsl.Dsl

Public Class AggregateIdentifierCodeGenerator
    Implements IEntityCodeGenerator

    Public Const AGGREGATE_FILENAME_IDENTIFIER As String = "aggregateidentifier"


    Private ReadOnly m_aggregate As AggregateIdentifier

    ''' <summary>
    ''' The filename base to use when saving this aggregate's code
    ''' </summary>
    Public ReadOnly Property FilenameBase As String Implements IEntityCodeGenerator.FilenameBase
        Get
            Return ModelCodeGenerator.MakeValidCodeFilenameBase(m_aggregate.Name) & "." & AGGREGATE_FILENAME_IDENTIFIER
        End Get
    End Property

    Public ReadOnly Property RequiredNamespaces As IEnumerable(Of CodeNamespaceImport) Implements IEntityCodeGenerator.RequiredNamespaces
        Get
            Return {
                New CodeNamespaceImport("CQRSAzure"),
                New CodeNamespaceImport("CQRSAzure.EventSourcing"),
                New CodeNamespaceImport("CQRSAzure.Aggregation"),
                CodeGenerationUtilities.CreateNamespaceImport({m_aggregate.CQRSModel.Name,
                    m_aggregate.Name,
                    EventCodeGenerator.EVENT_FILENAME_IDENTIFIER})
                }
        End Get
    End Property

    Public ReadOnly Property NamespaceHierarchy As IList(Of String) Implements IEntityCodeGenerator.NamespaceHierarchy
        Get
            If (m_aggregate IsNot Nothing) Then
                Return {m_aggregate.CQRSModel.Name,
                    m_aggregate.Name}
            Else
                Return {}
            End If
        End Get
    End Property

    Public Function GenerateInterface() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateInterface
        'add the imports namespace
        Dim aggregateInterfaceRet As New CodeCompileUnit()


        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        aggregateInterfaceRet.Namespaces.Add(aggregateNamespace)

        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next

        ' Add the interface declaration (partial)
        Dim interfaceDeclaration As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration(m_aggregate.Name)
        ' Comment the interface declaration
        If (Not String.IsNullOrWhiteSpace(m_aggregate.Description)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_aggregate.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_aggregate.Notes)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_aggregate.Notes}))
        End If

        'Make the interface inherit from IAggregationIdentifier(Of Type)
        Dim implementsGeneric As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("CQRSAzure.EventSourcing.IAggregationIdentifier", {ToMemberType(m_aggregate.KeyDataType)})
        interfaceDeclaration.BaseTypes.Add(implementsGeneric)

        'and also inherit IEventStream(Of TAggregate As IAggregationIdentifier)
        Dim implementsEventStreamGeneric As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("CQRSAzure.EventSourcing.IEventStream", {implementsGeneric})
        interfaceDeclaration.BaseTypes.Add(implementsEventStreamGeneric)

        aggregateNamespace.Types.Add(interfaceDeclaration)

        Return aggregateInterfaceRet

    End Function

    Public Function GenerateImplementation() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateImplementation

        'add the imports namespace
        Dim aggregateClassRet As New CodeCompileUnit()

        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next
        aggregateClassRet.Namespaces.Add(aggregateNamespace)
        If (Not String.IsNullOrWhiteSpace(m_aggregate.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_aggregate.Notes}))
        End If

        ' Add the class declaration (partial)
        Dim classDeclaration As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration(m_aggregate.Name)
        If (Not String.IsNullOrWhiteSpace(m_aggregate.Description)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_aggregate.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_aggregate.Notes)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_aggregate.Notes}))
        End If
        'Make the class implement the interface
        classDeclaration.BaseTypes.Add(New CodeTypeReference(GetType(Object)))
        classDeclaration.BaseTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(m_aggregate.Name))

        'Add the model name (DomainNameAttribute)
        If Not String.IsNullOrWhiteSpace(m_aggregate.CQRSModel.Name) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument("domainNameIn", New CodePrimitiveExpression(m_aggregate.CQRSModel.Name)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.DomainNameAttribute",
                                                              params))
        End If

        'm_key - private member
        classDeclaration.Members.Add(New CodeMemberField(ToMemberType(m_aggregate.KeyDataType), EventCodeGenerator.ToMemberName(KeyPropertyName, True)))

        'Public Function GetAggregateIdentifier() As String Implements CQRSAzure.EventSourcing.IAggregationIdentifier.GetAggregateIdentifier
        Dim getAggregeateIdentifierFunction As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedFunction("GetAggregateIdentifier",
                                                                                                                  EventCodeGenerator.ToMemberType(PropertyDataType.String),
                                                                                                                  Nothing)
        If (getAggregeateIdentifierFunction IsNot Nothing) Then
            getAggregeateIdentifierFunction.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Returns the unique identifier of this " & m_aggregate.Name}))
            'make it implement the interface IAggregationIdentifier.GetAggregateIdentifier
            getAggregeateIdentifierFunction.ImplementationTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference("AggregationIdentifier"))
            'make it return the current key
            If (m_aggregate.KeyDataType = KeyDataType.DomainUniqueString) Then
                'Just return the backing key string directly
                getAggregeateIdentifierFunction.Statements.Add(New CodeMethodReturnStatement(New CodeArgumentReferenceExpression(EventCodeGenerator.ToMemberName(KeyPropertyName, True))))
            Else
                'Return the backing key via ToString()
                getAggregeateIdentifierFunction.Statements.Add(New CodeMethodReturnStatement(New CodeMethodReferenceExpression(New CodeArgumentReferenceExpression(EventCodeGenerator.ToMemberName(KeyPropertyName, True)), "ToString")))
            End If
            classDeclaration.Members.Add(getAggregeateIdentifierFunction)
        End If

        'Public Sub SetKey(key As Integer) Implements CQRSAzure.EventSourcing.IAggregationIdentifier(Of Integer).SetKey 
        Dim setKeySub As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedSub("SetKey",
                                                                                       {New CodeParameterDeclarationExpression(ToMemberType(m_aggregate.KeyDataType),
                                                                                                                               EventCodeGenerator.ToMemberName(KeyPropertyName, False, True))})
        If (setKeySub IsNot Nothing) Then
            'make it implement the interface  CQRSAzure.EventSourcing.IAggregationIdentifier(Of T).SetKey
            Dim implementsGeneric As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("CQRSAzure.EventSourcing.IAggregationIdentifier", {ToMemberType(m_aggregate.KeyDataType)})
            If (implementsGeneric IsNot Nothing) Then
                setKeySub.ImplementationTypes.Add(implementsGeneric)
            End If
            ' add an assignment from this parameter to the internal field
            Dim targetField As New CodeFieldReferenceExpression()
            targetField.FieldName = EventCodeGenerator.ToMemberName(KeyPropertyName, True)
            Dim sourceField As New CodeFieldReferenceExpression()
            sourceField.FieldName = EventCodeGenerator.ToMemberName(KeyPropertyName, False, True)
            setKeySub.Statements.Add(New CodeAssignStatement(targetField, sourceField))
            ' Add this sub to the implementation class
            classDeclaration.Members.Add(setKeySub)
        End If



        Dim parentAggregateInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsInterfaceReference(m_aggregate.Name)
        Dim genericEventInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("IEvent", {parentAggregateInterface})
        Dim genericEventStreamInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("IEventStream", {parentAggregateInterface})

        Dim m_eventStreamMember As CodeMemberField = New CodeMemberField(genericEventStreamInterface, EventCodeGenerator.ToMemberName("eventStream", True))

        'm_eventStream private member
        If (genericEventStreamInterface IsNot Nothing) Then
            classDeclaration.Members.Add(m_eventStreamMember)
        End If

        'Public Sub AddEvent(ByVal eventToAdd As IEvent(Of TAggregate)) Implements IEventStream(Of TAggregate As IAggregationIdentifier)
        Dim addEventSub As CodeMemberMethod = MethodCodeGenerator.PrivateParameterisedSub("AddEvent",
                                                                                         {New CodeParameterDeclarationExpression(genericEventInterface,
                                                                                                                                 "eventToAdd")})

        Dim m_eventStreamReference As New CodeFieldReferenceExpression()
        m_eventStreamReference.FieldName = EventCodeGenerator.ToMemberName("eventStream", True)

        If (addEventSub IsNot Nothing) Then
            'make it implement the interface IEventStream(Of TAggregate As IAggregationIdentifier)
            If (genericEventStreamInterface IsNot Nothing) Then
                addEventSub.ImplementationTypes.Add(genericEventStreamInterface)
            End If

            'eventToAdd 
            Dim eventToAddReference As New CodeFieldReferenceExpression()
            eventToAddReference.FieldName = "eventToAdd"

            'call m_eventStream.Add(eventToAdd)

            Dim addMethod As New CodeMethodReferenceExpression(m_eventStreamReference, "Add")
            addEventSub.Statements.Add(New CodeMethodInvokeExpression(addMethod, {eventToAddReference}))

            ' Add this sub to the implementation class
            classDeclaration.Members.Add(addEventSub)
        End If

        'Add the specific event subs for each event in this aggregate
        If (m_aggregate.EventDefinitions IsNot Nothing) Then
            For Each evt As EventDefinition In m_aggregate.EventDefinitions
                'Get a reference to the event's interface type
                Dim parameterType As CodeTypeReference = New CodeTypeReference(ModelCodeGenerator.MakeInterfaceName(evt.Name))
                Dim onEventSub As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedSub(ModelCodeGenerator.MakeValidCodeName(evt.Name),
                                                                                         {New CodeParameterDeclarationExpression(parameterType,
                                                                                                                                 "eventToAdd")})
                If (onEventSub IsNot Nothing) Then
                    If (Not String.IsNullOrEmpty(evt.Description)) Then
                        ' Add the comment
                        onEventSub.Comments.AddRange(CommentGeneration.SummaryCommentSection({evt.Description}))
                    End If
                    ' Pass the event to add 
                    Dim addMethod As New CodeMethodReferenceExpression(m_eventStreamReference, "Add")
                    Dim eventToAddReference As New CodeFieldReferenceExpression()
                    eventToAddReference.FieldName = "eventToAdd"
                    Dim addMethodInvoke = New CodeMethodInvokeExpression(addMethod, {eventToAddReference})

                    onEventSub.Statements.Add(addMethodInvoke)

                    ' Add this sub to the implementation class
                    classDeclaration.Members.Add(onEventSub)
                End If
            Next
        End If

        'Add constructors
        Dim emptyConstructor As New CodeConstructor()
        emptyConstructor.Attributes += MemberAttributes.Public
        emptyConstructor.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Empty constructor for serialisation",
                                                                                   "This should be removed if serialisation is not needed"}))
        classDeclaration.Members.Add(emptyConstructor)


        Dim keyConstructor As CodeConstructor = ConstructorCodeGenerator.ParameterisedConstructor({New ParameterPropertyDefinition(KeyPropertyName,
                                                                                                                                   KeyTypeToParameterType(m_aggregate.KeyDataType))
                                                                                                  })
        keyConstructor.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Create an instance of the aggregate from its key identifier"}))
        'set the private member in this constructor

        classDeclaration.Members.Add(keyConstructor)

        'put the built class into the namespace
        aggregateNamespace.Types.Add(classDeclaration)

        Return aggregateClassRet

    End Function

    Private m_options As ModelCodeGenerationOptions = ModelCodeGenerationOptions.DefaultOptions()
    Public Sub SetCodeGenerationOptions(options As ModelCodeGenerationOptions) Implements IEntityCodeGenerator.SetCodeGenerationOptions
        m_options = options
    End Sub

    Private ReadOnly Property KeyPropertyName As String
        Get
            If (m_aggregate IsNot Nothing) Then
                If Not String.IsNullOrWhiteSpace(m_aggregate.KeyName) Then
                    Return m_aggregate.KeyName
                End If
            End If

            Return "Key"
        End Get
    End Property

#Region "Constructors"
    Public Sub New(ByVal aggregateToGenerate As AggregateIdentifier)
        m_aggregate = aggregateToGenerate
    End Sub



#End Region


    Public Shared Function ToMemberType(ByVal propertyType As KeyDataType) As CodeTypeReference

        Select Case propertyType
            Case KeyDataType.IncrementalNumber
                Return New CodeTypeReference(GetType(Integer))
            Case KeyDataType.DomainUniqueString
                Return New CodeTypeReference(GetType(String))
            Case KeyDataType.SystemGUID
                Return New CodeTypeReference(GetType(Guid))
            Case Else
                Return New CodeTypeReference(GetType(Object))
        End Select

    End Function

    Public Shared Function KeyTypeToParameterType(ByVal keyType As KeyDataType) As PropertyDataType

        Select Case keyType
            Case KeyDataType.DomainUniqueString
                Return PropertyDataType.String
            Case KeyDataType.IncrementalNumber
                Return PropertyDataType.Integer
            Case KeyDataType.SystemGUID
                Return PropertyDataType.GUID
            Case Else
                Return PropertyDataType.String
        End Select

    End Function

End Class
