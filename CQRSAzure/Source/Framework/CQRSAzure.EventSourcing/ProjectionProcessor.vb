Public NotInheritable Class ProjectionProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey)

    ''' <summary>
    ''' The stream reader instance that will be used to run the projections
    ''' </summary>
    Private ReadOnly m_streamReader As IEventStreamReader(Of TAggregate, TAggregateKey)


    ''' <summary>
    ''' Process the given projection using the event stream reader we have set up
    ''' </summary>
    ''' <param name="projectionToProcess">
    ''' The class that defines the projection operation we are going to process
    ''' </param>
    Public Sub Process(ByVal projectionToProcess As IProjection(Of TAggregate, TAggregateKey))

        If (m_streamReader IsNot Nothing) Then
            If (projectionToProcess IsNot Nothing) Then
                'Does it support snapshots?
                Dim startingSequence As UInteger = 0
                If (projectionToProcess.SupportsSnapshots) Then
                    'load the most recent snapshot for it

                    'and update the starting sequence
                End If
                For Each evt In m_streamReader.GetEvents(startingSequence)
                    If (projectionToProcess.HandlesEventType(evt.GetType())) Then
                        projectionToProcess.HandleEvent(evt)
                    End If
                Next
            End If
        Else
            'Unable to use this projection as it has no stream reader associated
        End If

    End Sub

    ''' <summary>
    ''' Create a new projection processor that will use the given event stream reader to do its processing
    ''' </summary>
    ''' <param name="readerTouse">
    ''' The event stream processor to use
    ''' </param>
    Friend Sub New(ByVal readerTouse As IEventStreamReader(Of TAggregate, TAggregateKey))
        m_streamReader = readerTouse
    End Sub

End Class
