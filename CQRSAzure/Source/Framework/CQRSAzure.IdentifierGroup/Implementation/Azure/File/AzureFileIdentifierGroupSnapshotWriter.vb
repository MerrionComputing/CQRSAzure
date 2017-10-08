Imports CQRSAzure.EventSourcing.Azure.File

Namespace Azure.File
    Public NotInheritable Class AzureFileIdentifierGroupSnapshotWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Implements IIdentifierGroupSnapshotWriter(Of TAggregate, TAggregateKey)

        Public Sub SaveSnapshot(ByVal group As IIdentifierGroup, snapshotToSave As IIdentifierGroupSnapshot(Of TAggregate, TAggregateKey)) Implements IIdentifierGroupSnapshotWriter(Of TAggregate, TAggregateKey).SaveSnapshot
            Throw New NotImplementedException()
        End Sub
    End Class
End Namespace