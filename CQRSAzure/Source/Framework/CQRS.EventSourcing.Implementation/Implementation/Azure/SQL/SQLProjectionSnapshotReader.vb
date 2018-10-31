Imports CQRSAzure.EventSourcing

Namespace Azure.SQL

    Public NotInheritable Class SQLProjectionSnapshotReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                TAggregateKey,
                                                                TProjection As IProjection)
        Inherits SQLProjectionSnapshotBase(Of TAggregate, TAggregateKey)
        Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

        Public Function GetSnapshot(key As TAggregateKey, Optional OnOrBeforeSequence As UInteger = 0) As IProjectionSnapshot(Of TAggregate, TAggregateKey) Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).GetSnapshot
            Throw New NotImplementedException()
        End Function

        Public Function GetLatestSnapshotSequence(key As TAggregateKey, Optional OnOrBeforeSequence As UInteger = 0) As UInteger Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).GetLatestSnapshotSequence
            Throw New NotImplementedException()
        End Function

    End Class

End Namespace