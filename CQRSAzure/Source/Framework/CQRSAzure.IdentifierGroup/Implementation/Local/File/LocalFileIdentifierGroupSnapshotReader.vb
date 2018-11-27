Imports System
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
    Public NotInheritable Class LocalFileIdentifierGroupSnapshotReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                      TAggregateKey)
        Inherits LocalFileIdentifierGroupSnapshotBase(Of TAggregate, TAggregateKey)
        Implements IIdentifierGroupSnapshotReader(Of TAggregate, TAggregateKey)

        ''' <summary>
        ''' Gets the latest snapshot of the identifier group members 
        ''' </summary>
        ''' <param name="group"></param>
        ''' <param name="OnOrBeforeTimestamp"></param>
        ''' <returns></returns>
        Public Function GetSnapshot(ByVal group As IIdentifierGroup,
                                    Optional OnOrBeforeTimestamp As Date? = Nothing) As IIdentifierGroupSnapshot(Of TAggregate, TAggregateKey) Implements IIdentifierGroupSnapshotReader(Of TAggregate, TAggregateKey).GetSnapshot
            Throw New NotImplementedException()
        End Function

    End Class
End Namespace