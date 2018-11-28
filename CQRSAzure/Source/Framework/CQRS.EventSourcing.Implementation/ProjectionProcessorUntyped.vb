
Imports CQRSAzure.EventSourcing

Public NotInheritable Class ProjectionProcessorUntyped
    Implements IProjectionProcessorUntyped

    ''' <summary>
    ''' The stream reader instance that will be used to run the projections
    ''' </summary>
    Private ReadOnly m_streamReader As IEventStreamReaderUntyped
    Private ReadOnly m_snapshotProcessor As ISnapshotProcessorUntyped

    Public Async Function Process(projectionToProcess As IProjectionUntyped) As Task Implements IProjectionProcessorUntyped.Process

        If (m_streamReader IsNot Nothing) Then
            If (projectionToProcess IsNot Nothing) Then
                'Does it support snapshots?
                Dim startingSequence As UInteger = projectionToProcess.CurrentSequenceNumber
                If (projectionToProcess.SupportsSnapshots) Then
                    'Load the most recent snapshot for it
                    Dim latestSnapshot As IProjectionSnapshot = Nothing
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
                For Each evt In Await m_streamReader.GetEventsWithContext(startingSequence)
                    projectionToProcess.OnEventRead(evt.SequenceNumber, EventAsOfDateAttribute.GetAsOfDate(evt.EventInstance))

                    'is it a JSON wrapped event
                    Dim jsonEvent As IJsonSerialisedEvent = TryCast(evt.EventInstance, IJsonSerialisedEvent)
                    If (jsonEvent Is Nothing) Then
                        If (projectionToProcess.HandlesEventType(evt.EventInstance.GetType)) Then
                            projectionToProcess.HandleEvent(evt.EventInstance)
                        End If
                    Else
                        If (projectionToProcess.HandlesEventTypeByName(jsonEvent.FullClassName)) Then
                            projectionToProcess.HandleEventJSon(jsonEvent.FullClassName, jsonEvent.EventInstanceAsJson)
                        End If
                    End If
                    projectionToProcess.MarkEventHandled(evt.SequenceNumber)

                Next
                If (projectionToProcess.SupportsSnapshots) Then
                    'save the current snapshot
                    If (m_snapshotProcessor IsNot Nothing) Then
                        'make a snapshot record
                        Dim postProcessingSnapshot As IProjectionSnapshot = Nothing
                        postProcessingSnapshot = projectionToProcess.ToSnapshot()
                        If (postProcessingSnapshot IsNot Nothing) Then
                            'and then save it
                            m_snapshotProcessor.SaveSnapshot(postProcessingSnapshot)
                        End If
                    End If
                End If
            End If
        End If

    End Function

    ''' <summary>
    ''' Create a new projection processor that will use the given event stream reader to do its processing
    ''' </summary>
    ''' <param name="readerTouse">
    ''' The event stream processor to use
    ''' </param>
    Friend Sub New(ByVal readerTouse As IEventStreamReaderUntyped,
                   Optional ByVal snapshotProcessor As ISnapshotProcessorUntyped = Nothing)
        m_streamReader = readerTouse
        If (snapshotProcessor IsNot Nothing) Then
            m_snapshotProcessor = snapshotProcessor
        End If
    End Sub

    Public Shared Function Create(ByVal readerTouse As IEventStreamReaderUntyped,
                   Optional ByVal snapshotProcessor As ISnapshotProcessorUntyped = Nothing) As ProjectionProcessorUntyped

        Return New ProjectionProcessorUntyped(readerTouse, snapshotProcessor)

    End Function

End Class
