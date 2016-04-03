Imports CQRSAzure.EventSourcing


''' <summary>
''' Class to run defined classifiers over an event stream to classify an aggregate instance as being 
''' inside or outside of the identity group the classifier pertains to
''' </summary>
''' <typeparam name="TAggregate">
''' The class of the aggregate of which this is an instance 
''' </typeparam>
''' <typeparam name="TAggregateKey">
''' The data type of the key that uniquely identifies an instance of this aggregate
''' </typeparam>
Public NotInheritable Class ClassifierProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey)

    ''' <summary>
    ''' The stream reader instance that will be used to run the projections
    ''' </summary>
    Private ReadOnly m_streamReader As IEventStreamReader(Of TAggregate, TAggregateKey)


    Public Function Classify(ByVal classifierToProcess As IClassifier(Of TAggregate, TAggregateKey)) As IClassifierEventHandler.EvaluationResult

        If m_streamReader IsNot Nothing Then
            If (classifierToProcess IsNot Nothing) Then
                'Does it support snapshots?
                Dim startingSequence As UInteger = 0
                If (classifierToProcess.SupportsSnapshots) Then
                    'load the most recent snapshot for it

                    'and update the starting sequence
                End If
                Dim retVal As IClassifierEventHandler.EvaluationResult = IClassifierEventHandler.EvaluationResult.Unchanged
                For Each evt In m_streamReader.GetEvents(startingSequence)
                    If (classifierToProcess.HandlesEventType(evt.GetType())) Then
                        retVal = classifierToProcess.Evaluate(evt)
                    End If
                Next
                ' Return the evaluation status as at the end of the event stream
                Return retVal
            End If
        End If

        'If no classification was performed, leave the result as unchanged..
        Return IClassifierEventHandler.EvaluationResult.Unchanged
    End Function

    ''' <summary>
    ''' Create a new classifer processor that will use the given event stream reader to do its processing
    ''' </summary>
    ''' <param name="readerTouse">
    ''' The event stream processor to use
    ''' </param>
    Friend Sub New(ByVal readerTouse As IEventStreamReader(Of TAggregate, TAggregateKey))
        m_streamReader = readerTouse
    End Sub
End Class
