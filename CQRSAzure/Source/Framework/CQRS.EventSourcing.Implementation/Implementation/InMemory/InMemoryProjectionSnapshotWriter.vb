Namespace InMemory

    ''' <summary>
    ''' Class to write projection snapshots (cached values) into an in-memory store
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The type of the base class to which the event stream is attached
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type by which an instance of that aggregation base class is uniquely identified
    ''' </typeparam>
    Public NotInheritable Class InMemoryProjectionSnapshotWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                     TAggregateKey,
                                                                     TProjection As IProjection)
        Inherits InMemoryProjectionSnapshotBase(Of TAggregate, TAggregateKey)
        Implements IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

        Public Sub SaveSnapshot(key As TAggregateKey, snapshotToSave As IProjectionSnapshot(Of TAggregate, TAggregateKey)) Implements IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection).SaveSnapshot

            AppendSnapshot(snapshotToSave)

        End Sub


        Private Sub New(ByVal aggregateType As TAggregate, ByVal aggregateIdentityKey As TAggregateKey, Optional ByVal settings As IInMemorySettings = Nothing)
            MyBase.New(aggregateType, aggregateIdentityKey, settings)

        End Sub

#Region "Factory methods"

        ''' <summary>
        ''' Creates an in-memory event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As IInMemorySettings = Nothing) As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

            Return New InMemoryProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)(instance, instance.GetKey(), settings)

        End Function

#End Region

    End Class

    Public Class InMemoryProjectionSnapshotWriter

        ''' <summary>
        ''' Creates an in-memory event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        Public Shared Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                          TAggregateKey,
                                          TProjection As IProjection)(
                                      ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As IInMemorySettings = Nothing
                                      ) As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

            Return InMemoryProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection).Create(instance, settings)

        End Function

    End Class

End Namespace