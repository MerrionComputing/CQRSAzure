Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.DocumentationGeneration

''' <summary>
''' A class to generate a single html documentation file for the given aggregate identifier
''' </summary>
Public Class AggregateIdentifierDocumentationGenerator
    Inherits EntityDocumentationGeneratorBase
    Implements IEntityDocumentationGenerator

    Private ReadOnly m_aggregate As AggregateIdentifier

    Public Overrides ReadOnly Property FilenameBase As String Implements IEntityDocumentationGenerator.FilenameBase
        Get
            If (m_aggregate IsNot Nothing) Then
                Return MakeValidDocumentationFilenameBase(m_aggregate.FullyQualifiedName)
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

        'And use it to write this aggregate identifier's details
        If Not (m_aggregate Is Nothing) Then
            writerToUse.CreatePage(Me.FilenameBase)
            writerToUse.WriteElement(Me.FilenameBase, IDocumentationWriter.DocumentationLevel.Heading, "Aggregate")
            WriteDescription(m_aggregate, writerToUse)
            WriteNotes(m_aggregate, writerToUse)
            If (m_aggregate.IdentityGrouped.Count > 0) Then
                writerToUse.WriteElement("Identity Groups", IDocumentationWriter.DocumentationLevel.SubHeading, "IdentityGroup")
                'List the identity groups..
                For Each idGroup In m_aggregate.IdentityGrouped
                    'Turn this into a document reference with name and description
                    writerToUse.WriteElement(writerToUse.GetPageReference(IdentityGroupDocumentationGenerator.DocumentFilenameReferenceBase(idGroup.AggregateIdentifier.CQRSModel.Name, idGroup.AggregateIdentifier.Name, idGroup.Name),
                                                                        idGroup.Name & " - " & idGroup.Description),
                                             IDocumentationWriter.DocumentationLevel.ListItem)
                Next
            End If
            If (m_aggregate.Classifiers.Count > 0) Then
                writerToUse.WriteElement("Classifiers", IDocumentationWriter.DocumentationLevel.SubHeading, "Classifier")
                'List the identity groups..
                For Each classifierInst In m_aggregate.Classifiers
                    'Turn this into a document reference with name and description
                    writerToUse.WriteElement(writerToUse.GetPageReference(ClassifierDocumentationGeneration.DocumentFilenameReferenceBase(classifierInst.AggregateIdentifier.CQRSModel.Name, classifierInst.AggregateIdentifier.Name, classifierInst.Name),
                                                                        classifierInst.Name & " - " & classifierInst.Description),
                                             IDocumentationWriter.DocumentationLevel.ListItem)
                Next
            End If
            If (m_aggregate.EventDefinitions.Count() > 0) Then
                writerToUse.WriteElement("Events", IDocumentationWriter.DocumentationLevel.SubHeading, "Event")
                'List the events
                For Each evt In m_aggregate.EventDefinitions
                    'Turn this into a document reference with name and description
                    writerToUse.WriteElement(writerToUse.GetPageReference(EventDocumentationGenerator.DocumentFilenameReferenceBase(evt.AggregateIdentifier.CQRSModel.Name, evt.AggregateIdentifier.Name, evt.Name),
                                                                        evt.Name & " - " & evt.Description),
                                             IDocumentationWriter.DocumentationLevel.ListItem)
                Next
            End If
            If (m_aggregate.CommandDefinitions.Count() > 0) Then
                writerToUse.WriteElement("Commands", IDocumentationWriter.DocumentationLevel.SubHeading, "Command")
                'List the commands
                For Each cmd In m_aggregate.CommandDefinitions
                    'Turn this into a document reference with name and description
                    writerToUse.WriteElement(writerToUse.GetPageReference(CommandDocumentationGenerator.DocumentFilenameReferenceBase(cmd.AggregateIdentifier.CQRSModel.Name, cmd.AggregateIdentifier.Name, cmd.Name),
                                                                        cmd.Name & " - " & cmd.Description),
                                             IDocumentationWriter.DocumentationLevel.ListItem)
                Next
            End If
            If (m_aggregate.QueryDefinitions.Count() > 0) Then
                writerToUse.WriteElement("Queries", IDocumentationWriter.DocumentationLevel.SubHeading, "Query")
                'List the queries
                For Each qry In m_aggregate.QueryDefinitions
                    'Turn this into a document reference with name and description
                    writerToUse.WriteElement(writerToUse.GetPageReference(QueryDocumentationGenerator.DocumentFilenameReferenceBase(qry.AggregateIdentifier.CQRSModel.Name, qry.AggregateIdentifier.Name, qry.Name),
                                                                        qry.Name & " - " & qry.Description),
                                             IDocumentationWriter.DocumentationLevel.ListItem)
                Next
            End If
            If (m_aggregate.ProjectionDefinitions.Count() > 0) Then
                writerToUse.WriteElement("Projections", IDocumentationWriter.DocumentationLevel.SubHeading, "Projection")
                'List the projections
                For Each prj In m_aggregate.ProjectionDefinitions
                    'Turn this into a document reference with name and description
                    writerToUse.WriteElement(writerToUse.GetPageReference(ProjectionDocumentationGenerator.DocumentFilenameReferenceBase(prj.AggregateIdentifier.CQRSModel.Name, prj.AggregateIdentifier.Name, prj.Name),
                                                                        prj.Name & " - " & prj.Description),
                                             IDocumentationWriter.DocumentationLevel.ListItem)
                Next
            End If
        End If

        'Put the navigation section at the bottom
        WriteNavigation(m_aggregate.CQRSModel.Name, m_aggregate.Name, writerToUse)

    End Sub

    Public Sub New(ByVal aggregate As AggregateIdentifier,
                   Optional ByVal options As IDocumentationGenerationOptions = Nothing,
                   Optional ByVal documentWriter As IDocumentationWriter = Nothing)
        MyBase.New(options, documentWriter)
        m_aggregate = aggregate

    End Sub

    ''' <summary>
    ''' Returns the base filename (without any extension) used to refer to the given named aggregate
    ''' </summary>
    ''' <param name="ModelName">
    ''' The domain model containing the aggregate
    ''' </param>
    ''' <param name="AggregateName">
    ''' The name of the aggregate
    ''' </param>
    Public Shared Function DocumentFilenameReferenceBase(ByVal ModelName As String, ByVal AggregateName As String) As String
        Return MakeValidDocumentationFilenameBase(ModelDocumentationGenerator.DocumentFilenameReferenceBase(ModelName) & "." & AggregateName)
    End Function

End Class
