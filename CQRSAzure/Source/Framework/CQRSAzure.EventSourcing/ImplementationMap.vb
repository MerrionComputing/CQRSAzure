
''' <summary>
''' Map between an aggregate and the actual implementation class for the events stream of instances of that aggregate
''' </summary>
''' <remarks>
''' This can be set on a per-aggregate-type basis or on a global basis in the application .config files
''' </remarks>
Public Module ImplementationMap

#Region "Delegate declarations"
    ''' <summary>
    ''' A function that calls a factory method to create an instance of an event stream reader
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The data type of the aggregate the event stream belongs to
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type of the key that uniquely identifies the aggregate isntanec for which to create an event stream reader
    ''' </typeparam>
    ''' <param name="instance">
    ''' The specific aggregate instance for which to create an event stream reader
    ''' </param>
    ''' <returns>
    ''' The event stream reader to use to process this event stream
    ''' </returns>
    Public Delegate Function ReaderCreationFunction(Of TAggregate As IAggregationIdentifier, TAggregateKey)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey)) As IEventStreamReader(Of TAggregate, TAggregateKey)

    ''' <summary>
    ''' A function that calls a factory method to create an instance of an event stream writer
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The data type of the aggregate the event stream belongs to
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type of the key that uniquely identifies the aggregate isntanec for which to create an event stream writer
    ''' </typeparam>
    ''' <param name="instance">
    ''' The specific aggregate instance for which to create an event stream writer
    ''' </param>
    ''' <returns>
    ''' The event stream writer to use to append to this event stream
    ''' </returns>
    Public Delegate Function WriterCreationFunction(Of TAggregate As IAggregationIdentifier, TAggregateKey)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey)) As IEventStreamWriter(Of TAggregate, TAggregateKey)

#End Region


    ''' <summary>
    ''' The different backing technologies that can be used for an event stream in this library
    ''' </summary>
    Public Enum SupportedEventStreamImplementations
        ''' <summary>
        ''' Use whatever is set as the system default
        ''' </summary>
        [Default] = 0
        ''' <summary>
        ''' In-memory event stream for unit testing and proof-of-concept work
        ''' </summary>
        InMemory = 1
        ''' <summary>
        ''' Azure append-only blobs
        ''' </summary>
        ''' <remarks>
        ''' This is currentlythe most performant option
        '''</remarks>
        AzureBlob = 10
        ''' <summary>
        ''' Azure storage files
        ''' </summary>
        AzureFile = 11
        ''' <summary>
        ''' Azure hosted SQL Server database 
        ''' </summary>
        AzureSQL = 12
        ''' <summary>
        ''' Azure Table
        ''' </summary>
        AzureTable = 13
    End Enum

    ''' <summary>
    ''' If no implementation is specified we fall back to the default
    ''' </summary>
    Public Const DefaultImplementation As SupportedEventStreamImplementations = SupportedEventStreamImplementations.AzureBlob

    ''' <summary>
    ''' Reads in any aggregate to implementation mappings from the config file
    ''' </summary>
    Public Sub InitialiseAggregateImplementationMap()

    End Sub

End Module
