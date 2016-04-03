''' <summary>
''' Options controlling how documentation is generated for this model
''' </summary>
Public Class ModelDocumentationGeneratorOptions

    ''' <summary>
    ''' The root directory to put the documentation files into
    ''' </summary>
    Public DirectoryRoot As System.IO.DirectoryInfo

    Public Sub New()

    End Sub

    Public Shared Function DefaultOptions() As ModelDocumentationGeneratorOptions
        Return New ModelDocumentationGeneratorOptions() With {.DirectoryRoot = New System.IO.DirectoryInfo(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Documentation"))}
    End Function

End Class
