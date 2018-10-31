Imports System.CodeDom
Imports System.CodeDom.Compiler
Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports Microsoft.CSharp
Imports Microsoft.VisualBasic
Imports Microsoft.VisualStudio.Modeling
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces.ModelCodegenerationOptionsBase
Imports CQRSAzure.CQRSdsl.CodeGeneration

''' <summary>
''' A class to perform the code generation for a complete CQRS model from it's XML
''' </summary>
''' <remarks>
''' Code will be generated in partial classes / partial interfaces so that it can readily be 
''' integrated with customisations
''' </remarks>
Public Class ModelCodeGenerator

    Private ReadOnly m_model As CQRSModel
    Private m_options As IModelCodeGenerationOptions = ModelCodeGenerationOptions.Default()

    Private m_modelProjects As New Dictionary(Of String, CodeProjectFile)


    ''' <summary>
    ''' The code generation options to use when generating the source code for this model
    ''' </summary>
    Public ReadOnly Property Options As IModelCodeGenerationOptions
        Get
            If (m_options Is Nothing) Then
                m_options = ModelCodeGenerationOptions.Default()
            End If
            Return m_options
        End Get
    End Property

    Public Sub GenerateCSharpCode()

        Using provider As New CSharpCodeProvider
            'C-Sharp specific initialisation
            Dim cSharpOptions As New CodeDom.Compiler.CodeGeneratorOptions()
            cSharpOptions.BlankLinesBetweenMembers = True
            cSharpOptions.BracingStyle = "C" ' Change this to "Block" to have the open brace on the current line (freak)

            GenerateModelCode(provider, cSharpOptions)


        End Using
    End Sub


    Public Sub GenerateVBNetCode()

        Using provider As New VBCodeProvider
            'Visual Basic specific initialisation
            Dim vbNetOptions As New CodeDom.Compiler.CodeGeneratorOptions()
            vbNetOptions.BlankLinesBetweenMembers = True

            GenerateModelCode(provider, vbNetOptions)
        End Using
    End Sub

    ''' <summary>
    ''' Generate the code in the language currently selected in the options
    ''' </summary>
    Public Sub GenerateCode()

        'clear any previous filename lists


        'perform any pre-generation operations
        If (m_options.SeparateFolderPerModel) Then

        End If

        If (m_options.CodeLanguage = SupportedLanguages.CSharp) Then
            GenerateCSharpCode()
        Else
            GenerateVBNetCode()
        End If

        'Generate the model project files
        For Each modelProject In m_modelProjects.Values
            GenerateModelProject(modelProject, m_options)
        Next

        'Generate any model solutions files (TBD if this makes sense)

    End Sub

    ''' <summary>
    ''' Create a project file (.csproj or .vbproj) for the code files linked to the given project
    ''' </summary>
    ''' <param name="modelProject">
    ''' The project template with the code files and references it will use
    ''' </param>
    ''' <param name="options">
    ''' Additional options on how the code should be generated
    ''' </param>
    ''' <remarks>
    ''' A code file may be included in multiple projects if that makes sense
    ''' </remarks>
    Private Sub GenerateModelProject(modelProject As CodeProjectFile,
                                     ByVal options As ModelCodeGenerationOptions)

        Dim filenameBase As String = modelProject.ProjectName
        If (options.CodeLanguage = SupportedLanguages.CSharp) Then
            filenameBase = filenameBase.Trim() & ".csproj"
        Else
            filenameBase = filenameBase.Trim() & ".vbproj"
        End If

        Using fWrite As IO.FileStream = System.IO.File.Create(System.IO.Path.Combine(options.DirectoryRoot.FullName, filenameBase))
            If (fWrite.CanWrite) Then
                Using sw As System.Xml.XmlWriter = System.Xml.XmlWriter.Create(fWrite)
                    'Make the XML indented for easier reading
                    sw.WriteStartDocument()

                    'write element <Project ToolsVersion="14.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
                    sw.WriteStartElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003")
                    sw.WriteAttributeString("ToolsVersion", "14.0")
                    sw.WriteAttributeString("DefaultTargets", "Build")

                    '<Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
                    sw.WriteStartElement(CodeProjectFile.ITEMGROUPMEMBER_IMPORT)
                    sw.WriteAttributeString("Project", "$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props")
                    sw.WriteAttributeString("Condition", "Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')")
                    sw.WriteEndElement()

                    '
                    sw.WriteStartElement("PropertyGroup")
                    sw.WriteElementString("ProjectGuid", modelProject.ProjectGuid.ToString("B"))
                    sw.WriteElementString("OutputType", "Library")
                    sw.WriteElementString("RootNamespace", m_model.Name)
                    sw.WriteElementString("AssemblyName", modelProject.ProjectName)
                    sw.WriteElementString("SchemaVersion", "2.0")
                    '<>v4.6</TargetFrameworkVersion>
                    sw.WriteElementString("TargetFrameworkVersion", "4.6")
                    sw.WriteEndElement()

                    'Build
                    sw.WriteStartElement("PropertyGroup")
                    sw.WriteAttributeString("Condition", " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ")
                    sw.WriteElementString("DebugSymbols", "true")
                    sw.WriteElementString("DebugType", "full")
                    sw.WriteElementString("Optimize", "false")
                    sw.WriteElementString("OutputPath", "bin\Debug\")
                    sw.WriteElementString("DefineConstants", "DEBUG;TRACE")
                    sw.WriteElementString("ErrorReport", "prompt")
                    sw.WriteElementString("WarningLevel", "4")
                    sw.WriteEndElement()


                    'Release
                    sw.WriteStartElement("PropertyGroup")
                    sw.WriteAttributeString("Condition", " '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ")
                    sw.WriteElementString("DebugType", "pdbonly")
                    sw.WriteElementString("Optimize", "true")
                    sw.WriteElementString("OutputPath", "bin\Release\")
                    sw.WriteElementString("DefineConstants", "TRACE")
                    sw.WriteElementString("ErrorReport", "prompt")
                    sw.WriteElementString("WarningLevel", "4")
                    sw.WriteEndElement()

                    If modelProject.IncludedReferences.Count > 0 Then
                        'make a section for all the references
                        sw.WriteStartElement("ItemGroup")
                        For Each reference In modelProject.IncludedReferences
                            sw.WriteStartElement(CodeProjectFile.ITEMGROUPMEMBER_REFERENCE)
                            sw.WriteAttributeString("Include", reference)
                            If Not String.IsNullOrWhiteSpace(CodeProjectFile.GetHintPath(reference)) Then
                                sw.WriteElementString("HintPath", CodeProjectFile.GetHintPath(reference))
                            End If
                            sw.WriteEndElement()
                        Next
                        sw.WriteEndElement() 'ItemGroup
                        'make a section for all the imports
                        sw.WriteStartElement("ItemGroup")
                        For Each reference In modelProject.IncludedReferences
                            sw.WriteStartElement(CodeProjectFile.ITEMGROUPMEMBER_IMPORT)
                            sw.WriteAttributeString("Include", reference)
                            sw.WriteEndElement()
                        Next
                        sw.WriteEndElement() 'ItemGroup
                    End If

                    If modelProject.IncludedSourceFilenames.Count > 0 Then
                        'make a section for all the included files
                        sw.WriteStartElement("ItemGroup")
                        For Each sourceFile In modelProject.IncludedSourceFilenames
                            sw.WriteStartElement(CodeProjectFile.ITEMGROUPMEMBER_COMPILE)
                            sw.WriteAttributeString("Include", sourceFile)
                            sw.WriteEndElement()
                        Next
                        sw.WriteEndElement() 'ItemGroup
                    End If

                    sw.WriteStartElement("Import")
                    If (options.CodeLanguage = SupportedLanguages.CSharp) Then
                        sw.WriteAttributeString("Project", "$(MSBuildToolsPath)\Microsoft.CSharp.targets")
                    End If

                    If (options.CodeLanguage = SupportedLanguages.VBNet) Then
                        sw.WriteAttributeString("Project", "$(MSBuildToolsPath)\Microsoft.VisualBasic.targets")
                    End If
                    sw.WriteEndElement()

                    'close the <Project> element
                    sw.WriteEndElement()

                    'Close the XML writer
                    sw.WriteEndDocument()
                End Using
            End If
        End Using

    End Sub

    ''' <summary>
    ''' Generate source code files for the given CQRS model given the code generator and options provided
    ''' </summary>
    ''' <param name="provider">
    ''' The code DOM provider for the specific language to generate the code in
    ''' </param>
    ''' <param name="codeGenOptions">
    ''' Additional formatting options to use when generating the code
    ''' </param>
    Private Sub GenerateModelCode(ByVal provider As CodeDomProvider, ByVal codeGenOptions As CodeGeneratorOptions)


        'Create each of the projects that will hold the source code...
        m_modelProjects.Add(CodeProjectFile.PROJECTNAME_QUERY_DEFINITION, New CodeProjectFile(MakeValidCodeName(m_model.Name) & "." & CodeProjectFile.PROJECTNAME_QUERY_DEFINITION))
        m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_DEFINITION).AddReference(CodeProjectFile.REFERENCE_QUERY_DEFINITION)
        m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_DEFINITION).AddReference(CodeProjectFile.REFERENCE_EVENTSOURCING_INTERFACES)
        m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_DEFINITION).AddReference(CodeProjectFile.REFERENCE_SYSTEM)

        m_modelProjects.Add(CodeProjectFile.PROJECTNAME_QUERY_HANDLER, New CodeProjectFile(MakeValidCodeName(m_model.Name) & "." & CodeProjectFile.PROJECTNAME_QUERY_HANDLER))
        m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_HANDLER).AddReference(CodeProjectFile.REFERENCE_QUERY_DEFINITION)
        m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_HANDLER).AddReference(CodeProjectFile.REFERENCE_QUERY_HANDLER)
        m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_HANDLER).AddReference(CodeProjectFile.REFERENCE_IDENTITYGROUP)
        m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_HANDLER).AddReference(CodeProjectFile.REFERENCE_EVENTSOURCING_INTERFACES)
        m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_HANDLER).AddReference(CodeProjectFile.REFERENCE_SYSTEM)

        m_modelProjects.Add(CodeProjectFile.PROJECTNAME_COMMAND_DEFINITION, New CodeProjectFile(MakeValidCodeName(m_model.Name) & "." & CodeProjectFile.PROJECTNAME_COMMAND_DEFINITION))
        m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_DEFINITION).AddReference(CodeProjectFile.REFERENCE_COMMAND_DEFINITION)
        m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_DEFINITION).AddReference(CodeProjectFile.REFERENCE_EVENTSOURCING_INTERFACES)
        m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_DEFINITION).AddReference(CodeProjectFile.REFERENCE_SYSTEM)

        m_modelProjects.Add(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER, New CodeProjectFile(MakeValidCodeName(m_model.Name) & "." & CodeProjectFile.PROJECTNAME_COMMAND_HANDLER))
        m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER).AddReference(CodeProjectFile.REFERENCE_COMMAND_DEFINITION)
        m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER).AddReference(CodeProjectFile.REFERENCE_COMMAND_HANDLER)
        m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER).AddReference(CodeProjectFile.REFERENCE_IDENTITYGROUP)
        m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER).AddReference(CodeProjectFile.REFERENCE_EVENTSOURCING_INTERFACES)
        m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER).AddReference(CodeProjectFile.REFERENCE_SYSTEM)

        m_modelProjects.Add(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP, New CodeProjectFile(MakeValidCodeName(m_model.Name) & "." & CodeProjectFile.PROJECTNAME_IDENTITY_GROUP))
        m_modelProjects(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP).AddReference(CodeProjectFile.REFERENCE_IDENTITYGROUP)
        m_modelProjects(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP).AddReference(CodeProjectFile.REFERENCE_EVENTSOURCING_INTERFACES)
        m_modelProjects(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP).AddReference(CodeProjectFile.REFERENCE_SYSTEM)

        m_modelProjects.Add(CodeProjectFile.PROJECTNAME_EVENT_SOURCING, New CodeProjectFile(MakeValidCodeName(m_model.Name) & "." & CodeProjectFile.PROJECTNAME_EVENT_SOURCING))
        m_modelProjects(CodeProjectFile.PROJECTNAME_EVENT_SOURCING).AddReference(CodeProjectFile.REFERENCE_EVENTSOURCING_INTERFACES)
        m_modelProjects(CodeProjectFile.PROJECTNAME_EVENT_SOURCING).AddReference(CodeProjectFile.REFERENCE_IDENTITYGROUP)
        m_modelProjects(CodeProjectFile.PROJECTNAME_EVENT_SOURCING).AddReference(CodeProjectFile.REFERENCE_SYSTEM)
        m_modelProjects(CodeProjectFile.PROJECTNAME_EVENT_SOURCING).AddReference(CodeProjectFile.REFERENCE_SYSTEM_REFLECTION)

        'Compile the model contents...
        For Each aggregate As AggregateIdentifier In m_model.AggregateIdentifiers
            Dim aggregateCodeGen As New AggregateIdentifierCodeGenerator(aggregate)
            aggregateCodeGen.SetCodeGenerationOptions(m_options)
            'Make the interface for the aggregate identifier
            CreateSourceCodeFile(MakeInterfaceName(aggregate.Name), aggregateCodeGen.GenerateInterface(), m_options, m_model.Name, ModelSourceFileType.AggregateIdentifierInterface)
            'Make the class for the aggregate identifier
            CreateSourceCodeFile(MakeImplementationClassName(aggregate.Name), aggregateCodeGen.GenerateImplementation(), m_options, m_model.Name, ModelSourceFileType.AggregateIdentifier)

            'Walk the tree and create the code required for each element
            For Each eventDef As EventDefinition In aggregate.EventDefinitions
                Dim eventCodeGen As New EventCodeGenerator(eventDef)
                eventCodeGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the event definition
                CreateSourceCodeFile(MakeInterfaceName(eventCodeGen.FilenameBase), eventCodeGen.GenerateInterface(), m_options, m_model.Name, ModelSourceFileType.EventDefinitionInterface)
                'Make the class for the event definition
                CreateSourceCodeFile(MakeImplementationClassName(eventCodeGen.FilenameBase), eventCodeGen.GenerateImplementation(), m_options, m_model.Name, ModelSourceFileType.EventDefinition)
                'Make the class for serialising the event definition
                Dim eventSerialisationCodeGen As New EventSerialisationCodeGenerator(eventDef)
                CreateSourceCodeFile(MakeImplementationClassName(eventSerialisationCodeGen.FilenameBase), eventSerialisationCodeGen.GenerateImplementation(), m_options, m_model.Name, ModelSourceFileType.EventDefinition)
            Next

            For Each projDef As ProjectionDefinition In aggregate.ProjectionDefinitions
                Dim projCodeGen As New ProjectionCodeGenerator(projDef)
                projCodeGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the projection definition
                CreateSourceCodeFile(MakeInterfaceName(projCodeGen.FilenameBase), projCodeGen.GenerateInterface(), m_options, m_model.Name, ModelSourceFileType.ProjectionInterface)
                'Make the class for the projection definition
                CreateSourceCodeFile(MakeImplementationClassName(projCodeGen.FilenameBase), projCodeGen.GenerateImplementation(), m_options, m_model.Name, ModelSourceFileType.Projection)
            Next

            For Each cmdDef As CommandDefinition In aggregate.CommandDefinitions
                Dim cmdCodeGen As New CommandDefinitionCodeGenerator(cmdDef)
                cmdCodeGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the command definition
                CreateSourceCodeFile(MakeInterfaceName(cmdCodeGen.FilenameBase), cmdCodeGen.GenerateInterface(), m_options, m_model.Name, ModelSourceFileType.CommandDefinitionInterface)
                'Make the class for the command definition
                CreateSourceCodeFile(MakeImplementationClassName(cmdCodeGen.FilenameBase), cmdCodeGen.GenerateImplementation(), m_options, m_model.Name, ModelSourceFileType.CommandDefinition)

                Dim cmdHandleCodeGen As New CommandHandlerCodeGenerator(cmdDef)
                cmdHandleCodeGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the command handler
                CreateSourceCodeFile(MakeInterfaceName(cmdHandleCodeGen.FilenameBase), cmdHandleCodeGen.GenerateInterface(), m_options, m_model.Name, ModelSourceFileType.CommandImplementationInterface)
                'Make the class for the command handler
                CreateSourceCodeFile(MakeImplementationClassName(cmdHandleCodeGen.FilenameBase), cmdHandleCodeGen.GenerateImplementation(), m_options, m_model.Name, ModelSourceFileType.CommandImplementation)
            Next

            For Each qryDef As QueryDefinition In aggregate.QueryDefinitions
                Dim qryCodeGen As New QueryDefinitionCodeGenerator(qryDef)
                qryCodeGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the query handler
                CreateSourceCodeFile(MakeInterfaceName(qryCodeGen.FilenameBase), qryCodeGen.GenerateInterface(), m_options, m_model.Name, ModelSourceFileType.QueryDefinitionInterface)
                'Make the class for the query handler
                CreateSourceCodeFile(MakeImplementationClassName(qryCodeGen.FilenameBase), qryCodeGen.GenerateImplementation(), m_options, m_model.Name, ModelSourceFileType.QueryDefinition)
                Dim qryHandleCodeGen As New QueryHandlerCodeGenerator(qryDef)
                qryHandleCodeGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the query handler
                CreateSourceCodeFile(MakeInterfaceName(qryHandleCodeGen.FilenameBase), qryHandleCodeGen.GenerateInterface(), m_options, m_model.Name, ModelSourceFileType.QueryImplementationInterface)
                'Make the class for the query handler
                CreateSourceCodeFile(MakeImplementationClassName(qryHandleCodeGen.FilenameBase), qryHandleCodeGen.GenerateImplementation(), m_options, m_model.Name, ModelSourceFileType.QueryImplementation)
            Next

            For Each idGrpDef As IdentityGroup In aggregate.IdentityGrouped
                Dim idGrpGen As New IdentityGroupCodeGenerator(idGrpDef)
                idGrpGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the identity group 
                CreateSourceCodeFile(MakeInterfaceName(idGrpGen.FilenameBase), idGrpGen.GenerateInterface(), m_options, m_model.Name, ModelSourceFileType.IdentityGroupInterface)
                'Make the class for the identity group 
                CreateSourceCodeFile(MakeImplementationClassName(idGrpGen.FilenameBase), idGrpGen.GenerateImplementation(), m_options, m_model.Name, ModelSourceFileType.IdentityGroup)
            Next

            For Each classInst As Classifier In aggregate.Classifiers
                Dim classGen As New ClassifierCodeGenerator(classInst)
                classGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the identity group 
                CreateSourceCodeFile(MakeInterfaceName(classGen.FilenameBase), classGen.GenerateInterface(), m_options, m_model.Name, ModelSourceFileType.ClassifierInterface)
                'Make the class for the identity group 
                CreateSourceCodeFile(MakeImplementationClassName(classGen.FilenameBase), classGen.GenerateImplementation(), m_options, m_model.Name, ModelSourceFileType.ClassifierInterface)
            Next

        Next

        'Do we need to create the .SQL code as well?
        If (m_options.GenerateEntityFrameworkClasses) Then
            'make the model-level DBSet<> DBContect container class
            Dim modelSQLGen As New SQLCodeGenerator(m_model)
            If (modelSQLGen IsNot Nothing) Then
                modelSQLGen.SetCodeGenerationOptions(m_options)
                CreateSourceCodeFile(MakeImplementationClassName(modelSQLGen.FilenameBase), modelSQLGen.GenerateImplementation(), m_options, m_model.Name)

                'Then make the aggregate instance classes
                For Each aggregate As AggregateIdentifier In m_model.AggregateIdentifiers
                    Dim aggSQLGen As New AggregateInstanceSQLCodeGenerator(aggregate)
                    If (aggSQLGen IsNot Nothing) Then
                        aggSQLGen.SetCodeGenerationOptions(m_options)
                        CreateSourceCodeFile(MakeImplementationClassName(aggSQLGen.FilenameBase), aggSQLGen.GenerateImplementation(), m_options, m_model.Name)

                        'And make the event details classes
                        For Each eventDef As EventDefinition In aggregate.EventDefinitions


                        Next
                    End If
                Next
            End If
        End If

    End Sub

