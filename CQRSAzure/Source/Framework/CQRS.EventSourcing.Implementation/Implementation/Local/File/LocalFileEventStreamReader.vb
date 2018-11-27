Imports System
Imports System.Collections.Generic
Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Local.File

Namespace Local.File
    Public NotInheritable Class LocalFileEventStreamReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits LocalFileEventStreamBase(Of TAggregate, TAggregateKey)
        Implements IEventStreamReader(Of TAggregate, TAggregateKey)


        Public ReadOnly Property Key As TAggregateKey Implements IEventStreamInstanceProvider(Of TAggregate, TAggregateKey).Key
            Get
                Return MyBase.m_key
            End Get
        End Property




        Public Async Function GetEvents() As Task(Of IEnumerable(Of IEvent(Of TAggregate))) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEvents

            Return Await GetEvents(0)

        End Function

        Public Async Function GetEvents(Optional StartingSequenceNumber As UInteger = 0,
                                  Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As Task(Of IEnumerable(Of IEvent(Of TAggregate))) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEvents

            Return Await Task.Run(Function()
                                      Dim ret As New List(Of IEvent(Of TAggregate))

                                      If (MyBase.m_file IsNot Nothing) Then
                                          m_file.Refresh()
                                          If (m_file.Exists) Then
                                              Using fr = m_file.OpenRead()
                                                  LoadEventStreamDetailBlock(fr)
                                                  If (StartingSequenceNumber > 0) Then
                                                      fr.Seek(StartingSequenceNumber, IO.SeekOrigin.Begin)
                                                  Else
                                                      fr.Seek(MyBase.m_streamstart, IO.SeekOrigin.Begin)
                                                  End If
                                                  While Not fr.Position >= m_file.Length
                                                      Dim record As LocalFileWrappedEvent = MyBase.Deserialise(Of LocalFileWrappedEvent)(fr)
                                                      If (record IsNot Nothing) Then
                                                          ret.Add(record.EventInstance)
                                                      End If
                                                  End While
                                              End Using
                                          End If
                                      End If

                                      Return ret

                                  End Function
                )

        End Function

        Public Async Function GetEventsWithContext(Optional StartingSequenceNumber As UInteger = 0,
                                             Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As Task(Of IEnumerable(Of IEventContext)) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEventsWithContext

            Return Await Task.Run(Function()
                                      Dim ret As New List(Of IEventContext)

                                      If (MyBase.m_file IsNot Nothing) Then
                                          'Re-read the file details from the OS
                                          MyBase.m_file.Refresh()
                                          Using fr = m_file.OpenRead()
                                              Dim bf As New BinaryFormatter()
                                              If (StartingSequenceNumber > 0) Then
                                                  fr.Seek(StartingSequenceNumber, IO.SeekOrigin.Begin)
                                              Else
                                                  fr.Seek(m_streamstart, IO.SeekOrigin.Begin)
                                              End If
                                              While Not fr.Position >= m_file.Length
                                                  Dim record As LocalFileWrappedEvent = CType(bf.Deserialize(fr), LocalFileWrappedEvent)
                                                  If (record IsNot Nothing) Then
                                                      Dim instance = InstanceWrappedEvent(Of TAggregateKey).Wrap(m_key, record.EventInstance, record.Version)
                                                      ret.Add(ContextWrappedEvent(Of TAggregateKey).Wrap(instance, record.Sequence, "", "", record.Timestamp, record.Version, ""))
                                                  End If
                                              End While
                                          End Using
                                      End If

                                      Return ret
                                  End Function
                )
        End Function



        Protected Sub New(AggregateDomainName As String,
                       AggregateKey As TAggregateKey,
                       Optional settings As ILocalFileSettings = Nothing)

            MyBase.New(AggregateDomainName, AggregateKey, writeAccess:=False, settings:=settings)

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
                                      Optional ByVal settings As ILocalFileSettings = Nothing) As IEventStreamReader(Of TAggregate, TAggregateKey)


            Return Create(instance, instance.GetKey(), settings)

        End Function


        ''' <summary>
        ''' Creates an local file storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                      ByVal key As TAggregateKey,
                                      Optional ByVal settings As ILocalFileSettings = Nothing) As IEventStreamReader(Of TAggregate, TAggregateKey)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            Return New LocalFileEventStreamReader(Of TAggregate, TAggregateKey)(domainName, key, settings)

        End Function

        ''' <summary>
        ''' Create a projection processor that works off an local file system backed event stream
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to run projections
        ''' </param>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Shared Function CreateProjectionProcessor(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                                         Optional ByVal settings As ILocalFileSettings = Nothing,
                                                         Optional ByVal snapshotProcessor As ISnapshotProcessor(Of TAggregate, TAggregateKey) = Nothing
                                                         ) As ProjectionProcessor(Of TAggregate, TAggregateKey)

            Return New ProjectionProcessor(Of TAggregate, TAggregateKey)(Create(instance, settings), snapshotProcessor)

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

    Public Module LocalFileEventStreamReaderFactory

        ''' <summary>
        ''' Creates an local file storage based event stream writer for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to write the event stream
        ''' </param>
        ''' <param name="settings">
        ''' Any additional settings 
        ''' </param>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
            ByVal key As TAggregateKey,
            Optional ByVal settings As ILocalFileSettings = Nothing) As IEventStreamReader(Of TAggregate, TAggregateKey)

            Return LocalFileEventStreamReader(Of TAggregate, TAggregateKey).Create(instance, key, settings)

        End Function

        ''' <summary>
        ''' Create a projection processor that works off an local file backed event stream
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to run projections
        ''' </param>
        ''' <param name="key">
        ''' The unique key by which the aggregate instance the projection is running over is identified
        ''' </param>
        ''' <param name="settings">
        ''' (Optional) Configuration settings controlling the event stream being read
        ''' </param>
        ''' <param name="snapshotProcessor">
        ''' (Optional) A processor to allow the projection to take point-in-time snapshots
        ''' </param>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Function CreateProjectionProcessor(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)(ByVal instance As TAggregate,
                                                                                                                                  ByVal key As TAggregateKey,
                                                                                                                                  Optional ByVal settings As ILocalFileSettings = Nothing,
                                                                                                                                  Optional ByVal snapshotProcessor As ISnapshotProcessor(Of TAggregate, TAggregateKey) = Nothing
                                                                                                                                  ) As ProjectionProcessor(Of TAggregate, TAggregateKey)

            Return LocalFileEventStreamReader(Of TAggregate, TAggregateKey).CreateProjectionProcessor(instance, settings, snapshotProcessor)

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