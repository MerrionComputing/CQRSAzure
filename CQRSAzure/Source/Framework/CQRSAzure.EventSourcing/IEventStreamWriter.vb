''' <summary>
''' Interface for any class that appends events to the end of an event stream
''' </summary>
''' <remarks>
''' An event stream is an append-only data structure (regardless of if the underlying storage mechanism allows deletes and inserts)
''' </remarks>
Public Interface IEventStreamWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)
    Inherits IEventStream(Of TAggregate, TAggregationKey)

    ''' <summary>
    ''' Save an event onto the end of the store
    ''' </summary>
    ''' <param name="EventInstance">
    ''' The specific event to append to the end of the store
    ''' </param>
    ''' <remarks>
    ''' The events store must be both immutable and forward-only so to cater for the concept of "delete" a 
    ''' reversal event needs to exist
    ''' </remarks>
    Sub AppendEvent(ByVal EventInstance As IEvent(Of TAggregate))

    ''' <summary>
    ''' Save a set of events onto the end of the store
    ''' </summary>
    ''' <param name="StartingSequence">
    ''' The initial sequence number to start numbering the events from
    ''' </param>
    ''' <param name="Events">
    ''' The set of events to record agains this aggregate
    ''' </param>
    ''' <remarks>
    ''' The events store must be both immutable and forward-only so to cater for the concept of "delete" a 
    ''' reversal event needs to exist
    ''' </remarks>
    Sub AppendEvents(ByVal StartingSequence As UInteger, ByVal Events As IEnumerable(Of IEvent(Of TAggregate)))

End Interface
