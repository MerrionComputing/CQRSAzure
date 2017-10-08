Imports CQRSAzure.EventSourcing

''' <summary>
''' Standard class for implementing a snapshot reader/writer for storing snapshots of a projection as it is run
''' </summary>
''' <typeparam name="TAggregate">
''' The type of aggregate that has the event stream over which the aggregate is run
''' </typeparam>
''' <typeparam name="TAggregateKey">
''' The data type by which an unique instance of that aggregate type is identified
''' </typeparam>
''' <typeparam name="TProjection">
''' The projection for which the snapshot processor works
''' </typeparam>
''' <remarks>
''' If there is no snapshot processor all projections will always run over the entire event stream.  Deciding whether or not to snapshot the projection 
''' is a performance consideration that is best decided by experimentation.
''' </remarks>
Public Class ProjectionSnapshotProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey, TProjection As IProjection)
    Implements ISnapshotProcessor(Of TAggregate, TAggregateKey)

    Private ReadOnly m_key As TAggregateKey
    Private ReadOnly m_reader As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)
    Private ReadOnly m_writer As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)


    Public Sub SaveSnapshot(snapshotToSave As IProjectionSnapshot(Of TAggregate, TAggregateKey)) Implements ISnapshotProcessor(Of TAggregate, TAggregateKey).SaveSnapshot

        If (m_writer IsNot Nothing) Then
#Region "Tracing"
            EventSourcing.LogVerboseInfo("Saving snapshot for aggregate " & m_key.ToString())
#End Region
            m_writer.SaveSnapshot(m_key, snapshotToSave)
        Else
#Region "Tracing"
            EventSourcing.LogError("No writer defined when attempting to save snapshot " & snapshotToSave.ToString())
#End Region
            Throw New ProjectionSnapshotWriteException(AggregateIdentifierAttribute.GetAggregateName(GetType(TAggregate)), m_key.ToString(), GetType(TProjection).Name, "Projection snapshot writer is not set")
        End If

    End Sub

    Public Function GetLatestSnapshot(Optional priorToSequence As UInteger = 0) As IProjectionSnapshot(Of TAggregate, TAggregateKey) Implements ISnapshotProcessor(Of TAggregate, TAggregateKey).GetLatestSnapshot

        If (m_reader IsNot Nothing) Then
#Region "Tracing"
            EventSourcing.LogVerboseInfo("Loading snapshot for aggregate " & m_key.ToString())
#End Region
            Return m_reader.GetSnapshot(m_key)
        Else
#Region "Tracing"
            EventSourcing.LogError("No reader defined when attempting to save snapshot ")
#End Region
            Throw New ProjectionSnapshotReadException(AggregateIdentifierAttribute.GetAggregateName(GetType(TAggregate)), m_key.ToString(), GetType(TProjection).Name, "Projection snapshot reader is not set")
        End If

    End Function

    Public Sub New(ByVal key As TAggregateKey,
                    ByVal reader As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection),
                    ByVal writer As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)
                    )

        m_reader = reader
        m_writer = writer
        m_key = key

    End Sub

End Class


''' <summary>
''' Factory pattern for creating instances of the projection snapshot processor 
''' </summary>
Public Module ProjectionSnapshotProcessorFactory


    Public Function Create(Of TAggregate As IAggregationIdentifier, TAggregateKey, TProjection As IProjection)(ByVal aggregateKey As TAggregateKey,
                                                                                                               ByVal projection As TProjection,
                                                                                                               ByVal reader As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection),
                                                                                                               ByVal writer As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)
                                                                                                               ) As ProjectionSnapshotProcessor(Of TAggregate, TAggregateKey, TProjection)
#Region "Tracing"
        EventSourcing.LogVerboseInfo("Creating snapshot processor for " & GetType(TProjection).Name & " instance " & aggregateKey.ToString())
#End Region

        Return New ProjectionSnapshotProcessor(Of TAggregate, TAggregateKey, TProjection)(aggregateKey, reader, writer)

    End Function

End Module