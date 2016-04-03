''' <summary>
''' A stream of zero or more events that have occured for a particular unique instance of an aggregate 
''' identifier
''' </summary>
''' <typeparam name="TAggregate">
''' The type of the aggregate against which these events have been recorded
''' </typeparam>
''' <typeparam name="TAggregationKey">
''' The data type that provides the unique identifier of this aggregate
''' </typeparam>
''' <remarks>
''' Ideally the aggregation key type should be a simple data type (integer, GUID etc.) rather than a compound class, 
''' to make the lookup process faster
''' </remarks>
Public Interface IEventStream(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)

    ''' <summary>
    ''' The unique key of the aggregation record this event stream pertains to
    ''' </summary>
    ReadOnly Property Key As TAggregationKey


    ''' <summary>
    ''' The number of record in this event stream
    ''' </summary>
    ReadOnly Property RecordCount As UInt64

    ''' <summary>
    ''' When was the last record written to this event stream
    ''' </summary>
    ReadOnly Property LastAddition As Nullable(Of DateTime)

End Interface
