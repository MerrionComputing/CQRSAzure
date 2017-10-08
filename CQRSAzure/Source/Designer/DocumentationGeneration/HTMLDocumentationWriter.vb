Imports System.IO
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.DocumentationGeneration

''' <summary>
''' Class to output documentation as a set of HTML files
''' </summary>
Public Class HTMLDocumentationWriter
    Implements IDocumentationWriter

    Public Const HTML_FILE_SUFFIX As String = ".html"
    Public Const CSS_FILE As String = "Resources/CQRSDocumentation.css"

    Private m_pages As New Dictionary(Of String, HTMLDocumentationPage)

    Private m_currentPage As String

    Private m_docOptions As IDocumentationGenerationOptions = Nothing

    Public Property CurrentPage As String Implements IDocumentationWriter.CurrentPage
        Get
            Return m_currentPage
        End Get
        Set(value As String)
            If Not String.IsNullOrEmpty(value) Then
                If Not m_pages.ContainsKey(value) Then
                    CreatePage(value)
                End If
            End If
            m_currentPage = value
        End Set
    End Property

    Public Sub CreatePage(pageName As String) Implements IDocumentationWriter.CreatePage
        If Not m_pages.ContainsKey(pageName) Then
            m_pages.Add(pageName, New HTMLDocumentationPage(pageName))
            'make this newly created page the current page
            CurrentPage = pageName
        End If
    End Sub



    Public Sub WriteElement(content As String, level As IDocumentationWriter.DocumentationLevel, Optional ByVal tag As String = "") Implements IDocumentationWriter.WriteElement

        If Not String.IsNullOrEmpty(CurrentPage) Then
            If (m_pages.ContainsKey(CurrentPage)) Then
                m_pages(CurrentPage).WriteElement(content, level, tag)
            End If
        End If

    End Sub

    Public Function GetPageReference(pageName As String, description As String) As String Implements IDocumentationWriter.GetPageReference

        If (String.IsNullOrEmpty(description)) Then
            Return "<a href=""" & MakeHTMLFilename(pageName) & """ >" & pageName & "</a>"
        Else
            Return "<a href=""" & MakeHTMLFilename(pageName) & """ >" & description & "</a>"
        End If

    End Function

    Public Sub Save() Implements IDocumentationWriter.Save

        CreateResources()

        For Each pgToSave As HTMLDocumentationPage In m_pages.Values
            'pass in the project global values
            'and save the page
            pgToSave.Save(m_docOptions)
        Next
    End Sub

    ''' <summary>
    ''' Create the "Resources" folder for this set of documentation and copy the CSS and images to it
    ''' </summary>
    Private Sub CreateResources()

        If Not m_docOptions.DirectoryRoot.Exists Then
            m_docOptions.DirectoryRoot.Create()
        End If
        Dim resourcesDirectory As DirectoryInfo = New DirectoryInfo(IO.Path.Combine(m_docOptions.DirectoryRoot.FullName, "Resources"))
        If Not resourcesDirectory.Exists Then
            resourcesDirectory.Create()
        End If

        'Copy the resources into that target directory
        For Each resourceFile As String In Me.GetType().Assembly.GetManifestResourceNames()

            If (resourceFile.EndsWith("png", StringComparison.OrdinalIgnoreCase) OrElse resourceFile.EndsWith("css", StringComparison.OrdinalIgnoreCase)) Then
                'copy the file to the resources directory
                Using fileOutput As Stream = File.Create(Path.Combine(resourcesDirectory.FullName, StripAssemblyReference(resourceFile)))
                    Using src As Stream = Me.GetType.Assembly.GetManifestResourceStream(resourceFile)
                        src.CopyTo(fileOutput)
                    End Using
                End Using
            End If

        Next


    End Sub

    ''' <summary>
    ''' Remove the assembly name from this filename 
    ''' </summary>
    ''' <param name="resourceFile"></param>
    ''' <returns></returns>
    Private Function StripAssemblyReference(resourceFile As String) As String

        Return resourceFile.Replace("CQRSAzure.CQRSdsl.DocumentationGeneration.", "")

    End Function

    ''' <summary>
    ''' Turns a page name into a valid HTML filename to save the file as
    ''' </summary>
    ''' <param name="filenameBase"></param>
    ''' <returns></returns>
    Public Shared Function MakeHTMLFilename(ByVal filenameBase As String) As String

        Return EntityDocumentationGeneratorBase.MakeValidDocumentationFilenameBase(filenameBase) & HTMLDocumentationWriter.HTML_FILE_SUFFIX

    End Function

    Public Sub New(Optional ByVal options As IDocumentationGenerationOptions = Nothing)

        If (options IsNot Nothing) Then
            m_docOptions = options
        Else
            m_docOptions = ModelDocumentationGeneratorOptions.DefaultOptions()
        End If

    End Sub

End Class

