
''' <summary>
''' Immutable class representing a single element in a documentation page/set
''' </summary>
Public Class DocumentElement

    Private ReadOnly m_level As IDocumentationWriter.DocumentationLevel
    Public ReadOnly Property Level As IDocumentationWriter.DocumentationLevel
        Get
            Return m_level
        End Get
    End Property


    Private ReadOnly m_content As String
    Public ReadOnly Property Content As String
        Get
            Return m_content
        End Get
    End Property

    Private ReadOnly m_tag As String
    Public ReadOnly Property Tag As String
        Get
            Return m_tag
        End Get
    End Property

    Public Sub New(contentIn As String,
                   levelIn As IDocumentationWriter.DocumentationLevel,
                   Optional tagIn As String = "")
        m_content = contentIn
        m_level = levelIn
        m_tag = tagIn
    End Sub

End Class
