''' <summary>
''' Interface for any implementation class that writes cached projection snapshot records
''' </summary>
''' <typeparam name="TAggregate">
''' The data type that the projection is run against
''' </typeparam>
''' <typeparam name="TAggregateKey">
''' The data type of the key which uniquely identifies an instance of the aggregate
''' </typeparam>
Public Interface IProjectionSnapshotWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                               TAggregateKey,
                                               TProjection As CQRSAzure.EventSourcing.IProjection)

    ''' <summary>
    ''' Save the snapshot data to the backing storage technology
    ''' </summary>
    ''' <param name="key">
    ''' The unique key of the aggregate we are storing a projection snapshot for
    ''' </param>
    ''' <param name="snapshotToSave">
    ''' The specific projection snapshot to save
    ''' </param>
    Sub SaveSnapshot(ByVal key As TAggregateKey, ByVal snapshotToSave As IProjectionSnapshot(Of TAggregate, TAggregateKey))

End Interface
