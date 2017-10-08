Imports CQRSAzure.EventSourcing

Namespace Azure.SQL

    Public NotInheritable Class SQLProjectionSnapshotWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                TAggregateKey,
                                                                TProjection As IProjection)
        Inherits SQLProjectionSnapshotBase(Of TAggregate, TAggregateKey)
        Implements IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

        Public Sub SaveSnapshot(key As TAggregateKey, snapshotToSave As IProjectionSnapshot(Of TAggregate, TAggregateKey)) Implements IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection).SaveSnapshot
            Throw New NotImplementedException()
        End Sub


    End Class

End Namespace
