Option Explicit On

''' <summary>
''' Marker interface to denote anything as being a projection over the given aggregate identifier
''' </summary>
''' <remarks>
''' The type-safety is to ensure the projection only operates on events of one kind
''' </remarks>
Public Interface IProjection(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Inherits IProjection

    ''' <summary>
    ''' Perform whatever processing is required to handle the specific event
    ''' </summary>
    ''' <param name="eventToHandle">
    ''' The specific event to handle and perform whatever processing is required
    ''' </param>
    Sub HandleEvent(Of TEvent As IEvent(Of TAggregate))(ByVal eventToHandle As TEvent)

    ''' <summary>
    ''' Load the snapshot of this projection as the starting point for populating this projection
    ''' </summary>
    ''' <param name="snapshotToLoad">
    ''' The stored snapshot for the given projection
    ''' </param>
    Sub LoadSnapshot(ByVal snapshotToLoad As IProjectionSnapshot(Of TAggregate, TAggregateKey))

End Interface

''' <summary>
''' Marker interface to denote anything as being a projection
''' </summary>
Public Interface IProjection

    ''' <summary>
    ''' Does this projection use snapshots to save the latest state or does it need to rebuild the entire 
    ''' projection every time?
    ''' </summary>
    ''' <returns>
    ''' True if the projection supports snapshots
    ''' </returns>
    ReadOnly Property SupportsSnapshots As Boolean


    ''' <summary>
    ''' Does the projection handle the data for the given event type
    ''' </summary>
    ''' <param name="eventType">
    ''' The type of the event containing the data that may or may not be handled
    ''' </param>
    ''' <returns>
    ''' True if this event type should get processed
    ''' </returns>
    Function HandlesEventType(ByVal eventType As Type) As Boolean


End Interface