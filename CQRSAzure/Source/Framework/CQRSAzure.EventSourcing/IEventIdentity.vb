''' <summary>
''' Interface to allow unique identification of an event
''' </summary>
''' <typeparam name="TAggregate">
''' The type which identifies the aggregation
''' </typeparam>
''' <remarks>
''' These are the infrastructure elements of an event that do not have any business meaning.
''' </remarks>
Public Interface IEventIdentity(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier)

    ''' <summary>
    ''' Get the identifier by which this events aggregate is uniquely known
    ''' </summary>
    ''' <remarks>
    ''' Most implementation suse a GUID for this but if you have a known unique identifier 
    ''' then that can be used instead - e.g. ISBN, CUSIP, VIN etc.
    ''' </remarks>
    Function GetAggregateIdentifier() As String

    ''' <summary>
    ''' The event sequence - this is the order in which the events occured for the aggregate
    ''' </summary>
    ReadOnly Property Sequence As UInteger

    ''' <summary>
    ''' The event that is identified by this event identity
    ''' </summary>
    ReadOnly Property EventInstance As IEvent(Of TAggregate)

End Interface
