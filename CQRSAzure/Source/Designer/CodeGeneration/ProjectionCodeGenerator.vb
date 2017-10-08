Imports System.CodeDom
Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces

''' <summary>
''' Code generation for a projection
''' </summary>
Public Class ProjectionCodeGenerator
    Implements IEntityCodeGenerator

    Public Const PROJECTION_FILENAME_IDENTIFIER = "projection"

    Private ReadOnly m_projection As ProjectionDefinition

    Public ReadOnly Property FilenameBase As String Implements IEntityCodeGenerator.FilenameBase
        Get
            Return ModelCodeGenerator.MakeValidCodeFilenameBase(m_projection.Name) & "." & PROJECTION_FILENAME_IDENTIFIER
        End Get
    End Property

    Public Function GenerateInterface() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateInterface
        'add the imports namespace
        Dim projectionInterfaceRet As New CodeCompileUnit()
        'put the namespace hierarchy in...model name..
        Dim projectionNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        projectionInterfaceRet.Namespaces.Add(projectionNamespace)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            projectionNamespace.Imports.Add(importNamespace)
        Next

        Dim interfaceDeclaration As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration(m_projection.Name)
        ' Comment the interface declaration
        If (Not String.IsNullOrWhiteSpace(m_projection.Description)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_projection.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_projection.Notes)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_projection.Notes}))
        End If

        ' Make the interface inherit Interface IProjection(Of TAggregate As IAggregationIdentifier, TAggregateKey)
        Dim parentAggregateInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsInterfaceReference(m_projection.AggregateIdentifier.Name)
        Dim genericEventInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("IProjection", {parentAggregateInterface, AggregateIdentifierCodeGenerator.ToMemberType(m_projection.AggregateIdentifier.KeyDataType)})
        interfaceDeclaration.BaseTypes.Add(genericEventInterface)

        'For each specific event handled, add a IHandleEvent(Of In TEvent As IEvent) for that event
        If (m_projection.EventDefinitions IsNot Nothing) Then
            If m_projection.EventDefinitions.Count > 0 Then
                For Each evd As EventDefinition In m_projection.EventDefinitions
                    Dim implementsIHandleEvent As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("CQRSAzure.EventSourcing.IHandleEvent", {New CodeTypeReference(ModelCodeGenerator.MakeInterfaceName(evd.Name))})
                    If (implementsIHandleEvent IsNot Nothing) Then
                        interfaceDeclaration.BaseTypes.Add(implementsIHandleEvent)
                    End If
                Next
            End If
        End If

        'Add the projection properties
        If (m_projection.ProjectionProperties IsNot Nothing) Then
            If m_projection.ProjectionProperties.Count > 0 Then
                For Each projProp In m_projection.ProjectionProperties
                    Dim eventmember As CodeMemberProperty = InterfaceCodeGeneration.SimplePropertyDeclaration(True, projProp.Name, projProp.DataType)
                    If (eventmember IsNot Nothing) Then
                        'Add business meaning comments
                        If Not String.IsNullOrEmpty(projProp.Description) Then
                            eventmember.Comments.AddRange(CommentGeneration.SummaryCommentSection({projProp.Description}))
                        End If
                        If Not String.IsNullOrEmpty(projProp.Notes) Then
                            eventmember.Comments.AddRange(CommentGeneration.RemarksCommentSection({projProp.Notes}))
                        End If
                        interfaceDeclaration.Members.Add(eventmember)
                    End If
                Next
            End If
        End If

        projectionNamespace.Types.Add(interfaceDeclaration)

        Return projectionInterfaceRet
    End Function

    Public Function GenerateImplementation() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateImplementation

        'add the imports namespace
        Dim projectionClasseRet As New CodeCompileUnit()
        'put the namespace hierarchy in...model name..
        Dim projectionNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        projectionClasseRet.Namespaces.Add(projectionNamespace)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            projectionNamespace.Imports.Add(importNamespace)
        Next

        Dim classDeclaration As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration(m_projection.Name)
        If (Not String.IsNullOrWhiteSpace(m_projection.Description)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_projection.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_projection.Notes)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_projection.Notes}))
        End If

        'Make the class inherit from CQRSAzure.EventSourcing.ProjectionBase<TAggregate,TAggregateKey>
        Dim parentAggregateInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsInterfaceReference(m_projection.AggregateIdentifier.Name)
        Dim projectionBaseType As CodeTypeReference = New CodeTypeReference("CQRSAzure.EventSourcing.ProjectionBase", {parentAggregateInterface, AggregateIdentifierCodeGenerator.ToMemberType(m_projection.AggregateIdentifier.KeyDataType)})

        classDeclaration.BaseTypes.Add(projectionBaseType)
        'Make the class implement the interface
        classDeclaration.BaseTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(m_projection.Name))

        'Add the model name (DomainNameAttribute)
        If Not String.IsNullOrWhiteSpace(m_projection.AggregateIdentifier.CQRSModel.Name) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument(New CodePrimitiveExpression(m_projection.AggregateIdentifier.CQRSModel.Name)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.DomainNameAttribute",
                                                              params))
        End If

        'Add the category attribute
        If Not String.IsNullOrWhiteSpace(m_projection.Category) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument(New CodePrimitiveExpression(m_projection.Category)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.Category",
                                                              params))
        End If

        'Add the projection properties
        '1) private members
        classDeclaration.Members.AddRange(PropertyCodeGeneration.PrivateBackingMembers(m_projection.ProjectionProperties.Cast(Of IPropertyEntity).ToList(), True))

        If (m_projection.ProjectionProperties IsNot Nothing) Then
            If m_projection.ProjectionProperties.Count > 0 Then
                For Each projProp In m_projection.ProjectionProperties
                    Dim propertyMember As CodeMemberProperty = PropertyCodeGeneration.PublicMember(projProp)
                    If (propertyMember IsNot Nothing) Then
                        propertyMember.ImplementationTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(m_projection.Name))
                        classDeclaration.Members.Add(propertyMember)
                    End If
                Next
            End If
        End If

        'For each specific event handled, add a Handles(T) for that event
        If (m_projection.EventDefinitions IsNot Nothing) Then
            If m_projection.EventDefinitions.Count > 0 Then
                For Each evd As EventDefinition In m_projection.EventDefinitions

                    Dim eventParameter As New CodeParameterDeclarationExpression(InterfaceCodeGeneration.ImplementsInterfaceReference(evd.Name), "eventToHandle")
                    Dim eventHandlerSub As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedSub("HandleEvent",
                                                                                                         {eventParameter})

                    If (eventHandlerSub IsNot Nothing) Then
                        'Make it implement the CQRSAzure.EventSourcing.IHandleEvent(Of IEvent) reference
                        Dim implementsIHandleEvent As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("CQRSAzure.EventSourcing.IHandleEvent", {New CodeTypeReference(ModelCodeGenerator.MakeInterfaceName(evd.Name))})
                        If (implementsIHandleEvent IsNot Nothing) Then
                            eventHandlerSub.ImplementationTypes.Add(implementsIHandleEvent)
                        End If

                        'Add the comments to this event handling method
                        If (Not String.IsNullOrWhiteSpace(evd.Description)) Then
                            eventHandlerSub.Comments.AddRange(CommentGeneration.SummaryCommentSection({evd.Description}))
                        Else
                            eventHandlerSub.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Handle the " & evd.Name & " event "}))
                        End If
                        If (Not String.IsNullOrWhiteSpace(evd.Notes)) Then
                            eventHandlerSub.Comments.AddRange(CommentGeneration.RemarksCommentSection({evd.Notes}))
                        End If

                        'Now get every property transformation for that event and code it...
                        For Each pt In m_projection.ProjectionEventPropertyOperations.Where(Function(ByVal po As ProjectionEventPropertyOperation) (po.EventName.Equals(evd.Name)))

                            'turn the pt into the relvant code
                            Dim evpCodeGenerator As New ProjectionEventPropertyOperationCodeGenerator(pt)
                            If (evpCodeGenerator IsNot Nothing) Then
                                eventHandlerSub.Statements.AddRange(evpCodeGenerator.Implementation)
                            End If

                        Next pt

                        classDeclaration.Members.Add(eventHandlerSub)

                    End If

                Next
            End If
        End If

        'Add the must-override parts of ProjectionBase<>
        '1) public override bool SupportsSnapshots
        Dim supportsSnapshotsProperty = PropertyCodeGeneration.PublicMember("SupportsSnapshots",
                                                                            PropertyDataType.Boolean,
                                                                            omitBackingProperty:=True
                                                                            )
        If (supportsSnapshotsProperty IsNot Nothing) Then
            MethodCodeGenerator.MakeOverrides(supportsSnapshotsProperty)
            If (m_projection.CanSnapshot) Then
                supportsSnapshotsProperty.GetStatements.Add(New CodeMethodReturnStatement(New CodePrimitiveExpression(True)))
            Else
                supportsSnapshotsProperty.GetStatements.Add(New CodeMethodReturnStatement(New CodePrimitiveExpression(False)))
            End If

            classDeclaration.Members.Add(supportsSnapshotsProperty)
        End If


        '2) public override bool HandlesEventType(Type eventType)
        Dim handleEventTypeParameter As New CodeParameterDeclarationExpression("Type", "eventType")
        Dim HandlesEventTypeFunction As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedFunction("HandlesEventType",
                                                                                            {handleEventTypeParameter},
                                                                                            returnType:=New CodeTypeReference(GetType(Boolean)))

        'Add comments to this sub
        Dim handledTypesDocumentation As New List(Of String)()
        handledTypesDocumentation.Add("Event types handled")
        For Each evd As EventDefinition In m_projection.EventDefinitions
            handledTypesDocumentation.Add(evd.Name & " - " & evd.Description)
        Next
        HandlesEventTypeFunction.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Does the projection handle this event type"}))
        HandlesEventTypeFunction.Comments.AddRange(CommentGeneration.ParamCommentsSection("eventType", {"The event type to check"}))
        HandlesEventTypeFunction.Comments.AddRange(CommentGeneration.RemarksCommentSection(handledTypesDocumentation.ToArray()))

        If (HandlesEventTypeFunction IsNot Nothing) Then
            'Make it override the base function
            MethodCodeGenerator.MakeOverrides(HandlesEventTypeFunction)

            For Each evd As EventDefinition In m_projection.EventDefinitions
                'If the type is this type, return true

            Next

            'Finally return false if we fell through to here
            HandlesEventTypeFunction.Statements.Add(New CodeMethodReturnStatement(New CodePrimitiveExpression(False)))

            classDeclaration.Members.Add(HandlesEventTypeFunction)
        End If

        '3) public override void HandleEvent<TEvent>(TEvent eventToHandle)
        Dim handleEventGenericParameters As New CodeTypeParameterCollection({New CodeTypeParameter("TEvent")})
        Dim handleEventParameter As New CodeParameterDeclarationExpression("TEvent", "eventToHandle")
        Dim handleEventSub As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedSub("HandleEvent",
                                                                                            {handleEventParameter},
                                                                                            genericTypeRestrictions:=handleEventGenericParameters)
        If (handleEventSub IsNot Nothing) Then
            'Make it override the base function
            MethodCodeGenerator.MakeOverrides(handleEventSub)

            For Each evd As EventDefinition In m_projection.EventDefinitions
                'If the type is this type, pass it on to the relevant handler already coded

            Next

            classDeclaration.Members.Add(handleEventSub)
        End If

        projectionNamespace.Types.Add(classDeclaration)

        Return projectionClasseRet

    End Function


    Public ReadOnly Property NamespaceHierarchy As IList(Of String) Implements IEntityCodeGenerator.NamespaceHierarchy
        Get
            If (m_projection IsNot Nothing) Then
                Return {m_projection.AggregateIdentifier.CQRSModel.Name,
                    m_projection.AggregateIdentifier.Name,
                    PROJECTION_FILENAME_IDENTIFIER}
            Else
                Return {}
            End If
        End Get
    End Property


    Public ReadOnly Property RequiredNamespaces As IEnumerable(Of CodeNamespaceImport) Implements IEntityCodeGenerator.RequiredNamespaces
        Get
            Return {
                New CodeNamespaceImport("System"),
                New CodeNamespaceImport("CQRSAzure.EventSourcing"),
                CodeGenerationUtilities.CreateNamespaceImport({m_projection.AggregateIdentifier.CQRSModel.Name,
                    m_projection.AggregateIdentifier.Name}),
                CodeGenerationUtilities.CreateNamespaceImport({
                                                              m_projection.AggregateIdentifier.CQRSModel.Name,
                                                              m_projection.AggregateIdentifier.Name,
                                                              EventCodeGenerator.EVENT_FILENAME_IDENTIFIER})
                }
        End Get
    End Property

    Private m_options As IModelCodeGenerationOptions = ModelCodeGenerationOptions.Default()
    Public Sub SetCodeGenerationOptions(options As IModelCodeGenerationOptions) Implements IEntityCodeGenerator.SetCodeGenerationOptions
        m_options = options
    End Sub

    Public Sub New(ByVal projectionToGenerate As ProjectionDefinition)
        m_projection = projectionToGenerate
    End Sub



    ''' <summary>
    ''' Generate the actual implementation of an event property operation
    ''' </summary>
    ''' <remarks>
    ''' This is not an entity code generation in its own right - we just separate this into a class of its own for unit testing
    ''' </remarks>
    Public Class ProjectionEventPropertyOperationCodeGenerator

        Private ReadOnly m_operation As ProjectionEventPropertyOperation


        Public ReadOnly Property Implementation As CodeStatementCollection
            Get

                Dim ret As New CodeStatementCollection()

                If (m_operation IsNot Nothing) Then

                    If (Not String.IsNullOrWhiteSpace(m_operation.Description)) Then
                        ret.Add(New CodeCommentStatement(m_operation.Description, False))
                    Else
                        ret.Add(New CodeCommentStatement(m_operation.ToString(), False))
                    End If

                    Dim m_TargetReference As New CodeFieldReferenceExpression()
                    m_TargetReference.FieldName = EventCodeGenerator.ToMemberName(m_operation.TargetPropertyName, True)

                    If (m_operation.PropertyOperationToPerform = PropertyOperation.DecrementByValue OrElse
                            m_operation.PropertyOperationToPerform = PropertyOperation.IncrementByValue OrElse
                            m_operation.PropertyOperationToPerform = PropertyOperation.SetToValue
                        ) Then

                        'We need a reference to the event source property
                        Dim m_SourceReference As New CodeFieldReferenceExpression()
                        m_SourceReference.FieldName = EventCodeGenerator.ToMemberName(m_operation.SourceEventPropertyName, False)
                        m_SourceReference.TargetObject = New CodeVariableReferenceExpression("eventToHandle")

                        Select Case m_operation.PropertyOperationToPerform
                            Case PropertyOperation.DecrementByValue
                                Dim rhs As New CodeBinaryOperatorExpression(
                                                m_TargetReference,
                                                CodeBinaryOperatorType.Subtract,
                                                m_SourceReference)
                                ret.Add(New CodeAssignStatement(m_TargetReference, rhs))

                            Case PropertyOperation.IncrementByValue
                                Dim rhs As New CodeBinaryOperatorExpression(
                                                m_TargetReference,
                                                CodeBinaryOperatorType.Add,
                                                m_SourceReference)
                                ret.Add(New CodeAssignStatement(m_TargetReference, rhs))

                            Case PropertyOperation.SetToValue
                                ret.Add(New CodeAssignStatement(m_TargetReference, m_SourceReference))

                        End Select

                    Else
                        Select Case m_operation.PropertyOperationToPerform
                            Case PropertyOperation.SetFlag
                                'set to true
                                ret.Add(New CodeAssignStatement(m_TargetReference, New CodePrimitiveExpression(True)))

                            Case PropertyOperation.UnsetFlag
                                'set to false
                                ret.Add(New CodeAssignStatement(m_TargetReference, New CodePrimitiveExpression(False)))

                            Case PropertyOperation.IncrementCount
                                Dim rhs As New CodeBinaryOperatorExpression(
                                                m_TargetReference,
                                                CodeBinaryOperatorType.Add,
                                                New CodePrimitiveExpression(1))
                                ret.Add(New CodeAssignStatement(m_TargetReference, rhs))

                            Case PropertyOperation.DecrementCount
                                Dim rhs As New CodeBinaryOperatorExpression(
                                                m_TargetReference,
                                                CodeBinaryOperatorType.Subtract,
                                                New CodePrimitiveExpression(1))
                                ret.Add(New CodeAssignStatement(m_TargetReference, rhs))
                        End Select
                    End If


                End If

                Return ret

            End Get
        End Property

        Public Sub New(ByVal operationToGenerate As ProjectionEventPropertyOperation)
            m_operation = operationToGenerate
        End Sub
    End Class

End Class

