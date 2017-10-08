Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.DocumentationGeneration

Public Class EventDocumentationGenerator
    Inherits EntityDocumentationGeneratorBase
    Implements IEntityDocumentationGenerator

    Private ReadOnly m_event As EventDefinition


    Public Overrides ReadOnly Property FilenameBase As String Implements IEntityDocumentationGenerator.FilenameBase
        Get
            If (Not m_event Is Nothing) Then
                Return MakeValidDocumentationFilenameBase(m_event.FullyQualifiedName)
            Else
                Return String.Empty
            End If
        End Get
    End Property

    Public Overrides Sub Generate(Optional docWriter As IDocumentationWriter = Nothing)

        'Get the documentation writer to use
        Dim writerToUse As IDocumentationWriter
        If (docWriter IsNot Nothing) Then
            writerToUse = docWriter
        Else
            writerToUse = MyBase.DocumentWriter
        End If

        If (m_event IsNot Nothing) Then
            'And use it to write this event details
            writerToUse.CreatePage(Me.FilenameBase)
            writerToUse.WriteElement(Me.FilenameBase, IDocumentationWriter.DocumentationLevel.Heading, "Event")
            WriteDescription(m_event, writerToUse)
            WriteNotes(m_event, writerToUse)
            'List the properties of the event
            If (m_event.EventProperties.Count > 0) Then
                writerToUse.WriteElement("Properties", IDocumentationWriter.DocumentationLevel.SubHeading, "Properties")
                For Each prp In m_event.EventProperties
                    writerToUse.WriteElement(prp.Name & " (" & prp.DataType.ToString() & ") - " & prp.Description, IDocumentationWriter.DocumentationLevel.Normal, "Properties")
                Next
            End If
            If (m_event.ProjectionDefinitions.Count > 0) Then
                writerToUse.WriteElement("Used by Projections", IDocumentationWriter.DocumentationLevel.SubHeading, "References")
                For Each prj In m_event.ProjectionDefinitions
                    writerToUse.WriteElement(writerToUse.GetPageReference(
                                             ProjectionDocumentationGenerator.DocumentFilenameReferenceBase(prj.AggregateIdentifier.CQRSModel.Name, prj.AggregateIdentifier.Name, prj.Name), prj.Name),
                                             IDocumentationWriter.DocumentationLevel.Normal)
                Next
            End If
            'Put the navigation section at the bottom
            WriteNavigation(m_event.AggregateIdentifier.CQRSModel.Name, m_event.AggregateIdentifier.Name, writerToUse)
        End If

    End Sub

    Public Sub New(ByVal eventDef As EventDefinition,
                   Optional ByVal options As IDocumentationGenerationOptions = Nothing,
                   Optional ByVal documentWriter As IDocumentationWriter = Nothing)
        MyBase.New(options, documentWriter)
        m_event = eventDef
    End Sub

    ''' <summary>
    ''' Returns the base filename (without any extension) used to refer to the given named event
    ''' </summary>
    ''' <param name="ModelName">
    ''' The domain model containing the aggregate
    ''' </param>
    ''' <param name="AggregateName">
    ''' The name of the aggregate this classifier operates on
    ''' </param>
    ''' <param name="EventName">
    ''' The name of the event itself
    ''' </param>
    Public Shared Function DocumentFilenameReferenceBase(ByVal ModelName As String, ByVal AggregateName As String, ByVal EventName As String) As String
        Return MakeValidDocumentationFilenameBase(AggregateIdentifierDocumentationGenerator.DocumentFilenameReferenceBase(ModelName, AggregateName) & "." & EventName)
    End Function

End Class
