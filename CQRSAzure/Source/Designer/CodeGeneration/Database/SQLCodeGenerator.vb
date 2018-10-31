Imports System.CodeDom
Imports System.Runtime.Serialization
Imports CQRSAzure.CQRSdsl.CodeGeneration
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.Dsl

''' <summary>
''' Code generator to create the DBContext derived classes required to implement the event streams for this CQRS model using Entity Framework
''' code-first 
''' </summary>
Public Class SQLCodeGenerator
    Implements IEntityImplementationCodeGenerator

    Public Const EVENT_FILENAME_IDENTIFIER = "dbContext"
    Private ReadOnly m_model As CQRSModel


    Public ReadOnly Property FilenameBase As String Implements IEntityCodeGeneratorBase.FilenameBase
        Get
            Return ModelCodeGenerator.MakeValidCodeFilenameBase(m_model.Name) & "." & EVENT_FILENAME_IDENTIFIER
        End Get
    End Property

    Public ReadOnly Property NamespaceHierarchy As IList(Of String) Implements IEntityCodeGeneratorBase.NamespaceHierarchy
        Get
            If (m_model IsNot Nothing) Then
                Return {m_model.Name,
                    EVENT_FILENAME_IDENTIFIER}
            Else
                Return {}
            End If
        End Get
    End Property

    Public ReadOnly Property RequiredNamespaces As IEnumerable(Of CodeNamespaceImport) Implements IEntityCodeGeneratorBase.RequiredNamespaces
        Get
            Return {
                New CodeNamespaceImport("System.Data.Entity"), 'For DbContext
                New CodeNamespaceImport("CQRSAzure.EventSourcing")
                }
        End Get
    End Property

    Private m_options As IModelCodeGenerationOptions = ModelCodeGenerationOptions.Default()
    Public Sub SetCodeGenerationOptions(options As IModelCodeGenerationOptions) Implements IEntityCodeGeneratorBase.SetCodeGenerationOptions
        m_options = options
    End Sub

    Public Function GenerateImplementation() As CodeCompileUnit Implements IEntityImplementationCodeGenerator.GenerateImplementation

        'add the imports namespace
        Dim dbContextClassRet As New CodeCompileUnit()


        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next
        dbContextClassRet.Namespaces.Add(aggregateNamespace)

        Dim classDeclaration As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration(Me.m_model.Name & "DBContext")
        If (Not String.IsNullOrWhiteSpace(m_model.Description)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_model.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_model.Notes)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_model.Notes}))
        End If
        'Make it inherit DbContext
        classDeclaration.BaseTypes.Add(New CodeTypeReference("DbContext"))


        'Add each aggregate in turn....
        For Each aggregateId As AggregateIdentifier In m_model.AggregateIdentifiers

            'Add a DbSet<{AggregateEventStream}> {aggregateName}+Instances {get;set;} property
            classDeclaration.Members.Add(EntityFrameworkCodeGeneration.CreateDBSetProperty(aggregateId.Name))

        Next

        'put the built class into the namespace
        aggregateNamespace.Types.Add(classDeclaration)

        Return dbContextClassRet

    End Function

    ''' <summary>
    ''' Create a new SQL code generator to create the DBContext code for this model
    ''' </summary>
    ''' <param name="modelToGenerate">
    ''' The CQRS model to generate the code for
    ''' </param>
    Public Sub New(ByVal modelToGenerate As CQRSModel)
        m_model = modelToGenerate
    End Sub

End Class




