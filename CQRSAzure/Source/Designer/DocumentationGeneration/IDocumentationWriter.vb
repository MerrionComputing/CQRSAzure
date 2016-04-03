''' <summary>
''' Interface to be supported by any class that creates the actual output documentation
''' for any given CQRS model
''' </summary>
''' <remarks>
''' This is to allow, for instance, the same code to output either HTML pages or a word document,
''' depending on the write used
''' </remarks>
Public Interface IDocumentationWriter


    ''' <summary>
    ''' The type of element to make out of a thing to be documented
    ''' </summary>
    Enum DocumentationLevel
        ''' <summary>
        ''' This element is plain document-level text
        ''' </summary>
        Normal = 0
        ''' <summary>
        ''' This element is a page heading 
        ''' </summary>
        Heading = 1
        ''' <summary>
        ''' This element is a sub-heading
        ''' </summary>
        SubHeading = 2
        ''' <summary>
        ''' This element is a member of a list
        ''' </summary>
        ListItem = 3
    End Enum

    ''' <summary>
    ''' Create a new page that can be directly referenced by name
    ''' </summary>
    ''' <param name="pageName">
    ''' The name of the new page to create
    ''' </param>
    Sub CreatePage(ByVal pageName As String)

    ''' <summary>
    ''' Gets the way of referencing the named page
    ''' </summary>
    ''' <param name="pageName">
    ''' The page we want a reference to
    ''' </param>
    ''' <param name="description">
    ''' 
    ''' </param>
    ''' <remarks>
    ''' For example an HTML writer would return a link for this
    ''' </remarks>
    Function GetPageReference(ByVal pageName As String, ByVal description As String) As String

    ''' <summary>
    ''' The name of the page we are currently working on
    ''' </summary>
    Property CurrentPage As String

    ''' <summary>
    ''' Write the given element onto the current page
    ''' </summary>
    ''' <param name="content">
    ''' The text to write
    ''' </param>
    ''' <param name="level">
    ''' The level (header, sub head etc.) to write it as
    ''' </param>
    Sub WriteElement(ByVal content As String, ByVal level As DocumentationLevel, Optional ByVal tag As String = "")

    ''' <summary>
    ''' Save the output file(s)
    ''' </summary>
    Sub Save()

End Interface
