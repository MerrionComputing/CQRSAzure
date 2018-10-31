Public Interface ISnapshotProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey)

    ''' <summary>
    ''' Save the given projection snapshot to the underlying storage technology
    ''' </summary>
    ''' <param name="snapshotToSave">
    ''' The projection snapshot to be saved to the backing storage
    ''' </param>
    Sub SaveSnapshot(ByVal snapshotToSave As IProjectionSnapshot(Of TAggregate, TAggregateKey))

    ''' <summary>
    ''' Get the most recent projection snapshot from the underlying storage technology
    ''' </summary>
    ''' <param name="priorToSequence">
    ''' if this is non zero get the snapshot record closest to the prior-to sequence number (inclusive)
    ''' </param>
    ''' <returns>
    ''' The most recent snapshot available if one has been saved - or Nothing (null) if none exists
    ''' </returns>
    Function GetLatestSnapshot(Optional ByVal priorToSequence As UInteger = 0) As IProjectionSnapshot(Of TAggregate, TAggregateKey)

End Interface
