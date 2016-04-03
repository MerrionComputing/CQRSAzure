Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.DocumentationGeneration

''' <summary>
''' A generator to create the documentation for a single classifier
''' </summary>
Public Class ClassifierDocumentationGeneration
    Inherits EntityDocumentationGeneratorBase
    Implements IEntityDocumentationGenerator

    Private ReadOnly m_classifier As Classifier

    Public Overrides ReadOnly Property FilenameBase As String Implements IEntityDocumentationGenerator.FilenameBase
        Get
            If (Not m_classifier Is Nothing) Then
                Return MakeValidDocumentationFilenameBase(m_classifier.FullyQualifiedName)
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

        If (m_classifier IsNot Nothing) Then
            'And use it to write this classifier's details
            writerToUse.CreatePage(Me.FilenameBase)
            writerToUse.WriteElement(Me.FilenameBase, IDocumentationWriter.DocumentationLevel.Heading, "Classifier")
            WriteDescription(m_classifier, writerToUse)
            WriteNotes(m_classifier, writerToUse)
            If (m_classifier.ClassifierEventEvaluations.Count > 0) Then
                writerToUse.WriteElement("Event Evaluations", IDocumentationWriter.DocumentationLevel.SubHeading, "Properties")
                For Each prp In m_classifier.ClassifierEventEvaluations
                    writerToUse.WriteElement(prp.ToString(), IDocumentationWriter.DocumentationLevel.ListItem, "Properties")
                Next
            End If

            'Put the navigation section at the bottom
            WriteNavigation(m_classifier.AggregateIdentifier.CQRSModel.Name, m_classifier.AggregateIdentifier.Name, writerToUse)
        End If

    End Sub

    Public Sub New(ByVal classifierInstance As Classifier,
               Optional ByVal options As ModelDocumentationGeneratorOptions = Nothing,
               Optional ByVal documentWriter As IDocumentationWriter = Nothing)
        MyBase.New(options, documentWriter)
        m_classifier = classifierInstance

    End Sub


    ''' <summary>
    ''' Returns the base filename (without any extension) used to refer to the given named classifier
    ''' </summary>
    ''' <param name="ModelName">
    ''' The domain model containing the aggregate
    ''' </param>
    ''' <param name="AggregateName">
    ''' The name of the aggregate this classifier operates on
    ''' </param>
    ''' <param name="ClassifierName">
    ''' The name of the classifier itself
    ''' </param>
    Public Shared Function DocumentFilenameReferenceBase(ByVal ModelName As String, ByVal AggregateName As String, ByVal ClassifierName As String) As String
        Return MakeValidDocumentationFilenameBase(AggregateIdentifierDocumentationGenerator.DocumentFilenameReferenceBase(ModelName, AggregateName) & "." & ClassifierName)
    End Function

End Class