Public Class HTMLDocumentationPage

    Private ReadOnly m_pageName As String

    Private m_elements As New List(Of DocumentElement)

    Private m_globalElements As New Dictionary(Of String, String)

    ''' <summary>
    ''' The unique name of the page
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property PageName As String
        Get
            Return m_pageName
        End Get
    End Property

    Public Sub WriteElement(content As String, level As IDocumentationWriter.DocumentationLevel, Optional ByVal tag As String = "")
        m_elements.Add(New DocumentElement(content, level, tag))
    End Sub


    Public Sub AddOrUpdateSetting(ByVal settingName As String, ByVal settingValue As String)

        If (Not String.IsNullOrWhiteSpace(settingName)) Then

            If (m_globalElements.ContainsKey(settingName)) Then
                m_globalElements(settingName) = settingValue
            Else
                m_globalElements.Add(settingName, settingValue)
            End If

        End If

    End Sub

    ''' <summary>
    ''' Save this file
    ''' </summary>
    Public Sub Save(Optional docOptions As IDocumentationGenerationOptions = Nothing)

        AddOrUpdateSetting("Documentation-PageName", PageName)

        'Create the documentation directory if it does not exists
        If (docOptions Is Nothing) Then
            docOptions = ModelDocumentationGeneratorOptions.DefaultOptions()
        End If
        If Not docOptions.DirectoryRoot.Exists Then
            docOptions.DirectoryRoot.Create()
        End If



        Using fWrite As IO.FileStream = System.IO.File.OpenWrite(System.IO.Path.Combine(docOptions.DirectoryRoot.FullName, HTMLDocumentationWriter.MakeHTMLFilename(PageName)))
            If (fWrite.CanWrite) Then
                Using sw As New System.IO.StreamWriter(fWrite)
                    'write the html start tag
                    sw.WriteLine("<html>")
                    'write the file header
                    sw.WriteLine("<head>")
                    If Not String.IsNullOrWhiteSpace(HTMLDocumentationWriter.CSS_FILE) Then
                        sw.WriteLine("<link rel=""stylesheet"" href=""" & HTMLDocumentationWriter.CSS_FILE & """ />")
                    End If
                    For Each metaElemet In m_globalElements
                        sw.Write("<meta name=""")
                        sw.Write(metaElemet.Key)
                        sw.Write(""" content=""")
                        sw.Write(metaElemet.Value)
                        sw.Write(""" />")
                        sw.WriteLine()
                    Next
                    sw.WriteLine("</head>")

                    'write the body start tag
                    sw.WriteLine("<body>")

                    'write the page content
                    Dim inList As Boolean = False
                    For Each pageElement As DocumentElement In m_elements
                        If pageElement.Level = IDocumentationWriter.DocumentationLevel.ListItem Then
                            If Not inList Then
                                sw.WriteLine("<ul>")
                                inList = True
                            End If
                        Else
                            If inList Then
                                sw.WriteLine("</ul>")
                                inList = False
                            End If
                        End If
                        sw.WriteLine(DocumentlementToString(pageElement))
                    Next

                    'If the last item was a list, close the list
                    If inList Then
                        sw.WriteLine("</ul>")
                        inList = False
                    End If

                    'write the body end tag
                    sw.WriteLine("</body>")
                    'write the html end tag
                    sw.WriteLine("</html>")
                End Using
            End If
        End Using

    End Sub

    ''' <summary>
    ''' Create a new page with the given unique name
    ''' </summary>
    ''' <param name="pageUniqueName">
    ''' The unique name to give this new page
    ''' </param>
    ''' <remarks>
    ''' This should be a fully model qualified name to avoid conflicts between similarily named
    ''' things in different models
    ''' </remarks>
    Public Sub New(ByVal pageUniqueName As String)
        m_pageName = pageUniqueName
    End Sub

    Public Shared Function DocumentlementToString(ByVal docElement As DocumentElement) As String


        Select Case docElement.Level
            Case IDocumentationWriter.DocumentationLevel.Heading
                If (String.IsNullOrWhiteSpace(docElement.Tag)) Then
                    Return "<h1>" & docElement.Content & "</h1>"
                Else
                    Return "<h1 class=""" & docElement.Tag & """ >" & docElement.Content & "</h1>"
                End If

            Case IDocumentationWriter.DocumentationLevel.ListItem
                Return "<li>" & docElement.Content & "</li>"
            Case IDocumentationWriter.DocumentationLevel.Normal
                Return "<p>" & docElement.Content & "</p>"
            Case IDocumentationWriter.DocumentationLevel.SubHeading
                If (String.IsNullOrWhiteSpace(docElement.Tag)) Then
                    Return "<h2>" & docElement.Content & "</h2>"
                Else
                    Return "<h2 class=""" & docElement.Tag & """ >" & docElement.Content & "</h2>"
                End If
        End Select

        Return docElement.Content

    End Function

End Class