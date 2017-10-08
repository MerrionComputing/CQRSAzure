

Public Interface IAggregateImplementationMap(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Inherits IAggregateImplementationMap

#Region "Functions"

    ''' <summary>
    ''' Create an event stream reader that can be used to read the event stream for this aggregate 
    ''' </summary>
    ''' <param name="aggregate">
    ''' The instance of the aggregate for which we want the event stream reader
    ''' </param>
    ''' <param name="key">
    ''' The unique identifier of the instance of the aggregate for which we want the eveny stream reader
    ''' </param>
    ''' <returns></returns>
    Function CreateReader(ByVal aggregate As TAggregate, key As TAggregateKey) As IEventStreamReader(Of TAggregate, TAggregateKey)

    ''' <summary>
    ''' Create a projection processor to run over event streams of this aggregate
    ''' </summary>
    ''' <param name="aggregate">
    ''' The instance of the aggregate for which we want to run projections
    ''' </param>
    ''' <param name="key">
    ''' The unique identifier of the instance of the aggregate for which we want to run projections
    ''' </param>
    Function CreateProjectionProcessor(ByVal aggregate As TAggregate, key As TAggregateKey) As IProjectionProcessor(Of TAggregate, TAggregateKey)

    ''' <summary>
    ''' Create an event stream writer that can be used to write events to the event stream for this aggregate instance
    ''' </summary>
    ''' <param name="aggregate">
    ''' The aggregate instance for which we are creating the event stream writer
    ''' </param>
    ''' <param name="key">
    ''' The unique identifier by which the instance of the aggregate is known
    ''' </param>
    Function CreateWriter(ByVal aggregate As TAggregate, key As TAggregateKey) As IEventStreamWriter(Of TAggregate, TAggregateKey)

    ''' <summary>
    ''' Create a snapshot reader of the given projection over this event stream
    ''' </summary>
    ''' <typeparam name="TProjection">
    ''' The type of the projection being run over the event stream
    ''' </typeparam>
    ''' <param name="instance">
    ''' The aggregate instance for which this event stream projection reader is being created
    ''' </param>
    ''' <param name="key">
    ''' The unique identifier by which the instance of the aggregate is known
    ''' </param>
    Function CreateSnapshotReader(Of TProjection As IProjection)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey), ByVal key As TAggregateKey) As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

    ''' <summary>
    ''' Create a snapshot writer for the given projection over this event stream
    ''' </summary>
    ''' <typeparam name="TProjection">
    ''' The type of the projection being run over the event stream
    ''' </typeparam>
    ''' <param name="instance">
    ''' The aggregate instance for which this event stream projection reader is being created
    ''' </param>
    ''' <param name="key">
    ''' The unique identifier by which the instance of the aggregate is known
    ''' </param>
    Function CreateSnapshotWriter(Of TProjection As IProjection)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey), ByVal key As TAggregateKey) As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

#End Region

End Interface

Public Interface IAggregateImplementationMap

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
    ''' <param name="key">
    ''' The key that uniquely identifies an instance of this aggregate type
    ''' </param>
    ''' <param name="settings">
    ''' Additional configuration settings used to set up the event stream
    ''' </param>
    ''' <returns>
    ''' The event stream reader to use to process this event stream
    ''' </returns>
    Delegate Function ReaderCreationFunction(Of TAggregate As IAggregationIdentifier, TAggregateKey)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                                                                                     ByVal key As TAggregateKey,
                                                                                                     ByVal settings As IEventStreamSettings) As IEventStreamReader(Of TAggregate, TAggregateKey)

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
    ''' <param name="key">
    ''' The key that uniquely identifies an instance of this aggregate type
    ''' </param>
    ''' <param name="settings">
    ''' Additional configuration settings used to set up the event stream
    ''' </param>
    ''' <returns>
    ''' The event stream writer to use to append to this event stream
    ''' </returns>
    Delegate Function WriterCreationFunction(Of TAggregate As IAggregationIdentifier, TAggregateKey)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                                                                                     ByVal key As TAggregateKey,
                                                                                                     ByVal settings As IEventStreamSettings) As IEventStreamWriter(Of TAggregate, TAggregateKey)

    ''' <summary>
    ''' A function that calls a factory method to create an instance of a projection snapshot reader
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The data type of the aggregate the event stream belongs to
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type of the key that uniquely identifies the aggregate isntanec for which to create a projection snapshot reader
    ''' </typeparam>
    ''' <param name="instance">
    ''' The specific aggregate instance for which to create a projection snapshot reader
    ''' </param>
    ''' <param name="key">
    ''' The key that uniquely identifies an instance of this aggregate type
    ''' </param>
    ''' <param name="settings">
    ''' Additional configuration settings used to set up the event stream
    ''' </param>
    Delegate Function SnapshotReaderCreationFunction(Of TAggregate As IAggregationIdentifier, TAggregateKey, TProjection As IProjection)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                                                                                                                         ByVal key As TAggregateKey,
                                                                                                                                         ByVal settings As IEventStreamSettings) As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)



    ''' <summary>
    ''' A function that calls a factory method to create an instance of a projection snapshot writer
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The data type of the aggregate the event stream belongs to
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type of the key that uniquely identifies the aggregate isntanec for which to create a projection snapshot writer
    ''' </typeparam>
    ''' <param name="instance">
    ''' The specific aggregate instance for which to create a projection snapshot writer
    ''' </param>
    ''' <param name="key">
    ''' The key that uniquely identifies an instance of this aggregate type
    ''' </param>
    ''' <param name="settings">
    ''' Additional configuration settings used to set up the event stream
    ''' </param>
    Delegate Function SnapshotWriterCreationFunction(Of TAggregate As IAggregationIdentifier, TAggregateKey, TProjection As IProjection)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                                                                                                                         ByVal key As TAggregateKey,
                                                                                                                                         ByVal settings As IEventStreamSettings) As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

#End Region




End Interface