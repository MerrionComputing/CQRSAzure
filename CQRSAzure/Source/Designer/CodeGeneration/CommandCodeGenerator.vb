Imports System.CodeDom
Imports CQRSAzure.CQRSdsl.Dsl

Public Class CommandDefinitionCodeGenerator
    Implements IEntityCodeGenerator

    Public Const COMMAND_FILENAME_IDENTIFIER = "commandDefinition"
    Public Const COMMAND_DEFINITION_SUFFIX As String = "Definition"

    Private ReadOnly m_command As CommandDefinition

    Public ReadOnly Property FilenameBase As String Implements IEntityCodeGenerator.FilenameBase
        Get
            Return ModelCodeGenerator.MakeValidCodeFilenameBase(m_command.Name) & "." & COMMAND_FILENAME_IDENTIFIER
        End Get
    End Property

    Public ReadOnly Property CommandDefinitionName As String
        Get
            If (m_command IsNot Nothing) Then
                Return m_command.Name & "_" & COMMAND_DEFINITION_SUFFIX
            Else
                Throw New ArgumentNullException("Command definition not initialised")
            End If
        End Get
    End Property

    Public Function GenerateInterface() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateInterface

        'add the imports namespace
        Dim commandInterfaceRet As New CodeCompileUnit()

        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        commandInterfaceRet.Namespaces.Add(aggregateNamespace)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next

        If (Not String.IsNullOrWhiteSpace(m_command.AggregateIdentifier.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_command.AggregateIdentifier.Notes}))
        End If

        ' Add the interface declaration (partial)
        Dim interfaceDeclaration As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration(Me.CommandDefinitionName)
        ' Comment the interface declaration
        If (Not String.IsNullOrWhiteSpace(m_command.Description)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_command.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_command.Notes)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_command.Notes}))
        End If

        'Create the ICommandDefinition based interface
        If (m_command.CommandParameters IsNot Nothing) Then
            For Each cmdPar In m_command.CommandParameters
                Dim eventmember As CodeMemberProperty = InterfaceCodeGeneration.SimplePropertyDeclaration(True, cmdPar.Name, cmdPar.ParameterType)
                If (eventmember IsNot Nothing) Then

                    'Add business meaning comments
                    If Not String.IsNullOrEmpty(cmdPar.Description) Then
                        eventmember.Comments.AddRange(CommentGeneration.SummaryCommentSection({cmdPar.Description}))
                    End If
                    If Not String.IsNullOrEmpty(cmdPar.Notes) Then
                        eventmember.Comments.AddRange(CommentGeneration.RemarksCommentSection({cmdPar.Notes}))
                    End If
                    interfaceDeclaration.Members.Add(eventmember)
                End If
            Next
        End If


        aggregateNamespace.Types.Add(interfaceDeclaration)
        Return commandInterfaceRet

    End Function

    Public Function GenerateImplementation() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateImplementation


        'add the imports namespace
        Dim commandClassRet As New CodeCompileUnit()


        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next
        commandClassRet.Namespaces.Add(aggregateNamespace)
        If (Not String.IsNullOrWhiteSpace(m_command.AggregateIdentifier.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_command.AggregateIdentifier.Notes}))
        End If

        ' Add the class declaration (partial)
        Dim classDeclaration As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration(Me.CommandDefinitionName)
        If (Not String.IsNullOrWhiteSpace(m_command.Description)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_command.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_command.Notes)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_command.Notes}))
        End If
        'Make the class inherit from the base class CommandDefinitionBase (not generic-typed)
        classDeclaration.BaseTypes.Add(New CodeTypeReference("CommandDefinitionBase"))
        classDeclaration.BaseTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(Me.CommandDefinitionName))

        'Add the model name (DomainNameAttribute)
        If Not String.IsNullOrWhiteSpace(m_command.AggregateIdentifier.CQRSModel.Name) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument("domainNameIn", New CodePrimitiveExpression(m_command.AggregateIdentifier.CQRSModel.Name)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.DomainNameAttribute",
                                                              params))
        End If

        'Add the category attribute
        If Not String.IsNullOrWhiteSpace(m_command.Category) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument("categoryNameIn", New CodePrimitiveExpression(m_command.Category)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.Category",
                                                              params))
        End If

        'TODO : Implement CommandDefinitionBase.CommandName As String 


        'Create the command properties
        ' Note - no backing parameter needed as we will be usingthe AddMember of the base class
        If (m_command.CommandParameters IsNot Nothing) Then
            For Each cmdPar In m_command.CommandParameters
                Dim cmdParamMember As CodeMemberProperty = InterfaceCodeGeneration.SimplePropertyDeclaration(False, cmdPar.Name, cmdPar.ParameterType)
                If (cmdParamMember IsNot Nothing) Then
                    'Add business meaning comments
                    If Not String.IsNullOrEmpty(cmdPar.Description) Then
                        cmdParamMember.Comments.AddRange(CommentGeneration.SummaryCommentSection({cmdPar.Description}))
                    End If
                    If Not String.IsNullOrEmpty(cmdPar.Notes) Then
                        cmdParamMember.Comments.AddRange(CommentGeneration.RemarksCommentSection({cmdPar.Notes}))
                    End If
                    ' Add the inner code to the get and set
                    'GetStatements --> MyBase.GetParameterValue(parameterName, 0)

                    Dim GetParameterValueParameters As CodeExpression() = {New CodePrimitiveExpression(cmdPar.Name), New CodePrimitiveExpression(0)}
                    Dim GetParameterValueMethod As New CodeMethodReferenceExpression(New CodeBaseReferenceExpression(), "GetParameterValue")
                    Dim GetParameterValueInvoke As New CodeMethodInvokeExpression(GetParameterValueMethod, GetParameterValueParameters)
                    cmdParamMember.GetStatements.Add(New CodeMethodReturnStatement(GetParameterValueInvoke))

                    'SetStatements --> MyBase.SetParameterValue
                    Dim SetParameterValueParameters As CodeExpression() = {New CodePrimitiveExpression(cmdPar.Name), New CodePrimitiveExpression(0), New CodeVariableReferenceExpression("Value")}
                    Dim SetParameterValueMethod As New CodeMethodReferenceExpression(New CodeBaseReferenceExpression(), "SetParameterValue")
                    Dim SetParameterValueInvoke As New CodeMethodInvokeExpression(SetParameterValueMethod, SetParameterValueParameters)
                    cmdParamMember.SetStatements.Add(SetParameterValueInvoke)

                    cmdParamMember.ImplementationTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(Me.CommandDefinitionName))

                    classDeclaration.Members.Add(cmdParamMember)
                End If
            Next
        End If

        'put the built class into the namespace
        aggregateNamespace.Types.Add(classDeclaration)

        Return commandClassRet


    End Function

    ''' <summary>
    ''' Return the namespace of the aggregate that owns this command definition
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property NamespaceHierarchy As IList(Of String) Implements IEntityCodeGenerator.NamespaceHierarchy
        Get
            If (m_command IsNot Nothing) Then
                Return {m_command.AggregateIdentifier.CQRSModel.Name,
                    m_command.AggregateIdentifier.Name,
                    COMMAND_FILENAME_IDENTIFIER}
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
                New CodeNamespaceImport("CQRSAzure.CommandDefinition"),
                CodeGenerationUtilities.CreateNamespaceImport({m_command.AggregateIdentifier.CQRSModel.Name,
                    m_command.AggregateIdentifier.Name})
                }
        End Get
    End Property

    Private m_options As ModelCodeGenerationOptions = ModelCodeGenerationOptions.DefaultOptions()
    Public Sub SetCodeGenerationOptions(options As ModelCodeGenerationOptions) Implements IEntityCodeGenerator.SetCodeGenerationOptions
        m_options = options
    End Sub

    Public Sub New(ByVal commandToGenerate As CommandDefinition)
        m_command = commandToGenerate
    End Sub
