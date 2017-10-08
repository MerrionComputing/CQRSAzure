Imports CQRSAzure.EventSourcing


Public Interface IClassifierSnapshotReader(Of TAggregate As IAggregationIdentifier,
                 TAggregateKey,
                 TClassifier As IClassifier)


    ''' <summary>
    ''' Load the snapshot data from the backing storage technology
    ''' </summary>
    ''' <param name="key">
    ''' The unique key of the aggregate we are retrieving an identity group snapshot for
    ''' </param>
    ''' <param name="OnOrBeforeTimestamp">
    ''' if specified, get the latest snapshot prior to the given timestamp
    ''' </param>
    Function GetSnapshot(ByVal key As TAggregateKey, Optional ByVal OnOrBeforeTimestamp As Nullable(Of DateTime) = Nothing) As IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier)


End Interface
