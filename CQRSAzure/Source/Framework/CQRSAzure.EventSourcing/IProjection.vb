Option Explicit On
Imports System
Imports System.Collections.Generic

''' <summary>
''' Marker interface to denote anything as being a projection over the given aggregate identifier
''' </summary>
''' <remarks>
''' The type-safety is to ensure the projection only operates on events of one kind
''' </remarks>
Public Interface IProjection(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Inherits IProjection

    ''' <summary>
    ''' Load the state of this projection from a saved snapshot
    ''' </summary>
    ''' <param name="snapshotToLoad">
    ''' The snapshot to load the projection state from
    ''' </param>
    Sub LoadFromSnapshot(ByVal snapshotToLoad As IProjectionSnapshot(Of TAggregate, TAggregateKey))

    ''' <summary>
    ''' Turn the current state of this projection to a snapshot
    ''' </summary>
    Function ToSnapshot() As IProjectionSnapshot(Of TAggregate, TAggregateKey)

End Interface


Public Interface IProjectionUntyped
    Inherits IProjection

    ''' <summary>
    ''' Does the projection handle the data for the given event type
    ''' </summary>
    ''' <param name="eventTypeFullName">
    ''' The full name of the event containing the data that may or may not be handled
    ''' </param>
    ''' <returns>
    ''' True if this event type should get processed
    ''' </returns>
    Function HandlesEventTypeByName(ByVal eventTypeFullName As String) As Boolean

    Sub HandleEventJSon(ByVal EventTypeFullName As String, ByVal eventToHandle As Newtonsoft.Json.Linq.JObject)

    ''' <summary>
    ''' Load the state of this projection from a saved snapshot
    ''' </summary>
    ''' <param name="snapshotToLoad">
    ''' The snapshot to load the projection state from
    ''' </param>
    Sub LoadFromSnapshot(ByVal snapshotToLoad As IProjectionSnapshot)

    ''' <summary>
    ''' Turn the current state of this projection to a snapshot
    ''' </summary>
    Function ToSnapshot() As IProjectionSnapshot

End Interface

''' <summary>
''' Marker interface to denote anything as being a projection
''' </summary>
Public Interface IProjection
    Inherits IStateChangeTracking

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

    ''' <summary>
    ''' The current sequence number of the projection class
    ''' </summary>
    ReadOnly Property CurrentSequenceNumber As UInteger

    ''' <summary>
    ''' Called when a projection has handled an event 
    ''' </summary>
    ''' <param name="handledEventSequenceNumber">
    ''' The sequence number of the event that has been competed - this allows the projection to keep track of where
    ''' in the event stream it has got to
    ''' </param>
    Sub MarkEventHandled(ByVal handledEventSequenceNumber As UInteger)

    ''' <summary>
    ''' Perform whatever processing is required to handle the specific event
    ''' </summary>
    ''' <param name="eventToHandle">
    ''' The specific event to handle and perform whatever processing is required
    ''' </param>
    Sub HandleEvent(Of TEvent As IEvent)(ByVal eventToHandle As TEvent)

    ''' <summary>
    ''' An event was read by the underlying event reader (whether it is handled or not)
    ''' </summary>
    ''' <param name="sequenceNumber">
    ''' The sequence number of the event read
    ''' </param>
    ''' <param name="asOfDate">
    ''' If the event has an "effective date" this is 
    ''' </param>
    Sub OnEventRead(ByVal sequenceNumber As UInteger, Optional ByVal asOfDate As Nullable(Of Date) = Nothing)

    ''' <summary>
    ''' The current as-of date for this projection
    ''' </summary>
    ''' <remarks>
    ''' This is only updated where an event is processed that has an as-of date field as part of its data properties
    ''' </remarks>
    ReadOnly Property CurrentAsOfDate As DateTime

    ''' <summary>
    ''' The current set of values the projection has as at the current point in time
    ''' </summary>
    ''' <remarks>
    ''' These are the business-meaningful properties of the projection
    ''' </remarks>
    ReadOnly Property CurrentValues As IEnumerable(Of ProjectionSnapshotProperty)

End Interface