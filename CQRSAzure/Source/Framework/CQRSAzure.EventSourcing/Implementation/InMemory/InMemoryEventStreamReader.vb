Namespace InMemory
    Public NotInheritable Class InMemoryEventStreamReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)
        Inherits InMemoryEventStreamBase(Of TAggregate, TAggregationKey)
        Implements IEventStreamReader(Of TAggregate, TAggregationKey)

        Public Overloads Function GetEvents() As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregationKey).GetEvents
            Return MyBase.GetEvents()
        End Function

        Public Overloads Function GetEvents(StartingVersion As UInteger) As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregationKey).GetEvents
            Return MyBase.GetEvents(StartingVersion)
        End Function

        Public Overloads Function GetEventsWithContext() As IEnumerable(Of IEventContext) Implements IEventStreamReader(Of TAggregate, TAggregationKey).GetEventsWithContext
            Return MyBase.GetEventsWithContext()
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
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregationKey)) As IEventStreamReader(Of TAggregate, TAggregationKey)

            Return New InMemoryEventStreamReader(Of TAggregate, TAggregationKey)(instance, instance.GetKey())

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
        Public Shared Function CreateProjectionProcessor(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregationKey)) As ProjectionProcessor(Of TAggregate, TAggregationKey)

            Return New ProjectionProcessor(Of TAggregate, TAggregationKey)(Create(instance))

        End Function
#End Region

    End Class
End Namespace