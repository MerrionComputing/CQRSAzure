Imports System.CodeDom
Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces

Public Class IdentityGroupCodeGenerator
    Implements IEntityCodeGenerator

    Public Const IDENTITY_GROUP_FILENAME_IDENTIFIER = "identitygroup"

    Private ReadOnly m_identitygroup As IdentityGroup

    Public ReadOnly Property FilenameBase As String Implements IEntityCodeGenerator.FilenameBase
        Get
            Return ModelCodeGenerator.MakeValidCodeFilenameBase(m_identitygroup.Name) & "." & IDENTITY_GROUP_FILENAME_IDENTIFIER
        End Get
    End Property


    Public Function GenerateInterface() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateInterface
        'add the imports namespace
        Dim identityGroupInterfaceRet As New CodeCompileUnit()

        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next
        identityGroupInterfaceRet.Namespaces.Add(aggregateNamespace)
        If (Not String.IsNullOrWhiteSpace(m_identitygroup.AggregateIdentifier.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_identitygroup.AggregateIdentifier.Notes}))
        End If

        ' Add the interface declaration (partial)
        Dim interfaceDeclaration As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration(m_identitygroup.Name)
        ' Comment the interface declaration
        If (Not String.IsNullOrWhiteSpace(m_identitygroup.Description)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_identitygroup.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_identitygroup.Notes)) Then
            interfaceDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_identitygroup.Notes}))
        End If

        aggregateNamespace.Types.Add(interfaceDeclaration)

        Return identityGroupInterfaceRet

    End Function

    Public Function GenerateImplementation() As CodeCompileUnit Implements IEntityCodeGenerator.GenerateImplementation

        'add the imports namespace
        Dim identityGroupClasseRet As New CodeCompileUnit()
        'put the namespace hierarchy in...model name..


        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next
        identityGroupClasseRet.Namespaces.Add(aggregateNamespace)
        If (Not String.IsNullOrWhiteSpace(m_identitygroup.AggregateIdentifier.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_identitygroup.AggregateIdentifier.Notes}))
        End If

        ' Add the class declaration (partial)
        Dim classDeclaration As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration(m_identitygroup.Name)
        If (Not String.IsNullOrWhiteSpace(m_identitygroup.Description)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_identitygroup.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_identitygroup.Notes)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_identitygroup.Notes}))
        End If
        'Make the class implement the interface
        classDeclaration.BaseTypes.Add(New CodeTypeReference(GetType(Object)))
        classDeclaration.BaseTypes.Add(InterfaceCodeGeneration.ImplementsInterfaceReference(m_identitygroup.Name))

        'Add the model name (DomainNameAttribute)
        If Not String.IsNullOrWhiteSpace(m_identitygroup.AggregateIdentifier.CQRSModel.Name) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument(New CodePrimitiveExpression(m_identitygroup.AggregateIdentifier.CQRSModel.Name)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.DomainNameAttribute",
                                                              params))
        End If

        'Add the category attribute
        If Not String.IsNullOrWhiteSpace(m_identitygroup.Category) Then
            Dim params As New List(Of CodeAttributeArgument)
            params.Add(New CodeAttributeArgument(New CodePrimitiveExpression(m_identitygroup.Category)))
            classDeclaration.CustomAttributes.Add(
                AttributeCodeGenerator.ParameterisedAttribute("CQRSAzure.EventSourcing.Category",
                                                              params))
        End If

        '3) add constructors
        Dim emptyConstructor As New CodeConstructor()
        emptyConstructor.Attributes += MemberAttributes.Public
        emptyConstructor.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Empty constructor for serialisation",
                                                                                   "This should be removed if serialisation is not needed"}))
        classDeclaration.Members.Add(emptyConstructor)

        'put the built class into the namespace
        aggregateNamespace.Types.Add(classDeclaration)

        Return identityGroupClasseRet

    End Function

    Public ReadOnly Property NamespaceHierarchy As IList(Of String) Implements IEntityCodeGenerator.NamespaceHierarchy
        Get
            If (m_identitygroup IsNot Nothing) Then
                Return {m_identitygroup.AggregateIdentifier.CQRSModel.Name,
                    m_identitygroup.AggregateIdentifier.Name,
                    IDENTITY_GROUP_FILENAME_IDENTIFIER}
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
                CodeGenerationUtilities.CreateNamespaceImport({m_identitygroup.AggregateIdentifier.CQRSModel.Name,
                    m_identitygroup.AggregateIdentifier.Name})
                }
        End Get
    End Property

    Private m_options As IModelCodeGenerationOptions = ModelCodeGenerationOptions.Default()
    Public Sub SetCodeGenerationOptions(options As IModelCodeGenerationOptions) Implements IEntityCodeGenerator.SetCodeGenerationOptions
        m_options = options
    End Sub


    Public Sub New(ByVal identityGroupToGenerate As IdentityGroup)
        m_identitygroup = identityGroupToGenerate
    End Sub

End Class
