Imports System.IO
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
''' <summary>
''' Options controlling how documentation is generated for this model
''' </summary>
Public Class ModelDocumentationGeneratorOptions
    Implements IDocumentationGenerationOptions

    ''' <summary>
    ''' The root directory to put the documentation files into
    ''' </summary>
    Private ReadOnly m_DirectoryRoot As System.IO.DirectoryInfo

    Public Sub New(Optional ByVal directoryRootIn As DirectoryInfo = Nothing)

        If (directoryRootIn IsNot Nothing) Then
            m_DirectoryRoot = directoryRootIn
        Else
            m_DirectoryRoot = New System.IO.DirectoryInfo(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Documentation"))
        End If

    End Sub

    Private ReadOnly Property DirectoryRoot As DirectoryInfo Implements IDocumentationGenerationOptions.DirectoryRoot
        Get
            Return m_DirectoryRoot
        End Get
    End Property

    Public Shared Function DefaultOptions() As IDocumentationGenerationOptions
        Return New ModelDocumentationGeneratorOptions()
    End Function

End Class
