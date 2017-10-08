Imports CQRSAzure.EventSourcing.Azure.File

Namespace Azure.File
    Public NotInheritable Class AzureFileIdentifierGroupSnapshotReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Implements IIdentifierGroupSnapshotReader(Of TAggregate, TAggregateKey)

        Public Function GetSnapshot(ByVal group As IIdentifierGroup, Optional OnOrBeforeTimestamp As Date? = Nothing) As IIdentifierGroupSnapshot(Of TAggregate, TAggregateKey) Implements IIdentifierGroupSnapshotReader(Of TAggregate, TAggregateKey).GetSnapshot
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace