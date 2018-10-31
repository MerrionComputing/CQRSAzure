Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.InMemory

Namespace InMemory

    ''' <summary>
    ''' Snapshot writer for classifier snapshots stored inmemory
    ''' </summary>
    Public NotInheritable Class InMemoryClassifierSnapshotWriter(Of TAggregate As IAggregationIdentifier,
                 TAggregateKey,
                 TClassifier As IClassifier)
        Inherits InMemoryClassifierSnapshotBase(Of TAggregate, TAggregateKey, TClassifier)
        Implements IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier)


        ''' <summary>
        ''' Save this snapshot to the in-memory collection
        ''' </summary>
        ''' <param name="key">
        ''' Unique identifier of the aggregate the classifier ran over
        ''' </param>
        ''' <param name="snapshotToSave">
        ''' Snapshot to save for the classifier at the given point in time
        ''' </param>
        Public Sub SaveSnapshot(key As TAggregateKey,
                                snapshotToSave As IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier)) Implements IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier).SaveSnapshot

            MyBase.AppendSnapshot(snapshotToSave)

        End Sub

#Region "Constructor"
        ''' <summary>
        ''' Create a new in-memory classifier snapshot writer to get the snapshots for the given aggregate's unique key
        ''' </summary>
        ''' <param name="aggregateIdentityKey">
        ''' The key by which the aggregate is uniquely identified
        ''' </param>
        ''' <param name="settings">
        ''' The settings specific to how event streams/snapshots are stored in memory
        ''' </param>
        Private Sub New(ByVal aggregateIdentityKey As TAggregateKey,
                        Optional ByVal settings As IInMemorySettings = Nothing)
            MyBase.New(aggregateIdentityKey, settings)

        End Sub

        Public Shared Function Create(ByVal aggregateIdentityKey As TAggregateKey,
                        Optional ByVal settings As IInMemorySettings = Nothing) As IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier)

            Return New InMemoryClassifierSnapshotWriter(Of TAggregate,
                TAggregateKey,
                TClassifier)(aggregateIdentityKey, settings)

        End Function
#End Region

    End Class

    Public Module InMemoryClassifierSnapshotWriterFactory

        Public Function Create(Of TAggregate As IAggregationIdentifier,
         TAggregateKey,
         TClassifier As IClassifier)(
                                    ByVal aggregate As TAggregate,
                                    ByVal aggregateIdentityKey As TAggregateKey,
                                    ByVal classifier As TClassifier,
                                    Optional ByVal settings As IInMemorySettings = Nothing
                                    ) As IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier)


            Return InMemoryClassifierSnapshotWriter(Of TAggregate,
                TAggregateKey,
                TClassifier).Create(aggregateIdentityKey, settings)

        End Function

    End Module

End Namespace
