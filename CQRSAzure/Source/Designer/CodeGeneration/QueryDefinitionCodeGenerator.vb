Imports System.CodeDom
Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces

''' <summary>
''' Code generation for a query definition
''' </summary>
Public Class QueryDefinitionCodeGenerator
    Implements IEntityCodeGenerator

    Public Const QUERY_FILENAME_IDENTIFIER = "queryDefinition"
    Public Const QUERY_DEFINITION_SUFFIX As String = "Definition"
    Public Const QUERY_RETURN_SUFFIX = "_Return"

    Private ReadOnly m_query As QueryDefinition

    Public ReadOnly Property QueryDefinitionName As String
        Get
            If (m_query IsNot Nothing) Then
                Return m_query.Name & "_" & QUERY_DEFINITION_SUFFIX
            Else
                Throw New ArgumentNullException("Query definition not initialised")
            End If
        End Get
    End Property

    Public ReadOnly Property FilenameBase As String Implements IEntityCodeGenerator.FilenameBase
        Get
            Return ModelCodeGenerator.MakeValidCodeFilenameBase(Me.QueryDefinitionName) & "." & QUERY_FILENAME_IDENTIFIER
        End Get
    End Property

    Public Function GenerateInterface() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateInterface

        'add the imports namespace
        Dim queryInterfaceRet As New CodeCompileUnit()

        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        queryInterfaceRet.Namespaces.Add(aggregateNamespace)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next

        If (Not String.IsNullOrWhiteSpace(m_query.AggregateIdentifier.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_query.AggregateIdentifier.Notes}))
        End If

        ' Add an interface definition of the "Return" (TResult) part of the query definition
        Dim interfaceReturnDeclaration As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration(Me.QueryDefinitionName & QUERY_RETURN_SUFFIX)
        If (m_query.QueryReturnParameters.Count > 0) Then
            For Each retProp In m_query.QueryReturnParameters
                Dim eventmember As CodeMemberProperty = InterfaceCodeGeneration.SimplePropertyDeclaration(True, retProp.Name, retProp.DataType)
                If (eventmember IsNot Nothing) Then

                    'Add business meaning comments
                    If Not String.IsNullOrEmpty(retProp.Description) Then
                        eventmember.Comments.AddRange(CommentGeneration.SummaryCommentSection({retProp.Description}))
                    End If
                    If Not String.IsNullOrEmpty(retProp.Notes) Then
                        eventmember.Comments.AddRange(CommentGeneration.RemarksCommentSection({retProp.Notes}))
                    End If
                    interfaceReturnDeclaration.Members.Add(eventmember)
                End If
            Next
        End If
        aggregateNamespace.Types.Add(interfaceReturnDeclaration)

        ' Add the interface declaration (partial)
        Dim interfaceDeclaration As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration(Me.QueryDefinitionName)
        ' Comment the interface declaration
        If (Not String.IsNullOrWhiteSpace(m_query.Description)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_query.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_query.Notes)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_query.Notes}))
        End If

        'Create the IQueryDefinition(Of TResult) based interface
        Dim genericEventInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("IQueryDefinition", {Me.GetReturnType()})
        interfaceDeclaration.BaseTypes.Add(genericEventInterface)

        ' Add the input parameters definition
        If (m_query.QueryInputParameters IsNot Nothing) Then
            If (m_query.QueryInputParameters.Count > 0) Then
                For Each qryInProp In m_query.QueryInputParameters
                    Dim eventmember As CodeMemberProperty = InterfaceCodeGeneration.SimplePropertyDeclaration(True, qryInProp.Name, qryInProp.DataType)
                    If (eventmember IsNot Nothing) Then

                        'Add business meaning comments
                        If Not String.IsNullOrEmpty(qryInProp.Description) Then
                            eventmember.Comments.AddRange(CommentGeneration.SummaryCommentSection({qryInProp.Description}))
                        End If
                        If Not String.IsNullOrEmpty(qryInProp.Notes) Then
                            eventmember.Comments.AddRange(CommentGeneration.RemarksCommentSection({qryInProp.Notes}))
                        End If
                        interfaceDeclaration.Members.Add(eventmember)
                    End If
                Next
            End If
        End If

        aggregateNamespace.Types.Add(interfaceDeclaration)
        Return queryInterfaceRet

    End Function

    Public Function GenerateImplementation() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateImplementation

        'add the imports namespace
        Dim queryClasseRet As New CodeCompileUnit()


        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next
        queryClasseRet.Namespaces.Add(aggregateNamespace)
        If (Not String.IsNullOrWhiteSpace(m_query.AggregateIdentifier.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_query.AggregateIdentifier.Notes}))
        End If


        ' Add the class declaration (partial)
        Dim classDeclaration As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration(Me.QueryDefinitionName)
        If (Not String.IsNullOrWhiteSpace(m_query.Description)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_query.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_query.Notes)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_query.Notes}))
        End If

        'Make the class inherit from QueryDefinitionBase(Of TReturnType)
        classDeclaration.BaseTypes.Add(New CodeTypeReference("QueryDefinitionBase", {Me.GetReturnType()}))
        'Make the class implement the interface
        classDeclaration.BaseTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(Me.QueryDefinitionName))

        'Add the model name (DomainNameAttribute)
        If Not String.IsNullOrWhiteSpace(m_query.AggregateIdentifier.CQRSModel.Name) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument("domainNameIn", New CodePrimitiveExpression(m_query.AggregateIdentifier.CQRSModel.Name)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.DomainNameAttribute",
                                                              params))
        End If

        'Add the category attribute
        If Not String.IsNullOrWhiteSpace(m_query.Category) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument("categoryNameIn", New CodePrimitiveExpression(m_query.Category)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.Category",
                                                              params))
        End If

        'Public MustOverride ReadOnly Property QueryName As String

        ' Add the input parameters definition
        ' (Note - no private memebrs required as we use the base.AddParameter syntax)
        If (m_query.QueryInputParameters IsNot Nothing) Then
            If (m_query.QueryInputParameters.Count > 0) Then
                For Each qryInProp In m_query.QueryInputParameters
                    Dim inputParamMember As CodeMemberProperty = InterfaceCodeGeneration.SimplePropertyDeclaration(False, qryInProp.Name, qryInProp.DataType)
                    If (inputParamMember IsNot Nothing) Then
                        'Add business meaning comments
                        If Not String.IsNullOrEmpty(qryInProp.Description) Then
                            inputParamMember.Comments.AddRange(CommentGeneration.SummaryCommentSection({qryInProp.Description}))
                        End If
                        If Not String.IsNullOrEmpty(qryInProp.Notes) Then
                            inputParamMember.Comments.AddRange(CommentGeneration.RemarksCommentSection({qryInProp.Notes}))
                        End If
                        ' Add the inner code to the get and set
                        'GetStatements --> MyBase.GetParameterValue(parameterName, 0)

                        Dim GetParameterValueParameters As CodeExpression() = {New CodePrimitiveExpression(qryInProp.Name), New CodePrimitiveExpression(0)}
                        Dim GetParameterValueMethod As New CodeMethodReferenceExpression(New CodeBaseReferenceExpression(), "GetParameterValue")
                        Dim GetParameterValueInvoke As New CodeMethodInvokeExpression(GetParameterValueMethod, GetParameterValueParameters)
                        inputParamMember.GetStatements.Add(New CodeMethodReturnStatement(GetParameterValueInvoke))

                        'SetStatements --> MyBase.SetParameterValue
                        Dim SetParameterValueParameters As CodeExpression() = {New CodePrimitiveExpression(qryInProp.Name), New CodePrimitiveExpression(0), New CodeVariableReferenceExpression("Value")}
                        Dim SetParameterValueMethod As New CodeMethodReferenceExpression(New CodeBaseReferenceExpression(), "SetParameterValue")
                        Dim SetParameterValueInvoke As New CodeMethodInvokeExpression(SetParameterValueMethod, SetParameterValueParameters)
                        inputParamMember.SetStatements.Add(SetParameterValueInvoke)

                        inputParamMember.ImplementationTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(Me.QueryDefinitionName))

                        classDeclaration.Members.Add(inputParamMember)
                    End If
                Next
            End If
        End If

        'put the built class into the namespace
        aggregateNamespace.Types.Add(classDeclaration)

        Return queryClasseRet

    End Function

    Private Function GetReturnType() As CodeTypeReference

        Return GetReturnType(m_query)

    End Function



    Public ReadOnly Property NamespaceHierarchy As IList(Of String) Implements IEntityCodeGenerator.NamespaceHierarchy
        Get
            If (m_query IsNot Nothing) Then
                Return {m_query.AggregateIdentifier.CQRSModel.Name,
                    m_query.AggregateIdentifier.Name,
                    QUERY_FILENAME_IDENTIFIER}
            Else
                Return {}
            End If
        End Get
    End Property


    Public ReadOnly Property RequiredNamespaces As IEnumerable(Of CodeNamespaceImport) Implements IEntityCodeGenerator.RequiredNamespaces
        Get
            Return {
                New CodeNamespaceImport("CQRSAzure"),
                New CodeNamespaceImport("CQRSAzure.EventSourcing"),
                New CodeNamespaceImport("CQRSAzure.Aggregation"),
                New CodeNamespaceImport("CQRSAzure.QueryDefinition"),
                CodeGenerationUtilities.CreateNamespaceImport({m_query.AggregateIdentifier.CQRSModel.Name,
                    m_query.AggregateIdentifier.Name})
                }
        End Get
    End Property


    Private m_options As ModelCodeGenerationOptions = ModelCodeGenerationOptions.DefaultOptions()
    Public Sub SetCodeGenerationOptions(options As ModelCodeGenerationOptions) Implements IEntityCodeGenerator.SetCodeGenerationOptions
        m_options = options
    End Sub

    Public Sub New(ByVal queryToGenerate As QueryDefinition)
        m_query = queryToGenerate
    End Sub

    Public Shared Function GetReturnType(ByVal queryDef As QueryDefinition) As CodeTypeReference

        If (queryDef.MultiRowResults) Then
            '(Of IEnumerable(Of TResult))
            Return New CodeTypeReference("IEnumerable", {InterfaceCodeGeneration.ImplementsInterfaceReference(queryDef.Name & "_" & QUERY_DEFINITION_SUFFIX & QUERY_RETURN_SUFFIX)})
        Else
            '(Of TResult)
            Return InterfaceCodeGeneration.ImplementsInterfaceReference(queryDef.Name & "_" & QUERY_DEFINITION_SUFFIX & QUERY_RETURN_SUFFIX)
        End If

    End Function

End Class

''' <summary>
''' Code generation for a query handler
''' </summary>
Public Class QueryHandlerCodeGenerator
    Implements IEntityCodeGenerator

    Public Const QUERYHANDLER_FILENAME_IDENTIFIER = "queryHandler"
    Public Const QUERY_HANDLER_SUFFIX As String = "Handler"

    Private ReadOnly m_query As QueryDefinition

    Public ReadOnly Property FilenameBase As String Implements IEntityCodeGenerator.FilenameBase
        Get
            Return ModelCodeGenerator.MakeValidCodeFilenameBase(m_query.Name) & "." & QUERYHANDLER_FILENAME_IDENTIFIER
        End Get
    End Property

    Public ReadOnly Property QueryHandlerName As String
        Get
            If (m_query IsNot Nothing) Then
                Return m_query.Name & "_" & QUERY_HANDLER_SUFFIX
            Else
                Throw New ArgumentNullException("Query definition not initialised")
            End If
        End Get
    End Property

    Public Function GenerateInterface() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateInterface

        'add the imports namespace
        Dim queryInterfaceRet As New CodeCompileUnit()

        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        queryInterfaceRet.Namespaces.Add(aggregateNamespace)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next

        If (Not String.IsNullOrWhiteSpace(m_query.AggregateIdentifier.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_query.AggregateIdentifier.Notes}))
        End If

        ' Add the interface declaration (partial)
        Dim interfaceDeclaration As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration(Me.QueryHandlerName)
        ' Comment the interface declaration
        If (Not String.IsNullOrWhiteSpace(m_query.Description)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_query.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_query.Notes)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_query.Notes}))
        End If

        'Create the IQueryHandler(Of IQueryDefinition) based interface
        Dim qryDefinitionInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsInterfaceReference(m_query.Name & "_" & QueryDefinitionCodeGenerator.QUERY_DEFINITION_SUFFIX)
        Dim qryResultsInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsInterfaceReference(m_query.Name & "_" & QueryDefinitionCodeGenerator.QUERY_RETURN_SUFFIX)
        Dim genericEventInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("IQueryHandler", {qryDefinitionInterface, qryResultsInterface})
        interfaceDeclaration.BaseTypes.Add(genericEventInterface)


        ' Add a function to take the definition and return the return type...e.g. like
        ' Public Function HandleQuery(byval qryToHandle As IQueryDefintion) as IQueryResult 
        Dim returnType As CodeTypeReference = QueryDefinitionCodeGenerator.GetReturnType(m_query)
        Dim qryParameter As New CodeParameterDeclarationExpression(qryDefinitionInterface, "qryToHandle")
        Dim handleQueryFunction = MethodCodeGenerator.PublicParameterisedFunction("HandleQuery", returnType, {})
        If (handleQueryFunction IsNot Nothing) Then
            If (String.IsNullOrWhiteSpace(m_query.Description)) Then
                handleQueryFunction.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Handle the query definition", m_query.Name}))
            Else
                handleQueryFunction.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Handle the query definition", m_query.Description}))
            End If
            interfaceDeclaration.Members.Add(handleQueryFunction)
        End If

        aggregateNamespace.Types.Add(interfaceDeclaration)
        Return queryInterfaceRet

    End Function

    Public Function GenerateImplementation() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateImplementation

        'add the imports namespace
        Dim queryClasseRet As New CodeCompileUnit()


        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next
        queryClasseRet.Namespaces.Add(aggregateNamespace)
        If (Not String.IsNullOrWhiteSpace(m_query.AggregateIdentifier.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_query.AggregateIdentifier.Notes}))
        End If

        ' Add the class declaration (partial)
        Dim classDeclaration As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration(Me.QueryHandlerName)
        If (Not String.IsNullOrWhiteSpace(m_query.Description)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_query.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_query.Notes)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_query.Notes}))
        End If

        'Add the model name (DomainNameAttribute)
        If Not String.IsNullOrWhiteSpace(m_query.AggregateIdentifier.CQRSModel.Name) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument("domainNameIn", New CodePrimitiveExpression(m_query.AggregateIdentifier.CQRSModel.Name)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.DomainNameAttribute",
                                                              params))
        End If

        'Add the category attribute
        If Not String.IsNullOrWhiteSpace(m_query.Category) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument("categoryNameIn", New CodePrimitiveExpression(m_query.Category)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.Category",
                                                              params))
        End If

        'Make the class implement the base class QueryHandlerBase(Of TQueryDefinition, TResults) and the  interface
        Dim qryDefinitionInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsInterfaceReference(m_query.Name & "_" & QueryDefinitionCodeGenerator.QUERY_DEFINITION_SUFFIX)
        Dim qryResultsInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsInterfaceReference(m_query.Name & "_" & QueryDefinitionCodeGenerator.QUERY_RETURN_SUFFIX)

        classDeclaration.BaseTypes.Add(New CodeTypeReference("QueryHandlerBase", {qryDefinitionInterface, qryResultsInterface}))
        classDeclaration.BaseTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(Me.QueryHandlerName))

        'Create the query properties...
        'Function HandleQuery()
        Dim returnType As CodeTypeReference = QueryDefinitionCodeGenerator.GetReturnType(m_query)
        Dim qryParameter As New CodeParameterDeclarationExpression(qryDefinitionInterface, "qryToHandle")
        Dim handleQueryFunction = MethodCodeGenerator.PublicParameterisedFunction("HandleQuery", returnType, {qryParameter})
        If (handleQueryFunction IsNot Nothing) Then
            'Add comment summary to the function
            If (String.IsNullOrWhiteSpace(m_query.Description)) Then
                handleQueryFunction.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Handle the query definition", m_query.Name}))
            Else
                handleQueryFunction.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Handle the query definition", m_query.Description}))
            End If
            'Add a parameter summary too
            handleQueryFunction.Comments.AddRange(CommentGeneration.ParamCommentsSection("qryToHandle", {"The instance of the query definition to handle and return data for"}))

            'Make it implement the interface
            handleQueryFunction.ImplementationTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(Me.QueryHandlerName))

            'Declare a return value 
            Dim retDecl As CodeVariableDeclarationStatement = New CodeVariableDeclarationStatement(returnType, "queryReturn")
            If (retDecl IsNot Nothing) Then
                retDecl.InitExpression = New CodePrimitiveExpression(Nothing)
                handleQueryFunction.Statements.Add(retDecl)

                'TODO : Populate the query return value
                handleQueryFunction.Statements.Add(New CodeCommentStatement("TODO : Populate the query return value", False))

                'Return retDecl
                handleQueryFunction.Statements.Add(New CodeMethodReturnStatement(New CodeVariableReferenceExpression(retDecl.Name)))

            End If

            classDeclaration.Members.Add(handleQueryFunction)
        End If

        'put the built class into the namespace
        aggregateNamespace.Types.Add(classDeclaration)

        Return queryClasseRet

    End Function

    Public ReadOnly Property NamespaceHierarchy As IList(Of String) Implements IEntityCodeGenerator.NamespaceHierarchy
        Get
            If (m_query IsNot Nothing) Then
                Return {m_query.AggregateIdentifier.CQRSModel.Name,
                    m_query.AggregateIdentifier.Name,
                    QUERYHANDLER_FILENAME_IDENTIFIER}
            Else
                Return {}
            End If
        End Get
    End Property


    Public ReadOnly Property RequiredNamespaces As IEnumerable(Of CodeNamespaceImport) Implements IEntityCodeGenerator.RequiredNamespaces
        Get
            Return {
                New CodeNamespaceImport("CQRSAzure"),
                New CodeNamespaceImport("CQRSAzure.EventSourcing"),
                New CodeNamespaceImport("CQRSAzure.Aggregation"),
                New CodeNamespaceImport("CQRSAzure.QueryDefinition"),
                New CodeNamespaceImport("CQRSAzure.QueryHandler"),
                CodeGenerationUtilities.CreateNamespaceImport({m_query.AggregateIdentifier.CQRSModel.Name,
                    m_query.AggregateIdentifier.Name})
                }
        End Get
    End Property


    Private m_options As ModelCodeGenerationOptions = ModelCodeGenerationOptions.DefaultOptions()
    Public Sub SetCodeGenerationOptions(options As ModelCodeGenerationOptions) Implements IEntityCodeGenerator.SetCodeGenerationOptions
        m_options = options
    End Sub

    Public Sub New(ByVal queryToGenerate As QueryDefinition)
        m_query = queryToGenerate
    End Sub
End Class
