Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.DocumentationGeneration

''' <summary>
''' A class to generate a single file document for the given identity group passed into it
''' </summary>
Public Class IdentityGroupDocumentationGenerator
    Inherits EntityDocumentationGeneratorBase
    Implements IEntityDocumentationGenerator

    Private ReadOnly m_identityGroup As IdentityGroup

    Public Overrides ReadOnly Property FilenameBase As String Implements IEntityDocumentationGenerator.FilenameBase
        Get
            If (Not m_identityGroup Is Nothing) Then
                Return MakeValidDocumentationFilenameBase(m_identityGroup.FullyQualifiedName)
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

        If (m_identityGroup IsNot Nothing) Then
            'And use it to write this event details
            writerToUse.CreatePage(Me.FilenameBase)
            writerToUse.WriteElement(Me.FilenameBase, IDocumentationWriter.DocumentationLevel.Heading, "IdentityGroup")
            writerToUse.WriteElement("(" & m_identityGroup.ParentName & ")", IDocumentationWriter.DocumentationLevel.Normal)
            WriteDescription(m_identityGroup, writerToUse)
            WriteNotes(m_identityGroup, writerToUse)
            If (m_identityGroup.QueryDefinitions.Count > 0) Then
                writerToUse.WriteElement("Used by Queries", IDocumentationWriter.DocumentationLevel.SubHeading, "References")
                For Each qry In m_identityGroup.QueryDefinitions
                    writerToUse.WriteElement(qry.Name, IDocumentationWriter.DocumentationLevel.Normal)
                Next
            End If
            'Put the navigation section at the bottom
            WriteNavigation(m_identityGroup.AggregateIdentifier.CQRSModel.Name, m_identityGroup.AggregateIdentifier.Name, writerToUse)
        End If

    End Sub

    Public Sub New(ByVal identityGroupIn As IdentityGroup,
               Optional ByVal options As IDocumentationGenerationOptions = Nothing,
               Optional ByVal documentWriter As IDocumentationWriter = Nothing)
        MyBase.New(options, documentWriter)
        m_identityGroup = identityGroupIn
    End Sub

    ''' <summary>
    ''' Returns the base filename (without any extension) used to refer to the given named identity group
    ''' </summary>
    ''' <param name="ModelName">
    ''' The domain model containing the aggregate
    ''' </param>
    ''' <param name="AggregateName">
    ''' The name of the aggregate this classifier operates on
    ''' </param>
    ''' <param name="GroupName">
    ''' The name of the identity group itself
    ''' </param>
    Public Shared Function DocumentFilenameReferenceBase(ByVal ModelName As String, ByVal AggregateName As String, ByVal GroupName As String) As String
        Return MakeValidDocumentationFilenameBase(AggregateIdentifierDocumentationGenerator.DocumentFilenameReferenceBase(ModelName, AggregateName) & "." & GroupName)
    End Function

End Class
