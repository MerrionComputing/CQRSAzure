Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.DocumentationGeneration

Public MustInherit Class EntityDocumentationGeneratorBase
    Implements IEntityDocumentationGenerator

    Private m_docWriter As IDocumentationWriter

    Private m_options As IDocumentationGenerationOptions

    ''' <summary>
    ''' The filename base depends on what type of entity we are processing...
    ''' </summary>
    Public MustOverride ReadOnly Property FilenameBase As String Implements IEntityDocumentationGenerator.FilenameBase

    ''' <summary>
    ''' Generate the documentation for this entity
    ''' </summary>
    ''' <param name="docWriter">
    ''' </param>
    Public MustOverride Sub Generate(Optional docWriter As IDocumentationWriter = Nothing) Implements IEntityDocumentationGenerator.Generate

    Public Sub SetCodeDocumentationOptions(options As IDocumentationGenerationOptions) Implements IEntityDocumentationGenerator.SetCodeDocumentationOptions
        m_options = options
    End Sub

    Public ReadOnly Property DocumentWriter As IDocumentationWriter
        Get
            Return m_docWriter
        End Get
    End Property

    Public Sub New(Optional ByVal options As IDocumentationGenerationOptions = Nothing,
                   Optional ByVal documentWriter As IDocumentationWriter = Nothing)
        If (options IsNot Nothing) Then
            m_options = options
        End If
        If (documentWriter IsNot Nothing) Then
            m_docWriter = documentWriter
        Else
            m_docWriter = New HTMLDocumentationWriter()
        End If
    End Sub

    ''' <summary>
    ''' Strip out any illegal characters from the entity name for use in it's documentation filename
    ''' </summary>
    ''' <param name="entityName">
    ''' The name of the code entity to write to a documentation file
    ''' </param>
    ''' <returns></returns>
    Public Shared Function MakeValidDocumentationFilenameBase(ByVal entityName As String) As String

        'For now, just strip spaces
        Return String.Join("_", entityName.Split(System.IO.Path.GetInvalidFileNameChars()))

    End Function

    ''' <summary>
    ''' Write the description for the given entity to the document if it is not blank
    ''' </summary>
    ''' <param name="entity">
    ''' The thing we are documenting
    ''' </param>
    ''' <param name="writer">
    ''' The document writer we are documenting it on
    ''' </param>
    Public Shared Sub WriteDescription(ByVal entity As IDocumentedEntity, ByVal writer As IDocumentationWriter)

        If (entity IsNot Nothing) Then
            If (writer IsNot Nothing) Then
                If Not (String.IsNullOrEmpty(entity.Description)) Then
                    writer.WriteElement("Description", IDocumentationWriter.DocumentationLevel.SubHeading, "Description")
                    writer.WriteElement(entity.Description, IDocumentationWriter.DocumentationLevel.Normal)
                End If
            End If
        End If

    End Sub


    ''' <summary>
    ''' Write the notes for the given entity to the document if it is not blank
    ''' </summary>
    ''' <param name="entity">
    ''' The thing we are documenting
    ''' </param>
    ''' <param name="writer">
    ''' The document writer we are documenting it on
    ''' </param>
    Public Shared Sub WriteNotes(ByVal entity As IDocumentedEntity, ByVal writer As IDocumentationWriter)

        If (entity IsNot Nothing) Then
            If (writer IsNot Nothing) Then
                If Not (String.IsNullOrEmpty(entity.Notes)) Then
                    writer.WriteElement("Notes", IDocumentationWriter.DocumentationLevel.SubHeading, "Notes")
                    writer.WriteElement(entity.Notes, IDocumentationWriter.DocumentationLevel.Normal)
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' Write the navigation links to the model and aggregate for this entity
    ''' </summary>
    ''' <param name="modelName">
    ''' The name of the domain model this entity belongs to
    ''' </param>
    ''' <param name="aggregateIdentifierName">
    ''' The name of the aggregate identifier that this entity is linked to
    ''' </param>
    ''' <param name="writer">
    ''' The writer class that is to create this documentation
    ''' </param>
    Public Shared Sub WriteNavigation(ByVal modelName As String, ByVal aggregateIdentifierName As String, ByVal writer As IDocumentationWriter)

        If (writer IsNot Nothing) Then
            writer.WriteElement("Links", IDocumentationWriter.DocumentationLevel.SubHeading, "Notes")
            If (Not String.IsNullOrWhiteSpace(modelName)) Then
                writer.WriteElement("Model : " & writer.GetPageReference(ModelDocumentationGenerator.DocumentFilenameReferenceBase(modelName), modelName),
                                    IDocumentationWriter.DocumentationLevel.Normal)
            End If
            If (Not String.IsNullOrWhiteSpace(aggregateIdentifierName)) Then
                writer.WriteElement("Aggregate : " & writer.GetPageReference(AggregateIdentifierDocumentationGenerator.DocumentFilenameReferenceBase(modelName, aggregateIdentifierName), aggregateIdentifierName),
                                    IDocumentationWriter.DocumentationLevel.Normal)
            End If
        End If

    End Sub

End Class
