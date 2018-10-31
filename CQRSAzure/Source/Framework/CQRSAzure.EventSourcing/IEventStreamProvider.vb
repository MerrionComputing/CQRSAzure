''' <summary>
''' Interface to be implemented by any class that provides an implementation of an individual event stream - for example anything 
''' that is a reader or writer
''' </summary>
''' <typeparam name="TaggregateKey">
''' The data type of the aggregate that identifies the event stream to read
''' </typeparam>
''' <remarks>
''' This allows for a common set of functionality against backing technologies that may have very different actual im[plementation
''' details
''' </remarks>
Public Interface IEventStreamInstanceProvider(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TaggregateKey)

    ''' <summary>
    ''' The unique key of the aggregate whose event stream this reader is operating over
    ''' </summary>
    ReadOnly Property Key As TaggregateKey


End Interface

''' <summary>
''' Interface to be implemented by any class that provides an implementation of a type of event stream - for example anything 
''' that is a reader or writer
''' </summary>
''' <typeparam name="TaggregateKey">
''' The data type of the aggregate that identifies the event stream to read
''' </typeparam>
''' <remarks>
''' This allows for a common set of functionality against backing technologies that may have very different actual implementation
''' details
''' </remarks>
Public Interface IEventStreamProvider(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TaggregateKey)

    ''' <summary>
    ''' The name of the class providing the event stream(s) over which this provider operates
    ''' </summary>
    ReadOnly Property AggregateClassName As String

    ''' <summary>
    ''' Get all the known event streams' unique identifiers as at the given point in time (or currently)
    ''' </summary>
    ''' <param name="asOfDate">
    ''' if supplied, the point in time for which we want to know all the members (otherwise assumed to be as of now)
    ''' </param>
    Function GetAllStreamKeys(Optional ByVal asOfDate As Nullable(Of DateTime) = Nothing) As IEnumerable(Of TaggregateKey)

End Interface

Public Interface IEventStreamProviderUntyped

    ''' <summary>
    ''' The name of the class providing the event stream(s) over which this provider operates
    ''' </summary>
    ReadOnly Property AggregateClassName As String

    ''' <summary>
    ''' Get all the known event streams' unique identifiers (as string) as at the given point in time (or currently)
    ''' </summary>
    ''' <param name="asOfDate">
    ''' if supplied, the point in time for which we want to know all the members (otherwise assumed to be as of now)
    ''' </param>
    Function GetAllStreamKeys(Optional ByVal asOfDate As Nullable(Of DateTime) = Nothing) As IEnumerable(Of String)

End Interface