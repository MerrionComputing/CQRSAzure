Imports System
Imports CQRSAzure.EventSourcing.InMemory

Namespace InMemory
    Public NotInheritable Class InMemoryIdentifierGroupSnapshotWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits InMemoryIdentifierGroupSnapshotBase(Of TAggregate, TAggregateKey)
        Implements IIdentifierGroupSnapshotWriter(Of TAggregate, TAggregateKey)

        Public Sub SaveSnapshot(ByVal group As IIdentifierGroup, snapshotToSave As IIdentifierGroupSnapshot(Of TAggregate, TAggregateKey)) Implements IIdentifierGroupSnapshotWriter(Of TAggregate, TAggregateKey).SaveSnapshot
            Throw New NotImplementedException()
        End Sub

#Region "constructor"

        Private Sub New(ByVal groupName As String, Optional ByVal settings As IInMemorySettings = Nothing)
            MyBase.New(groupName, settings)

        End Sub

        Public Shared Function Create(ByVal groupName As String, Optional ByVal settings As IInMemorySettings = Nothing) As IIdentifierGroupSnapshotWriter(Of TAggregate, TAggregateKey)
            Return New InMemoryIdentifierGroupSnapshotWriter(Of TAggregate, TAggregateKey)(groupName, settings)
        End Function

#End Region

    End Class

    Public Module InMemoryIdentifierGroupSnapshotWriterFactory

        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)(ByVal groupName As String,
                                                                                                                ByVal aggregate As TAggregate,
                                                                                                                ByVal key As TAggregateKey,
                                                                                                                Optional ByVal settings As IInMemorySettings = Nothing
                                                                                                                ) As IIdentifierGroupSnapshotWriter(Of TAggregate, TAggregateKey)

            Return InMemoryIdentifierGroupSnapshotWriter(Of TAggregate, TAggregateKey).Create(groupName, settings)

        End Function

    End Module

End Namespace