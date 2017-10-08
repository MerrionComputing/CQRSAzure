Imports CQRSAzure.EventSourcing.InMemory

Namespace InMemory
    Public NotInheritable Class InMemoryIdentifierGroupSnapshotReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits InMemoryIdentifierGroupSnapshotBase(Of TAggregate, TAggregateKey)
        Implements IIdentifierGroupSnapshotReader(Of TAggregate, TAggregateKey)

        Public Function GetSnapshot(ByVal group As IIdentifierGroup, Optional OnOrBeforeTimestamp As Date? = Nothing) As IIdentifierGroupSnapshot(Of TAggregate, TAggregateKey) Implements IIdentifierGroupSnapshotReader(Of TAggregate, TAggregateKey).GetSnapshot
            Throw New NotImplementedException()
        End Function

#Region "constructor"

        Private Sub New(ByVal groupName As String,
                        Optional ByVal settings As IInMemorySettings = Nothing)
            MyBase.New(groupName, settings)

        End Sub

        Public Shared Function Create(ByVal groupName As String, Optional ByVal settings As IInMemorySettings = Nothing) As IIdentifierGroupSnapshotReader(Of TAggregate, TAggregateKey)
            Return New InMemoryIdentifierGroupSnapshotReader(Of TAggregate, TAggregateKey)(groupName, settings)
        End Function

#End Region


    End Class

    Public Module InMemoryIdentifierGroupSnapshotReaderFactory

        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)(ByVal groupName As String,
                                                                                                                ByVal aggregate As TAggregate,
                                                                                                                ByVal key As TAggregateKey,
                                                                                                                Optional ByVal settings As IInMemorySettings = Nothing
                                                                                                                ) As IIdentifierGroupSnapshotReader(Of TAggregate, TAggregateKey)

            Return InMemoryIdentifierGroupSnapshotReader(Of TAggregate, TAggregateKey).Create(groupName, settings)

        End Function

    End Module

End Namespace