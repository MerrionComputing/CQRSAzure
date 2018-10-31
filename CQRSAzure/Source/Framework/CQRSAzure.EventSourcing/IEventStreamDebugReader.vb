''' <summary>
''' A reader for an event stream that supports stepping through events one at a atime for diagnostic or demostration purposes
''' </summary>
''' <typeparam name="TAggregate">
''' The data type of the aggregate that identifies the event stream to read
''' </typeparam>
''' <typeparam name="TAggregateKey">
''' The data type of the key that uniquely identifies the specific event stream instance to read
''' </typeparam>
''' <remarks>
''' This is intentionally kept separate to the core Event Stream Reader functionality to prevent it being used as part of the 
''' main production system
''' </remarks>
Public Interface IEventStreamDebugReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
    Inherits IEventStreamReader(Of TAggregate, TAggregateKey)

    ''' <summary>
    ''' The current event sequence in the stream being read
    ''' </summary>
    ''' <remarks>
    ''' This is somewhat analoguous to an object pointer to the event in a memory based system
    ''' </remarks>
    ReadOnly Property CurrentSequenceNumber As Long

    ''' <summary>
    ''' Get the next event from the stream given the current sequence number
    ''' </summary>
    ''' <returns>
    ''' An event wrapped in the event context information
    ''' </returns>
    Function GetNextEventWithContext() As IEventContext

    ''' <summary>
    ''' Reset the event stream to the starting position 
    ''' </summary>
    Sub ResetToStart()

End Interface
