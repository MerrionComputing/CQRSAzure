Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces

''' <summary>
''' A class to perform the documentation generation for a complete CQRS model from its XML model
''' </summary>
Public Class ModelDocumentationGenerator
    Private ReadOnly m_model As CQRSModel
    Private ReadOnly m_options As ModelDocumentationGeneratorOptions
    Private ReadOnly m_docWriter As IDocumentationWriter

    ''' <summary>
    ''' The CQRS model for which we are creating the documentation
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Model As CQRSModel
        Get
            Return m_model
        End Get
    End Property


    Public ReadOnly Property Options As ModelDocumentationGeneratorOptions
        Get
            Return m_options
        End Get
    End Property

    Public ReadOnly Property DocumentationWriter As IDocumentationWriter
        Get
            Return m_docWriter
        End Get
    End Property

    Public Sub GenerateDocumentation()

        'Document the model top-level properties, including aggregates list
        Me.DocumentationWriter.CreatePage(EntityDocumentationGeneratorBase.MakeValidDocumentationFilenameBase(m_model.Name))
        'Add the model name as the heading
        Me.DocumentationWriter.WriteElement(m_model.Name, IDocumentationWriter.DocumentationLevel.Heading, "Model")
        'Add the notes and description
        EntityDocumentationGeneratorBase.WriteDescription(m_model, Me.DocumentationWriter)
        EntityDocumentationGeneratorBase.WriteNotes(m_model, Me.DocumentationWriter)

        If (m_model.AggregateIdentifiers.Count > 0) Then
            Me.DocumentationWriter.WriteElement("Aggregates", IDocumentationWriter.DocumentationLevel.SubHeading)
            For Each aggregate As AggregateIdentifier In m_model.AggregateIdentifiers
                Dim aggDoc As New AggregateIdentifierDocumentationGenerator(aggregate, Me.Options, Me.DocumentationWriter)
                Me.DocumentationWriter.WriteElement(Me.DocumentationWriter.GetPageReference(aggDoc.FilenameBase, aggregate.Name), IDocumentationWriter.DocumentationLevel.ListItem)
            Next
        End If

        For Each aggregate As AggregateIdentifier In m_model.AggregateIdentifiers

            'Document the top level aggegrate..
            Dim aggDoc As New AggregateIdentifierDocumentationGenerator(aggregate, Me.Options, Me.DocumentationWriter)
            aggDoc.Generate()

            'Document its events
            For Each eventDef As EventDefinition In aggregate.EventDefinitions
                Dim evtDoc As New EventDocumentationGenerator(eventDef, Me.Options, Me.DocumentationWriter)
                evtDoc.Generate()
            Next

            'document the query definitions
            For Each qryDef As QueryDefinition In aggregate.QueryDefinitions
                Dim qryDoc As New QueryDocumentationGenerator(qryDef, Me.Options, Me.DocumentationWriter)
                qryDoc.Generate()
            Next

            'document the command definitions
            For Each cmdDef As CommandDefinition In aggregate.CommandDefinitions
                Dim cmdDoc As New CommandDocumentationGenerator(cmdDef, Me.Options, Me.DocumentationWriter)
                cmdDoc.Generate()
            Next

            'document the projection definitions
            For Each prjDef As ProjectionDefinition In aggregate.ProjectionDefinitions
                Dim prjDoc As New ProjectionDocumentationGenerator(prjDef, Me.Options, Me.DocumentationWriter)
                prjDoc.Generate()
            Next

            'document the identity groups
            For Each idGrp As IdentityGroup In aggregate.IdentityGrouped
                Dim idGrpDoc As New IdentityGroupDocumentationGenerator(idGrp, Me.Options, Me.DocumentationWriter)
                idGrpDoc.Generate()
            Next

        Next

        'Finally save the resuling documentation
        DocumentationWriter.Save()


    End Sub

#Region "Constructors"
    Public Sub New(ByVal modelToDocument As CQRSModel,
                   Optional ByVal options As ModelDocumentationGeneratorOptions = Nothing,
                   Optional ByVal docWriter As IDocumentationWriter = Nothing)

        If (options IsNot Nothing) Then
            m_options = options
        Else
            m_options = ModelDocumentationGeneratorOptions.DefaultOptions()
        End If
        If (docWriter IsNot Nothing) Then
            m_docWriter = docWriter
        Else
            m_docWriter = New HTMLDocumentationWriter()
        End If
        m_model = modelToDocument

    End Sub
#End Region

    ''' <summary>
    ''' Returns the base filename (without any extension) used to refer to the given named domain model
    ''' </summary>
    ''' <param name="ModelName">
    ''' The domain model root
    ''' </param>
    Public Shared Function DocumentFilenameReferenceBase(ByVal ModelName As String) As String
        Return EntityDocumentationGeneratorBase.MakeValidDocumentationFilenameBase(ModelName)
    End Function
End Class
