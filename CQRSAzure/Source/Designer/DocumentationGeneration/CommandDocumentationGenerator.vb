Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.DocumentationGeneration

Public Class CommandDocumentationGenerator
    Inherits EntityDocumentationGeneratorBase
    Implements IEntityDocumentationGenerator

    Private ReadOnly m_cmdDef As CommandDefinition

    Public Overrides ReadOnly Property FilenameBase As String Implements IEntityDocumentationGenerator.FilenameBase
        Get
            If (Not m_cmdDef Is Nothing) Then
                Return MakeValidDocumentationFilenameBase(m_cmdDef.FullyQualifiedName)
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

        If (m_cmdDef IsNot Nothing) Then
            'And use it to write this aggregate identifier's details
            writerToUse.CreatePage(Me.FilenameBase)
            writerToUse.WriteElement(Me.FilenameBase, IDocumentationWriter.DocumentationLevel.Heading, "Command")
            WriteDescription(m_cmdDef, writerToUse)
            WriteNotes(m_cmdDef, writerToUse)
            If (m_cmdDef.CommandParameters.Count > 0) Then
                writerToUse.WriteElement("Parameters", IDocumentationWriter.DocumentationLevel.SubHeading, "Properties")
                For Each prp In m_cmdDef.CommandParameters
                    writerToUse.WriteElement(prp.Name & " (" & prp.ParameterType.ToString() & ") - " & prp.Description, IDocumentationWriter.DocumentationLevel.ListItem, "Properties")
                Next
            End If
            'Put the navigation section at the bottom
            WriteNavigation(m_cmdDef.AggregateIdentifier.CQRSModel.Name, m_cmdDef.AggregateIdentifier.Name, writerToUse)
        End If

    End Sub

    Public Sub New(ByVal cmdDef As CommandDefinition,
                   Optional ByVal options As IDocumentationGenerationOptions = Nothing,
                   Optional ByVal documentWriter As IDocumentationWriter = Nothing)
        MyBase.New(options, documentWriter)
        m_cmdDef = cmdDef

    End Sub


    ''' <summary>
    ''' Returns the base filename (without any extension) used to refer to the given named command
    ''' </summary>
    ''' <param name="ModelName">
    ''' The domain model containing the aggregate
    ''' </param>
    ''' <param name="AggregateName">
    ''' The name of the aggregate this classifier operates on
    ''' </param>
    ''' <param name="CommandName">
    ''' The name of the command itself
    ''' </param>
    Public Shared Function DocumentFilenameReferenceBase(ByVal ModelName As String, ByVal AggregateName As String, ByVal CommandName As String) As String
        Return MakeValidDocumentationFilenameBase(AggregateIdentifierDocumentationGenerator.DocumentFilenameReferenceBase(ModelName, AggregateName) & "." & CommandName)
    End Function

End Class
