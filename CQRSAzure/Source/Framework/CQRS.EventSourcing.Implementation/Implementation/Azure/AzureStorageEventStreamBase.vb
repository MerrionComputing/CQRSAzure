Imports System.Configuration
Imports CQRSAzure.EventSourcing.Azure
Imports Microsoft.WindowsAzure.Storage

Imports Microsoft.Extensions.Configuration
Imports System.IO

''' <summary>
''' Common functionality that both reader and writer use to access any event store based on Azure storage
''' </summary>
Public MustInherit Class AzureStorageEventStreamBase
    Inherits EventStreamBase

#Region "Private members"
    Private ReadOnly m_domainName As String
    Protected Friend ReadOnly m_storageAccount As CloudStorageAccount
#End Region

    ''' <summary>
    ''' The name of the domain model this event stream belongs to
    ''' </summary>
    ''' <remarks>
    ''' This allows multiple domains to share the same Azure storage account/area
    ''' </remarks>
    Protected ReadOnly Property DomainName As String
        Get
            Return m_domainName
        End Get
    End Property

    ''' <summary>
    ''' Gets the storage connection string settting's name for this domain
    ''' </summary>
    ''' <returns>
    ''' e.g. [Domain]StorageConnectionString
    ''' </returns>
    Protected ReadOnly Property StorageConnectionStringSettingName As String
        Get
            Dim invalidCharacters As Char() = " _!,.;':@£$%^&*()+=/\#~{}[]?<>"
            Return String.Join("", DomainName.Split(invalidCharacters)).Trim() & "StorageConnectionString"
        End Get
    End Property

    Protected Sub New(ByVal AggregateDomainName As String,
                      Optional ByVal writeAccess As Boolean = False,
                      Optional ByVal connectionStringName As String = "",
                      Optional ByVal settings As IAzureStorageSettings = Nothing)



        m_domainName = AggregateDomainName

        If (String.IsNullOrWhiteSpace(connectionStringName)) Then
            connectionStringName = StorageConnectionStringSettingName
        End If

        'Create a connection to the cloud storage account to use
        Dim builder As New ConfigurationBuilder()
        builder.SetBasePath(Directory.GetCurrentDirectory())
        builder.AddJsonFile("appsettings.json", True)
        builder.AddJsonFile("config.local.json", True)
        builder.AddJsonFile("config.json", True)
        builder.AddJsonFile("connectionstrings.json", True)
        builder.AddEnvironmentVariables()

        Dim config As IConfigurationRoot = builder.Build()

        Dim connectionString As String = ""


        If (config IsNot Nothing) Then
            connectionString = config.GetConnectionString(connectionStringName)
            If String.IsNullOrWhiteSpace(connectionString) Then
                connectionString = config.Item(connectionStringName)
            End If
        End If

        If (String.IsNullOrWhiteSpace(connectionString)) Then
            If Not String.IsNullOrWhiteSpace(connectionStringName) Then
                If (ConfigurationManager.ConnectionStrings IsNot Nothing) Then
                    'Fall back on local settings file
                    Dim settingsInUse = ConfigurationManager.ConnectionStrings(connectionStringName)
                    If (settingsInUse IsNot Nothing) Then
                        connectionString = settingsInUse.ConnectionString
                    Else
                        Throw New EventStreamUnderlyingStorageUnavailableException(AggregateDomainName, "All",
                                                                                   NameOf(AzureStorageEventStreamBase),
                                                                                   "No connection string named " & connectionStringName & " in application config")
                    End If
                Else
                    Throw New EventStreamUnderlyingStorageUnavailableException(AggregateDomainName, "All",
                                                                               NameOf(AzureStorageEventStreamBase),
                                                                               "No connection strings in application config")
                End If
            Else
                'Indicate that thisreader does not have a valid connection string - throw an error
                Throw New EventStreamUnderlyingStorageUnavailableException(AggregateDomainName, "All",
                                                                           NameOf(AzureStorageEventStreamBase),
                                                                           "Missing connection String")
            End If
        End If

        m_storageAccount = CloudStorageAccount.Parse(connectionString)

        If (m_storageAccount Is Nothing) Then
            Throw New EventStreamUnderlyingStorageUnavailableException(AggregateDomainName, "All",
                                                                               NameOf(AzureStorageEventStreamBase),
                                                                               "Unable to create storage account from " & connectionString)
        End If


    End Sub

    ''' <summary>
    ''' Get the name of the connection string to use when writing to the event stream
    ''' </summary>
    ''' <param name="connectionStringName">
    ''' The name of the global connection string explicitly passed in
    ''' </param>
    ''' <param name="settings">
    ''' Any specif settings that could override that connection string
    ''' </param>
    Protected Shared Function GetWriteConnectionStringName(ByVal connectionStringName As String,
                                                           ByVal settings As IAzureStorageSettings) As String

        If (settings Is Nothing) Then
            Return connectionStringName
        Else
            If Not String.IsNullOrWhiteSpace(settings.ConnectionStringName) Then
                Return settings.ConnectionStringName
            Else
                Return connectionStringName
            End If
        End If

    End Function

    ''' <summary>
    ''' Get the name of the connection string to use when reading from the event stream
    ''' </summary>
    ''' <param name="connectionStringName">
    ''' The name of the global connection string explicitly passed in
    ''' </param>
    ''' <param name="settings">
    ''' Any specific settings that could override that connection string
    ''' </param>
    Protected Shared Function GetReadConnectionStringName(ByVal connectionStringName As String,
                                                           ByVal settings As IAzureStorageSettings) As String

        If (settings Is Nothing) Then
            Return connectionStringName
        Else
            If Not String.IsNullOrWhiteSpace(settings.ReadSideConnectionStringName) Then
                Return settings.ReadSideConnectionStringName
            ElseIf Not String.IsNullOrWhiteSpace(settings.ConnectionStringName) Then
                Return settings.ConnectionStringName
            Else
                Return connectionStringName
            End If
        End If

    End Function


End Class
