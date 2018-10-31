Imports CQRSAzure.EventSourcing.Local.File

Namespace Local.File
    ''' <summary>
    ''' Class for reader using local file storage for snapshots of identifier group membership
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The data type of the aggregate whose members are in the group
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type by which these aggregate instances are uniquely identified
    ''' </typeparam>
    Public NotInheritable Class LocalFileIdentifierGroupSnapshotWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                      TAggregateKey)
        Inherits LocalFileIdentifierGroupSnapshotBase(Of TAggregate, TAggregateKey)
        Implements IIdentifierGroupSnapshotWriter(Of TAggregate, TAggregateKey)

        Public Sub SaveSnapshot(ByVal group As IIdentifierGroup,
                                snapshotToSave As IIdentifierGroupSnapshot(Of TAggregate, TAggregateKey)) Implements IIdentifierGroupSnapshotWriter(Of TAggregate, TAggregateKey).SaveSnapshot
            Throw New NotImplementedException()
        End Sub

    End Class
End Namespace