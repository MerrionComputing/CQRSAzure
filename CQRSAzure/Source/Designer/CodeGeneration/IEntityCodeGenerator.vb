Imports System.CodeDom
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports Microsoft.CodeDom.Providers.DotNetCompilerPlatform

''' <summary>
''' Interface for the common functionality that all the code generrators provide
''' </summary>
Public Interface IEntityCodeGenerator
    Inherits IEntityInterfaceCodeGenerator
    Inherits IEntityImplementationCodeGenerator



End Interface

''' <summary>
''' Interface for the functionality used to create an interface for a generated class to 
''' implement
''' </summary>
''' <remarks>
''' This is used to allow separation by interface - for example if generatin mock classes for 
''' unit testing
''' </remarks>
Public Interface IEntityInterfaceCodeGenerator
    Inherits IEntityCodeGeneratorBase

    ''' <summary>
    ''' Generate the code compile unit graph for the interface generated for this entity
    ''' </summary>
    ''' <returns>
    ''' A code graph that will create the specific interface
    ''' </returns>
    Function GenerateInterface() As CodeDom.CodeCompileUnit

End Interface

Public Interface IEntityImplementationCodeGenerator
    Inherits IEntityCodeGeneratorBase

    ''' <summary>
    ''' Generate the code compile unit graph for the code that implements the interface
    ''' for this specific entity
    ''' </summary>
    Function GenerateImplementation() As CodeDom.CodeCompileUnit

End Interface

Public Interface IEntityCodeGeneratorBase

    ''' <summary>
    ''' The filename base to use when saving this entity's code
    ''' </summary>
    ''' <remarks>
    ''' This will have the .vb or .cs extension added as appropriate when the code is generated
    ''' </remarks>
    ReadOnly Property FilenameBase As String




    ''' <summary>
    ''' Set any per-user options that can affect how this code is generated
    ''' </summary>
    ''' <param name="options">
    ''' The specific options to use
    ''' </param>
    ''' <remarks>
    ''' Each entity code genrator will need to decide which options do or do not apply to it
    ''' </remarks>
    Sub SetCodeGenerationOptions(ByVal options As IModelCodeGenerationOptions)


    ''' <summary>
    ''' The list of namespaces that make up the entity namespace in code
    ''' </summary>
    ''' <returns>
    ''' This is implemented as a list rather than a single string so it can be shown
    ''' hierarchically in the documentation side
    ''' </returns>
    ReadOnly Property NamespaceHierarchy As IList(Of String)

    ''' <summary>
    ''' The set of namespaces required when code-generating this entity
    ''' </summary>
    ReadOnly Property RequiredNamespaces As IEnumerable(Of CodeNamespaceImport)


End Interface