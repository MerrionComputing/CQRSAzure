
''' <summary>
''' Options for generating how the code for a given CQRS model is generated
''' </summary>
''' <remarks>
''' This is not kept in the model itself as we want to allow different users to have their own settings even
''' if they are sharing the underlying model
''' </remarks>
Public Class ModelCodeGenerationOptions

    ''' <summary>
    ''' The languages that the code generation can be done in
    ''' </summary>
    ''' <remarks>
    ''' If additional languages are supported (F# , C++ maybe?) they should be added here
    ''' </remarks>
    Public Enum SupportedLanguages
        ''' <summary>
        ''' Code generation in VB.Net
        ''' </summary>
        VBNet = 0
        ''' <summary>
        ''' Code generation in C Sharp
        ''' </summary>
        CSharp = 1
    End Enum

    ''' <summary>
    ''' The language with which to generate the source code
    ''' </summary>
    Public Property CodeLanguage As SupportedLanguages

    ''' <summary>
    ''' Where is the code to be generated
    ''' </summary>
    ''' <returns>
    ''' Sub-directories will be used for each type - aggregate/event/query/command/projections
    ''' </returns>
    Public Property DirectoryRoot As System.IO.DirectoryInfo

    ''' <summary>
    ''' What type of constructor to use in the generated classes
    ''' </summary>
    ''' <remarks>
    ''' This could be a per-model rather than per-user preference 
    ''' </remarks>
    Public Enum ConstructorPreferenceSetting
        ''' <summary>
        ''' (Default) Generate both an interface derived constructor and a parameters derived constructor
        ''' </summary>
        GenerateBoth = 0
        ''' <summary>
        ''' Only generate an interface derived constructor
        ''' </summary>
        InterfaceOnly = 1
        ''' <summary>
        ''' Only derive a parameters derived cosntructor
        ''' </summary>
        ParametersOnly = 2
    End Enum

    ''' <summary>
    ''' What type of constructor to use in the generated classes
    ''' </summary>
    Public Property ConstructorPreference As ConstructorPreferenceSetting

    ''' <summary>
    ''' Should each model be put in its own code subdirectory?
    ''' </summary>
    ''' <remarks>
    ''' This is usually a good idea
    ''' </remarks>
    Public Property SeparateFolderPerModel As Boolean

    ''' <summary>
    ''' Should each aggregate be put in its own code subdirectory?
    ''' </summary>
    ''' <remarks>
    ''' This is usually a good idea
    ''' </remarks>
    Public Property SeparateFolderPerAggregate As Boolean

    Public Sub New()

    End Sub

    Public Shared Function DefaultOptions() As ModelCodeGenerationOptions
        Return New ModelCodeGenerationOptions() With {.CodeLanguage = SupportedLanguages.VBNet,
            .DirectoryRoot = New System.IO.DirectoryInfo(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Code")),
                                                         .SeparateFolderPerModel = True,
                                                         .SeparateFolderPerAggregate = True}
    End Function

End Class
