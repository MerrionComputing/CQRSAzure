Imports CQRSAzure.EventSourcing
Imports Microsoft.WindowsAzure.Storage
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Azure.Blob.Untyped
    Public MustInherit Class BlobProjectionSnapshotBaseUntyped
        Inherits BlobProjectionSnapshotBase
        Implements IEventStreamUntypedIdentity

        Private ReadOnly m_blob As CloudAppendBlob


        Public ReadOnly Property IEventStreamUntypedIdentity_DomainName As String Implements IEventStreamUntypedIdentity.DomainName
            Get
                Return MyBase.DomainName
            End Get
        End Property

        Private ReadOnly m_aggregateTpeName As String
        Public ReadOnly Property AggregateTypeName As String Implements IEventStreamUntypedIdentity.AggregateTypeName
            Get
                Return m_aggregateTpeName
            End Get
        End Property

        Private ReadOnly m_InstanceKey As String
        Public ReadOnly Property InstanceKey As String Implements IEventStreamUntypedIdentity.InstanceKey
            Get
                Return m_InstanceKey
            End Get
        End Property

        Protected ReadOnly Property AggregateClassName As String
            Get
                Return MakeValidStorageFolderName(AggregateTypeName)
            End Get
        End Property

        Private ReadOnly m_ProjectionClassName As String
        Protected ReadOnly Property ProjectionClassName As String
            Get
                Return MakeValidStorageFolderName(m_ProjectionClassName)
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
                Return ContainerBasePath & MakeValidStorageFolderName(InstanceKey) & ".snapshots"
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
        Public Function GetHighestSequence() As Long

            If (AppendBlob IsNot Nothing) Then
                Try
                    AppendBlob.FetchAttributes()
                    Dim m_sequence As Long
                    If (Long.TryParse(AppendBlob.Metadata(METADATA_SEQUENCE), m_sequence)) Then
                        Return m_sequence
                    End If

                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamReadException(DomainName, AggregateClassName, InstanceKey.ToString(), 0, "Unable to get the sequence number for this projection snapshot set", exBlob)
                End Try
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, InstanceKey.ToString(), 0, "Unable to get the sequence number for this projection snapshot set")
            End If

            Return 0

        End Function

        Protected Sub ResetBlob()

            If (BlobContainer IsNot Nothing) Then
                If Not m_blob.Exists() Then
                    'Make the file to append to if it doesn't already exist
                    m_blob.CreateOrReplace()
                    'Set the initial metadata
                    m_blob.Metadata(METATDATA_DOMAIN) = DomainName
                    m_blob.Metadata(METADATA_AGGREGATE_CLASS) = AggregateClassName
                    m_blob.Metadata(METADATA_PROJECTION_CLASS) = ProjectionClassName
                    m_blob.Metadata(METADATA_AGGREGATE_KEY) = InstanceKey
                    m_blob.Metadata(METADATA_SEQUENCE) = "0" 'Sequence starts at zero
                    m_blob.SetMetadata()
                Else
                    m_blob.FetchAttributes()
                End If
            End If

        End Sub

        Protected Sub New(ByVal identifier As IEventStreamUntypedIdentity,
                  ByVal projectionClassName As String,
                  Optional ByVal writeAccess As Boolean = False,
                  Optional ByVal connectionStringName As String = "",
                  Optional ByVal settings As IBlobStreamSettings = Nothing)

            MyBase.New(identifier.DomainName,
                       writeAccess:=writeAccess,
                       connectionStringName:=connectionStringName,
                       settings:=settings)

            m_aggregateTpeName = identifier.AggregateTypeName
            m_InstanceKey = identifier.InstanceKey
            m_ProjectionClassName = projectionClassName

            If (BlobContainer IsNot Nothing) Then
                m_blob = BlobContainer.GetAppendBlobReference(ProjectionSnapshotBlobFilename)
            End If
            Call ResetBlob()

        End Sub

    End Class

End Namespace
