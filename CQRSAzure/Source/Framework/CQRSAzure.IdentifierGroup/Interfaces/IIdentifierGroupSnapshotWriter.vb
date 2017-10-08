Public Interface IIdentifierGroupSnapshotWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                    TAggregateKey)

    ''' <summary>
    ''' Save the snapshot data to the backing storage technology
    ''' </summary>
    ''' <param name="group">
    ''' The identifier of the group we are retrieving an identity group snapshot for
    ''' </param>
    ''' <param name="snapshotToSave">
    ''' The specific identity group snapshot to save
    ''' </param>
    Sub SaveSnapshot(ByVal group As IIdentifierGroup, ByVal snapshotToSave As IIdentifierGroupSnapshot(Of TAggregate, TAggregateKey))

End Interface
