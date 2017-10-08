Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.DocumentationGeneration

Public Class QueryDocumentationGenerator
    Inherits EntityDocumentationGeneratorBase
    Implements IEntityDocumentationGenerator

    Private ReadOnly m_qryDef As QueryDefinition

    Public Overrides ReadOnly Property FilenameBase As String Implements IEntityDocumentationGenerator.FilenameBase
        Get
            If (Not m_qryDef Is Nothing) Then
                Return MakeValidDocumentationFilenameBase(m_qryDef.FullyQualifiedName)
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

        If (m_qryDef IsNot Nothing) Then
            'And use it to write this aggregate identifier's details
            writerToUse.CreatePage(Me.FilenameBase)
            writerToUse.WriteElement(Me.FilenameBase, IDocumentationWriter.DocumentationLevel.Heading, "Query")
            WriteDescription(m_qryDef, writerToUse)
            WriteNotes(m_qryDef, writerToUse)
            If (m_qryDef.QueryInputParameters.Count > 0) Then
                writerToUse.WriteElement("Input Parameters", IDocumentationWriter.DocumentationLevel.SubHeading, "Properties")
                For Each prp In m_qryDef.QueryInputParameters
                    writerToUse.WriteElement(prp.Name & " (" & prp.DataType.ToString() & ") - " & prp.Description, IDocumentationWriter.DocumentationLevel.Normal, "Properties")
                Next
            End If
            If (m_qryDef.QueryReturnParameters.Count > 0) Then
                writerToUse.WriteElement("Return Parameters", IDocumentationWriter.DocumentationLevel.SubHeading, "Properties")
                For Each prp In m_qryDef.QueryReturnParameters
                    writerToUse.WriteElement(prp.Name & " (" & prp.DataType.ToString() & ") - " & prp.Description, IDocumentationWriter.DocumentationLevel.Normal, "Properties")
                Next
            End If
            If (m_qryDef.IdentityGroup IsNot Nothing) Then
                writerToUse.WriteElement("Default identity Group", IDocumentationWriter.DocumentationLevel.SubHeading, "References")
                writerToUse.WriteElement(m_qryDef.IdentityGroup.Name & " - " & m_qryDef.IdentityGroup.Description, IDocumentationWriter.DocumentationLevel.Normal, "Properties")
            End If
            'Put the navigation section at the bottom
            WriteNavigation(m_qryDef.AggregateIdentifier.CQRSModel.Name, m_qryDef.AggregateIdentifier.Name, writerToUse)
        End If

    End Sub

    Public Sub New(ByVal qryDef As QueryDefinition,
                   Optional ByVal options As IDocumentationGenerationOptions = Nothing,
                   Optional ByVal documentWriter As IDocumentationWriter = Nothing)
        MyBase.New(options, documentWriter)
        m_qryDef = qryDef

    End Sub

    ''' <summary>
    ''' Returns the base filename (without any extension) used to refer to the given named query
    ''' </summary>
    ''' <param name="ModelName">
    ''' The domain model containing the aggregate
    ''' </param>
    ''' <param name="AggregateName">
    ''' The name of the aggregate this classifier operates on
    ''' </param>
    ''' <param name="QueryName">
    ''' The name of the query itself
    ''' </param>
    Public Shared Function DocumentFilenameReferenceBase(ByVal ModelName As String, ByVal AggregateName As String, ByVal QueryName As String) As String
        Return MakeValidDocumentationFilenameBase(AggregateIdentifierDocumentationGenerator.DocumentFilenameReferenceBase(ModelName, AggregateName) & "." & QueryName)
    End Function

End Class
