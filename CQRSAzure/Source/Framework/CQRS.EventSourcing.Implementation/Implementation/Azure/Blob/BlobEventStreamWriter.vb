Imports CQRSAzure.EventSourcing.Azure.Blob
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Azure.Blob
    Public NotInheritable Class BlobEventStreamWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits BlobEventStreamBase(Of TAggregate, TAggregateKey)
        Implements IEventStreamWriter(Of TAggregate, TAggregateKey)


#Region "Event stream information"
        Public ReadOnly Property Key As TAggregateKey Implements IEventStream(Of TAggregate, TAggregateKey).Key, IEventStreamInstanceProvider(Of TAggregate, TAggregateKey).Key
            Get
                Return m_key
            End Get
        End Property

        Private m_recordCount As ULong
        Public ReadOnly Property RecordCount As ULong Implements IEventStream(Of TAggregate, TAggregateKey).RecordCount
            Get
                Return m_recordCount
            End Get
        End Property

        Public ReadOnly Property LastAddition As Date? Implements IEventStream(Of TAggregate, TAggregateKey).LastAddition
            Get
                If (AppendBlob IsNot Nothing) Then
                    Return AppendBlob.Properties.LastModified.GetValueOrDefault.UtcDateTime
                Else
                    Return Nothing
                End If
            End Get
        End Property

#End Region

        Public Async Function AppendEvent(EventInstance As IEvent(Of TAggregate),
                               Optional ByVal ExpectedTopSequence As Long = 0) As Task Implements IEventStreamWriter(Of TAggregate, TAggregateKey).AppendEvent

            If (AppendBlob IsNot Nothing) Then
                Dim nextSequence As Long = Await GetSequence()


                Dim evtToWrite As New BlobBlockWrappedEvent(nextSequence, EventInstance.Version, DateTime.UtcNow, EventInstance)
                'Turn the event into a binary stream and append it to the blob
                Dim recordWritten As Boolean = False
                Try
                    Using es As System.IO.Stream = evtToWrite.ToBinaryStream()
                        Dim offset As Long = Await AppendBlob.AppendBlockAsync(es)
                        recordWritten = True
                    End Using

                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamWriteException(DomainName,
                                                        AggregateClassName,
                                                        Key.ToString(),
                                                        nextSequence,
                                                        "Storage exception : unable to save a record to the event stream - " & evtToWrite.EventName, exBlob)
                Catch ex As Exception
                    Throw New EventStreamWriteException(DomainName,
                                                        AggregateClassName,
                                                        Key.ToString(),
                                                        nextSequence,
                                                        "Unable to save a record to the event stream - " & evtToWrite.EventName, ex)
                End Try

                If (recordWritten) Then
                    Await IncrementRecordCountAndSequence()
                End If
            End If

        End Function

        Public Async Function AppendEvents(StartingSequence As Long, Events As IEnumerable(Of IEvent(Of TAggregate))) As Task Implements IEventStreamWriter(Of TAggregate, TAggregateKey).AppendEvents

            If (Events IsNot Nothing) Then
                If (Events.Count > 0) Then
                    Dim currentSequence As Long = Await GetSequence()
                    If (StartingSequence < currentSequence) Then
                        Throw New ArgumentException("Out of sequence event(s) appended")
                    Else
                        'Set the current version to StartingVersion
                        SetSequence(StartingSequence)
                        For Each evt In Events
                            Await AppendEvent(evt)
                        Next
                    End If
                End If
            End If
        End Function


        Private Sub New(ByVal AggregateDomainName As String,
                        ByVal AggregateKey As TAggregateKey,
                        Optional ByVal settings As IBlobStreamSettings = Nothing)
            MyBase.New(AggregateDomainName, AggregateKey, writeAccess:=True, connectionStringName:=GetWriteConnectionStringName("", settings), settings:=settings)

            'Get the event stream current record count
            Dim getRC As Task(Of Long) = MyBase.GetRecordCount()
            getRC.Wait()
            m_recordCount = getRC.Result

        End Sub

        ''' <summary>
        ''' Update the sequence number metadata and return the new sequence number
        ''' </summary>
        Private Function IncrementSequence() As Long

            If (MyBase.AppendBlob IsNot Nothing) Then
                Try
                    AppendBlob.FetchAttributesAsync()
                    Dim m_sequence As Long
                    If (Long.TryParse(AppendBlob.Metadata(METADATA_SEQUENCE), m_sequence)) Then
                        m_sequence += 1
                        AppendBlob.Metadata(METADATA_SEQUENCE) = m_sequence.ToString()
                        AppendBlob.SetMetadataAsync()
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
        Private Async Function IncrementRecordCountAndSequence() As Task(Of Long)

            If (MyBase.AppendBlob IsNot Nothing) Then
                Try
                    Await AppendBlob.FetchAttributesAsync()
                    If Not AppendBlob.Metadata.ContainsKey(METADATA_RECORD_COUNT) Then
                        AppendBlob.Metadata(METADATA_RECORD_COUNT) = "0"
                    End If
                    If Not AppendBlob.Metadata.ContainsKey(METADATA_SEQUENCE) Then
                        AppendBlob.Metadata(METADATA_SEQUENCE) = "0"
                    End If
                    If (Long.TryParse(AppendBlob.Metadata(METADATA_RECORD_COUNT), m_recordCount)) Then
                        m_recordCount += 1
                        AppendBlob.Metadata(METADATA_RECORD_COUNT) = m_recordCount.ToString()
                    End If
                    Dim currentSequence As Long
                    If (Long.TryParse(AppendBlob.Metadata(METADATA_SEQUENCE), currentSequence)) Then
                        currentSequence += 1
                        AppendBlob.Metadata(METADATA_SEQUENCE) = currentSequence.ToString()
                    End If
                    Await AppendBlob.SetMetadataAsync()
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
                    AppendBlob.FetchAttributesAsync()
                    AppendBlob.Metadata(METADATA_SEQUENCE) = newSequence.ToString()
                    AppendBlob.SetMetadataAsync()
                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamWriteException(DomainName, AggregateClassName, Key.ToString(), 0, "Unable to increment the sequence number for this event stream", exBlob)
                End Try
            Else
                Throw New EventStreamWriteException(DomainName, AggregateClassName, Key.ToString(), 0, "Unable to increment the sequence number for this event stream")
            End If
        End Sub

        Private m_context As IWriteContext
        Public Sub SetContext(writerContext As IWriteContext) Implements IEventStreamWriter(Of TAggregate, TAggregateKey).SetContext
            m_context = writerContext
        End Sub

        ''' <summary>
        ''' Clear down the event stream
        ''' </summary>
        ''' <remarks>
        ''' This will delete existing events so should not be done in any production environment therefore this is not
        ''' part of the IEventStreamWriter interface
        ''' </remarks>
        Public Async Function Reset() As Task

            If (AppendBlob IsNot Nothing) Then
                Dim exists As Boolean = Await AppendBlob.ExistsAsync
                If (exists) Then
                    Await AppendBlob.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots, Nothing,
                                             New BlobRequestOptions(),
                                             New Microsoft.WindowsAzure.Storage.OperationContext())

                End If
                'Recreate the blob file that was deleted
                Await MyBase.ResetBlob()
            End If
        End Function

