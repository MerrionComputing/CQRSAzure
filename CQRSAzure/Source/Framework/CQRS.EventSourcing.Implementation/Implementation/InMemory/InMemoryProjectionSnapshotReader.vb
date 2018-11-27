Imports CQRSAzure.EventSourcing.InMemory

Namespace InMemory

    ''' <summary>
    ''' Class to read projection snapshots (cached values) from an in-memory store
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The type of the base class to which the event stream is attached
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type by which an instance of that aggregation base class is uniquely identified
    ''' </typeparam>
    Public NotInheritable Class InMemoryProjectionSnapshotReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                     TAggregateKey,
                                                                     TProjection As IProjection)
        Inherits InMemoryProjectionSnapshotBase(Of TAggregate, TAggregateKey)
        Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)



        Public Overloads Function GetSnapshot(key As TAggregateKey, Optional OnOrBeforeSequence As UInteger = 0) As Task(Of IProjectionSnapshot(Of TAggregate, TAggregateKey)) Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).GetSnapshot

            Return MyBase.GetSnapshot(OnOrBeforeSequence)

        End Function

        Public Function GetLatestSnapshotSequence(key As TAggregateKey, Optional OnOrBeforeSequence As UInteger = 0) As Task(Of UInteger) Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).GetLatestSnapshotSequence

            'if we didn't return anything then there is no latest snapshot
            Return Task.Factory.StartNew(Of UInteger)(
                Function()
                    Return 0
                End Function
                )

        End Function

        Private Sub New(ByVal aggregateType As TAggregate,
                        ByVal aggregateIdentityKey As TAggregateKey,
                        Optional ByVal settings As IInMemorySettings = Nothing)
            MyBase.New(aggregateType, aggregateIdentityKey, settings)

        End Sub

#Region "Factory methods"

        ''' <summary>
        ''' Creates a projection reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As IInMemorySettings = Nothing) As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

            Return New InMemoryProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)(instance, instance.GetKey(), settings)

        End Function

#End Region

    End Class

    Public NotInheritable Class InMemoryProjectionSnapshotReader

#Region "Factory methods"

        ''' <summary>
        ''' Creates a projection reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        Public Shared Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                          TAggregateKey,
                                          TProjection As IProjection)(
                                      ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As IInMemorySettings = Nothing
                                      ) As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

            Return InMemoryProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).Create(instance, settings)

        End Function

#End Region

    End Class

End Namespace