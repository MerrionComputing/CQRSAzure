''' <summary>
''' Definition for any implementation that can read events from an event stream
''' </summary>
''' <typeparam name="TAggregate">
''' The data type of the aggregate that identifies the event stream to read
''' </typeparam>
''' <typeparam name="TAggregationKey">
''' The data type of the key that uniquely identifies the specific event stream instance to read
''' </typeparam>
Public Interface IEventStreamReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)

    ''' <summary>
    ''' Get the event stream for a given aggregate
    ''' </summary>
    Function GetEvents() As IEnumerable(Of IEvent(Of TAggregate))

    ''' <summary>
    ''' Gets the event stream for a given aggregate from a given starting version
    ''' </summary>
    ''' <param name="StartingVersion">
    ''' The starting version number for our snapshot
    ''' </param>
    ''' <remarks>
    ''' This is used in scenario where we are starting from a given snapshot version
    ''' </remarks>
    Function GetEvents(ByVal StartingVersion As UInteger) As IEnumerable(Of IEvent(Of TAggregate))

    ''' <summary>
    ''' Gets the event stream and the context information recorded for each event
    ''' </summary>
    ''' <remarks>
    ''' This is typically only used for audit trails as all business functionality should depend on the event data alone
    ''' </remarks>
    Function GetEventsWithContext() As IEnumerable(Of IEventContext)


End Interface

''' <summary>
''' Definition for any implementation that can read events from an event stream with filtering applied
''' </summary>
''' <typeparam name="TAggregate">
''' The data type of the aggregate that identifies the event stream to read
''' </typeparam>
''' <typeparam name="TAggregationKey">
''' The data type of the key that uniquely identifies the specific event stream instance to read
''' </typeparam>
''' <remarks>
''' This allows projection definitions and classifies which know the event types they handle in advance to only receive these 
''' </remarks>
Public Interface IEventStreamFilteredReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)
    Inherits IEventStreamReader(Of TAggregate, TAggregationKey)

    ''' <summary>
    ''' Get the event stream for a given aggregate filtered to only return the valid event types
    ''' </summary>
    ''' <param name="ValidEventTypes">
    ''' The set of valid event types to return
    ''' </param>
    Function GetFilteredEvents(ByVal ValidEventTypes As IEnumerable(Of Type)) As IEnumerable(Of IEvent)

    ''' <summary>
    ''' Gets the event stream for a given aggregate from a given starting version,
    ''' filtered to only return the valid event types
    ''' </summary>
    ''' <param name="StartingVersion">
    ''' The starting version number for our snapshot
    ''' </param>
    ''' <param name="ValidEventTypes">
    ''' The set of valid event types to return
    ''' </param>
    ''' <remarks>
    ''' This is used in scenario where we are starting from a given snapshot version
    ''' </remarks>
    Function GetFilteredEvents(ByVal StartingVersion As UInteger, ByVal ValidEventTypes As IEnumerable(Of Type)) As IEnumerable(Of IEvent)


End Interface