#Region "Factory methods"

        ''' <summary>
        ''' Creates an azure blob storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As IBlobStreamSettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If

            Return New BlobEventStreamWriter(Of TAggregate, TAggregateKey)(domainName, instance.GetKey(), settings)

        End Function



#End Region
    End Class

    Public Module BlobEventStreamWriterFactory

        ''' <summary>
        ''' Creates an azure append blob file storage based event stream writer for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to write the event stream
        ''' </param>
        ''' <param name="settings">
        ''' Any additional settings 
        ''' </param>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)(ByVal instance As TAggregate,
            ByVal key As TAggregateKey,
            Optional ByVal settings As IBlobStreamSettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)

            Return BlobEventStreamWriter(Of TAggregate, TAggregateKey).Create(instance, settings)

        End Function

        ''' <summary>
        ''' Generate a function that can be used to create an event stream writer of the given type
        ''' </summary>
        ''' <typeparam name="TAggregate">
        ''' The data type of the aggregate class
        ''' </typeparam>
        ''' <typeparam name="TAggregateKey">
        ''' The data type that provides the unique identification of an instance of the reader class
        ''' </typeparam>
        Public Function GenerateCreationFunctionDelegate(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                   TAggregateKey)() As IAggregateImplementationMap.WriterCreationFunction(Of TAggregate, TAggregateKey)


            'Make delegate for this module Create() function....
            Return New IAggregateImplementationMap.WriterCreationFunction(Of TAggregate, TAggregateKey)(AddressOf Create(Of TAggregate, TAggregateKey))

        End Function
    End Module
End Namespace