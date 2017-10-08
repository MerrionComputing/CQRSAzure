''' <summary>
''' Class to be used to generate the project file(s) - eiother .csproj or .vbproj for the CQRS model
''' </summary>
''' <remarks>
''' There are differences between .csproj and .vbproj files that this class needs to deal with
''' </remarks>
Public Class CodeProjectFile

    Public Const ITEMGROUPMEMBER_COMPILE As String = "Compile"
    Public Const ITEMGROUPMEMBER_REFERENCE As String = "Reference"
    Public Const ITEMGROUPMEMBER_IMPORT As String = "Import"

#Region "Standard projects"
    Public Const PROJECTNAME_QUERY_DEFINITION = "QueryDefinition"
    Public const PROJECTNAME_QUERY_HANDLER = "QueryHandler"
    Public Const PROJECTNAME_COMMAND_DEFINITION = "CommandDefinition"
    Public Const PROJECTNAME_COMMAND_HANDLER = "CommandHandler"
    Public Const PROJECTNAME_IDENTITY_GROUP = "IdentityGroup"
    Public Const PROJECTNAME_EVENT_SOURCING = "EventSourcing"
#End Region

#Region "Standard CQRS on Azure framework references"

    Public Const REFERENCE_EVENTSOURCING_INTERFACES As String = "CQRSAzure.EventSourcing"
    Public Const REFERENCE_EVENTSOURCING_IMPLEMENTATION As String = "CQRSAzure.EventSourcing.Implementation"
    Public Const REFERENCE_IDENTITYGROUP As String = "CQRSAzure.IdentifierGroup"

    Public Const REFERENCE_COMMAND_DEFINITION As String = "CQRSAzure.CommandDefinition"
    Public Const REFERENCE_COMMAND_HANDLER As String = "CQRSAzure.CommandHandler"

    Public Const REFERENCE_QUERY_DEFINITION As String = "CQRSAzure.QueryDefinition"
    Public Const REFERENCE_QUERY_HANDLER As String = "CQRSAzure.QueryHandler"

    Public Const REFERENCE_HOSTING As String = "CQRSAzure.Hosting"

#End Region

#Region "Common system requirements"

    Public Const REFERENCE_SYSTEM As String = "System"
    Public Const REFERENCE_SYSTEM_CORE As String = "System.Core"
    Public Const REFERENCE_SYSTEM_REFLECTION As String = "System.Reflection"
    Public Const REFERENCE_SYSTEM_RUNTIME_SERIALIZATION As String = "System.Runtime.Serialization"

#End Region



    Private ReadOnly m_projectGuid As Guid
    ''' <summary>
    ''' The unique identifier of the project
    ''' </summary>
    Public ReadOnly Property ProjectGuid As Guid
        Get
            Return m_projectGuid
        End Get
    End Property

    Private ReadOnly m_projectName As String
    ''' <summary>
    ''' The base name that will be the name of the project file 
    ''' </summary>
    ''' <remarks>
    ''' This will need a .vbproj or .csproj suffix as appropriate
    ''' </remarks>
    Public ReadOnly Property ProjectName As String
        Get
            Return m_projectName
        End Get
    End Property

    Private m_includeSourceFilenames As New List(Of String)
    Private m_includeReferences As New List(Of String)

    ''' <summary>
    ''' The set of source filenames to compile in this project
    ''' </summary>
    Public ReadOnly Property IncludedSourceFilenames As List(Of String)
        Get
            Return m_includeSourceFilenames
        End Get
    End Property

    ''' <summary>
    ''' The list of references in the project
    ''' </summary>
    Public ReadOnly Property IncludedReferences As List(Of String)
        Get
            Return m_includeReferences
        End Get
    End Property

    ''' <summary>
    ''' Add a newly generated source file to the project source files list
    ''' </summary>
    ''' <param name="filename">
    ''' The name of the generated source filename
    ''' </param>
    ''' <remarks>
    ''' These are added to an item group using Compile Include="" 
    ''' </remarks>
    Public Sub AddSourceFile(ByVal filename As String)
        If (Not m_includeSourceFilenames.Contains(filename)) Then
            m_includeSourceFilenames.Add(filename)
        End If
    End Sub

    ''' <summary>
    ''' Add a required reference to the set of references in the project
    ''' </summary>
    ''' <param name="referenceName">
    ''' The fully qualified name of the reference to add
    ''' </param>
    ''' <remarks>
    ''' No hit path is specified as that would vary between machines
    ''' </remarks>
    Public Sub AddReference(ByVal referenceName As String)
        If (Not m_includeReferences.Contains(referenceName)) Then
            m_includeReferences.Add(referenceName)
        End If
    End Sub

    Public Shared Function GetHintPath(ByVal referenceName As String) As String

        If referenceName.StartsWith("CQRSAzure") Then
            Return referenceName.Trim() & ".dll"
        End If

        Return ""

    End Function

    Public Sub New(ByVal projectNameBase As String)
        m_projectGuid = Guid.NewGuid()
        m_projectName = projectNameBase
    End Sub

End Class
