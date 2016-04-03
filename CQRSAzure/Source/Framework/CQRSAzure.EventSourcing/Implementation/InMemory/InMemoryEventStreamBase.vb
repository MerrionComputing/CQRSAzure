Option Explicit On
Option Strict On

Namespace InMemory
    Public MustInherit Class InMemoryEventStreamBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)


        'The shared data area that backs this aggregate type's event streams
        Private Shared m_eventStream As New Dictionary(Of TAggregationKey, List(Of IEventContext(Of TAggregationKey)))

        ReadOnly m_key As TAggregationKey
        Protected ReadOnly Property AggregationKey As TAggregationKey
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
                m_eventStream.Add(m_key, New List(Of IEventContext(Of TAggregationKey)))
            End If

            If (sequence = 0) Then
                sequence = m_eventStream(m_key).LongCount
            End If

            'Because, in this implementation, all the streams are stored in the same data structure we wrap them in identity when storing them
            Dim instanceEvent As IEventInstance(Of TAggregationKey) = InstanceWrappedEvent(Of TAggregationKey).Wrap(m_key, EventInstance, 0)

            'Events are also wrapped in context information 
            Dim contextEvent As IEventContext(Of TAggregationKey) = ContextWrappedEvent(Of TAggregationKey).Wrap(instanceEvent,
                                                                                                                 sequence,
                                                                                                                commentary,
                                                                                                                source,
                                                                                                                DateTime.UtcNow,
                                                                                                                0,
                                                                                                                who)

            m_eventStream(m_key).Add(contextEvent)

        End Sub

        Protected Overloads Function GetEvents() As IEnumerable(Of IEvent(Of TAggregate))

            ' If we do not yet have an event stream for this aggregagate, create one
            If Not (m_eventStream.ContainsKey(m_key)) Then
                m_eventStream.Add(m_key, New List(Of IEventContext(Of TAggregationKey)))
            End If

            'Strip all the wrappers off the events
            Dim dryAllEvents = From x In m_eventStream(m_key)
                               Select x.EventInstance

            Return dryAllEvents.AsEnumerable().Cast(Of IEvent(Of TAggregate))

        End Function

        Protected Overloads Function GetEvents(StartingVersion As UInteger) As IEnumerable(Of IEvent(Of TAggregate))

            ' If we do not yet have an event stream for this aggregagate, create one
            If Not (m_eventStream.ContainsKey(m_key)) Then
                m_eventStream.Add(m_key, New List(Of IEventContext(Of TAggregationKey)))
            End If

            'Strip all the wrappers off the events
            Dim dryAllEvents = From x In m_eventStream(m_key)
                               Where (x.SequenceNumber >= StartingVersion)
                               Select x.EventInstance

            Return dryAllEvents.AsEnumerable().Cast(Of IEvent(Of TAggregate))

        End Function

        Protected Function GetEventsWithContext() As IEnumerable(Of IEventContext)

            ' If we do not yet have an event stream for this aggregagate, create one
            If Not (m_eventStream.ContainsKey(m_key)) Then
                m_eventStream.Add(m_key, New List(Of IEventContext(Of TAggregationKey)))
            End If

            Return m_eventStream(m_key)

        End Function

        ''' <summary>
        ''' Creates a new event stream writer to read events from the event stream for the given aggregate
        ''' </summary>
        ''' <param name="aggregateType">
        ''' The type (class) of aggregate this is
        ''' </param>
        ''' <param name="aggregateIdentityKey">
        ''' The unique identifier fo the instance of that aggregate class
        ''' </param>
        ''' <param name="settings">
        ''' The additional settings to control how event streams are created in memory
        ''' </param>
        Protected Sub New(ByVal aggregateType As TAggregate,
                          ByVal aggregateIdentityKey As TAggregationKey,
                          Optional ByVal settings As IInMemorySettings = Nothing)

            m_key = aggregateIdentityKey

            ' If we do not yet have an event stream for thsi aggregagate, create one
            If Not (m_eventStream.ContainsKey(aggregateIdentityKey)) Then
                m_eventStream.Add(aggregateIdentityKey, New List(Of IEventContext(Of TAggregationKey)))
            End If

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
        Public Shared Sub ResetStream(ByVal aggregateIdentityKey As TAggregationKey)

            If (m_eventStream IsNot Nothing) Then
                If (m_eventStream.ContainsKey(aggregateIdentityKey)) Then
                    m_eventStream(aggregateIdentityKey).Clear()
                End If
            End If

        End Sub

    End Class
End Namespace