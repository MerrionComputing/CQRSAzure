Imports Microsoft.WindowsAzure.Storage.Blob
Imports CQRSAzure.EventSourcing
Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing.Azure.Blob
Imports Microsoft.WindowsAzure.Storage

Namespace Azure.Blob

    ''' <summary>
    ''' Class to read events from an event stream implemented as a windows azure append blob
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The data type of the aggregate that the event stream belongs to
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The type which provides the unique key by which to recognise an instance of the aggregate
    ''' </typeparam>
    Public NotInheritable Class BlobEventStreamReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits BlobEventStreamBase(Of TAggregate, TAggregateKey)
        Implements IEventStreamFilteredReader(Of TAggregate, TAggregateKey)

        Private ReadOnly m_validEventTypes As IEnumerable(Of Type)
        Private ReadOnly m_eventFilterFunction As FilterFunctions.EventFilterFunction

        ''' <summary>
        ''' The unique key of the aggregate whose event stream this azure blob reader is operating over
        ''' </summary>
        Public ReadOnly Property Key As TAggregateKey Implements IEventStreamInstanceProvider(Of TAggregate, TAggregateKey).Key
            Get
                Return MyBase.m_key
            End Get
        End Property

        Public Function IsEventValid(eventType As Type) As Boolean Implements IEventStreamFilteredReader(Of TAggregate, TAggregateKey).IsEventValid

            If (m_eventFilterFunction IsNot Nothing) Then
                Return m_eventFilterFunction.Invoke(eventType)
            Else
                If (m_validEventTypes Is Nothing) Then
                    Return True
                Else
                    Return m_validEventTypes.Contains(eventType)
                End If
            End If

        End Function

        ''' <summary>
        ''' Get a snapshot of the append blob to use when reading this event stream
        ''' </summary>
        ''' <returns></returns>
        Private Function GetAppendBlobSnapshot() As CloudAppendBlob
            If (AppendBlob IsNot Nothing) Then
                Return AppendBlob.CreateSnapshot()
            Else
                Return Nothing
            End If
        End Function

        Private Function GetUnderlyingStream() As System.IO.Stream

            If (AppendBlob IsNot Nothing) Then
                Dim targetStream As New System.IO.MemoryStream()
                Try
                    GetAppendBlobSnapshot().DownloadToStream(targetStream)
                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamReadException(DomainName, AggregateClassName, m_key.ToString(), 0, "Unable to access underlying event stream", exBlob)
                End Try
                targetStream.Seek(0, IO.SeekOrigin.Begin)
                Return targetStream
            Else
                Return Nothing
            End If

        End Function

        Public Function GetEvents() As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEvents

            If (AppendBlob IsNot Nothing) Then
                Dim ret As New List(Of IEvent(Of TAggregate))
                Dim bf As New BinaryFormatter()
                Using rawStream As System.IO.Stream = GetUnderlyingStream()
                    While Not (rawStream.Position >= rawStream.Length)
                        Dim record As BlobBlockWrappedEvent = CType(bf.Deserialize(rawStream), BlobBlockWrappedEvent)
                        If (record IsNot Nothing) Then
                            If (IsEventValid(record.EventInstance.GetType())) Then
                                ret.Add(record.EventInstance)
                            End If
                        End If
                    End While
                End Using
                Return ret
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, MyBase.m_key.ToString(), 0, "Unable to read events - Azure blob not initialised")
            End If

        End Function

        Public Function GetEvents(Optional ByVal StartingVersion As UInteger = 0,
                                  Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEvents

            If (AppendBlob IsNot Nothing) Then
                Dim ret As New List(Of IEvent(Of TAggregate))
                Dim bf As New BinaryFormatter()
                Using rawStream As System.IO.Stream = GetUnderlyingStream()
                    While Not (rawStream.Position >= rawStream.Length)
                        Dim record As BlobBlockWrappedEvent = CType(bf.Deserialize(rawStream), BlobBlockWrappedEvent)
                        If (record IsNot Nothing) Then
                            If (record.Sequence >= StartingVersion) Then
                                If (IsEventValid(record.EventInstance.GetType())) Then
                                    ret.Add(record.EventInstance)
                                End If
                            End If
                        End If
                    End While
                End Using
                Return ret
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, MyBase.m_key.ToString(), 0, "Unable to read events - Azure blob not initialised")
            End If

        End Function

        Public Function GetEventsWithContext(Optional ByVal StartingVersion As UInteger = 0,
                                             Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As IEnumerable(Of IEventContext) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEventsWithContext

            If (AppendBlob IsNot Nothing) Then
                Dim ret As New List(Of IEventContext)
                Dim bf As New BinaryFormatter()
                Using rawStream As System.IO.Stream = GetUnderlyingStream()
                    While Not (rawStream.Position >= rawStream.Length)
                        Dim record As BlobBlockWrappedEvent = CType(bf.Deserialize(rawStream), BlobBlockWrappedEvent)
                        If (record IsNot Nothing) Then
                            If (record.Sequence >= StartingVersion) Then
                                If (IsEventValid(record.EventInstance.GetType())) Then
                                    Dim instance = InstanceWrappedEvent(Of TAggregateKey).Wrap(m_key, record.EventInstance, record.Version)
                                    ret.Add(ContextWrappedEvent(Of TAggregateKey).Wrap(instance, record.Sequence, "", "", record.Timestamp, record.Version, ""))
                                End If
                            End If
                        End If
                    End While
                End Using
                Return ret
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, MyBase.m_key.ToString(), 0, "Unable to read events - Azure blob not initialised")
            End If

        End Function


        ''' <summary>
        ''' Create a new windows azure blob stream reader to read events from the blob
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The domain in which the aggregate resides
        ''' </param>
        ''' <remarks>
        ''' The unique key which identifies the instance of the aggregate to read the event stream for
        ''' </remarks>
        Private Sub New(ByVal AggregateDomainName As String,
                        ByVal AggregateKey As TAggregateKey,
                        Optional ByVal settings As IBlobStreamSettings = Nothing,
                        Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing,
                        Optional ByVal eventFilterFunction As FilterFunctions.EventFilterFunction = Nothing)

            MyBase.New(AggregateDomainName, AggregateKey, writeAccess:=False, connectionStringName:=GetReadConnectionStringName("", settings), settings:=settings)

            If (eventFilter IsNot Nothing) Then
                m_validEventTypes = eventFilter
            End If
            If (eventFilterFunction IsNot Nothing) Then
                m_eventFilterFunction = eventFilterFunction
            End If

        End Sub

