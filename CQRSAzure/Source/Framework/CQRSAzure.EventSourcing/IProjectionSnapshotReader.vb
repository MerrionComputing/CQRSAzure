''' <summary>
''' Interface for any implementation class that finds and reads cached projection snapshot records
''' </summary>
''' <typeparam name="TAggregate">
''' The data type that the projection is run against
''' </typeparam>
''' <typeparam name="TAggregateKey">
''' The data type of the key which uniquely identifies an instance of the aggregate
''' </typeparam>
''' <typeparam name="TProjection">
''' The data type of the projection that is having a snapshot saved
''' </typeparam>
Public Interface IProjectionSnapshotReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                               TAggregateKey,
                                               TProjection As CQRSAzure.EventSourcing.IProjection)

    ''' <summary>
    ''' Load the snapshot data to the backing storage technology
    ''' </summary>
    ''' <param name="key">
    ''' The unique key of the aggregate we are retrieving a projection snapshot for
    ''' </param>
    ''' <param name="OnOrBeforeSequence">
    ''' if specified, get the latest snapshot prior to the given sequence number
    ''' </param>
    Function GetSnapshot(ByVal key As TAggregateKey, Optional ByVal OnOrBeforeSequence As UInteger = 0) As IProjectionSnapshot(Of TAggregate, TAggregateKey)

    ''' <summary>
    ''' Gets the sequence number of the latest snapshot held for a given aggregate instance
    ''' </summary>
    ''' <param name="key">
    ''' The unique key of the aggregate we are retrieving a projection snapshot for
    ''' </param>
    ''' <param name="OnOrBeforeSequence">
    ''' if specified, get the latest snapshot prior to the given sequence number
    ''' </param>
    ''' <returns>
    ''' If zero, there are no snapshots found for this aggregate key
    ''' </returns>
    Function GetLatestSnapshotSequence(ByVal key As TAggregateKey, Optional ByVal OnOrBeforeSequence As UInteger = 0) As UInteger

End Interface
