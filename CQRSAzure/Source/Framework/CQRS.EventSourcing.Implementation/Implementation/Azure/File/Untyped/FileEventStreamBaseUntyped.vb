Imports System
Imports CQRSAzure.EventSourcing.Azure.File
Imports Microsoft.WindowsAzure.Storage.File

Namespace Azure.File

    ''' <summary>
    ''' Common functionality that both untyped reader and untyped writer use to access the event store in a file on the Azure storage
    ''' </summary>
    ''' <remarks>
    ''' Care must be taken to make sure this remains compatible with the type-safe implementation
    ''' </remarks>
    Public MustInherit Class FileEventStreamBaseUntyped
        Inherits FileEventStreamBase
        Implements IEventStreamUntypedIdentity

        Private ReadOnly m_directory As CloudFileDirectory

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

        ''' <summary>
        ''' The file underlying the event stream
        ''' </summary>
        Private ReadOnly m_file As CloudFile
        Protected ReadOnly Property File As CloudFile
            Get
                Return m_file
            End Get
        End Property

        Protected ReadOnly Property EventStreamFilename As String
            Get
                Return MakeValidStorageFolderName(m_InstanceKey) & ".eventstream"
            End Get
        End Property

        Protected Friend Async Function GetSequence() As Task(Of Long)

            If (File IsNot Nothing) Then
                Await File.FetchAttributesAsync()
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
                File.FetchAttributesAsync()
                If (File.Metadata.ContainsKey(METADATA_SEQUENCE)) Then
                    m_file.Metadata(METADATA_SEQUENCE) = startingSequence.ToString()
                End If
                File.SetMetadataAsync()
            End If

        End Sub

        Protected Function GetRecordCount()

            If (File IsNot Nothing) Then
                File.FetchAttributesAsync()
                Dim m_records As Long = 0
                If (File.Metadata.ContainsKey(METADATA_RECORD_COUNT)) Then
                    If (Long.TryParse(File.Metadata(METADATA_RECORD_COUNT), m_records)) Then
                        Return m_records
                    End If
                End If
            End If

            Return 0
        End Function



        Friend Async Sub ResetFile()

            Dim exists As Boolean = Await m_file.ExistsAsync()
            If Not exists Then
                Await m_file.CreateAsync(m_configuredFileSize)
                'Set the initial metadata
                m_file.Metadata(METATDATA_DOMAIN) = DomainName
                m_file.Metadata(METADATA_AGGREGATE_CLASS) = AggregateTypeName
                m_file.Metadata(METADATA_AGGREGATE_KEY) = InstanceKey
                m_file.Metadata(METADATA_SEQUENCE) = "0" 'Sequence starts at zero
                m_file.Metadata(METADATA_RECORD_COUNT) = "0" 'Record count starts at zero
                m_file.Metadata(METADATA_DATE_CREATED) = DateTime.UtcNow.ToString("O") 'use universal date/time
                Await m_file.SetMetadataAsync()
            Else
                'set the current sequence number
                If (Not File.Metadata.ContainsKey(METADATA_SEQUENCE)) Then
                    Throw New EventStreamReadException(DomainName, AggregateTypeName, InstanceKey, 0,
                                                       "Unable to get the sequence number for this event stream file")
                End If
            End If

        End Sub

        Protected Sub New(ByVal identifier As IEventStreamUntypedIdentity,
                  Optional ByVal writeAccess As Boolean = False,
                  Optional ByVal connectionStringName As String = "",
                  Optional ByVal settings As IFileStreamSettings = Nothing)

            MyBase.New(identifier.DomainName,
                        writeAccess,
                        connectionStringName,
                        settings)

            m_aggregateTpeName = identifier.AggregateTypeName
            m_InstanceKey = identifier.InstanceKey

            If (BaseFileShare IsNot Nothing) Then
                'Get the directory for the aggregation type
                m_directory = BaseFileShare.GetRootDirectoryReference().GetDirectoryReference(FileEventStreamBase.MakeValidStorageFolderName(identifier.AggregateTypeName))
                m_directory.CreateIfNotExistsAsync()
                'and from that, get the instance file
                m_file = m_directory.GetFileReference(EventStreamFilename)
                ResetFile()
            End If

        End Sub
    End Class
End Namespace
