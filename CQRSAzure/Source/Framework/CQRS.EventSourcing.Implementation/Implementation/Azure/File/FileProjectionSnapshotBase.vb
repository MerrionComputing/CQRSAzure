Imports Microsoft.WindowsAzure.Storage.File
Imports Microsoft.WindowsAzure.Storage.Auth
Imports Microsoft.WindowsAzure
Imports Microsoft.Azure
Imports System.Configuration
Imports Microsoft.WindowsAzure.Storage
Imports System.Collections.Generic
Imports System
Imports CQRSAzure.EventSourcing.Azure.File

Namespace Azure.File

    ''' <summary>
    ''' Common functionality that both reader and writer use to access the file based projection snapshots on the Azure storage for a specified
    ''' aggregation type
    ''' </summary>
    ''' <remarks>
    ''' There is only one snapshot stored in any given file
    ''' The file path consists of the projection name and snapshot sequence number (incremental)
    ''' Therefore getting the latest snapshot means doing a directory listing of the [aggregate]\[key]\snapshots\[projection name]\ folder
    ''' e.g.
    ''' [Aggregate]\[key]\snapshots\[projection]\103.[asofdate] is snapshot as at seqeuence # 103
    ''' </remarks>
    Public MustInherit Class FileProjectionSnapshotBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                            TAggregateKey,
                                                            TProjection As IProjection)
        Inherits FileProjectionSnapshotBase

#Region "private members"
        Private ReadOnly m_directory As CloudFileDirectory
        Private ReadOnly m_cloudBasePath As CloudFileShare
        Private ReadOnly m_key As TAggregateKey
#End Region

        Protected ReadOnly Property AggregateClassName As String
            Get
                Dim typeName As String = GetType(TAggregate).Name
                Return FileEventStreamBase.MakeValidStorageFolderName(typeName)
            End Get
        End Property

        Protected ReadOnly Property ProjectionClassName As String
            Get
                Dim typeName As String = GetType(TProjection).Name
                Return FileEventStreamBase.MakeValidStorageFolderName(typeName)
            End Get
        End Property

        ''' <summary>
        ''' The root path that this aggregate/projection combination will store its snapshot records in
        ''' e.g.
        ''' [Aggregate]/[key]/snapshots/[projection]/
        ''' </summary>
        Protected ReadOnly Property SnapshotsDirectory As CloudFileDirectory
            Get
                Return m_directory
            End Get
        End Property

        ''' <summary>
        ''' The root path that this aggregate/projection combination will store its snapshot records in
        ''' e.g.
        ''' [Aggregate]/[key]/snapshots/[projection]/
        ''' </summary>
        ''' <remarks>
        ''' This can be used to make an URI directly into that directory
        ''' </remarks>
        Protected ReadOnly Property SnapshotBasePath As String
            Get
                Return FileEventStreamBase.MakeValidStorageFolderName(AggregateClassName) & "/" _
                       & FileEventStreamBase.MakeValidStorageFolderName(m_key.ToString()) & "/" _
                       & FileEventStreamBase.SNAPSHOTS_FOLDER & "/" _
                       & FileEventStreamBase.MakeValidStorageFolderName(ProjectionClassName) & "/"
            End Get
        End Property

        ''' <summary>
        ''' Make a filename to use to store the given snapshot in
        ''' </summary>
        ''' <param name="snapshotSequence">
        ''' The sequence number the snapshot is effective for
        ''' </param>
        ''' <param name="snapshotAsOfDate">
        ''' The effective date the snapshot is effective for
        ''' </param>
        ''' <returns>
        ''' a filename like 00000000203.20160715113910 that uniquely identifies the sequence and effective date of a snapshot
        ''' </returns>
        Public Function MakeSnapshotFilename(ByVal snapshotSequence As UInteger, ByVal snapshotAsOfDate As DateTime) As String

            Return FormatSequenceNumber(snapshotSequence) & "." & FormatSequenceAsOfDate(snapshotAsOfDate)

        End Function



        ''' <summary>
        ''' List the snapshot files stored for this aggregate key
        ''' </summary>
        ''' <returns></returns>
        Protected Async Function ListSnapshotFiles() As Task(Of IEnumerable(Of IListFileItem))

            If (m_directory IsNot Nothing) Then
                Dim continueToken As New FileContinuationToken()
                Dim files = Await m_directory.ListFilesAndDirectoriesSegmentedAsync(continueToken)
                Return files.Results
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, m_key.ToString(), 0, "Missing snapshots directory")
            End If

        End Function



        ''' <summary>
        ''' Create a new base for a projection snapshot reader or writer class in the given domain
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

            m_key = AggregateKey

            If (MyBase.FileClient IsNot Nothing) Then
                'Create the reference to this aggregate type's event stream base folder
                'e.g. [Aggregate]/[key]/snapshots/[projection]/
                m_cloudBasePath = MyBase.FileClient.GetShareReference(FileEventStreamBase.MakeValidStorageFolderName(AggregateClassName))
                If Not (m_cloudBasePath.ExistsAsync().Result) Then
                    m_cloudBasePath.CreateIfNotExistsAsync()
                End If
                ' This has to be created one level at a time
                m_directory = m_cloudBasePath.GetRootDirectoryReference().GetDirectoryReference(FileEventStreamBase.MakeValidStorageFolderName(m_key.ToString()))
                m_directory.CreateIfNotExistsAsync()
                m_directory = m_directory.GetDirectoryReference(FileEventStreamBase.SNAPSHOTS_FOLDER)
                m_directory.CreateIfNotExistsAsync()
                m_directory = m_directory.GetDirectoryReference(FileEventStreamBase.MakeValidStorageFolderName(ProjectionClassName))
                m_directory.CreateIfNotExistsAsync()
            End If

        End Sub
    End Class

    ''' <summary>
    ''' Base class for all file based projection snapshot operations
    ''' </summary>
    Public MustInherit Class FileProjectionSnapshotBase
        Inherits AzureStorageEventStreamBase

        Public Const SEQUENCE_NUMBER_FORMAT As String = "0000000000"
        Public Const ASOFDATE_FORMAT As String = "yyyyMMddHHmmssffff"

#Region "private members"
        Private ReadOnly m_fileClient As CloudFileClient
#End Region

        ''' <summary>
        ''' The connection to the azure storage through which file operations are performed
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FileClient As CloudFileClient
            Get
                Return m_fileClient
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
            End If

            If (String.IsNullOrWhiteSpace(connectionStringName)) Then
                connectionStringName = StorageConnectionStringSettingName
            End If


            If (m_storageAccount IsNot Nothing) Then
                m_fileClient = m_storageAccount.CreateCloudFileClient()
            End If
        End Sub

        ''' <summary>
        ''' Formats the effective date of a snapshot as a string
        ''' </summary>
        ''' <param name="snapshotAsOfDate">
        ''' The as-of date of the string
        ''' </param>
        ''' <returns>
        ''' A date formatted as a alphabetically sortable string e.g. 207601021403
        ''' </returns>
        Public Shared Function FormatSequenceAsOfDate(snapshotAsOfDate As DateTime) As String

            Return snapshotAsOfDate.ToString(ASOFDATE_FORMAT)

        End Function

        ''' <summary>
        ''' Formats a sequence number (up to a maximum of 4294967295)
        ''' </summary>
        ''' <param name="snapshotSequence">
        ''' The seque3nce number to format
        ''' </param>
        ''' <remarks>
        ''' The number is left padded with zeroes so both numeric and alphabetical sorts match
        ''' </remarks>
        Public Shared Function FormatSequenceNumber(snapshotSequence As UInteger) As String

            Return snapshotSequence.ToString(SEQUENCE_NUMBER_FORMAT)

        End Function

        ''' <summary>
        ''' Turn a snapshot filename into the sequence number component
        ''' </summary>
        ''' <param name="filename">
        ''' e.g. 0000000017.20160415092207
        ''' </param>
        Public Shared Function FilenameToSequenceNumber(ByVal filename As String) As UInteger

            If String.IsNullOrWhiteSpace(filename) Then
                Return 0
            Else
                Dim ret As UInteger

                If UInteger.TryParse(filename.Substring(0, 10), ret) Then
                    Return ret
                Else
                    Return 0
                End If
            End If

        End Function

        ''' <summary>
        ''' Turn a snapshot filename into the as-of date component
        ''' </summary>
        ''' <param name="filename">
        ''' e.g. 0000000017.20160415092207
        ''' </param>
        Public Shared Function FilenameToAsOfDate(ByVal filename As String) As Nullable(Of DateTime)

            If (String.IsNullOrWhiteSpace(filename) OrElse filename.Length < 11) Then
                Return Nothing
            Else
                Dim ret As DateTime
                If DateTime.TryParseExact(filename.Substring(11), ASOFDATE_FORMAT, Nothing, Globalization.DateTimeStyles.None, ret) Then
                    Return ret
                Else
                    Return Nothing
                End If
            End If

        End Function

    End Class

End Namespace