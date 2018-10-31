''' <summary>
''' Interface for any class that appends events to the end of an event stream, baserd only on the domain and aggregate
''' names (This is not type safe)
''' </summary>
''' <remarks>
''' An event stream is an append-only data structure (regardless of if the underlying storage mechanism allows deletes and inserts)
''' </remarks>
Public Interface IEventStreamWriterUntyped
    Inherits IEventStreamUntyped

    ''' <summary>
    ''' Set the context under which this event stream writer is writing events
    ''' </summary>
    ''' <param name="writerContext">
    ''' This can set the user id or commentary to tage the events with
    ''' </param>
    Sub SetContext(writerContext As IWriteContext)

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
    Sub AppendEvent(ByVal EventInstance As IEvent,
                    Optional ByVal ExpectedTopSequence As Long = 0,
                    Optional ByVal Version As UInteger = 1)

End Interface
