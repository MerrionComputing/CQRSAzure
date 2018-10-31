Imports CQRSAzure.EventSourcing

Public Interface IClassifierSnapshotWriter(Of TAggregate As IAggregationIdentifier,
                                               TAggregateKey,
                                               TClassifier As IClassifier)



    ''' <summary>
    ''' Save the snapshot data to the backing storage technology
    ''' </summary>
    ''' <param name="key">
    ''' The unique key of the aggregate we are storing a classifier snapshot for
    ''' </param>
    ''' <param name="snapshotToSave">
    ''' The specific classifier snapshot to save
    ''' </param>
    Sub SaveSnapshot(ByVal key As TAggregateKey, ByVal snapshotToSave As IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier))


End Interface

Public Interface IClassifierSnapshotWriterUntyped

    ''' <summary>
    ''' Save the snapshot data to the backing storage technology
    ''' </summary>
    ''' <param name="key">
    ''' The unique key of the aggregate we are storing a classifier snapshot for
    ''' </param>
    ''' <param name="snapshotToSave">
    ''' The specific classifier snapshot to save
    ''' </param>
    Sub SaveSnapshot(ByVal key As String, ByVal snapshotToSave As IClassifierSnapshotUntyped)



End Interface