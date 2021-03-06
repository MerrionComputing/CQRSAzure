﻿Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.Blob
Imports Microsoft.WindowsAzure.Storage
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Azure.Blob
    Public MustInherit Class BlobProjectionSnapshotBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                            TAggregateKey,
                                                            TProjection As IProjection)
        Inherits BlobProjectionSnapshotBase

        Private ReadOnly m_blob As CloudAppendBlob
        Protected ReadOnly m_key As TAggregateKey




        Protected ReadOnly Property AggregateClassName As String
            Get
                Dim typeName As String = GetType(TAggregate).Name
                Return MakeValidStorageFolderName(typeName)
            End Get
        End Property

        Protected ReadOnly Property ProjectionClassName As String
            Get
                Dim typeName As String = GetType(TProjection).Name
                Return MakeValidStorageFolderName(typeName)
            End Get
        End Property

        ''' <summary>
        ''' The storage location that the event streams for this event stream are located
        ''' </summary>
        ''' <returns>
        ''' /snapshots/[aggregate-class-name]/[projection-name]/
        ''' </returns>
        ''' <remarks>
        ''' This is a virtual path that is added to the blob name to create a traversable path
        ''' </remarks>
        Protected ReadOnly Property ContainerBasePath As String
            Get
                Return BlobEventStreamBase.SNAPSHOTS_FOLDER & "/" & MakeValidStorageFolderName(AggregateClassName) & "/" & MakeValidStorageFolderName(ProjectionClassName) & "/"
            End Get
        End Property

        ''' <summary>
        ''' The filename that this specific projection snapshot stream will be written to
        ''' </summary>
        ''' <returns>
        ''' e.g. /snapshots/car/olj565m.snapshots 
        ''' </returns>
        Protected ReadOnly Property ProjectionSnapshotBlobFilename As String
            Get
                Return ContainerBasePath & MakeValidStorageFolderName(m_key.ToString) & ".snapshots"
            End Get
        End Property

        ''' <summary>
        ''' The blob that the events are written to/read from
        ''' </summary>
        Protected ReadOnly Property AppendBlob As CloudAppendBlob
            Get
                Return m_blob
            End Get
        End Property


        ''' <summary>
        ''' The system-wide options for writing to the event stream
        ''' </summary>
        Private ReadOnly Property RequestOptions As BlobRequestOptions
            Get
                Dim ret As New BlobRequestOptions()

                'As the projection snapshots MUST be single-writer we can ignore conditional errors when retrying
                ret.AbsorbConditionalErrorsOnRetry = True

                ret.RetryPolicy = New RetryPolicies.LinearRetry()

                'If required, encryption keys could be on a per-key basis...

                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Get the highest sequence number snapshot stored in this append blob
        ''' </summary>
        Public Async Function GetHighestSequence() As Task(Of Long)

            If (AppendBlob IsNot Nothing) Then
                Try
                    Await AppendBlob.FetchAttributesAsync()
                    Dim m_sequence As Long
                    If (Long.TryParse(AppendBlob.Metadata(METADATA_SEQUENCE), m_sequence)) Then
                        Return m_sequence
                    End If

                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamReadException(DomainName, AggregateClassName, m_key.ToString(), 0, "Unable to get the sequence number for this projection snapshot set", exBlob)
                End Try
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, m_key.ToString(), 0, "Unable to get the sequence number for this projection snapshot set")
            End If

            Return 0

        End Function

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
                          Optional ByVal settings As IBlobStreamSettings = Nothing)
            MyBase.New(AggregateDomainName, writeAccess, connectionStringName, settings)

            'Get the aggregation instance key to use when creating a blob file name
            m_key = AggregateKey

            If (BlobContainer IsNot Nothing) Then
                m_blob = BlobContainer.GetAppendBlobReference(ProjectionSnapshotBlobFilename)
            End If
            Call ResetBlob()

        End Sub


        Protected Async Function ResetBlob() As Task

            If (BlobContainer IsNot Nothing) Then
                Dim exists As Boolean = Await m_blob.ExistsAsync()
                If Not exists Then
                    'Make the file to append to if it doesn't already exist
                    Await m_blob.CreateOrReplaceAsync()
                    'Set the initial metadata
                    m_blob.Metadata(METATDATA_DOMAIN) = DomainName
                    m_blob.Metadata(METADATA_AGGREGATE_CLASS) = GetType(TAggregate).Name
                    m_blob.Metadata(METADATA_PROJECTION_CLASS) = GetType(TProjection).Name
                    m_blob.Metadata(METADATA_AGGREGATE_KEY) = m_key.ToString()
                    m_blob.Metadata(METADATA_SEQUENCE) = "0" 'Sequence starts at zero
                    Await m_blob.SetMetadataAsync()
                Else
                    Await m_blob.FetchAttributesAsync()
                End If
            End If

        End Function

    End Class

    Public MustInherit Class BlobProjectionSnapshotBase
        Inherits AzureStorageEventStreamBase

#Region "private members"
        Private ReadOnly m_blobClient As CloudBlobClient
        Private ReadOnly m_blobBasePath As CloudBlobContainer
#End Region

        Protected Const METATDATA_DOMAIN As String = "DOMAIN"
        Protected Const METADATA_AGGREGATE_CLASS As String = "AGGREGATECLASS"
        Protected Const METADATA_PROJECTION_CLASS As String = "PROJECTIONCLASS"
        Protected Const METADATA_SEQUENCE As String = "SEQUENCE"
        Protected Const METADATA_AGGREGATE_KEY As String = "AGGREGATEKEY"

        Protected ReadOnly Property BlobContainer As CloudBlobContainer
            Get
                Return m_blobBasePath
            End Get
        End Property

        ''' <summary>
        ''' Turn a name into a valid folder name for azure blob storage
        ''' </summary>
        ''' <param name="rawName">
        ''' The name of the thing we want to turn into a blob storage folder name
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
                Return BlobEventStreamBase.ORPHANS_FOLDER
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

        ''' <summary>
        ''' Create a new base for a reader or writer class in the given domain
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The name of the domain to store/retrieve the event streams under
        ''' </param>
        ''' <param name="connectionStringName">
        ''' The configuration name to use to look up the Azure storage account connection string - this 
        ''' allows the read and write side to have different connection strings
        ''' </param>
        ''' <param name="writeAccess">
        ''' Does the process need write access to the underlying storage account
        ''' </param>
        Protected Sub New(ByVal AggregateDomainName As String,
                          Optional ByVal writeAccess As Boolean = False,
                          Optional ByVal connectionStringName As String = "",
                          Optional ByVal settings As IBlobStreamSettings = Nothing)
            MyBase.New(AggregateDomainName, writeAccess:=writeAccess, connectionStringName:=connectionStringName, settings:=settings)

            If (m_storageAccount IsNot Nothing) Then
                m_blobClient = m_storageAccount.CreateCloudBlobClient()
                If (m_blobClient IsNot Nothing) Then
                    'Create the reference to this aggretate type's event stream base folder
                    'e.g. /[domain]/
                    m_blobBasePath = m_blobClient.GetContainerReference(MakeValidStorageFolderName(DomainName))
                    If (m_blobBasePath IsNot Nothing) Then
                        m_blobBasePath.CreateIfNotExistsAsync()
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace