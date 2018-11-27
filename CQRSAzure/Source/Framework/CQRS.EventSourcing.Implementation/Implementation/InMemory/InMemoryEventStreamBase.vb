Option Explicit On
Option Strict On
Imports System
Imports System.Collections.Concurrent
Imports System.Linq
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.InMemory

Namespace InMemory
    Public MustInherit Class InMemoryEventStreamBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits InMemoryEventStreamProvider(Of TAggregate, TAggregateKey)

        Protected ReadOnly m_debugMessages As Boolean

        'The shared data area that backs this aggregate type's event streams
        Protected Shared m_eventStream As New ConcurrentDictionary(Of TAggregateKey, ConcurrentBag(Of IEventContext(Of TAggregateKey)))


        ReadOnly m_key As TAggregateKey
        Protected ReadOnly Property AggregationKey As TAggregateKey
            Get
                Return m_key
            End Get
        End Property

        Protected ReadOnly Property RecordCount As Integer
            Get
                If (m_eventStream.ContainsKey(m_key)) Then
                    Return m_eventStream(m_key).Count()
                Else
                    Return 0
                End If
            End Get
        End Property

        ''' <summary>
        ''' Wrap an event in context information and add it to the internal stream for the aggregate
        ''' </summary>
        ''' <param name="EventInstance"></param>
        Protected Sub AppendEvent(ByVal EventInstance As IEvent,
                                  Optional ByVal commentary As String = "",
                                  Optional ByVal sequence As Long = 0,
                                  Optional ByVal source As String = "",
                                  Optional ByVal who As String = "")

            If Not (m_eventStream.ContainsKey(m_key)) Then
                m_eventStream.AddOrUpdate(m_key,
                                          New ConcurrentBag(Of IEventContext(Of TAggregateKey)),
                                          Function(key, oldValue) oldValue)
            End If

            If Not (m_eventStreamCreation.ContainsKey(m_key)) Then
                m_eventStreamCreation.AddOrUpdate(m_key, DateTime.UtcNow, Function(key, value) value)
            End If

            If (sequence = 0) Then
                sequence = m_eventStream(m_key).LongCount
            End If

            'Because, in this implementation, all the streams are stored in the same data structure we wrap them in identity when storing them
            Dim instanceEvent As IEventInstance(Of TAggregateKey) = InstanceWrappedEvent(Of TAggregateKey).Wrap(m_key, EventInstance, 0)

            'Events are also wrapped in context information 
            Dim contextEvent As IEventContext(Of TAggregateKey) = ContextWrappedEvent(Of TAggregateKey).Wrap(instanceEvent,
                                                                                                                 sequence,
                                                                                                                commentary,
                                                                                                                source,
                                                                                                                DateTime.UtcNow,
                                                                                                                0,
                                                                                                                who)

            m_eventStream(m_key).Add(contextEvent)

#Region "Debug messages"
            If (m_debugMessages) Then
                System.Diagnostics.Debug.WriteLine("Appended event number {0} - {1}", {sequence, EventInstance})
            End If
#End Region

        End Sub



        Protected Sub CreateStreamIfNotCreated()
            If Not (m_eventStream.ContainsKey(m_key)) Then
                m_eventStream.AddOrUpdate(m_key, New ConcurrentBag(Of IEventContext(Of TAggregateKey)), Function(key, oldValue) oldValue)
                m_eventStreamCreation.AddOrUpdate(m_key, DateTime.UtcNow, Function(key, value) DateTime.UtcNow)
            End If
        End Sub

        ''' <summary>
        ''' Creates a new event stream writer to read events from the event stream for the given aggregate
        ''' </summary>
        ''' <param name="aggregateIdentityKey">
        ''' The unique identifier fo the instance of that aggregate class
        ''' </param>
        ''' <param name="settings">
        ''' The additional settings to control how event streams are created in memory
        ''' </param>
        Protected Sub New(ByVal aggregateIdentityKey As TAggregateKey,
                          Optional ByVal settings As IInMemorySettings = Nothing)

            m_key = aggregateIdentityKey

            If (settings IsNot Nothing) Then
                m_debugMessages = settings.DebugMessages
            End If

            ' If we do not yet have an event stream for this aggregagate, create one
            CreateStreamIfNotCreated()

        End Sub

        ''' <summary>
        ''' Reset the specific event stream
        ''' </summary>
        ''' <param name="aggregateIdentityKey">
        ''' The unique key for which to clear all events
        ''' </param>
        ''' <remarks>
        ''' This should be used to ensure a known "start state" for any given event stream for 
        ''' reliable repeatable unit tests
        ''' </remarks>
        Public Shared Sub ResetStream(ByVal aggregateIdentityKey As TAggregateKey)

            If (m_eventStream IsNot Nothing) Then
                If (m_eventStream.ContainsKey(aggregateIdentityKey)) Then
                    m_eventStream.AddOrUpdate(aggregateIdentityKey, New ConcurrentBag(Of IEventContext(Of TAggregateKey)), Function(key, value) New ConcurrentBag(Of IEventContext(Of TAggregateKey)))
                    Dim createdDate As Date
                    m_eventStreamCreation.TryRemove(aggregateIdentityKey, createdDate)
                End If
            End If

        End Sub


    End Class
End Namespace