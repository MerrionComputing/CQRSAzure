''' <summary>
''' Interface for the common functionality provided by each specific documentation
''' generation class
''' </summary>
Public Interface IEntityDocumentationGenerator

    ''' <summary>
    ''' The filename base to use when saving this entity's documentation
    ''' </summary>
    ''' <remarks>
    ''' This will have the .htm (or other) extension added as appropriate when the code is generated
    ''' </remarks>
    ReadOnly Property FilenameBase As String

    ''' <summary>
    ''' Set any per-user options that can affect how this model is documented
    ''' </summary>
    ''' <param name="options">
    ''' The specific options to use
    ''' </param>
    Sub SetCodeDocumentationOptions(ByVal options As ModelDocumentationGeneratorOptions)

    ''' <summary>
    ''' Generate the documentation for this entity to the given document writer
    ''' </summary>
    ''' <param name="docWriter">
    ''' The documentation generator on which to document this entity
    ''' </param>
    ''' <remarks>
    ''' If no documentation writer is explicitly passed in then a default generator is used
    ''' </remarks>
    Sub Generate(Optional docWriter As IDocumentationWriter = Nothing)

End Interface
