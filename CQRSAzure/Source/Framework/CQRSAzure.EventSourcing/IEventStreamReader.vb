''' <summary>
''' Definition for any implementation that can read events from an event stream
''' </summary>
''' <typeparam name="TAggregate">
''' The data type of the aggregate that identifies the event stream to read
''' </typeparam>
''' <typeparam name="TAggregateKey">
''' The data type of the key that uniquely identifies the specific event stream instance to read
''' </typeparam>
Public Interface IEventStreamReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
    Inherits IEventStreamInstanceProvider(Of TAggregate, TAggregateKey)




    ''' <summary>
    ''' Get the event stream for a given aggregate
    ''' </summary>
    Function GetEvents() As IEnumerable(Of IEvent(Of TAggregate))

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
                       Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As IEnumerable(Of IEvent(Of TAggregate))

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
                                  Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As IEnumerable(Of IEventContext)


End Interface
