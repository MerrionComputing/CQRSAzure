Imports System
Imports System.Collections.Generic
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Local.File

Namespace Local.File

    ''' <summary>
    ''' Implementation class to write to an event stream based on an local files backing store
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The type of the aggregate this event stream is attached to
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The type of the key that uniquely identifies an instance of that key
    ''' </typeparam>
    Public NotInheritable Class LocalFileEventStreamWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits LocalFileEventStreamBase(Of TAggregate, TAggregateKey)
        Implements IEventStreamWriter(Of TAggregate, TAggregateKey)

        Private m_writerContext As IWriteContext

        Public ReadOnly Property Key As TAggregateKey Implements IEventStream(Of TAggregate, TAggregateKey).Key, IEventStreamInstanceProvider(Of TAggregate, TAggregateKey).Key
            Get
                Return MyBase.m_key
            End Get
        End Property

        Public ReadOnly Property LastAddition As Date? Implements IEventStream(Of TAggregate, TAggregateKey).LastAddition
            Get
                If (MyBase.m_file IsNot Nothing) Then
                    If (MyBase.m_file.Exists) Then
                        Return MyBase.m_file.LastWriteTimeUtc()
                    End If
                End If

                Return Nothing
            End Get
        End Property

        Public ReadOnly Property RecordCount As ULong Implements IEventStream(Of TAggregate, TAggregateKey).RecordCount
            Get
                If (MyBase.m_eventStreamDetailBlock IsNot Nothing) Then
                    Return m_eventStreamDetailBlock.RecordCount
                End If
                Return 0
            End Get
        End Property


        Public Async Function AppendEvent(EventInstance As IEvent(Of TAggregate),
                               Optional ByVal ExpectedTopSequence As Long = 0) As Task Implements IEventStreamWriter(Of TAggregate, TAggregateKey).AppendEvent

            InitialiseFileIfNotExists()

            Await AppendEventInternal(EventInstance)

            SaveEventStreamDetailBlock()

        End Function


        Public Async Function AppendEvents(StartingSequence As Long, Events As IEnumerable(Of IEvent(Of TAggregate))) As Task Implements IEventStreamWriter(Of TAggregate, TAggregateKey).AppendEvents

            InitialiseFileIfNotExists()

            For Each evt As IEvent(Of TAggregate) In Events
                Await AppendEventInternal(evt)
            Next

            SaveEventStreamDetailBlock()

        End Function

        ''' <summary>
        ''' Append an event to the file, without saving the record count
        ''' </summary>
        ''' <param name="EventInstance"></param>
        Private Async Function AppendEventInternal(EventInstance As IEvent(Of TAggregate)) As Task

            Await Task.Run(Sub()
                               If (MyBase.m_file IsNot Nothing) Then

                                   Dim evtToWrite As New LocalFileWrappedEvent(m_eventStreamDetailBlock.SequenceNumber,
                                                            EventInstance.Version,
                                                            DateTime.UtcNow,
                                                            EventInstance,
                                                            m_setings.UnderlyingSerialiser)
                                   If (evtToWrite IsNot Nothing) Then
                                       Using fs = m_file.OpenWrite()
                                           fs.Seek(0, IO.SeekOrigin.End)
                                           'write the event to the stream here..
                                           MyBase.Serialise(fs, evtToWrite)
                                           m_eventStreamDetailBlock.SequenceNumber = fs.Position
                                       End Using
                                       m_eventStreamDetailBlock.RecordCount += 1
                                   End If
                               End If
                           End Sub
                           )
        End Function


        Private Sub InitialiseFileIfNotExists()

            If (Not MyBase.m_file.Exists) Then
                'save the current 
                SaveEventStreamDetailBlock()
                m_streamstart = m_eventStreamDetailBlock.SequenceNumber
            End If

        End Sub

        Private Sub SaveEventStreamDetailBlock()

            Using fr = MyBase.m_file.OpenWrite()
                fr.Seek(0, IO.SeekOrigin.Begin)
                MyBase.Serialise(Of EventStreamDetailBlock)(fr, MyBase.m_eventStreamDetailBlock)
                If (MyBase.m_eventStreamDetailBlock.SequenceNumber = 0) Then
                    MyBase.m_eventStreamDetailBlock.SequenceNumber = fr.Position
                End If
            End Using

        End Sub

        Public Sub SetContext(writerContext As IWriteContext) Implements IEventStreamWriter(Of TAggregate, TAggregateKey).SetContext
            m_writerContext = writerContext
        End Sub



        ''' <summary>
        ''' Clear down the event stream
        ''' </summary>
        ''' <remarks>
        ''' This will delete existing events so should not be done in any production environment therefore this is not
        ''' part of the IEventStreamWriter interface
        ''' </remarks>
        Public Sub Reset()

            If (MyBase.m_file IsNot Nothing) Then
                MyBase.m_file.Delete()
                MyBase.m_file.Refresh()
                'Recreate the blob file that was deleted
                MyBase.m_eventStreamDetailBlock.RecordCount = 0
                MyBase.m_eventStreamDetailBlock.DateCreated = DateTime.UtcNow
                MyBase.m_eventStreamDetailBlock.SequenceNumber = 0
                InitialiseFileIfNotExists()
            End If
        End Sub

        Protected Sub New(AggregateDomainName As String,
               AggregateKey As TAggregateKey,
               Optional settings As ILocalFileSettings = Nothing)

            MyBase.New(AggregateDomainName, AggregateKey, writeAccess:=True, settings:=settings)

        End Sub

#Region "Factory methods"

        ''' <summary>
        ''' Creates an local file storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As ILocalFileSettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)


            Return Create(instance, instance.GetKey(), settings)

        End Function


        ''' <summary>
        ''' Creates an local file storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                      ByVal key As TAggregateKey,
                                      Optional ByVal settings As ILocalFileSettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            Return New LocalFileEventStreamWriter(Of TAggregate, TAggregateKey)(domainName, key, settings)

        End Function

#End Region

    End Class

    Public Module LocalFileEventStreamWriterFactory

        ''' <summary>
        ''' Creates an local file storage based event stream writer for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to write the event stream
        ''' </param>
        ''' <param name="settings">
        ''' Any additional settings 
        ''' </param>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)(ByVal instance As TAggregate,
            ByVal key As TAggregateKey,
            Optional ByVal settings As ILocalFileSettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)

            Return LocalFileEventStreamWriter(Of TAggregate, TAggregateKey).Create(instance, key, settings)

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