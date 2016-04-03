Namespace InMemory
    Public NotInheritable Class InMemoryEventStreamWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)
        Inherits InMemoryEventStreamBase(Of TAggregate, TAggregationKey)
        Implements IEventStreamWriter(Of TAggregate, TAggregationKey)

        Private m_sequence As UInteger = 0


#Region "Event stream information"

        Public ReadOnly Property Key As TAggregationKey Implements IEventStream(Of TAggregate, TAggregationKey).Key
            Get
                Return MyBase.AggregationKey
            End Get
        End Property

        Public Overloads ReadOnly Property RecordCount As ULong Implements IEventStream(Of TAggregate, TAggregationKey).RecordCount
            Get
                Return MyBase.RecordCount
            End Get
        End Property

        Private m_lastAddition As Nullable(Of Date)
        Public ReadOnly Property LastAddition As Date? Implements IEventStream(Of TAggregate, TAggregationKey).LastAddition
            Get
                Return m_lastAddition
            End Get
        End Property
#End Region

#Region "Event stream functionality"
        Public Overloads Sub AppendEvent(EventInstance As IEvent(Of TAggregate)) Implements IEventStreamWriter(Of TAggregate, TAggregationKey).AppendEvent


            MyBase.AppendEvent(EventInstance)

            'and update the last addition date/time
            m_lastAddition = DateTime.UtcNow
        End Sub

        Public Sub AppendEvents(StartingVersion As UInteger, Events As IEnumerable(Of IEvent(Of TAggregate))) Implements IEventStreamWriter(Of TAggregate, TAggregationKey).AppendEvents

            If (StartingVersion < (m_sequence + Events.Count)) Then
                Throw New ArgumentException("Out of sequence event(s) appended")
            End If

            For Each evt In Events
                AppendEvent(evt)
                StartingVersion += 1
            Next

        End Sub
#End Region

        ''' <summary>
        ''' Creates a new event stream writer to write events to the event stream for the given aggregate
        ''' </summary>
        ''' <param name="aggregateType">
        ''' The type (class) of aggregate this is
        ''' </param>
        ''' <param name="aggregateIdentityKey">
        ''' The unique identifier fo the instance of that class
        ''' </param>
        Private Sub New(ByVal aggregateType As TAggregate, ByVal aggregateIdentityKey As TAggregationKey)
            MyBase.New(aggregateType, aggregateIdentityKey)

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
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregationKey)) As IEventStreamWriter(Of TAggregate, TAggregationKey)

            Return New InMemoryEventStreamWriter(Of TAggregate, TAggregationKey)(instance, instance.GetKey())

        End Function

#End Region

    End Class
End Namespace