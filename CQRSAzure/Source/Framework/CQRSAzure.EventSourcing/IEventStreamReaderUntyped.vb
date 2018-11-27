Imports System
Imports System.Collections.Generic
''' <summary>
''' Definition for any implementation that can read events from an event stream that is untyped
''' </summary>
''' <remarks>
''' This requires the domain name, aggregate type name and key to be explicitly set as they
''' cannot be derived from the aggregate identifier class
''' </remarks>
Public Interface IEventStreamReaderUntyped
    Inherits IEventStreamUntypedIdentity

    ''' <summary>
    ''' The unique identifier of the instance for which we are reading the events 
    ''' </summary>
    ReadOnly Property Key As String

    ''' <summary>
    ''' Get the event stream for a given aggregate instance
    ''' </summary>
    Function GetEvents() As Task(Of IEnumerable(Of IEvent))


    ''' <summary>
    ''' Gets the event stream for a given aggregate from a given starting version
    ''' </summary>
    ''' <param name="StartingSequenceNumber">
    ''' The starting sequence number for our snapshot
    ''' </param>
    ''' <remarks>
    ''' This is used in scenario where we are starting from a given snapshot version
    ''' </remarks>
    Function GetEvents(Optional ByVal StartingSequenceNumber As UInteger = 0,
                       Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As Task(Of IEnumerable(Of IEvent))

    ''' <summary>
    ''' Gets the event stream and the context information recorded for each event
    ''' </summary>
    ''' <param name="StartingSequenceNumber">
    ''' The starting sequence number for our snapshot
    ''' </param>
    ''' <remarks>
    ''' This is typically only used for audit trails as all business functionality should depend on the event data alone
    ''' </remarks>
    Function GetEventsWithContext(Optional ByVal StartingSequenceNumber As UInteger = 0,
                                  Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As Task(Of IEnumerable(Of IEventContext))


End Interface
