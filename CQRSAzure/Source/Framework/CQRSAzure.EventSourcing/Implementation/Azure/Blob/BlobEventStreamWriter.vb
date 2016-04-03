Imports CQRSAzure.EventSourcing
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Azure.Blob
    Public Class BlobEventStreamWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)
        Inherits BlobEventStreamBase(Of TAggregate, TAggregationKey)
        Implements IEventStreamWriter(Of TAggregate, TAggregationKey)


#Region "Event stream information"
        Public ReadOnly Property Key As TAggregationKey Implements IEventStream(Of TAggregate, TAggregationKey).Key
            Get
                Return m_key
            End Get
        End Property

        Private m_recordCount As ULong
        Public ReadOnly Property RecordCount As ULong Implements IEventStream(Of TAggregate, TAggregationKey).RecordCount
            Get
                Return m_recordCount
            End Get
        End Property

        Public ReadOnly Property LastAddition As Date? Implements IEventStream(Of TAggregate, TAggregationKey).LastAddition
            Get
                If (AppendBlob IsNot Nothing) Then
                    Return AppendBlob.Properties.LastModified.GetValueOrDefault.UtcDateTime
                Else
                    Return Nothing
                End If
            End Get
        End Property
#End Region

        Public Sub AppendEvent(EventInstance As IEvent(Of TAggregate)) Implements IEventStreamWriter(Of TAggregate, TAggregationKey).AppendEvent

            If (AppendBlob IsNot Nothing) Then
                Dim nextSequence As Long = IncrementSequence()


                Dim evtToWrite As New BlobBlockWrappedEvent(nextSequence, 0, DateTime.UtcNow, EventInstance)
                'Turn the event into a binary stream and append it to the blob
                Dim recordWritten As Boolean = False
                Try
                    Using es As System.IO.Stream = evtToWrite.ToBinaryStream()

                        Dim offset As Long = AppendBlob.AppendBlock(es)
                    End Using
                    recordWritten = True
                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamWriteException(DomainName, AggregateClassName, Key.ToString(), nextSequence, "Unable to save a record to the event stream - " & evtToWrite.EventName, exBlob)
                End Try

                If (recordWritten) Then
                    IncrementRecordCount()
                End If
            End If

        End Sub

        Public Sub AppendEvents(StartingSequence As UInteger, Events As IEnumerable(Of IEvent(Of TAggregate))) Implements IEventStreamWriter(Of TAggregate, TAggregationKey).AppendEvents

            If (Events IsNot Nothing) Then
                If (Events.Count > 0) Then
                    If (StartingSequence < GetSequence()) Then
                        Throw New ArgumentException("Out of sequence event(s) appended")
                    Else
                        'Set the current version to StartingVersion
                        SetSequence(StartingSequence)
                        For Each evt In Events
                            AppendEvent(evt)
                        Next
                    End If
                End If
            End If
        End Sub

        Private Sub New(ByVal AggregateDomainName As String, ByVal AggregateKey As TAggregationKey)
            MyBase.New(AggregateDomainName, AggregateKey)

            'Get teh event stream current record count
            m_recordCount = Me.RecordCount
        End Sub

        ''' <summary>
        ''' Update the sequence number metadata and return the new sequence number
        ''' </summary>
        Private Function IncrementSequence() As Long

            If (MyBase.AppendBlob IsNot Nothing) Then
                Try
                    AppendBlob.FetchAttributes()
                    Dim m_sequence As Long
                    If (Long.TryParse(AppendBlob.Metadata(METADATA_SEQUENCE), m_sequence)) Then
                        m_sequence += 1
                        AppendBlob.Metadata(METADATA_SEQUENCE) = m_sequence.ToString()
                        AppendBlob.SetMetadata()
                    End If
                    Return m_sequence
                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamWriteException(DomainName, AggregateClassName, Key.ToString(), 0, "Unable to increment the sequence number for this event stream", exBlob)
                End Try
            Else
                Throw New EventStreamWriteException(DomainName, AggregateClassName, Key.ToString(), 0, "Unable to increment the sequence number for this event stream")
            End If

        End Function

        ''' <summary>
        ''' Update the sequence number metadata and return the new sequence number
        ''' </summary>
        Private Function IncrementRecordCount() As Long

            If (MyBase.AppendBlob IsNot Nothing) Then
                Try
                    AppendBlob.FetchAttributes()
                    If Not AppendBlob.Metadata.ContainsKey(METADATA_RECORD_COUNT) Then
                        AppendBlob.Metadata(METADATA_RECORD_COUNT) = "0"
                    End If
                    If (Long.TryParse(AppendBlob.Metadata(METADATA_RECORD_COUNT), m_recordCount)) Then
                        m_recordCount += 1
                        AppendBlob.Metadata(METADATA_RECORD_COUNT) = m_recordCount.ToString()
                        AppendBlob.SetMetadata()
                    End If
                    Return m_recordCount
                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamWriteException(DomainName, AggregateClassName, Key.ToString(), 0, "Unable to increment the record count for this event stream", exBlob)
                End Try
            Else
                Throw New EventStreamWriteException(DomainName, AggregateClassName, Key.ToString(), 0, "Unable to increment the record count for this event stream")
            End If

        End Function

        Private Sub SetSequence(ByVal newSequence As Long)
            If (MyBase.AppendBlob IsNot Nothing) Then
                Try
                    AppendBlob.FetchAttributes()
                    AppendBlob.Metadata(METADATA_SEQUENCE) = newSequence.ToString()
                    AppendBlob.SetMetadata()
                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamWriteException(DomainName, AggregateClassName, Key.ToString(), 0, "Unable to increment the sequence number for this event stream", exBlob)
                End Try
            Else
                Throw New EventStreamWriteException(DomainName, AggregateClassName, Key.ToString(), 0, "Unable to increment the sequence number for this event stream")
            End If
        End Sub

#Region "Factory methods"

        ''' <summary>
        ''' Creates an azure blob storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregationKey)) As IEventStreamWriter(Of TAggregate, TAggregationKey)

            Return New BlobEventStreamWriter(Of TAggregate, TAggregationKey)(DomainNameAttribute.GetDomainName(instance), instance.GetKey())

        End Function


#End Region
    End Class
End Namespace