Imports Microsoft.WindowsAzure.Storage.File
Imports Microsoft.WindowsAzure.Storage.Auth
Imports Microsoft.WindowsAzure
Imports Microsoft.Azure
Imports System.Configuration
Imports Microsoft.WindowsAzure.Storage

Namespace Azure.File

    ''' <summary>
    ''' Common functionality that both reader and writer use to access the file based event store on the Azure storage for a specified
    ''' aggregation type
    ''' </summary>
    Public MustInherit Class FileEventStreamBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits FileEventStreamBase

        Protected ReadOnly m_key As TAggregateKey
        Private ReadOnly m_converter As IKeyConverter(Of TAggregateKey)
        Private ReadOnly m_directory As CloudFileDirectory
        Private ReadOnly m_file As CloudFile

        Private m_aggregatename As String
        Protected ReadOnly Property AggregateClassName As String
            Get
                If (String.IsNullOrWhiteSpace(m_aggregatename)) Then
                    Dim typeName As String = GetType(TAggregate).Name
                    m_aggregatename = MakeValidStorageFolderName(typeName)
                End If
                Return m_aggregatename
            End Get
        End Property

        Protected ReadOnly Property EventStreamFilename As String
            Get
                Return MakeValidStorageFolderName(m_converter.ToUniqueString(m_key)) & ".eventstream"
            End Get
        End Property

        ''' <summary>
        ''' The file underlying the event stream
        ''' </summary>
        Protected ReadOnly Property File As CloudFile
            Get
                Return m_file
            End Get
        End Property

        ''' <summary>
        ''' Create a new base for a reader or writer class in the given domain
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The name of the domain to store/retrieve the event streams under
        ''' </param>
        Protected Sub New(ByVal AggregateDomainName As String,
                          ByVal AggregateKey As TAggregateKey,
                          Optional ByVal writeAccess As Boolean = False,
                          Optional ByVal connectionStringName As String = "",
                          Optional ByVal settings As IFileStreamSettings = Nothing)

            MyBase.New(AggregateDomainName, writeAccess, connectionStringName, settings)

            'Get the aggregation instance key to use when creating a blob file name
            m_key = AggregateKey
            m_converter = KeyConverterFactory.CreateKeyConverter(Of TAggregateKey)

            If (BaseFileShare IsNot Nothing) Then
                'Get the directory for the aggregation type
                m_directory = BaseFileShare.GetRootDirectoryReference().GetDirectoryReference(FileEventStreamBase.MakeValidStorageFolderName(AggregateClassName))
                m_directory.CreateIfNotExists()
                'and from that, get the instance file
                m_file = m_directory.GetFileReference(EventStreamFilename)
                ResetFile()
            End If



        End Sub

        Protected Function GetSequence() As Long

            If (File IsNot Nothing) Then
                File.FetchAttributes()
                Dim m_sequence As Long = 0
                If (File.Metadata.ContainsKey(METADATA_SEQUENCE)) Then
                    If (Long.TryParse(File.Metadata(METADATA_SEQUENCE), m_sequence)) Then
                        Return m_sequence
                    End If
                End If
            End If

            Return 0

        End Function

        Protected Sub SetSequence(startingSequence As UInteger)

            If (File IsNot Nothing) Then
                File.FetchAttributes()
                If (File.Metadata.ContainsKey(METADATA_SEQUENCE)) Then
                    m_file.Metadata(METADATA_SEQUENCE) = startingSequence.ToString()
                End If
                File.SetMetadata()
            End If

        End Sub

        Friend Sub ResetFile()

            If Not (m_file.Exists) Then
                m_file.Create(m_configuredFileSize)
                'Set the initial metadata
                m_file.Metadata(METATDATA_DOMAIN) = DomainName
                m_file.Metadata(METADATA_AGGREGATE_CLASS) = GetType(TAggregate).Name
                m_file.Metadata(METADATA_AGGREGATE_KEY) = m_key.ToString()
                m_file.Metadata(METADATA_SEQUENCE) = "0" 'Sequence starts at zero
                m_file.Metadata(METADATA_RECORD_COUNT) = "0" 'Record count starts at zero
                m_file.Metadata(METADATA_DATE_CREATED) = DateTime.UtcNow.ToString("O") 'use universal date/time
                m_file.SetMetadata()
            Else
                'set the current sequence number
                If (Not File.Metadata.ContainsKey(METADATA_SEQUENCE)) Then
                    Throw New EventStreamReadException(DomainName, GetType(TAggregate).Name, m_key.ToString(), 0, "Unable to get the sequence number for this event stream file")
                End If
            End If

        End Sub

        Protected Function GetRecordCount()

            If (File IsNot Nothing) Then
                File.FetchAttributes()
                Dim m_records As Long = 0
                If (File.Metadata.ContainsKey(METADATA_RECORD_COUNT)) Then
                    If (Long.TryParse(File.Metadata(METADATA_RECORD_COUNT), m_records)) Then
                        Return m_records
                    End If
                End If
            End If

            Return 0
        End Function

        Protected Function GetAllStreamKeysBase(asOfDate As Date?) As IEnumerable(Of TAggregateKey)

            'list all the files in this folder...
            Dim ret As New List(Of TAggregateKey)
            If (m_directory IsNot Nothing) Then
                'List all the files in the folder
                For Each streamFileEntry In m_directory.ListFilesAndDirectories()
                    Dim streamFile As CloudFile = TryCast(streamFileEntry, CloudFile)
                    'streamFile.Properties.
                    If (streamFile IsNot Nothing) Then
                        Dim ignore As Boolean = False
                        streamFile.FetchAttributes()
                        If (asOfDate.HasValue) Then
                            'compare to file create time
                            'if the creation date is after as-of-date then skip it
                            Dim creationDate As DateTime
                            If (streamFile.Metadata.ContainsKey(METADATA_DATE_CREATED)) Then
                                If DateTime.TryParseExact(streamFile.Metadata(METADATA_DATE_CREATED), "O", Nothing, Globalization.DateTimeStyles.None, creationDate) Then
                                    If (creationDate.Year > 1900) Then
                                        If (creationDate < asOfDate.Value) Then
                                            ignore = True
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        If Not ignore Then
                            'add the key to the returned set
                            Dim keyString As String = streamFile.Metadata(METADATA_AGGREGATE_KEY)
                            If Not String.IsNullOrWhiteSpace(keyString) Then
                                'Try and turn it to the TAggregateKey
                                If (GetType(TAggregateKey) Is GetType(String)) Then
                                    ret.Add(CTypeDynamic(Of TAggregateKey)(keyString))
                                Else
                                    'need to convert the key as we had to store it as a string
                                    ret.Add(m_converter.FromString(keyString))
                                End If
                            End If
                        End If
                    End If
                Next
            End If
            Return ret
        End Function
    End Class


    ''' <summary>
    ''' Common functionality that both reader and writer use to access the file based event store on the Azure storage
    ''' </summary>
    Public MustInherit Class FileEventStreamBase
        Inherits AzureStorageEventStreamBase

#Region "private members"
        Private ReadOnly m_fileClient As CloudFileClient
        Private ReadOnly m_cloudBasePath As CloudFileShare

        Protected ReadOnly m_configuredFileSize As Long
#End Region


        Public Const METATDATA_DOMAIN As String = "DOMAIN"
        Public Const METADATA_AGGREGATE_CLASS As String = "AGGREGATECLASS"
        Public Const METADATA_SEQUENCE As String = "SEQUENCE"
        Public Const METADATA_RECORD_COUNT As String = "RECORDCOUNT"
        Public Const METADATA_AGGREGATE_KEY As String = "AGGREGATEKEY"
        Public Const METADATA_DATE_CREATED As String = "DATECREATED"

        Public Const ORPHANS_FOLDER As String = "uncategorised"
        Public Const EVENTSTREAM_FOLDER As String = "eventstreams"
        Public Const SNAPSHOTS_FOLDER As String = "snapshots"

        ' Options for setting your max size for an event stream
        Public Const GIGABYTE As Long = 1073741824
        Public Const MEGABYTE As Long = 1048576

        Public Const MAX_STREAM_SIZE As Long = MEGABYTE ' Max size is set to 1MB in this implementation - although Azure can go up to and enourmous 1Tb if needed
        Public Const MAX_SHARE_QUOTA As Long = 10


        Protected ReadOnly Property BaseFileShare As CloudFileShare
            Get
                Return m_cloudBasePath
            End Get
        End Property

        ''' <summary>
        ''' Turn a name into a valid folder name for azure file storage
        ''' </summary>
        ''' <param name="rawName">
        ''' The name of the thing we want to turn into a file storage folder name
        ''' </param>
        ''' <returns>
        ''' A folder name that can be used to locate this object type's event streams
        ''' </returns>
        ''' <remarks>
        ''' Container names must start With a letter Or number, And can contain only letters, numbers, And the dash (-) character.
        ''' Every dash (-) character must be immediately preceded And followed by a letter Or number; consecutive dashes are Not permitted in container names.
        ''' All letters in a container name must be lowercase.
        ''' Container names must be from 3 through 63 characters long.
        ''' </remarks>
        Public Shared Function MakeValidStorageFolderName(ByVal rawName As String) As String

            If (String.IsNullOrWhiteSpace(rawName)) Then
                ' Don't allow empty folder names - assign an orphan folder for it
                Return ORPHANS_FOLDER
            Else
                Dim invalidCharacters As Char() = " _!,.;':@£$%^&*()+=/\#~{}[]?<>"
                Dim cleanName As String = String.Join("-", rawName.Split(invalidCharacters))
                If (cleanName.StartsWith("-")) Then
                    cleanName = cleanName.TrimStart("-")
                End If
                If (cleanName.EndsWith("-")) Then
                    cleanName = cleanName.TrimEnd("-")
                End If
                If (cleanName.Length < 3) Then
                    cleanName += "-abc"
                End If
                If (cleanName.Length > 63) Then
                    Dim uniqueness As Integer = cleanName.GetHashCode()
                    Dim uniqueid As String = uniqueness.ToString()
                    cleanName = cleanName.Substring(0, 63 - uniqueid.Length) & uniqueid
                End If
                cleanName = cleanName.Replace("--", "-")
                Return cleanName.ToLower()

            End If

        End Function

        Private ReadOnly Property DefaultRequestOptions As FileRequestOptions
            Get
                Return New FileRequestOptions() With {.RetryPolicy = New RetryPolicies.ExponentialRetry()}
            End Get
        End Property

        ''' <summary>
        ''' Create a new base for a reader or writer class in the given domain
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The name of the domain to store/retrieve the event streams under
        ''' </param>
        Protected Sub New(ByVal AggregateDomainName As String,
                          Optional ByVal writeAccess As Boolean = False,
                          Optional ByVal connectionStringName As String = "",
                          Optional ByVal settings As IFileStreamSettings = Nothing)

            MyBase.New(AggregateDomainName, writeAccess, connectionStringName, settings)


            If (settings IsNot Nothing) Then
                connectionStringName = settings.ConnectionStringName
                m_configuredFileSize = settings.InitialSize
            Else
                m_configuredFileSize = MAX_STREAM_SIZE
            End If
            If (String.IsNullOrWhiteSpace(connectionStringName)) Then
                connectionStringName = StorageConnectionStringSettingName
            End If


            If (m_storageAccount IsNot Nothing) Then
                m_fileClient = m_storageAccount.CreateCloudFileClient()
                If (m_fileClient IsNot Nothing) Then
                    'Create the reference to this aggregate type's event stream base folder
                    'e.g. /[domain]/
                    m_cloudBasePath = m_fileClient.GetShareReference(MakeValidStorageFolderName(DomainName))
                    If Not (m_cloudBasePath.Exists) Then
                        m_cloudBasePath.CreateIfNotExists()
                    End If
                End If
            End If
        End Sub


    End Class
End Namespace