#Region "Constructors"
    ''' <summary>
    ''' Create a code generator for the specific CQRS model
    ''' </summary>
    ''' <param name="modelToGenerate">
    ''' The CQRS model that will be turned into code
    ''' </param>
    Public Sub New(ByVal modelToGenerate As CQRSModel)
        m_model = modelToGenerate
        m_options = ModelCodeGenerationOptions.Default
    End Sub

    ''' <summary>
    ''' Create a code generator for the specific CQRS model with teh given code gen options
    ''' </summary>
    ''' <param name="modelToGenerate">
    ''' The CQRS model that will be turned into code
    ''' </param>
    Public Sub New(ByVal modelToGenerate As CQRSModel, ByVal codeGenOptions As IModelCodeGenerationOptions)
        m_model = modelToGenerate
        m_options = codeGenOptions
    End Sub
#End Region

    Public Shared Function MakeValidCodeFilenameBase(ByVal entityName As String) As String

        'For now, just strip spaces
        Return String.Join("_", entityName.Split(System.IO.Path.GetInvalidFileNameChars()))

    End Function

    Public Shared Function MakeInterfaceName(ByVal entityName As String) As String

        Return "I" & MakeImplementationClassName(entityName)

    End Function

    Public Shared Function MakeImplementationClassName(ByVal entityName As String) As String

        Return MakeValidCodeName(entityName)

    End Function

    Public Shared Function MakeValidCodeName(ByVal nameIn As String) As String
        Dim invalidCharacters As Char() = " -!, .;':@£$%^&*()-+=/\#~"
        Return String.Join("_", nameIn.Split(invalidCharacters))
    End Function

    ''' <summary>
    ''' Standard code generated attribute to add onto everything we generate
    ''' </summary>
    ''' <returns>
    ''' &lt;GeneratedCodeAttribute("CQRS On Azure - DSL", "1.0.0.0") &gt;
    ''' </returns>
    Public Shared Function StandardCodeGeneratedAttribute() As CodeAttributeDeclaration

        ' Declare a new generated code attribute
        Dim generatedCodeAttributeRet As GeneratedCodeAttribute =
            New GeneratedCodeAttribute("CQRS On Azure - DSL", "1.0.0.0")

        ' Use the generated code attribute members in the attribute declaration
        Dim codeAttrDecl As CodeAttributeDeclaration =
            New CodeAttributeDeclaration(GetType(GeneratedCodeAttribute).Name,
                New CodeAttributeArgument(
                    New CodePrimitiveExpression(generatedCodeAttributeRet.Tool)),
                New CodeAttributeArgument(
                    New CodePrimitiveExpression(generatedCodeAttributeRet.Version)))


        Return codeAttrDecl

    End Function

    ''' <summary>
    ''' The different types of source code that can be code generated by this library from the CQRS model
    ''' </summary>
    Public Enum ModelSourceFileType
        ''' <summary>
        ''' Unknown or not set file type
        ''' </summary>
        NotSet = 0
        ''' <summary>
        ''' An interface for an aggregate in the model
        ''' </summary>
        AggregateIdentifierInterface = 1
        ''' <summary>
        ''' The implementation code of an aggregate in the model
        ''' </summary>
        AggregateIdentifier = 2
        ''' <summary>
        ''' Interface file for a query definition
        ''' </summary>
        QueryDefinitionInterface = 3
        ''' <summary>
        ''' Implementation for a query definition
        ''' </summary>
        QueryDefinition = 4
        ''' <summary>
        ''' Interface for a query implementation 
        ''' </summary>
        QueryImplementationInterface = 5
        ''' <summary>
        ''' Implementation code for a query implementation
        ''' </summary>
        QueryImplementation = 6
        ''' <summary>
        ''' Interface defining a command definition
        ''' </summary>
        CommandDefinitionInterface = 7
        ''' <summary>
        ''' Implementation code of a command definition
        ''' </summary>
        CommandDefinition = 8
        ''' <summary>
        ''' Interface of a command execution implementation
        ''' </summary>
        CommandImplementationInterface = 9
        ''' <summary>
        ''' Implementation code for a command execution
        ''' </summary>
        CommandImplementation = 10
        ''' <summary>
        ''' Interface defining an event
        ''' </summary>
        EventDefinitionInterface = 11
        ''' <summary>
        ''' Implementation of the definition of an event
        ''' </summary>
        EventDefinition = 12
        ''' <summary>
        ''' Interface defining a projection
        ''' </summary>
        ProjectionInterface = 13
        ''' <summary>
        ''' Implementation code of a projection
        ''' </summary>
        Projection = 14
        ''' <summary>
        ''' Interface defining an identity group
        ''' </summary>
        IdentityGroupInterface = 15
        ''' <summary>
        ''' Concrete implementation of an identity group
        ''' </summary>
        IdentityGroup = 16
        ''' <summary>
        ''' Interface for an identity group classifier
        ''' </summary>
        ClassifierInterface = 17
        ''' <summary>
        ''' Concrete implementation of an identity group classifier
        ''' </summary>
        Classifier = 18
    End Enum

    ''' <summary>
    ''' Creates a source code file to hold the source code for a particular part of the CQRS domain model
    ''' </summary>
    ''' <param name="filenameBase">
    ''' The filename (without language suffix) tpo store the code in
    ''' </param>
    ''' <param name="codeUnit">
    ''' The Roslyn code compile unit that makes up the source code to write to the file
    ''' </param>
    ''' <param name="options">
    ''' Additional code generation options that affect how source code files are generated
    ''' </param>
    ''' <param name="modelName">
    ''' The root name of the CQRS model to which this new source file belongs
    ''' </param>
    ''' <param name="sourceFileType">
    ''' The type of file this is 
    ''' </param>
    Public Sub CreateSourceCodeFile(ByVal filenameBase As String,
                                           ByVal codeUnit As CodeCompileUnit,
                                           ByVal options As ModelCodeGenerationOptions,
                                           Optional ByVal modelName As String = "",
                                           Optional ByVal sourceFileType As ModelSourceFileType = ModelSourceFileType.NotSet)

        Dim CodeLanguage As SupportedLanguages = options.CodeLanguage
        If (CodeLanguage = SupportedLanguages.CSharp) Then
            filenameBase = filenameBase.Trim() & ".cs"
        Else
            filenameBase = filenameBase.Trim() & ".vb"
        End If

        Dim targetDirectoryName As String = options.DirectoryRoot.FullName
        If (Not options.DirectoryRoot.Exists) Then
            options.DirectoryRoot.Create()
        End If

        If (options.SeparateFolderPerModel) Then
            If Not String.IsNullOrWhiteSpace(modelName) Then
                Dim modelDirectory As New System.IO.DirectoryInfo(System.IO.Path.Combine(targetDirectoryName, MakeValidCodeName(modelName)))
                If Not modelDirectory.Exists() Then
                    modelDirectory.Create()
                End If
                targetDirectoryName = modelDirectory.FullName
            End If
        End If

        Using fWrite As IO.FileStream = System.IO.File.Create(System.IO.Path.Combine(targetDirectoryName, filenameBase))
            If (fWrite.CanWrite) Then
                Using sw As New System.IO.StreamWriter(fWrite)
                    If (CodeLanguage = SupportedLanguages.CSharp) Then
                        sw.Write(CodeGenerationUtilities.ToCSharpCodeString(codeUnit))
                    Else
                        sw.Write(CodeGenerationUtilities.ToVBCodeString(codeUnit))
                    End If
                End Using
            End If
        End Using

        'add the file to the appropriate projects
        AddSourceFileToProjects(filenameBase, sourceFileType)


    End Sub

    ''' <summary>
    ''' Depending on the type of the file decide which project(s) to add it to and do so
    ''' </summary>
    ''' <param name="filenameBase">
    ''' The filename of the generated code
    ''' </param>
    ''' <param name="sourceFileType">
    ''' The defined type of file this code file is for
    ''' </param>
    Private Sub AddSourceFileToProjects(filenameBase As String, sourceFileType As ModelSourceFileType)

        Select Case sourceFileType
            Case ModelSourceFileType.NotSet
                'Not known so cannot add it to any file

            Case ModelSourceFileType.QueryDefinitionInterface
                'add to query def and query handler projects
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_QUERY_DEFINITION)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_DEFINITION).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_QUERY_HANDLER)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_HANDLER).AddSourceFile(filenameBase)
                End If
            Case ModelSourceFileType.QueryDefinition
                'add to query def and query handler projects
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_QUERY_DEFINITION)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_DEFINITION).AddSourceFile(filenameBase)
                End If
            Case ModelSourceFileType.QueryImplementationInterface
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_QUERY_HANDLER)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_HANDLER).AddSourceFile(filenameBase)
                End If
            Case ModelSourceFileType.QueryImplementation
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_QUERY_HANDLER)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_HANDLER).AddSourceFile(filenameBase)
                End If

            Case ModelSourceFileType.CommandDefinitionInterface
                'add to query def and query handler projects
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_COMMAND_DEFINITION)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_DEFINITION).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER).AddSourceFile(filenameBase)
                End If
            Case ModelSourceFileType.CommandDefinition
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER).AddSourceFile(filenameBase)
                End If
            Case ModelSourceFileType.CommandImplementationInterface
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER).AddSourceFile(filenameBase)
                End If
            Case ModelSourceFileType.CommandImplementation
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER).AddSourceFile(filenameBase)
                End If

            Case ModelSourceFileType.ProjectionInterface
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_QUERY_HANDLER)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_HANDLER).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_EVENT_SOURCING)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_EVENT_SOURCING).AddSourceFile(filenameBase)
                End If
            Case ModelSourceFileType.Projection
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_EVENT_SOURCING)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_EVENT_SOURCING).AddSourceFile(filenameBase)
                End If

            Case ModelSourceFileType.EventDefinitionInterface
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_EVENT_SOURCING)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_EVENT_SOURCING).AddSourceFile(filenameBase)
                End If
            Case ModelSourceFileType.EventDefinition
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_EVENT_SOURCING)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_EVENT_SOURCING).AddSourceFile(filenameBase)
                End If

            Case ModelSourceFileType.IdentityGroupInterface
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_COMMAND_DEFINITION)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_DEFINITION).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_QUERY_DEFINITION)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_DEFINITION).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_QUERY_HANDLER)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_HANDLER).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_EVENT_SOURCING)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_EVENT_SOURCING).AddSourceFile(filenameBase)
                End If
            Case ModelSourceFileType.IdentityGroup
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP).AddSourceFile(filenameBase)
                End If

            Case ModelSourceFileType.AggregateIdentifier
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_EVENT_SOURCING)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_EVENT_SOURCING).AddSourceFile(filenameBase)
                End If
            Case ModelSourceFileType.AggregateIdentifierInterface
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_COMMAND_DEFINITION)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_DEFINITION).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_COMMAND_HANDLER).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_QUERY_DEFINITION)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_DEFINITION).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_QUERY_HANDLER)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_QUERY_HANDLER).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_EVENT_SOURCING)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_EVENT_SOURCING).AddSourceFile(filenameBase)
                End If

            Case ModelSourceFileType.ClassifierInterface
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP).AddSourceFile(filenameBase)
                End If
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_EVENT_SOURCING)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_EVENT_SOURCING).AddSourceFile(filenameBase)
                End If
            Case ModelSourceFileType.Classifier
                If (m_modelProjects.ContainsKey(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP)) Then
                    m_modelProjects(CodeProjectFile.PROJECTNAME_IDENTITY_GROUP).AddSourceFile(filenameBase)
                End If

        End Select

    End Sub
End Class

''' <summary>
''' Utility functions for generating comments in the code
''' </summary>
Public Class CommentGeneration

    ''' <summary>
    ''' Add a summary comment section
    ''' </summary>
    ''' <param name="commentText">
    ''' The lines of comment to put in the summary section
    ''' </param>
    Public Shared Function SummaryCommentSection(ByVal commentText As IEnumerable(Of String)) As CodeCommentStatementCollection

        Return BaseCommentSection("summary", Nothing, commentText)

    End Function

    ''' <summary>
    ''' Add a returns comment section
    ''' </summary>
    ''' <param name="commentText">
    ''' The lines of comment to put in the returns section
    ''' </param>
    Public Shared Function ReturnsCommentSection(ByVal commentText As IEnumerable(Of String)) As CodeCommentStatementCollection

        Return BaseCommentSection("returns", Nothing, commentText)

    End Function

    ''' <summary>
    ''' Add a remarks comment section
    ''' </summary>
    ''' <param name="commentText">
    ''' The lines of comment to put in the remarks section
    ''' </param>
    Public Shared Function RemarksCommentSection(ByVal commentText As IEnumerable(Of String)) As CodeCommentStatementCollection

        Return BaseCommentSection("remarks", Nothing, commentText)

    End Function

    ''' <summary>
    ''' Add a parameter comment for the named parameter
    ''' </summary>
    ''' <param name="parameterName">
    ''' The name of the parameter
    ''' </param>
    ''' <param name="commentText">
    ''' The comment to attach to the parameter
    ''' </param>
    ''' <returns></returns>
    Public Shared Function ParamCommentsSection(ByVal parameterName As String, ByVal commentText As IEnumerable(Of String)) As CodeCommentStatementCollection

        Dim nameValue As New Dictionary(Of String, String)()
        nameValue.Add("name", parameterName)

        Return BaseCommentSection("param", nameValue, commentText)

    End Function

    ''' <summary>
    ''' A general function to wrap a comment in XML documentation attributes
    ''' </summary>
    ''' <param name="sectionName">The name of the section - e.g. summary, remarks, returns </param>
    ''' <param name="commentText">The content of the section
    ''' </param>
    ''' <returns>
    ''' A code comment collection that can be attached to anything that takes XML comments
    ''' </returns>
    Private Shared Function BaseCommentSection(ByVal sectionName As String,
                                                  ByVal sectionAttributes As Dictionary(Of String, String),
                                                  ByVal commentText As IEnumerable(Of String)) As CodeCommentStatementCollection

        Dim ret As New CodeCommentStatementCollection()

        ret.Add(OpenCommentSection(sectionName, sectionAttributes))
        For Each commentLine As String In commentText
            'Add any non-blank strings
            If Not String.IsNullOrEmpty(commentLine) Then
                ret.Add(New CodeCommentStatement(commentLine, True))
            End If
        Next
        ret.Add(CloseCommentSection(sectionName))

        Return ret

    End Function

    ''' <summary>
    ''' Create a comment to start an XML comment section
    ''' </summary>
    ''' <param name="sectionName">
    ''' The name of the section - remarks, summary, returns, param etc.
    ''' </param>
    ''' <param name="sectionAttributes">
    ''' Additional attributes to put in the open section tag (optional)
    ''' </param>
    Private Shared Function OpenCommentSection(ByVal sectionName As String, ByVal sectionAttributes As Dictionary(Of String, String)) As CodeCommentStatement

        Dim commentInner As String = sectionName

        If (sectionAttributes IsNot Nothing) Then
            For Each sectionNameAttribute As String In sectionAttributes.Keys
                commentInner &= " " & sectionNameAttribute & "=""" & sectionAttributes(sectionNameAttribute) & """"
            Next
        End If

        Return New CodeCommentStatement("<" & commentInner.Trim & ">", True)

    End Function

    Private Shared Function CloseCommentSection(ByVal sectionName As String) As CodeCommentStatement

        Return New CodeCommentStatement("</" & sectionName.Trim & ">", True)

    End Function



End Class

''' <summary>
''' Utility functions for generating interfaces in the code generation
''' </summary>
Public Class InterfaceCodeGeneration

    Public Shared Function SimplePropertyDeclaration(ByVal readOnlyFlag As Boolean,
                                                     ByVal propertyName As String,
                                                     ByVal propertyType As PropertyDataType,
                                                     Optional ByVal publicMember As Boolean = False) As CodeMemberProperty

        Dim ret As New CodeMemberProperty()
        ret.Name = ModelCodeGenerator.MakeValidCodeName(propertyName)
        ret.HasGet = True
        If (readOnlyFlag) Then
            ret.HasSet = False
        Else
            ret.HasSet = True
        End If

        If (publicMember) Then
            ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Public
        End If

        ret.Type = ToPropertyTypeReference(propertyType)

        Return ret

    End Function

    Public Shared Function ToPropertyTypeReference(ByVal propertyType As PropertyDataType) As CodeTypeReference

        Select Case propertyType
            Case PropertyDataType.Boolean
                Return New CodeTypeReference(GetType(Boolean))
            Case PropertyDataType.Date
                Return New CodeTypeReference(GetType(DateTime))
            Case PropertyDataType.Decimal
                Return New CodeTypeReference(GetType(Decimal))
            Case PropertyDataType.FloatingPointNumber
                Return New CodeTypeReference(GetType(Double))
            Case PropertyDataType.Image
                Return New CodeTypeReference(GetType(Byte()))
            Case PropertyDataType.Integer
                Return New CodeTypeReference(GetType(Integer))
            Case PropertyDataType.String
                Return New CodeTypeReference(GetType(String))
            Case Else
                Return New CodeTypeReference(GetType(Object))
        End Select

    End Function

    Public Shared Function InterfaceDeclaration(ByVal entityName As String) As CodeTypeDeclaration

        Dim interfaceDeclarationRet As CodeTypeDeclaration = New CodeTypeDeclaration(ModelCodeGenerator.MakeInterfaceName(entityName))
        interfaceDeclarationRet.IsPartial = True
        interfaceDeclarationRet.IsInterface = True

        Return interfaceDeclarationRet

    End Function

    Public Shared Function ImplementsInterfaceReference(ByVal entityName As String) As CodeTypeReference

        Return New CodeTypeReference(ModelCodeGenerator.MakeInterfaceName(entityName))

    End Function

    Public Shared Function ImplementsInterfaceReference(ByVal interfaceName As String, ByVal propertyName As String) As CodeTypeReference

        Return New CodeTypeReference(interfaceName.Trim() & "." & propertyName.Trim())

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="interfaceName">
    ''' The base name of the interface
    ''' </param>
    ''' <param name="genericTypes">
    ''' The set of types (in order) of the generic interface
    ''' </param>
    ''' <returns></returns>
    Public Shared Function ImplementsGenericInterfaceReference(ByVal interfaceName As String, ByVal genericTypes As IList(Of CodeTypeReference)) As CodeTypeReference

        If (genericTypes.Count = 0) Then
            Return New CodeTypeReference(interfaceName)
        Else
            Dim ret As New CodeTypeReference(interfaceName)
            ret.TypeArguments.AddRange(genericTypes.ToArray)
            Return ret
        End If

    End Function

End Class

Public Class ClassCodeGeneration

    Public Shared Function ClassDeclaration(ByVal entityName As String) As CodeTypeDeclaration

        Dim classDeclarationRet As CodeTypeDeclaration = New CodeTypeDeclaration(ModelCodeGenerator.MakeImplementationClassName(entityName))
        classDeclarationRet.IsPartial = True
        classDeclarationRet.IsClass = True

        Return classDeclarationRet

    End Function

End Class

''' <summary>
''' Utility functions for creating the properties of the classes in code generation
''' </summary>
Public Class PropertyCodeGeneration


    ''' <summary>
    ''' Create a new public constant for the given name and data type and value
    ''' </summary>
    ''' <param name="constantName">
    ''' The name of the constant 
    ''' </param>
    ''' <param name="dataType">
    ''' The data type of the constant
    ''' </param>
    ''' <param name="value">
    ''' The value that the constant is set to
    ''' </param>
    ''' <returns></returns>
    Public Shared Function PublicConstant(ByVal constantName As String, ByVal dataType As CodeTypeReference, ByVal value As CodePrimitiveExpression) As CodeMemberField

        Dim constMember As New CodeMemberField()
        constMember.Name = EventCodeGenerator.ToMemberName(constantName, False)
        constMember.Type = dataType

        ' Clear the scope mask
        constMember.Attributes = (constMember.Attributes And (Not MemberAttributes.ScopeMask))
        ' Make it public accessible outside the class
        constMember.Attributes = (constMember.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Public Or MemberAttributes.Const

        'Put the constant value in
        constMember.InitExpression = value

        Return constMember
    End Function

    ''' <summary>
    ''' Create the set of private fields backing an event/projection etc. property 
    ''' </summary>
    ''' <param name="eventProperties">
    ''' The set of properties for which to create a member declaration
    ''' </param>
    ''' <param name="readOnlyProperty">
    ''' If true, make the proeprty read-only so it may only be set in the class constructor
    ''' </param>
    ''' <returns>
    ''' Ideally the properties should be read only so that the class can be considered immutable
    ''' </returns>
    Public Overloads Shared Function PrivateBackingMembers(eventProperties As IList(Of IPropertyEntity), Optional readOnlyProperty As Boolean = False) As CodeTypeMember()

        Dim ret As New List(Of CodeTypeMember)

        For Each eventProperty In eventProperties
            Dim privateMember As New CodeMemberField(EventCodeGenerator.ToMemberType(eventProperty.DataType), EventCodeGenerator.ToMemberName(eventProperty.Name, True))
            If (readOnlyProperty) Then
                'This is not possible in CodeDOM as not all languages support this....you'd need to use a CodeSnippet (VB.Net or C#)..
                privateMember.Comments.Add(New CodeCommentStatement("TODO: Make this property read only"))
            End If
            ret.Add(privateMember)
        Next

        RegionCodeGeneration.WrapInRegion(RegionCodeGeneration.REGION_PRIVATE_MEMBERS, ret)

        Return ret.ToArray

    End Function

    Public Overloads Shared Function PrivateBackingMembers(eventProperties As IList(Of IEventPropertyEntity), Optional readOnlyProperty As Boolean = False) As CodeTypeMember()

        Return PrivateBackingMembers(eventProperties.Cast(Of IPropertyEntity).ToList(), readOnlyProperty)

    End Function

    ''' <summary>
    ''' Generate a public (get-only) property backed by the private member 
    ''' </summary>
    ''' <param name="eventProp">
    ''' The property desfinition for which to create a public member declaration
    ''' </param>
    ''' <param name="interfaceName">
    ''' The name of the interface this property is implementing
    ''' </param>
    ''' <returns></returns>
    Public Shared Function PublicMember(eventProp As IPropertyEntity, Optional ByVal interfaceName As String = "") As CodeMemberProperty

        Dim ret As New CodeMemberProperty()
        ret.Name = EventCodeGenerator.ToMemberName(eventProp.Name, False)
        ret.Type = EventCodeGenerator.ToMemberType(eventProp.DataType)

        ' Make it final
        ret.Attributes = (ret.Attributes And (Not MemberAttributes.ScopeMask)) Or MemberAttributes.Final
        ' Make it public accessible outside the class
        ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Public

        'Make it implement the interface if one is passed in...
        If (Not String.IsNullOrWhiteSpace(interfaceName)) Then
            ret.ImplementationTypes.Add(New CodeTypeReference(interfaceName))
        End If

        'add the getter
        Dim memberReference As New CodeFieldReferenceExpression()
        memberReference.FieldName = EventCodeGenerator.ToMemberName(eventProp.Name, True)

        ret.GetStatements.Add((New CodeMethodReturnStatement(memberReference)))


        ' Add-in the documentation for the property
        If (Not String.IsNullOrWhiteSpace(eventProp.Description)) Then
            ret.Comments.AddRange(CommentGeneration.SummaryCommentSection({eventProp.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(eventProp.Notes)) Then
            ret.Comments.AddRange(CommentGeneration.RemarksCommentSection({eventProp.Notes}))
        End If

        Return ret

    End Function

    Public Shared Function PublicMember(propertyName As String,
                                        dataType As PropertyDataType,
                                        Optional ByVal interfaceName As String = "",
                                        Optional ByVal backingProperty As String = "",
                                        Optional ByVal omitBackingProperty As Boolean = False) As CodeMemberProperty

        Dim ret As New CodeMemberProperty()
        ret.Name = EventCodeGenerator.ToMemberName(propertyName, False)
        ret.Type = EventCodeGenerator.ToMemberType(dataType)

        ' Make it final
        ret.Attributes = (ret.Attributes And (Not MemberAttributes.ScopeMask)) Or MemberAttributes.Final
        ' Make it public accessible outside the class
        ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Public

        'Make it implement the interface if one is passed in...
        If (Not String.IsNullOrWhiteSpace(interfaceName)) Then
            ret.ImplementationTypes.Add(New CodeTypeReference(interfaceName))
        End If

        'add the getter
        Dim memberReference As New CodeFieldReferenceExpression()

        If (Not omitBackingProperty) Then
            If (String.IsNullOrWhiteSpace(backingProperty)) Then
                memberReference.FieldName = EventCodeGenerator.ToMemberName(propertyName, True)
            Else
                memberReference.FieldName = backingProperty
            End If

            ret.GetStatements.Add((New CodeMethodReturnStatement(memberReference)))
        End If


        Return ret
    End Function

End Class

Public Class RegionCodeGeneration

    Public Const REGION_PRIVATE_MEMBERS As String = "Private members"

    Public Shared Sub WrapInRegion(ByVal RegionText As String, ByVal codeLines As ICollection(Of CodeTypeMember))

        If (codeLines.Count > 0) Then
            codeLines(0).StartDirectives.Add(New CodeRegionDirective(CodeRegionMode.Start, RegionText))
            codeLines(codeLines.Count - 1).EndDirectives.Add(New CodeRegionDirective(CodeRegionMode.End, String.Empty))
        End If

    End Sub

End Class


Public Class ConstructorCodeGenerator


    Public Shared Function ParameterisedConstructor(eventProperties As IList(Of IEventPropertyEntity)) As CodeConstructor

        Dim fromParametersConstructor As New CodeConstructor()
        fromParametersConstructor.Attributes = (fromParametersConstructor.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Public

        If (eventProperties IsNot Nothing) Then
            For Each eventProperty As IEventPropertyEntity In eventProperties
                fromParametersConstructor.Parameters.Add(New CodeParameterDeclarationExpression(EventCodeGenerator.ToMemberType(eventProperty.DataType), EventCodeGenerator.ToMemberName(eventProperty.Name, False, True)))
                ' add an assignment from this parameter to the internal field
                Dim targetField As New CodeFieldReferenceExpression()
                targetField.FieldName = EventCodeGenerator.ToMemberName(eventProperty.Name, True)
                Dim sourceField As New CodeFieldReferenceExpression()
                sourceField.FieldName = EventCodeGenerator.ToMemberName(eventProperty.Name, False, True)
                fromParametersConstructor.Statements.Add(New CodeAssignStatement(targetField, sourceField))
            Next
        End If

        Return fromParametersConstructor

    End Function

    Public Shared Function InterfaceBasedConstructor(eventProperties As IList(Of IEventPropertyEntity), ByVal interfaceTypeName As String, ByVal interfaceName As String) As CodeConstructor

        Dim fromInterfaceConstructor As New CodeConstructor()
        fromInterfaceConstructor.Attributes = (fromInterfaceConstructor.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Public

        'add a Init parameter..
        fromInterfaceConstructor.Parameters.Add(New CodeParameterDeclarationExpression(interfaceTypeName, interfaceName))

        If (eventProperties IsNot Nothing) Then
            For Each eventProperty As IEventPropertyEntity In eventProperties
                ' add an assignment from this parameter to the internal field
                Dim targetField As New CodeFieldReferenceExpression()
                targetField.FieldName = EventCodeGenerator.ToMemberName(eventProperty.Name, True)
                Dim sourceField As New CodeFieldReferenceExpression()
                sourceField.TargetObject = New CodeArgumentReferenceExpression(interfaceName)
                sourceField.FieldName = EventCodeGenerator.ToMemberName(eventProperty.Name, False)
                fromInterfaceConstructor.Statements.Add(New CodeAssignStatement(targetField, sourceField))
            Next
        End If

        Return fromInterfaceConstructor
    End Function

    ''' <summary>
    ''' Returns a call to invoke the given event class using the parameters passed in
    ''' </summary>
    ''' <param name="classToInvoke">
    ''' The reference to the class to invoke the constructor on
    ''' </param>
    ''' <param name="eventProperties">
    ''' The properties that will be turned into parameters 
    ''' </param>
    ''' <returns></returns>
    Public Shared Function EventConstructor(ByVal classToInvoke As CodeTypeReference, eventProperties As IList(Of IEventPropertyEntity)) As CodeObjectCreateExpression

        Dim objCreate As New CodeObjectCreateExpression(classToInvoke)
        For Each prop As IEventPropertyEntity In eventProperties
            Dim sourceField As New CodeFieldReferenceExpression()
            sourceField.FieldName = EventCodeGenerator.ToMemberName(prop.Name, False, True)
            objCreate.Parameters.Add(sourceField)
        Next

        Return objCreate

    End Function

    ''' <summary>
    ''' Create a serialization constructor that fills all the properties from the serialisation context
    ''' </summary>
    ''' <param name="eventProperties">
    ''' The list of properties in the event definition to be read from the serialization info 
    ''' </param>
    ''' <returns>
    ''' Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
    ''' </returns>
    Public Shared Function SerializationConstructor(eventProperties As IEnumerable(Of IEventPropertyEntity)) As CodeConstructor

        Dim serialiseConstructor As New CodeConstructor()
        serialiseConstructor.Attributes += MemberAttributes.Public

        'add a info parameter..
        serialiseConstructor.Parameters.Add(New CodeParameterDeclarationExpression("SerializationInfo", "info"))
        'add a context parameter..
        serialiseConstructor.Parameters.Add(New CodeParameterDeclarationExpression("StreamingContext", "context"))

        'populate the properties
        For Each evtProperty In eventProperties
            'Define the assignment target e.g. m_eventOneStringProperty 
            Dim targetField As New CodeFieldReferenceExpression()
            targetField.FieldName = EventCodeGenerator.ToMemberName(evtProperty.Name, True)

            Dim sourceField As CodeMethodInvokeExpression

            Select Case evtProperty.DataType
                Case PropertyDataType.Boolean
                    sourceField = New CodeMethodInvokeExpression(
                      New CodeMethodReferenceExpression(New CodeArgumentReferenceExpression("info"), "GetBoolean"),
                      {New CodePrimitiveExpression(EventCodeGenerator.ToMemberName(evtProperty.Name, False))})
                Case PropertyDataType.Decimal
                    sourceField = New CodeMethodInvokeExpression(
                      New CodeMethodReferenceExpression(New CodeArgumentReferenceExpression("info"), "GetDecimal"),
                      {New CodePrimitiveExpression(EventCodeGenerator.ToMemberName(evtProperty.Name, False))})
                Case PropertyDataType.Date
                    sourceField = New CodeMethodInvokeExpression(
                      New CodeMethodReferenceExpression(New CodeArgumentReferenceExpression("info"), "GetDateTime"),
                      {New CodePrimitiveExpression(EventCodeGenerator.ToMemberName(evtProperty.Name, False))})
                Case PropertyDataType.String
                    sourceField = New CodeMethodInvokeExpression(
                      New CodeMethodReferenceExpression(New CodeArgumentReferenceExpression("info"), "GetString"),
                      {New CodePrimitiveExpression(EventCodeGenerator.ToMemberName(evtProperty.Name, False))})
                Case PropertyDataType.Integer
                    sourceField = New CodeMethodInvokeExpression(
                      New CodeMethodReferenceExpression(New CodeArgumentReferenceExpression("info"), "GetInt32"),
                      {New CodePrimitiveExpression(EventCodeGenerator.ToMemberName(evtProperty.Name, False))})
                Case Else
                    'Define the assignment source e.g. info.GetValue("EventOneStringProperty", GetType(String))
                    sourceField = New CodeMethodInvokeExpression(
                        New CodeMethodReferenceExpression(New CodeArgumentReferenceExpression("info"), "GetValue"),
                        {New CodePrimitiveExpression(EventCodeGenerator.ToMemberName(evtProperty.Name, False)),
                        New CodeMethodInvokeExpression(New CodeMethodReferenceExpression(Nothing, "GetType"), {
                         New CodeTypeReferenceExpression(EventCodeGenerator.ToMemberType(evtProperty.DataType))
                        })
                        })
            End Select



            'Add the assigmnemt to the constructor body
            serialiseConstructor.Statements.Add(New CodeAssignStatement(targetField, sourceField))
        Next

        Return serialiseConstructor

    End Function
End Class

Public Class MethodCodeGenerator

    ''' <summary>
    ''' Return the definition of a parameterised function 
    ''' </summary>
    ''' <param name="functionName">
    ''' The name to use for this function
    ''' </param>
    ''' <param name="returnType">
    ''' The data type to be returned from the function
    ''' </param>
    ''' <param name="parameters">
    ''' The set of input parameters for the function
    ''' </param>
    Public Shared Function PublicParameterisedFunction(ByVal functionName As String, ByVal returnType As CodeTypeReference, ByVal parameters As IList(Of CodeParameterDeclarationExpression)) As CodeMemberMethod

        Dim ret As New CodeMemberMethod()
        ret.Name = functionName
        ret.ReturnType = returnType
        ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Public
        If (parameters IsNot Nothing) Then
            If (parameters.Count > 0) Then
                ret.Parameters.AddRange(parameters.ToArray)
            End If
        End If


        Return ret

    End Function

    ''' <summary>
    ''' Return the definition of a parameterised subroutine (void function) 
    ''' </summary>
    ''' <param name="functionName">
    ''' The name to use for this function
    ''' </param>
    ''' <param name="parameters">
    ''' The set of input parameters for the function
    ''' </param>
    ''' <param name="makeStatic">
    ''' Whether to make this a static method or not
    ''' </param>
    Public Shared Function PublicParameterisedSub(ByVal functionName As String,
                                                  ByVal parameters As IList(Of CodeParameterDeclarationExpression),
                                                  Optional ByVal makeStatic As Boolean = False,
                                                  Optional ByVal genericTypeRestrictions As CodeTypeParameterCollection = Nothing) As CodeMemberMethod

        Return PublicParameterisedFunction(functionName, parameters, makeStatic:=makeStatic, genericTypeRestrictions:=genericTypeRestrictions)

    End Function

    ''' <summary>
    ''' Create a public function with the given name and properties
    ''' </summary>
    ''' <param name="functionName">
    ''' The name of the function
    ''' </param>
    ''' <param name="parameters">
    ''' The parameters to apply
    ''' </param>
    ''' <param name="returnType">
    ''' The data type the function returns (if not set this will return void and thereby be a subroutine)
    ''' </param>
    ''' <param name="makeStatic">
    ''' If set make this a static function
    ''' </param>
    ''' <param name="genericTypeRestrictions">
    ''' If set, apply these generic type restrictions to the function
    ''' </param>
    Public Shared Function PublicParameterisedFunction(ByVal functionName As String,
                                              ByVal parameters As IList(Of CodeParameterDeclarationExpression),
                                              Optional ByVal returnType As CodeTypeReference = Nothing,
                                              Optional ByVal makeStatic As Boolean = False,
                                              Optional ByVal genericTypeRestrictions As CodeTypeParameterCollection = Nothing) As CodeMemberMethod

        Dim ret As New CodeMemberMethod()
        ret.Name = functionName
        ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Public
        If (makeStatic) Then
            ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Static
        End If
        If (returnType IsNot Nothing) Then
            ret.ReturnType = returnType
        End If
        If (parameters IsNot Nothing) Then
            If (parameters.Count > 0) Then
                ret.Parameters.AddRange(parameters.ToArray)
            End If
        End If
        If (genericTypeRestrictions IsNot Nothing) Then
            ret.TypeParameters.AddRange(genericTypeRestrictions)
        End If

        Return ret

    End Function

    Public Shared Function PrivateParameterisedSub(ByVal functionName As String,
                                                  ByVal parameters As IList(Of CodeParameterDeclarationExpression),
                                                  Optional ByVal makeStatic As Boolean = False) As CodeMemberMethod

        Return PrivateParameterisedFunction(functionName, parameters, makeStatic:=makeStatic)

    End Function

    Public Shared Function PrivateParameterisedFunction(ByVal functionName As String,
                                          ByVal parameters As IList(Of CodeParameterDeclarationExpression),
                                          Optional ByVal returnType As CodeTypeReference = Nothing,
                                          Optional ByVal makeStatic As Boolean = False) As CodeMemberMethod

        Dim ret As New CodeMemberMethod()
        ret.Name = functionName
        ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Private
        If (makeStatic) Then
            ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Static
        End If
        If (returnType IsNot Nothing) Then
            ret.ReturnType = returnType
        End If
        If (parameters IsNot Nothing) Then
            If (parameters.Count > 0) Then
                ret.Parameters.AddRange(parameters.ToArray)
            End If
        End If


        Return ret

    End Function

    Public Shared Sub MakeOverrides(ByRef methodToMakeOverride As CodeMemberMethod)

        methodToMakeOverride.Attributes = (methodToMakeOverride.Attributes And (Not MemberAttributes.ScopeMask)) Or MemberAttributes.Override

    End Sub

    Public Shared Sub MakeOverrides(ByRef propertyToMakeOverride As CodeMemberProperty)

        propertyToMakeOverride.Attributes = (propertyToMakeOverride.Attributes And (Not MemberAttributes.ScopeMask)) Or MemberAttributes.Override

    End Sub

    Public Shared Function MakeTypeParameter(ByVal name As String,
                                             Optional ByVal constraints As IList(Of CodeTypeReference) = Nothing) As CodeTypeParameter

        Dim ret As New CodeTypeParameter(name)
        If (constraints IsNot Nothing) Then
            ret.Constraints.AddRange(constraints.ToArray())
        End If
        Return ret

    End Function

End Class

Public Class AttributeCodeGenerator


#Region "Defined attributes"
    Public Const ATTRIBUTENAME_DOMAIN As String = "CQRSAzure.EventSourcing.DomainNameAttribute"
    Public Const ATTRIBUTENAME_CATEGORY As String = "CQRSAzure.EventSourcing.Category"
    Public Const ATTRIBUTENAME_AGGREGATE_KEY As String = "CQRSAzure.EventSourcing.AggregagteKey"
    Public Const ATTRIBUTENAME_IDENTITY_GROUP As String = "CQRSAzure.EventSourcing.IdentityGroup"
#End Region

    ''' <summary>
    ''' Create a parameterised attribute to tag a class or property
    ''' </summary>
    ''' <param name="AttributeName">
    ''' The attribute name to use - this should be a fully qualified name if ambiguity is possible
    ''' </param>
    ''' <param name="parameters">
    ''' The set of parameters to use to construct the attribute 
    ''' </param>
    ''' <returns></returns>
    Public Shared Function ParameterisedAttribute(ByVal AttributeName As String,
                                                  ByVal parameters As IList(Of CodeAttributeArgument)
                                                  ) As CodeAttributeDeclaration



        If parameters IsNot Nothing Then
            Return New CodeAttributeDeclaration(AttributeName, parameters.ToArray())
        Else
            Return New CodeAttributeDeclaration(AttributeName)
        End If


    End Function


    Public Shared Function SerializableAttribute() As CodeAttributeDeclaration

        Return New CodeAttributeDeclaration("Serializable")

    End Function
End Class

Public Class ParameterPropertyDefinition
    Implements CustomCode.Interfaces.IEventPropertyEntity


    Private m_DataType As PropertyDataType
    Public Property DataType As PropertyDataType Implements IEventPropertyEntity.DataType
        Get
            Return m_DataType
        End Get
        Set(value As PropertyDataType)
            m_DataType = value
        End Set
    End Property

    Private m_description As String
    Public Property Description As String Implements IDocumentedEntity.Description
        Get
            Return m_description
        End Get
        Set(value As String)
            m_description = value
        End Set
    End Property

    Private m_name As String
    Public Property Name As String Implements INamedEntity.Name
        Get
            Return m_name
        End Get
        Set(value As String)
            m_name = value
        End Set
    End Property

    Private m_notes As String
    Public Property Notes As String Implements IDocumentedEntity.Notes
        Get
            Return m_notes
        End Get
        Set(value As String)
            m_notes = value
        End Set
    End Property

    Public ReadOnly Property FullyQualifiedName As String Implements INamedEntity.FullyQualifiedName
        Get
            Return ""
        End Get
    End Property

    Private m_isEffectiveDate As Boolean
    Public Property IsEffectiveDate As Boolean Implements IEventPropertyEntity.IsEffectiveDate
        Get
            Return m_isEffectiveDate
        End Get
        Set(value As Boolean)
            m_isEffectiveDate = value
        End Set
    End Property

    Public Sub New(ByVal nameIn As String, ByVal dataTypeIn As PropertyDataType)
        m_name = nameIn
        m_DataType = dataTypeIn
    End Sub
End Class