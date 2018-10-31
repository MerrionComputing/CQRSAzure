Option Strict Off

Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing
Imports Microsoft.WindowsAzure.Storage.File

Namespace Azure.File

    ''' <summary>
    ''' Class to read events from an event stream implemented as a windows azure append blob
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The data type of the aggregate that the event stream belongs to
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The type which provides the unique key by which to recognise an instance of the aggregate
    ''' </typeparam>
    Public Class FileEventStreamReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits FileEventStreamBase(Of TAggregate, TAggregateKey)
        Implements IEventStreamFilteredReader(Of TAggregate, TAggregateKey)

        Private ReadOnly m_validEventTypes As IEnumerable(Of Type)
        Private ReadOnly m_eventFilterFunction As FilterFunctions.EventFilterFunction

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


        Public Function GetEvents() As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEvents

            If (MyBase.File IsNot Nothing) Then
                Dim ret As New List(Of IEvent(Of TAggregate))
                Dim bf As New BinaryFormatter()
                Dim currentLength As Long = GetSequence()
                Using fs As System.IO.Stream = MyBase.File.OpenRead()
                    fs.Seek(0, IO.SeekOrigin.Begin)
                    While Not (fs.Position >= currentLength)
                        Dim record As FileBlockWrappedEvent = CType(bf.Deserialize(fs), FileBlockWrappedEvent)
                        If (record IsNot Nothing) Then
                            If (IsEventValid(record.EventInstance.GetType())) Then
                                ret.Add(record.EventInstance)
                            End If
                        End If
                    End While
                End Using
                Return ret
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, MyBase.m_key.ToString(), 0, "Unable to read events - Azure file not initialised")
            End If

        End Function

        Public Function GetEvents(Optional ByVal StartingVersion As UInteger = 0,
                                  Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEvents

            If (MyBase.File IsNot Nothing) Then
                Dim ret As New List(Of IEvent(Of TAggregate))
                Dim bf As New BinaryFormatter()
                Dim currentLength As Long = GetSequence()
                Using fs As System.IO.Stream = MyBase.File.OpenRead()
                    'Go to the starting 
                    fs.Seek(StartingVersion, IO.SeekOrigin.Begin)
                    While Not (fs.Position >= currentLength)
                        Dim record As FileBlockWrappedEvent = CType(bf.Deserialize(fs), FileBlockWrappedEvent)
                        If (record IsNot Nothing) Then
                            If (IsEventValid(record.EventInstance.GetType())) Then
                                ret.Add(record.EventInstance)
                            End If
                        End If
                    End While
                End Using
                Return ret
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, MyBase.m_key.ToString(), 0, "Unable to read events - Azure file not initialised")
            End If

        End Function

        Public Function GetEventsWithContext(Optional ByVal StartingVersion As UInteger = 0,
                                             Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As IEnumerable(Of IEventContext) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEventsWithContext

            If (MyBase.File IsNot Nothing) Then
                Dim currentLength As Long = GetSequence()
                Dim ret As New List(Of IEventContext)
                Dim bf As New BinaryFormatter()
                Using fs As System.IO.Stream = MyBase.File.OpenRead()
                    fs.Seek(StartingVersion, IO.SeekOrigin.Begin)
                    While Not (fs.Position >= currentLength)
                        Dim record As FileBlockWrappedEvent = CType(bf.Deserialize(fs), FileBlockWrappedEvent)
                        If (record IsNot Nothing) Then
                            If (IsEventValid(record.EventInstance.GetType())) Then
                                Dim instance = InstanceWrappedEvent(Of TAggregateKey).Wrap(m_key, record.EventInstance, record.Version)
                                ret.Add(ContextWrappedEvent(Of TAggregateKey).Wrap(instance, record.Sequence, "", "", record.Timestamp, record.Version, ""))
                            End If
                        End If
                    End While
                End Using
                Return ret
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, MyBase.m_key.ToString(), 0, "Unable to read events - Azure file not initialised")
            End If

        End Function




        ''' <summary>
        ''' Create a new windows azure file stream reader to read events from the file
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The domain in which the aggregate resides
        ''' </param>
        ''' <remarks>
        ''' The unique key which identifies the instance of the aggregate to read the event stream for
        ''' </remarks>
        Private Sub New(ByVal AggregateDomainName As String,
                        ByVal AggregateKey As TAggregateKey,
                        Optional ByVal settings As IFileStreamSettings = Nothing,
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
        ''' Creates an azure file storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As IFileStreamSettings = Nothing,
                                      Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing) As IEventStreamReader(Of TAggregate, TAggregateKey)


            Return Create(instance, instance.GetKey(), settings, eventFilter)

        End Function


        ''' <summary>
        ''' Creates an azure file storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                      ByVal key As TAggregateKey,
                                      Optional ByVal settings As IFileStreamSettings = Nothing,
                                      Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing) As IEventStreamReader(Of TAggregate, TAggregateKey)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If
            Return New FileEventStreamReader(Of TAggregate, TAggregateKey)(domainName, key, settings, eventFilter)

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
                                                         Optional ByVal settings As IFileStreamSettings = Nothing,
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


    Public Module FileEventStreamReaderFactory

        ''' <summary>
        ''' Creates an azure file storage based event stream writer for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to write the event stream
        ''' </param>
        ''' <param name="settings">
        ''' Any additional settings 
        ''' </param>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
            ByVal key As TAggregateKey,
            Optional ByVal settings As IFileStreamSettings = Nothing) As IEventStreamReader(Of TAggregate, TAggregateKey)

            Return FileEventStreamReader(Of TAggregate, TAggregateKey).Create(instance, key, settings)

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
                                                         Optional ByVal settings As IFileStreamSettings = Nothing,
                                                         Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing) As ProjectionProcessor(Of TAggregate, TAggregateKey)

            Return FileEventStreamReader(Of TAggregate, TAggregateKey).CreateProjectionProcessor(instance, settings, eventFilter)

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