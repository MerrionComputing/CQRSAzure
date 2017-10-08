Public NotInheritable Class ProjectionProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Implements IProjectionProcessor(Of TAggregate, TAggregateKey)

    ''' <summary>
    ''' The stream reader instance that will be used to run the projections
    ''' </summary>
    Private ReadOnly m_streamReader As IEventStreamReader(Of TAggregate, TAggregateKey)
    Private ReadOnly m_snapshotProcessor As ISnapshotProcessor(Of TAggregate, TAggregateKey)

    ''' <summary>
    ''' Process the given projection using the event stream reader we have set up
    ''' </summary>
    ''' <param name="projectionToProcess">
    ''' The class that defines the projection operation we are going to process
    ''' </param>
    Public Sub Process(ByVal projectionToProcess As IProjection(Of TAggregate, TAggregateKey)) Implements IProjectionProcessor(Of TAggregate, TAggregateKey).Process

        If (m_streamReader IsNot Nothing) Then
            If (projectionToProcess IsNot Nothing) Then
                'Does it support snapshots?
                Dim startingSequence As UInteger = projectionToProcess.CurrentSequenceNumber
                If (projectionToProcess.SupportsSnapshots) Then
                    'Load the most recent snapshot for it
                    Dim latestSnapshot As IProjectionSnapshot(Of TAggregate, TAggregateKey) = Nothing
                    If (m_snapshotProcessor IsNot Nothing) Then
                        latestSnapshot = m_snapshotProcessor.GetLatestSnapshot()
                    End If
                    If (latestSnapshot IsNot Nothing) Then
                        '1. Set the current state of the projection from the snapshot
                        projectionToProcess.LoadFromSnapshot(latestSnapshot)
                        '2. Update the starting sequence
                        startingSequence = latestSnapshot.Sequence
                    Else
                        'Unable to load snapshots so start from the last record read of the event stream
                    End If
                End If
                For Each evt In m_streamReader.GetEventsWithContext(startingSequence)
                    projectionToProcess.OnEventRead(evt.SequenceNumber, EventAsOfDateAttribute.GetAsOfDate(evt.EventInstance))
                    If (projectionToProcess.HandlesEventType(evt.EventInstance.GetType())) Then
                        projectionToProcess.HandleEvent(evt.EventInstance)
                        projectionToProcess.MarkEventHandled(evt.SequenceNumber)
                    End If
                Next
                If (projectionToProcess.SupportsSnapshots) Then
                    'save the current snapshot
                    If (m_snapshotProcessor IsNot Nothing) Then
                        'make a snapshot record
                        Dim postProcessingSnapshot As IProjectionSnapshot(Of TAggregate, TAggregateKey) = Nothing
                        postProcessingSnapshot = projectionToProcess.ToSnapshot()
                        If (postProcessingSnapshot IsNot Nothing) Then
                            'and then save it
                            m_snapshotProcessor.SaveSnapshot(postProcessingSnapshot)
                        End If
                    End If
                End If
            End If
        Else
            'Unable to use this projection as it has no stream reader associated
            Throw New UnmappedAggregateException(GetType(TAggregate), Nothing)
        End If

    End Sub

    ''' <summary>
    ''' Create a new projection processor that will use the given event stream reader to do its processing
    ''' </summary>
    ''' <param name="readerTouse">
    ''' The event stream processor to use
    ''' </param>
    Friend Sub New(ByVal readerTouse As IEventStreamReader(Of TAggregate, TAggregateKey),
                   Optional ByVal snapshotProcessor As ISnapshotProcessor(Of TAggregate, TAggregateKey) = Nothing)
        m_streamReader = readerTouse
        If (snapshotProcessor IsNot Nothing) Then
            m_snapshotProcessor = snapshotProcessor
        End If
    End Sub

End Class
