Imports System.Runtime.Serialization
''' <summary>
''' marker interface to denote anything as being an event
''' </summary>
''' <remarks>
''' Ideally the type-safe versio of this that is an aggregate's event
''' should be used where possible
''' </remarks>
Public Interface IEvent

End Interface

Public Interface IJsonSerialisedEvent
    Inherits IEvent

    ReadOnly Property FullClassName As String

    ReadOnly Property EventInstanceAsJson As Newtonsoft.Json.Linq.JObject

End Interface

''' <summary>
''' Marker interface for an event pertaining to an aggregation
''' </summary>
''' <typeparam name="TAggregate">
''' The type which identifies the aggregation
''' </typeparam>
Public Interface IEvent(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier)
    Inherits IEvent, ISerializable

    ''' <summary>
    ''' The version of the event 
    ''' </summary>
    ReadOnly Property Version As UInteger

End Interface