#Region "Factory methods"

        ''' <summary>
        ''' Creates an azure blob storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <param name="settings">
        ''' The settings to use to connect to the azure storage account to use
        ''' </param>
        ''' <param name="eventFilter">
        ''' The filter to apply when reading back the events
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As IBlobStreamSettings = Nothing,
                                      Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing) As IEventStreamReader(Of TAggregate, TAggregateKey)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If

            Return New BlobEventStreamReader(Of TAggregate, TAggregateKey)(domainName, instance.GetKey(), settings)

        End Function

        ''' <summary>
        ''' Create a projection processor that works off an azure blob backed event stream
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to run projections
        ''' </param>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Shared Function CreateProjectionProcessor(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                                         Optional ByVal settings As IBlobStreamSettings = Nothing,
                                                         Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing) As ProjectionProcessor(Of TAggregate, TAggregateKey)

            Return New ProjectionProcessor(Of TAggregate, TAggregateKey)(Create(instance, settings, eventFilter))

        End Function

        ''' <summary>
        ''' Create a projection processor that works off an in-memory backed event stream
        ''' </summary>
        ''' <param name="readerToUse">
        ''' The event stream reader to use to run the projection
        ''' </param>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Shared Function CreateProjectionProcessor(ByVal readerToUse As IEventStreamReader(Of TAggregate, TAggregateKey)) As ProjectionProcessor(Of TAggregate, TAggregateKey)

            Return New ProjectionProcessor(Of TAggregate, TAggregateKey)(readerToUse)

        End Function
#End Region
    End Class

    Public Module BlobEventStreamReaderFactory

        ''' <summary>
        ''' Creates an azure blob based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <param name="eventFilter">
        ''' An optional array of event definitions to use to filter the incoming event stream by
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                   TAggregateKey)(ByVal instance As TAggregate,
                                      ByVal key As TAggregateKey,
                                      ByVal settings As IBlobStreamSettings,
                                      Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing
                                      ) As IEventStreamReader(Of TAggregate, TAggregateKey)

            Return BlobEventStreamReader(Of TAggregate, TAggregateKey).Create(instance, settings, eventFilter)

        End Function

        ''' <summary>
        ''' Create a projection processor that works off an azure file backed event stream
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to run projections
        ''' </param>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Function CreateProjectionProcessor(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)(ByVal instance As TAggregate,
                                                                                                                                    ByVal key As TAggregateKey,
                                                         Optional ByVal settings As IBlobStreamSettings = Nothing,
                                                         Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing) As ProjectionProcessor(Of TAggregate, TAggregateKey)

            Return BlobEventStreamReader(Of TAggregate, TAggregateKey).CreateProjectionProcessor(instance, settings, eventFilter)

        End Function

        ''' <summary>
        ''' Generate a function that can be used to create a reader of the given type
        ''' </summary>
        ''' <typeparam name="TAggregate">
        ''' The data type of the aggregate class
        ''' </typeparam>
        ''' <typeparam name="TAggregateKey">
        ''' The data type that provides the unique identification of an instance of the reader class
        ''' </typeparam>
        Public Function GenerateCreationFunctionDelegate(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                   TAggregateKey)() As IAggregateImplementationMap.ReaderCreationFunction(Of TAggregate, TAggregateKey)


            'Make delegate for this module Create() function....
            Return New IAggregateImplementationMap.ReaderCreationFunction(Of TAggregate, TAggregateKey)(AddressOf Create(Of TAggregate, TAggregateKey))

        End Function

    End Module
End Namespace