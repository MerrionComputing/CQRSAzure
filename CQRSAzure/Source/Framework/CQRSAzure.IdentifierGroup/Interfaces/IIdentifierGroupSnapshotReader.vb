Public Interface IIdentifierGroupSnapshotReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                    TAggregateKey)


    ''' <summary>
    ''' Load the snapshot data from the backing storage technology
    ''' </summary>
    ''' <param name="group">
    ''' The identifier of the group we are retrieving an identity group snapshot for
    ''' </param>
    ''' <param name="OnOrBeforeTimestamp">
    ''' if specified, get the latest snapshot prior to the given timestamp
    ''' </param>
    Function GetSnapshot(ByVal group As IIdentifierGroup, Optional ByVal OnOrBeforeTimestamp As Nullable(Of DateTime) = Nothing) As IIdentifierGroupSnapshot(Of TAggregate, TAggregateKey)

End Interface
