
Imports System
''' <summary>
''' Definition for any implementation that can read events from an event stream with filtering applied
''' </summary>
''' <typeparam name="TAggregate">
''' The data type of the aggregate that identifies the event stream to read
''' </typeparam>
''' <typeparam name="TAggregateKey">
''' The data type of the key that uniquely identifies the specific event stream instance to read
''' </typeparam>
''' <remarks>
''' This allows projection definitions and classifies which know the event types they handle in advance to only receive these 
''' </remarks>
Public Interface IEventStreamFilteredReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
    Inherits IEventStreamReader(Of TAggregate, TAggregateKey)

    ''' <summary>
    ''' Is the given event type valid for the currently set filter conditions
    ''' </summary>
    ''' <param name="eventType">
    ''' The type of the event read from the read stream
    ''' </param>
    Function IsEventValid(ByVal eventType As Type) As Boolean

End Interface