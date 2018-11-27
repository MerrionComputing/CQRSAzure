Imports System
Imports CQRSAzure.EventSourcing.Azure.Blob
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Azure.Blob.Untyped
    ''' <summary>
    ''' Common functionality that both untyped reader and untyped writer use to access the event store in a Blob on the Azure storage
    ''' </summary>
    ''' <remarks>
    ''' Care must be taken to make sure this remains compatible with the type-safe implementation
    ''' </remarks>
    Public MustInherit Class BlobEventStreamBaseUntyped
        Inherits BlobEventStreamBase
        Implements IEventStreamUntypedIdentity

        Private ReadOnly m_blob As CloudAppendBlob


        Protected ReadOnly Property AggregateClassName As String
            Get
                Return MakeValidStorageFolderName(Me.AggregateTypeName)
            End Get
        End Property

        ''' <summary>
        ''' The storage location that the event streams for this event stream are located
        ''' </summary>
        ''' <returns>
        ''' /eventstreams/[aggregate-class-name]/
        ''' </returns>
        ''' <remarks>
        ''' This is a virtual path that is added to the blob name to create a traversable path
        ''' </remarks>
        Protected ReadOnly Property ContainerBasePath As String
            Get
                Return EVENTSTREAM_FOLDER & "/" & MakeValidStorageFolderName(AggregateClassName) & "/"
            End Get
        End Property

        ''' <summary>
        ''' The filename that this specific event stream will be written to
        ''' </summary>
        ''' <returns>
        ''' e.g. /eventstreams/car/olj565m.events 
        ''' </returns>
        Protected ReadOnly Property EventStreamBlobFilename As String
            Get
                Return ContainerBasePath & MakeValidStorageFolderName(Me.InstanceKey.ToString) & ".events"
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

        Private ReadOnly Property IEventStreamUntypedIdentity_DomainName As String Implements IEventStreamUntypedIdentity.DomainName
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


        Public Async Function GetSequence() As Task(Of Long)

            If (AppendBlob IsNot Nothing) Then
                Try
                    Dim exists As Boolean = Await AppendBlob.ExistsAsync()
                    If (exists) Then
                        Await AppendBlob.FetchAttributesAsync()
                        Dim m_sequence As Long
                        If (Long.TryParse(AppendBlob.Metadata(METADATA_SEQUENCE), m_sequence)) Then
                            Return m_sequence
                        End If
                    Else
                        Return 0
                    End If
                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamReadException(DomainName, AggregateClassName, InstanceKey, 0, "Unable to get the sequence number for this event stream", exBlob)
                End Try
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, InstanceKey, 0, "Unable to get the sequence number for this event stream")
            End If

            Return 0

        End Function

        Public Async Function GetRecordCount() As Task(Of Long)

            If (AppendBlob IsNot Nothing) Then
                Try
                    Await AppendBlob.FetchAttributesAsync()
                    Dim m_sequence As Long
                    If (Long.TryParse(AppendBlob.Metadata(METADATA_RECORD_COUNT), m_sequence)) Then
                        Return m_sequence
                    End If

                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamReadException(DomainName, AggregateClassName, Me.InstanceKey, 0, "Unable to get the record count for this event stream", exBlob)
                End Try
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, InstanceKey, 0, "Unable to get the record counr for this event stream")
            End If

            Return 0

        End Function

        Public Sub New(ByVal identifier As IEventStreamUntypedIdentity,
                       Optional ByVal writeAccess As Boolean = False,
                       Optional ByVal connectionStringName As String = "",
                       Optional ByVal settings As IBlobStreamSettings = Nothing)

            MyBase.New(identifier.DomainName, writeAccess, connectionStringName, settings)

            m_aggregateTpeName = identifier.AggregateTypeName
            m_InstanceKey = identifier.InstanceKey


            If (BlobContainer IsNot Nothing) Then
                m_blob = BlobContainer.GetAppendBlobReference(EventStreamBlobFilename)
            End If
            Call ResetBlob()

        End Sub


        ''' <summary>
        ''' Create the Blob or get the reference to an existing one
        ''' </summary>
        Protected Async Function ResetBlob() As Task

            If (BlobContainer IsNot Nothing) Then
                Dim exists As Boolean = Await m_blob.ExistsAsync()
                If Not exists Then
                    'Make the file to append to if it doesn't already exist
                    Await m_blob.CreateOrReplaceAsync()
                    'Set the initial metadata
                    m_blob.Metadata(METATDATA_DOMAIN) = DomainName
                    m_blob.Metadata(METADATA_AGGREGATE_CLASS) = Me.AggregateTypeName
                    m_blob.Metadata(METADATA_AGGREGATE_KEY) = Me.InstanceKey
                    m_blob.Metadata(METADATA_SEQUENCE) = "0" 'Sequence starts at zero
                    m_blob.Metadata(METADATA_RECORD_COUNT) = "0" 'Record count starts at zero
                    m_blob.Metadata(METADATA_DATE_CREATED) = DateTime.UtcNow.ToString("O") 'use universal date/time
                    Await m_blob.SetMetadataAsync()
                Else
                    Await m_blob.FetchAttributesAsync()
                End If
            End If
        End Function

    End Class
End Namespace
