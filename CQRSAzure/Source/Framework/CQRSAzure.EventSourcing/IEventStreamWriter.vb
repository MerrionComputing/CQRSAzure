Imports System.Collections.Generic
''' <summary>
''' Interface for any class that appends events to the end of an event stream
''' </summary>
''' <remarks>
''' An event stream is an append-only data structure (regardless of if the underlying storage mechanism allows deletes and inserts)
''' </remarks>
Public Interface IEventStreamWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
    Inherits IEventStream(Of TAggregate, TAggregateKey)

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
    Function AppendEvent(ByVal EventInstance As IEvent(Of TAggregate),
                    Optional ByVal ExpectedTopSequence As Long = 0) As Task

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
    Function AppendEvents(ByVal StartingSequence As Long, ByVal Events As IEnumerable(Of IEvent(Of TAggregate))) As Task


    ''' <summary>
    ''' Set the context under which the writer is currently writing to the file
    ''' </summary>
    ''' <param name="writerContext">
    ''' The additional context details that teh writer should write alongside the event when persisting it
    ''' </param>
    ''' <remarks>
    ''' This is a property applied to the writer rather than the event as the event may not know its context when it
    ''' is being passed to a writer (for example if it comes from a batch / IoT event producer)
    ''' </remarks>
    Sub SetContext(ByVal writerContext As IWriteContext)

End Interface
