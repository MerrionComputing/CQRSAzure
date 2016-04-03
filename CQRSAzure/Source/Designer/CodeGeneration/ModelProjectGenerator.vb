''' <summary>
''' A class to generate a .csprj or .vbprj for a subset of the files
''' generated 
''' </summary>
''' <remarks>
''' There are no CodeDom classes for this so this is just a fudge xml writer that makes
''' files that look like Visual Studio projects
''' </remarks>
Public Class ModelProjectGenerator

    Public Enum ProjectClassification
        ''' <summary>
        ''' The definition of commands into the domain
        ''' </summary>
        ''' <remarks>
        ''' This can be used to define an API 
        ''' </remarks>
        CommandDefinition
        ''' <summary>
        ''' The processing of commands in the domain
        ''' </summary>
        ''' <remarks>
        ''' This should only be deployed on the server(s) side
        ''' </remarks>
        CommandHandler
        ''' <summary>
        ''' The definition of queries out of the domain
        ''' </summary>
        ''' <remarks>
        ''' This can be used to define an API
        ''' </remarks>
        QueryDefinition
        ''' <summary>
        ''' The processing of queries on the domain
        ''' </summary>
        ''' <remarks>
        ''' This should only be deployed on the server(s) side
        ''' </remarks>
        QueryHandler
        ''' <summary>
        ''' The events and projections that store the data history of the domain
        ''' </summary>
        EventSourcing
        ''' <summary>
        ''' The classifier and identity groups logic
        ''' </summary>
        ''' <remarks>
        ''' These are kept separate from event sourcing but that is not
        ''' mandatory
        ''' </remarks>
        IdentityGroups
    End Enum

    Private ReadOnly _classification As ProjectClassification
    Public ReadOnly Property Classification As ProjectClassification
        Get
            Return _classification
        End Get
    End Property

    Private ReadOnly _projectName As String
    ''' <summary>
    ''' The base name to save this project file as
    ''' </summary>
    ''' <returns>
    ''' e.g. [domain].events , [domain].[comand] etc.
    ''' </returns>
    Public ReadOnly Property ProjectName As String
        Get
            Return _projectName
        End Get
    End Property

    Public Function GetProjectFilename(ByVal codelanguage As CodeGeneration.ModelCodeGenerationOptions.SupportedLanguages)

        Dim filenameBase As String = ProjectName.Trim() & "." & Classification.ToString()
        Select Case codelanguage
            Case ModelCodeGenerationOptions.SupportedLanguages.CSharp
                Return filenameBase & ".csproj"
            Case ModelCodeGenerationOptions.SupportedLanguages.VBNet
                Return filenameBase & ".vbproj"
        End Select

        Return filenameBase
    End Function


    ' The item groups to include in this project
    Private _groups As New Dictionary(Of ProjectItem.ValidCompileActions, ProjectItemGroup)

    Public Sub AddProjectItem(ByVal action As ProjectItem.ValidCompileActions,
                   ByVal include As String,
                   Optional ByVal dependencyOn As String = "")

        If Not _groups.ContainsKey(action) Then
            _groups.Add(action, New ProjectItemGroup())
        End If

        _groups(action).AddItem(New ProjectItem(action, include, dependencyOn))

    End Sub



    Public Sub New(ByVal ClassificationSetting As ProjectClassification,
                   ByVal ModelName As String)

        _classification = ClassificationSetting
        _projectName = CodeGeneration.ModelCodeGenerator.MakeValidCodeFilenameBase(ModelName)

    End Sub
End Class


Public Class ProjectItemGroup

    Private _items As New List(Of ProjectItem)

    Public Sub AddItem(ByVal item As ProjectItem)
        _items.Add(item)
    End Sub

    Public Overrides Function ToString() As String
        Dim ret As New System.Text.StringBuilder()
        ret.AppendLine("<ItemGroup>")
        For Each item As ProjectItem In _items
            ret.AppendLine("   " & item.ToString())
        Next
        ret.AppendLine("</ItemGroup>")

        Return ret.ToString()
    End Function

End Class

Public Class ProjectItem

    ''' <summary>
    ''' The compile actions that are valid for our model code generation context
    ''' </summary>
    Public Enum ValidCompileActions
        ''' <summary>
        ''' Do nothing but reference the file in the solution
        ''' </summary>
        None = 0
        ''' <summary>
        ''' Reference a namespace
        ''' </summary>
        Reference = 1
        ''' <summary>
        ''' Import a specific namespace
        ''' </summary>
        Import = 2
        ''' <summary>
        ''' Compile the referenced source code item
        ''' </summary>
        Compile = 3
    End Enum

    Private ReadOnly _compileAction As ValidCompileActions

    Private ReadOnly _includeItem As String
    Public ReadOnly Property IncludeItem As String
        Get
            Return _includeItem
        End Get
    End Property

    Private ReadOnly _dependency As String
    Public ReadOnly Property Dependency As String
        Get
            Return _dependency
        End Get
    End Property

    Public Overrides Function ToString() As String

        If (String.IsNullOrWhiteSpace(_dependency)) Then
            Return "<" & _compileAction.ToString() & " Include=""" & _includeItem.Trim() & """ />"
        Else
            Dim ret As New System.Text.StringBuilder()
            ret.AppendLine("<" & _compileAction.ToString() & " Include=""" & _includeItem.Trim() & """ >")
            ret.AppendLine("   <DependentUpon>" & _dependency.Trim() & "</DependentUpon>")
            ret.AppendLine("</" & _compileAction.ToString() & ">")
            Return ret.ToString()
        End If

    End Function

    Public Sub New(ByVal action As ValidCompileActions,
                   ByVal include As String,
                   Optional ByVal dependencyOn As String = "")

        _compileAction = action
        _includeItem = include
        _dependency = dependencyOn
    End Sub

End Class