End Class

Public Class CommandHandlerCodeGenerator
    Implements IEntityCodeGenerator

    Public Const COMMANDHANDLER_FILENAME_IDENTIFIER = "commandHandler"
    Public Const COMMAND_HANDLER_SUFFIX As String = "Handler"

    Private ReadOnly m_command As CommandDefinition

    Public ReadOnly Property FilenameBase As String Implements IEntityCodeGenerator.FilenameBase
        Get
            Return ModelCodeGenerator.MakeValidCodeFilenameBase(m_command.Name) & "." & COMMANDHANDLER_FILENAME_IDENTIFIER
        End Get
    End Property

    Public ReadOnly Property CommandHandlerName As String
        Get
            If (m_command IsNot Nothing) Then
                Return m_command.Name & "_" & COMMAND_HANDLER_SUFFIX
            Else
                Throw New ArgumentNullException("Command definition not initialised")
            End If
        End Get
    End Property

    Public Function GenerateInterface() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateInterface

        'add the imports namespace
        Dim commandInterfaceRet As New CodeCompileUnit()

        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        commandInterfaceRet.Namespaces.Add(aggregateNamespace)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next

        If (Not String.IsNullOrWhiteSpace(m_command.AggregateIdentifier.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_command.AggregateIdentifier.Notes}))
        End If

        ' Add the interface declaration (partial)
        Dim interfaceDeclaration As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration(Me.CommandHandlerName)
        ' Comment the interface declaration
        If (Not String.IsNullOrWhiteSpace(m_command.Description)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_command.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_command.Notes)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_command.Notes}))
        End If


        'Create the ICommandHandler(Of TCommandDefinition As ICommandDefinition) based interface
        Dim cmdDefinitionInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsInterfaceReference(m_command.Name & "_" & CommandDefinitionCodeGenerator.COMMAND_DEFINITION_SUFFIX)
        Dim genericEventInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("ICommandHandler", {cmdDefinitionInterface})
        interfaceDeclaration.BaseTypes.Add(genericEventInterface)


        aggregateNamespace.Types.Add(interfaceDeclaration)
        Return commandInterfaceRet

    End Function

    Public Function GenerateImplementation() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateImplementation

        'add the imports namespace
        Dim commandClassRet As New CodeCompileUnit()


        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next
        commandClassRet.Namespaces.Add(aggregateNamespace)
        If (Not String.IsNullOrWhiteSpace(m_command.AggregateIdentifier.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_command.AggregateIdentifier.Notes}))
        End If

        ' Add the class declaration (partial)
        Dim classDeclaration As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration(Me.CommandHandlerName)
        If (Not String.IsNullOrWhiteSpace(m_command.Description)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_command.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_command.Notes)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_command.Notes}))
        End If
        'Make the class implement the base class CommandHandlerBase(Of TCommandDefinition)  and the interface
        Dim cmdDefinitionInterface As CodeTypeReference = InterfaceCodeGeneration.ImplementsInterfaceReference(m_command.Name & "_" & CommandDefinitionCodeGenerator.COMMAND_DEFINITION_SUFFIX)
        classDeclaration.BaseTypes.Add(New CodeTypeReference("CommandHandlerBase", {cmdDefinitionInterface}))
        classDeclaration.BaseTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(Me.CommandHandlerName))

        'Add the model name (DomainNameAttribute)
        If Not String.IsNullOrWhiteSpace(m_command.AggregateIdentifier.CQRSModel.Name) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument("domainNameIn", New CodePrimitiveExpression(m_command.AggregateIdentifier.CQRSModel.Name)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.DomainNameAttribute",
                                                              params))
        End If

        'Add the category attribute
        If Not String.IsNullOrWhiteSpace(m_command.Category) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument("categoryNameIn", New CodePrimitiveExpression(m_command.Category)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.Category",
                                                              params))
        End If

        'Override Public MustOverride Sub HandleCommand(cmdToHandle As TCommandDefinition) 
        Dim cmdParameter As New CodeParameterDeclarationExpression(cmdDefinitionInterface, "cmdToHandle")
        Dim handleCommandSub = MethodCodeGenerator.PublicParameterisedSub("HandleCommand", {cmdParameter})
        If (handleCommandSub IsNot Nothing) Then
            'add comment summary to the command
            If (String.IsNullOrWhiteSpace(m_command.Description)) Then
                handleCommandSub.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Handle the command definition", m_command.Name}))
            Else
                handleCommandSub.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Handle the command definition", m_command.Description}))
            End If
            'Add a parameter summary too
            handleCommandSub.Comments.AddRange(CommentGeneration.ParamCommentsSection("cmdToHandle", {"The instance of the command definition to handle"}))

            'Make it implement the interface
            handleCommandSub.ImplementationTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(Me.CommandHandlerName))

            'TODO : Process the command
            handleCommandSub.Statements.Add(New CodeCommentStatement("TODO : Process the command definition passed in", False))

            classDeclaration.Members.Add(handleCommandSub)
        End If

        'put the built class into the namespace
        aggregateNamespace.Types.Add(classDeclaration)

        Return commandClassRet


    End Function

    Public ReadOnly Property NamespaceHierarchy As IList(Of String) Implements IEntityCodeGenerator.NamespaceHierarchy
        Get
            If (m_command IsNot Nothing) Then
                Return {m_command.AggregateIdentifier.CQRSModel.Name,
                    m_command.AggregateIdentifier.Name,
                    COMMANDHANDLER_FILENAME_IDENTIFIER}
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
                New CodeNamespaceImport("CQRSAzure.CommandDefinition"),
                New CodeNamespaceImport("CQRSAzure.CommandHandler"),
                CodeGenerationUtilities.CreateNamespaceImport({m_command.AggregateIdentifier.CQRSModel.Name,
                    m_command.AggregateIdentifier.Name})
                }
        End Get
    End Property

    Private m_options As ModelCodeGenerationOptions = ModelCodeGenerationOptions.DefaultOptions()
    Public Sub SetCodeGenerationOptions(options As ModelCodeGenerationOptions) Implements IEntityCodeGenerator.SetCodeGenerationOptions
        m_options = options
    End Sub

    Public Sub New(ByVal commandToGenerate As CommandDefinition)
        m_command = commandToGenerate
    End Sub
End Class