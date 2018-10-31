Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.InMemory

Namespace InMemory

    ''' <summary>
    ''' Snapshot reader for classifier snapshots stored inmemory
    ''' </summary>
    Public NotInheritable Class InMemoryClassifierSnapshotReader(Of TAggregate As IAggregationIdentifier,
                 TAggregateKey,
                 TClassifier As IClassifier)
        Inherits InMemoryClassifierSnapshotBase(Of TAggregate, TAggregateKey, TClassifier)
        Implements IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier)


        Public Overloads Function GetSnapshot(key As TAggregateKey,
                                    Optional OnOrBeforeTimestamp As Date? = Nothing) As IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier) Implements IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier).GetSnapshot

            Return MyBase.GetSnapshot(OnOrBeforeTimestamp)

        End Function

#Region "Constructor"
        ''' <summary>
        ''' Create a new in-memory classifier snapshot reader to get the snapshots for the given aggregate's unique key
        ''' </summary>
        ''' <param name="aggregateIdentityKey">
        ''' The key by which the aggregate is usniquely identified
        ''' </param>
        ''' <param name="settings">
        ''' The settings specific to how event streams/snapshots are stored in memory
        ''' </param>
        Private Sub New(ByVal aggregateIdentityKey As TAggregateKey,
                        Optional ByVal settings As IInMemorySettings = Nothing)
            MyBase.New(aggregateIdentityKey, settings)

        End Sub

        Public Shared Function Create(ByVal aggregateIdentityKey As TAggregateKey,
                        Optional ByVal settings As IInMemorySettings = Nothing) As IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier)

            Return New InMemoryClassifierSnapshotReader(Of TAggregate,
                TAggregateKey,
                TClassifier)(aggregateIdentityKey, settings)

        End Function
#End Region



    End Class

    Public Module InMemoryClassifierSnapshotReaderFactory

        Public Function Create(Of TAggregate As IAggregationIdentifier,
                 TAggregateKey,
                 TClassifier As IClassifier)(
                                            ByVal aggregate As TAggregate,
                                            ByVal aggregateIdentityKey As TAggregateKey,
                                            ByVal classifier As TClassifier,
                                            Optional ByVal settings As IInMemorySettings = Nothing
                                            ) As IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier)


            Return InMemoryClassifierSnapshotReader(Of TAggregate,
                TAggregateKey,
                TClassifier).Create(aggregateIdentityKey, settings)

        End Function

    End Module

End Namespace