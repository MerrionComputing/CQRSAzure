Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports CQRSAzure.EventSourcing.InMemory

Namespace InMemory

    ''' <summary>
    ''' Class to write events to an in-memory store
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The type of the base class to which the event stream is attached
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type by which an instance of that aggregation base class is uniquely identified
    ''' </typeparam>
    Public NotInheritable Class InMemoryEventStreamWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits InMemoryEventStreamBase(Of TAggregate, TAggregateKey)
        Implements IEventStreamWriter(Of TAggregate, TAggregateKey)

        Private m_sequence As UInteger = 0


#Region "Event stream information"

        Public ReadOnly Property Key As TAggregateKey Implements IEventStream(Of TAggregate, TAggregateKey).Key, IEventStreamInstanceProvider(Of TAggregate, TAggregateKey).Key
            Get
                Return MyBase.AggregationKey
            End Get
        End Property

        Public Overloads ReadOnly Property RecordCount As ULong Implements IEventStream(Of TAggregate, TAggregateKey).RecordCount
            Get
                Return MyBase.RecordCount
            End Get
        End Property

        Private m_lastAddition As Nullable(Of Date)
        Public ReadOnly Property LastAddition As Date? Implements IEventStream(Of TAggregate, TAggregateKey).LastAddition
            Get
                Return m_lastAddition
            End Get
        End Property

#End Region

#Region "Event stream functionality"
        Public Overloads Async Function AppendEvent(EventInstance As IEvent(Of TAggregate),
                                         Optional ByVal ExpectedTopSequence As Long = 0) As Task Implements IEventStreamWriter(Of TAggregate, TAggregateKey).AppendEvent

            Await Task.Run(Sub()
                               If (m_context Is Nothing) Then
                                   MyBase.AppendEvent(EventInstance)
                               Else
                                   MyBase.AppendEvent(EventInstance,
                                  commentary:=m_context.Commentary,
                                   source:=m_context.Source,
                                   who:=m_context.Who)
                               End If

                               'and update the last addition date/time
                               m_lastAddition = DateTime.UtcNow

                           End Sub
            )

        End Function

        Public Async Function AppendEvents(StartingVersion As Long, Events As IEnumerable(Of IEvent(Of TAggregate))) As Task Implements IEventStreamWriter(Of TAggregate, TAggregateKey).AppendEvents

            If (StartingVersion < (m_sequence + Events.Count)) Then
                Throw New ArgumentException("Out of sequence event(s) appended")
            End If

            For Each evt In Events
                Await AppendEvent(evt)
                StartingVersion += 1
            Next

        End Function

        Private m_context As IWriteContext
        Public Sub SetContext(writerContext As IWriteContext) Implements IEventStreamWriter(Of TAggregate, TAggregateKey).SetContext
            m_context = writerContext
        End Sub
#End Region

        ''' <summary>
        ''' Clear down the event stream
        ''' </summary>
        ''' <remarks>
        ''' This will delete existing events so should not be done in any production environment therefore this is not
        ''' part of the IEventStreamWriter interface
        ''' </remarks>
        Public Sub Reset()

            MyBase.m_eventStream.Clear()

        End Sub


        ''' <summary>
        ''' Creates a new event stream writer to write events to the event stream for the given aggregate
        ''' </summary>
        ''' <param name="aggregateIdentityKey">
        ''' The unique identifier fo the instance of that class
        ''' </param>
        Private Sub New(ByVal aggregateIdentityKey As TAggregateKey,
                        Optional ByVal settings As IInMemorySettings = Nothing)
            MyBase.New(aggregateIdentityKey, settings)

        End Sub

#Region "Factory methods"

        ''' <summary>
        ''' Creates an in-memory event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As IInMemorySettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)

            Return New InMemoryEventStreamWriter(Of TAggregate, TAggregateKey)(instance.GetKey(), settings)

        End Function

        ''' <summary>
        ''' Creates an in-memory event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instanceKey">
        ''' The unique identifier of the instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function Create(ByVal instanceKey As TAggregateKey,
                                      Optional ByVal settings As IInMemorySettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)

            Return New InMemoryEventStreamWriter(Of TAggregate, TAggregateKey)(instanceKey, settings)

        End Function

#End Region

    End Class

    Public Module InMemoryEventStreamWriterFactory

        ''' <summary>
        ''' Creates an in-memory event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                   TAggregateKey)(ByVal instance As TAggregate,
                                                  ByVal key As TAggregateKey,
                                                  ByVal settings As IInMemorySettings) As IEventStreamWriter(Of TAggregate, TAggregateKey)

            Return InMemoryEventStreamWriter(Of TAggregate, TAggregateKey).Create(instance, settings)

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