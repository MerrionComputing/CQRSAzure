Imports System

Namespace Debugging

    ''' <summary>
    ''' A special type of projection processor that emits debug information and allows single-step operations for
    ''' debugging and technology explaining
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The type of the aggregate whose event stream we are to run the projection over
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The type of the key that uniquely identifies an instance of that
    ''' </typeparam>
    ''' <remarks>
    ''' This should not be used in any production system as it will affect performance
    ''' </remarks>
    Public NotInheritable Class DebuggingProjectionProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey)
        Implements IProjectionProcessor(Of TAggregate, TAggregateKey)

#Region "Processing events"
        Public Event SnapshotLoaded(ByVal sender As Object, ByVal e As EventArgs)
        Public Event EventHandled(ByVal sender As Object, ByVal e As EventArgs)
        Public Event EventIgnored(ByVal sender As Object, ByVal e As EventArgs)
#End Region

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
        Public Async Function Process(ByVal projectionToProcess As IProjection(Of TAggregate, TAggregateKey)) As Task Implements IProjectionProcessor(Of TAggregate, TAggregateKey).Process

            If (m_streamReader IsNot Nothing) Then
                If (projectionToProcess IsNot Nothing) Then
                    'Does it support snapshots?
                    Dim startingSequence As UInteger = 0
                    If (projectionToProcess.SupportsSnapshots) Then
                        'Load the most recent snapshot for it
                        '1. Get the latest snapshot
                        Dim latestSnapshot As IProjectionSnapshot(Of TAggregate, TAggregateKey) = Nothing
                        If (m_snapshotProcessor IsNot Nothing) Then
                            latestSnapshot = m_snapshotProcessor.GetLatestSnapshot()
                        End If
                        If (latestSnapshot IsNot Nothing) Then
                            '2. Update the starting sequence
                            startingSequence = latestSnapshot.Sequence
                        End If
                    End If
                    For Each evt In Await m_streamReader.GetEvents(startingSequence)
                        If (projectionToProcess.HandlesEventType(evt.GetType())) Then
                            projectionToProcess.HandleEvent(evt)
                            'raise an EventHandled event
                        Else
                            'raise an EventIgnored event
                        End If
                    Next
                    If (projectionToProcess.SupportsSnapshots) Then
                        'save the current snapshot

                    End If
                End If
            Else
                'Unable to use this projection as it has no stream reader associated
                Throw New UnmappedAggregateException(GetType(TAggregate), Nothing)
            End If

        End Function

        ''' <summary>
        ''' Run the next event through the projection
        ''' </summary>
        ''' <param name="projectionToProcess">
        ''' The projection we are running
        ''' </param>
        ''' <remarks>
        ''' The single step process is intended for debugging or demonstrations 
        ''' </remarks>
        Public Sub ProcessSingleStep(ByVal projectionToProcess As IProjection(Of TAggregate, TAggregateKey))

            If (m_streamReader IsNot Nothing) Then
                If (projectionToProcess IsNot Nothing) Then
                    'TODO: Get the next valid event 
                End If
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

End Namespace