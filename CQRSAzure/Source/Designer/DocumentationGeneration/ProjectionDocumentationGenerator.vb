Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.DocumentationGeneration

Public Class ProjectionDocumentationGenerator
    Inherits EntityDocumentationGeneratorBase
    Implements IEntityDocumentationGenerator

    Private ReadOnly m_prjDef As ProjectionDefinition

    Public Overrides ReadOnly Property FilenameBase As String Implements IEntityDocumentationGenerator.FilenameBase
        Get
            If (Not m_prjDef Is Nothing) Then
                Return MakeValidDocumentationFilenameBase(m_prjDef.FullyQualifiedName)
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

        If (m_prjDef IsNot Nothing) Then
            'And use it to write this aggregate identifier's details
            writerToUse.CreatePage(Me.FilenameBase)
            writerToUse.WriteElement(Me.FilenameBase, IDocumentationWriter.DocumentationLevel.Heading, "Projection")
            WriteDescription(m_prjDef, writerToUse)
            WriteNotes(m_prjDef, writerToUse)
            'List the properties of the projection
            If (m_prjDef.ProjectionProperties.Count > 0) Then
                writerToUse.WriteElement("Properties", IDocumentationWriter.DocumentationLevel.SubHeading, "Properties")
                For Each prp In m_prjDef.ProjectionProperties
                    writerToUse.WriteElement(prp.Name & " (" & prp.DataType.ToString() & ") - " & prp.Description, IDocumentationWriter.DocumentationLevel.Normal, "Properties")
                Next
            End If
            ' List all the events handled by the projection
            If (m_prjDef.EventDefinitions.Count > 0) Then
                writerToUse.WriteElement("Events handled", IDocumentationWriter.DocumentationLevel.SubHeading, "Events")
                For Each evt In m_prjDef.EventDefinitions
                    writerToUse.WriteElement(writerToUse.GetPageReference(EventDocumentationGenerator.DocumentFilenameReferenceBase(evt.AggregateIdentifier.CQRSModel.Name,
                                                                                                                                    evt.AggregateIdentifier.Name,
                                                                                                                                    evt.Name),
                                                                          evt.Name & " - " & evt.Description),
                                             IDocumentationWriter.DocumentationLevel.Normal,
                                             "Events")
                    'List all the property operations for that event
                    For Each po In m_prjDef.ProjectionEventPropertyOperations
                        If (po.EventName = evt.Name) Then
                            writerToUse.WriteElement(po.ToString(), IDocumentationWriter.DocumentationLevel.ListItem)
                        End If
                    Next
                Next
            End If
            'Put the navigation section at the bottom
            WriteNavigation(m_prjDef.AggregateIdentifier.CQRSModel.Name, m_prjDef.AggregateIdentifier.Name, writerToUse)
        End If

    End Sub

    Public Sub New(ByVal prjDef As ProjectionDefinition,
                   Optional ByVal options As IDocumentationGenerationOptions = Nothing,
                   Optional ByVal documentWriter As IDocumentationWriter = Nothing)
        MyBase.New(options, documentWriter)
        m_prjDef = prjDef

    End Sub

    ''' <summary>
    ''' Returns the base filename (without any extension) used to refer to the given named projection
    ''' </summary>
    ''' <param name="ModelName">
    ''' The domain model containing the aggregate
    ''' </param>
    ''' <param name="AggregateName">
    ''' The name of the aggregate this classifier operates on
    ''' </param>
    ''' <param name="ProjectionName">
    ''' The name of the projectionitself
    ''' </param>
    Public Shared Function DocumentFilenameReferenceBase(ByVal ModelName As String, ByVal AggregateName As String, ByVal ProjectionName As String) As String
        Return MakeValidDocumentationFilenameBase(AggregateIdentifierDocumentationGenerator.DocumentFilenameReferenceBase(ModelName, AggregateName) & "." & ProjectionName)
    End Function

End Class
