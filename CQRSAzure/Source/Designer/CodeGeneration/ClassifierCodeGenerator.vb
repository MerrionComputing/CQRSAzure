Imports System.CodeDom
Imports System.CodeDom.Compiler
Imports CQRSAzure.CQRSdsl.CodeGeneration
Imports CQRSAzure.CQRSdsl.Dsl

''' <summary>
''' Code generation for a single instance of a classifier for an identity group
''' </summary>
Public Class ClassifierCodeGenerator
    Implements IEntityCodeGenerator

    Public Const CLASSIFIER_FILENAME_IDENTIFIER As String = "classifier"

    Private ReadOnly m_classifier As Classifier

    ''' <summary>
    ''' The filename base to use when saving this classifier's code
    ''' </summary>
    Public ReadOnly Property FilenameBase As String Implements IEntityCodeGenerator.FilenameBase
        Get
            Return ModelCodeGenerator.MakeValidCodeFilenameBase(m_classifier.Name) & "." & CLASSIFIER_FILENAME_IDENTIFIER
        End Get
    End Property

    Public ReadOnly Property RequiredNamespaces As IEnumerable(Of CodeNamespaceImport) Implements IEntityCodeGenerator.RequiredNamespaces
        Get
            Return {
                New CodeNamespaceImport("CQRSAzure"),
                New CodeNamespaceImport("CQRSAzure.IdentifierGroup"),
                New CodeNamespaceImport("CQRSAzure.EventSourcing"),
                New CodeNamespaceImport("CQRSAzure.Aggregation"),
                CodeGenerationUtilities.CreateNamespaceImport({m_classifier.AggregateIdentifier.CQRSModel.Name,
                    m_classifier.AggregateIdentifier.Name})
                }
        End Get
    End Property

    Public ReadOnly Property NamespaceHierarchy As IList(Of String) Implements IEntityCodeGenerator.NamespaceHierarchy
        Get
            If (m_classifier IsNot Nothing) Then
                Return {m_classifier.AggregateIdentifier.CQRSModel.Name,
                    m_classifier.AggregateIdentifier.Name,
                    CLASSIFIER_FILENAME_IDENTIFIER}
            Else
                Return {}
            End If
        End Get
    End Property


    Public Function GenerateInterface() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateInterface
        'add the imports namespace
        Dim classifierInterfaceRet As New CodeCompileUnit()


        Dim classifierNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        classifierInterfaceRet.Namespaces.Add(classifierNamespace)

        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            classifierNamespace.Imports.Add(importNamespace)
        Next

        ' Add the interface declaration (partial)
        Dim interfaceDeclaration As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration(m_classifier.Name)
        ' Comment the interface declaration
        If (Not String.IsNullOrWhiteSpace(m_classifier.Description)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_classifier.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_classifier.Notes)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_classifier.Notes}))
        End If

        If (m_classifier.EventDefinitions IsNot Nothing) Then
            For Each evt In m_classifier.EventDefinitions
                'ClassifierEventHandler(Of TEvent As IEvent)
                Dim implementsIHandleEvent As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("CQRSAzure.IdentifierGroup.IClassifierEventHandler",
                                                                                                                              {New CodeTypeReference(ModelCodeGenerator.MakeInterfaceName(evt.Name))}
                                                                                                                              )
                If (implementsIHandleEvent IsNot Nothing) Then
                    interfaceDeclaration.BaseTypes.Add(implementsIHandleEvent)
                End If
            Next
        End If

        classifierNamespace.Types.Add(interfaceDeclaration)

        Return classifierInterfaceRet

    End Function

    Public Function GenerateImplementation() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateImplementation

        'add the imports namespace
        Dim classifierClassRet As New CodeCompileUnit()

        Dim classifierNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            classifierNamespace.Imports.Add(importNamespace)
        Next
        classifierClassRet.Namespaces.Add(classifierNamespace)
        If (Not String.IsNullOrWhiteSpace(m_classifier.Notes)) Then
            classifierNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_classifier.Notes}))
        End If

        ' Add the class declaration (partial)
        Dim classDeclaration As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration(m_classifier.Name)
        If (Not String.IsNullOrWhiteSpace(m_classifier.Description)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_classifier.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_classifier.Notes)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_classifier.Notes}))
        End If
        'Make the class implement the interface
        classDeclaration.BaseTypes.Add(New CodeTypeReference(GetType(Object)))
        classDeclaration.BaseTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(m_classifier.Name))

        'Add the model name (DomainNameAttribute)
        If Not String.IsNullOrWhiteSpace(m_classifier.AggregateIdentifier.CQRSModel.Name) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument("domainNameIn", New CodePrimitiveExpression(m_classifier.AggregateIdentifier.CQRSModel.Name)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.DomainNameAttribute",
                                                              params))
        End If

        'Add the category attribute
        If Not String.IsNullOrWhiteSpace(m_classifier.Category) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument("categoryNameIn", New CodePrimitiveExpression(m_classifier.Category)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.Category",
                                                              params))
        End If

        '
        If (m_classifier.EventDefinitions IsNot Nothing) Then
            Dim returnType As New CodeTypeReference("EvaluationResult")
            For Each evt In m_classifier.EventDefinitions
                'Create a Function Evaluate(ByVal eventToEvaluate As TEvent) As EvaluationResult
                Dim eventParameter As New CodeParameterDeclarationExpression(InterfaceCodeGeneration.ImplementsInterfaceReference(evt.Name), "eventToEvaluate")
                Dim eventHandlerMethod As CodeMemberMethod = MethodCodeGenerator.PrivateParameterisedFunction("Evaluate",
                                                                                                              {eventParameter},
                                                                                                              returnType)

                If (eventHandlerMethod IsNot Nothing) Then
                    ' Make it implement the interface
                    Dim implementsIHandleEvent As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("CQRSAzure.IdentifierGroup.IClassifierEventHandler",
                                                                                                                                  {New CodeTypeReference(ModelCodeGenerator.MakeInterfaceName(evt.Name))})
                    If (implementsIHandleEvent IsNot Nothing) Then
                        eventHandlerMethod.ImplementationTypes.Add(implementsIHandleEvent)
                    End If
                    '
                    If (Not String.IsNullOrWhiteSpace(evt.Description)) Then
                        eventHandlerMethod.Comments.AddRange(CommentGeneration.SummaryCommentSection({evt.Description}))
                    Else
                        eventHandlerMethod.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Handle the " & evt.Name & " event "}))
                    End If
                    If (Not String.IsNullOrWhiteSpace(evt.Notes)) Then
                        eventHandlerMethod.Comments.AddRange(CommentGeneration.RemarksCommentSection({evt.Notes}))
                    End If

                    'Perform any classification operations
                    For Each eval In m_classifier.ClassifierEventEvaluations.Where(Function(ByVal cee As ClassifierEventEvaluation) (cee.EventName.Equals(evt.Name)))
                        eventHandlerMethod.Statements.Add(New CodeCommentStatement(eval.ToString))
                        If (eval.PropertyEvaluationToPerform = PropertyEvaluation.Always) Then
                            'just return the [value if true]
                            eventHandlerMethod.Statements.Add(ToReturnStatement(eval.OnTrue))
                        Else
                            If (eval.PropertyEvaluationToPerform = PropertyEvaluation.Custom) Then
                                eventHandlerMethod.Statements.Add(New CodeCommentStatement("TODO : Code the custom evaluation for this event"))
                            End If
                            'Get the data type of the thing we are testing against
                            Dim sourceDataType As PropertyDataType = PropertyDataType.String
                            If (Not String.IsNullOrWhiteSpace(eval.SourceEventPropertyName)) Then
                                Dim evp As EventProperty = eval.SelectedEvent.EventProperties.
                                Where(Function(ByVal f As EventProperty) f.Name = eval.SourceEventPropertyName).FirstOrDefault()
                                If (evp IsNot Nothing) Then
                                    sourceDataType = evp.DataType
                                End If
                            End If
                            'make an if/else to evaluate the test given
                            eventHandlerMethod.Statements.Add(ToConditionalReturnStatement(eval.PropertyEvaluationToPerform,
                                                                                           eval.SourceEventPropertyName,
                                                                                           sourceDataType,
                                                                                           eval.Target,
                                                                                           eval.TargetType,
                                                                                           eval.OnTrue,
                                                                                           eval.OnFalse))

                        End If
                    Next

                    classDeclaration.Members.Add(eventHandlerMethod)

                End If

            Next
        End If

        'Add constructors
        Dim emptyConstructor As New CodeConstructor()
        emptyConstructor.Attributes += MemberAttributes.Public
        emptyConstructor.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Empty constructor for serialisation",
                                                                                   "This should be removed if serialisation is not needed"}))
        classDeclaration.Members.Add(emptyConstructor)


        'put the built class into the namespace
        classifierNamespace.Types.Add(classDeclaration)

        Return classifierClassRet

    End Function


    Private m_options As ModelCodeGenerationOptions = ModelCodeGenerationOptions.DefaultOptions()
    Public Sub SetCodeGenerationOptions(options As ModelCodeGenerationOptions) Implements IEntityCodeGenerator.SetCodeGenerationOptions
        m_options = options
    End Sub

    Public Sub New(ByVal classifierToGenerate As Classifier)
        m_classifier = classifierToGenerate
    End Sub

    Private Shared Function ToReturnStatement(ByVal valueToReturn As IdentityGroupClassification) As CodeStatement

        Dim expression As CodeExpression = Nothing
        Dim EvaluationResultExpression As CodeExpression = New CodeTypeReferenceExpression("EvaluationResult")
        If (valueToReturn = IdentityGroupClassification.Exclude) Then
            'EvaluationResult.Exclude
            expression = New CodeFieldReferenceExpression(EvaluationResultExpression, "Exclude")
        ElseIf (valueToReturn = IdentityGroupClassification.Include) Then
            'EvaluationResult.Include
            expression = New CodeFieldReferenceExpression(EvaluationResultExpression, "Include")
        Else
            'EvaluationResult.Unchanged
            expression = New CodeFieldReferenceExpression(EvaluationResultExpression, "Unchanged")
        End If
        If (expression IsNot Nothing) Then
            Return New CodeMethodReturnStatement(expression)
        Else
            Return New CodeMethodReturnStatement()
        End If


    End Function

    Private Shared Function ToConditionalReturnStatement(propertyEvaluationToPerform As PropertyEvaluation,
                                              sourceEventPropertyName As String,
                                              sourcePropertyType As PropertyDataType,
                                              target As String,
                                              targetType As EvaluationTargetType,
                                              onTrue As IdentityGroupClassification,
                                              onFalse As IdentityGroupClassification) As CodeStatement
        '
        Dim onTrueStatement As CodeStatement = ToReturnStatement(onTrue)
        Dim onFalseStatement As CodeStatement = ToReturnStatement(onFalse)

        Dim ifThenElse As CodeConditionStatement = New CodeConditionStatement()
        ifThenElse.TrueStatements.Add(onTrueStatement)
        ifThenElse.FalseStatements.Add(onFalseStatement)

        Dim sourceReference As New CodeFieldReferenceExpression()
        sourceReference.FieldName = EventCodeGenerator.ToMemberName(sourceEventPropertyName, False)
        sourceReference.TargetObject = New CodeVariableReferenceExpression("eventToEvaluate")

        Dim targetReference As CodeExpression = Nothing
        If (targetType = EvaluationTargetType.Constant) Then
            If (sourcePropertyType = PropertyDataType.String) Then
                targetReference = New CodePrimitiveExpression(target)
            Else
                'Need to convert the target to the expected data type
                Select Case sourcePropertyType
                    Case PropertyDataType.Boolean
                        Dim boolSource As Boolean
                        If (Boolean.TryParse(target, boolSource)) Then
                            targetReference = New CodePrimitiveExpression(boolSource)
                        End If
                    Case PropertyDataType.Date
                        Dim dateSource As Date
                        If (Date.TryParse(target, dateSource)) Then
                            targetReference = New CodePrimitiveExpression(dateSource)
                        End If
                    Case PropertyDataType.Decimal
                        Dim decimalSource As Decimal
                        If (Decimal.TryParse(target, decimalSource)) Then
                            targetReference = New CodePrimitiveExpression(decimalSource)
                        End If
                    Case PropertyDataType.FloatingPointNumber
                        Dim doubleSource As Double
                        If (Double.TryParse(target, doubleSource)) Then
                            targetReference = New CodePrimitiveExpression(doubleSource)
                        End If
                    Case PropertyDataType.GUID
                        Dim guidSource As Guid
                        If (Guid.TryParse(target, guidSource)) Then
                            targetReference = New CodePrimitiveExpression(guidSource)
                        End If
                    Case PropertyDataType.Image
                        ' 
                    Case PropertyDataType.Integer
                        Dim integerSource As Integer
                        If (Integer.TryParse(target, integerSource)) Then
                            targetReference = New CodePrimitiveExpression(integerSource)
                        End If
                End Select
            End If
        Else
            'Assume this is the name of a variable...
            targetReference = New CodeVariableReferenceExpression(target)
        End If

        'If we can't work out what to do with the target...?
        If (targetReference Is Nothing) Then

        End If

        Dim stringReference As New CodeTypeReferenceExpression(GetType(String))

        'Add the actual evaluation
        '' ifThenElse.Condition
        Dim boolCondition As New CodeBinaryOperatorExpression()
        Select Case propertyEvaluationToPerform
            Case PropertyEvaluation.Custom
                'Put an empty evaluator and a TODO note
                boolCondition.Left = New CodePrimitiveExpression(True)
                boolCondition.Right = New CodePrimitiveExpression(True)
                boolCondition.Operator = CodeBinaryOperatorType.ValueEquality
                ifThenElse.Condition = boolCondition

            Case PropertyEvaluation.Contains
                '
                Dim containsMethod As New CodeMethodInvokeExpression(
                    New CodeMethodReferenceExpression(stringReference, "Contains"
                    ), {targetReference})
                ifThenElse.Condition = containsMethod

            Case PropertyEvaluation.EndsWith
                '
                Dim endsWithMethod As New CodeMethodInvokeExpression(
                    New CodeMethodReferenceExpression(stringReference, "EndsWith"
                    ), {targetReference})
                ifThenElse.Condition = endsWithMethod

            Case PropertyEvaluation.Equals
                boolCondition.Left = sourceReference
                boolCondition.Right = targetReference
                boolCondition.Operator = CodeBinaryOperatorType.ValueEquality
                ifThenElse.Condition = boolCondition

            Case PropertyEvaluation.IsEmpty
                '

            Case PropertyEvaluation.IsGreaterThan
                boolCondition.Left = sourceReference
                boolCondition.Right = targetReference
                boolCondition.Operator = CodeBinaryOperatorType.GreaterThan
                ifThenElse.Condition = boolCondition

            Case PropertyEvaluation.IsGreaterThanOrEqualTo
                boolCondition.Left = sourceReference
                boolCondition.Right = targetReference
                boolCondition.Operator = CodeBinaryOperatorType.GreaterThanOrEqual
                ifThenElse.Condition = boolCondition

            Case PropertyEvaluation.IsLessThan
                boolCondition.Left = sourceReference
                boolCondition.Right = targetReference
                boolCondition.Operator = CodeBinaryOperatorType.LessThan
                ifThenElse.Condition = boolCondition

            Case PropertyEvaluation.IsLessThanOrEqualTo
                boolCondition.Left = sourceReference
                boolCondition.Right = targetReference
                boolCondition.Operator = CodeBinaryOperatorType.LessThanOrEqual
                ifThenElse.Condition = boolCondition

            Case PropertyEvaluation.StartsWith
                '
                Dim startsWithMethod As New CodeMethodInvokeExpression(
                    New CodeMethodReferenceExpression(stringReference, "StartsWith"
                    ), {targetReference})
                ifThenElse.Condition = startsWithMethod

        End Select

        Return ifThenElse

    End Function



End Class
