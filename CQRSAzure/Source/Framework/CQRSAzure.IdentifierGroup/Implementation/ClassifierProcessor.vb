Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup

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
Public NotInheritable Class ClassifierProcessor(Of TAggregate As IAggregationIdentifier,
                                                    TAggregateKey,
                                                    TClassifier As {IClassifier(Of TAggregate, TAggregateKey), New})
    Implements IClassifierProcessor(Of TAggregate, TAggregateKey, TClassifier)

    ''' <summary>
    ''' The stream reader instance that will be used to run the projections
    ''' </summary>
    Private ReadOnly m_streamReader As IEventStreamReader(Of TAggregate, TAggregateKey)
    Private ReadOnly m_snapshotReader As IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier)
    Private ReadOnly m_snapshotWriter As IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier)
    Private ReadOnly m_classifier As IClassifier(Of TAggregate, TAggregateKey)

    ''' <summary>
    ''' Run this classifier over the event stream using the reader set up
    ''' </summary>
    ''' <param name="classifierToProcess">
    ''' The classifier function to run over the event stream
    ''' </param>
    ''' <param name="forceExclude">
    ''' If true then if we don't find a reason to include then make this exclude
    ''' </param>
    ''' <returns></returns>
    Public Function Classify(Optional ByVal classifierToProcess As IClassifier(Of TAggregate, TAggregateKey) = Nothing,
                             Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing,
                             Optional ByVal forceExclude As Boolean = False) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifierProcessor(Of TAggregate, TAggregateKey, TClassifier).Classify

#Region "Tracing"
        If (classifierToProcess IsNot Nothing) Then
            IdentifierGroup.LogVerboseInfo("Running classifier for " & classifierToProcess.ToString())
        Else
            IdentifierGroup.LogError("Attempt to run classifier but no classifier passed in")
        End If
#End Region

        If (classifierToProcess Is Nothing) Then
            If (m_classifier IsNot Nothing) Then
                classifierToProcess = m_classifier
            End If
        End If

        If m_streamReader IsNot Nothing Then
            If (classifierToProcess IsNot Nothing) Then
                'Does it support snapshots?
                Dim startingSequence As UInteger = 0
                If (classifierToProcess.SupportsSnapshots) Then
                    If (m_snapshotReader IsNot Nothing) Then
                        'load the most recent snapshot for it
                        Dim latestSnapshot As IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier) = Nothing
                        latestSnapshot = m_snapshotReader.GetSnapshot(m_streamReader.Key, effectiveDateTime)
                        'and update the starting sequence
                        If (latestSnapshot IsNot Nothing) Then
                            '1. Set the current state of the projection from the snapshot
                            classifierToProcess.LoadFromSnapshot(latestSnapshot)
                            '2. Update the starting sequence
                            startingSequence = latestSnapshot.EffectiveSequenceNumber
#Region "Tracing"
                            IdentifierGroup.LogVerboseInfo("Loaded classifier state from snapshot at " & latestSnapshot.EffectiveSequenceNumber.ToString())
#End Region
                        End If
                    End If
                End If

                Dim retVal As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged
                For Each evt In m_streamReader.GetEvents(startingSequence, effectiveDateTime)
                    If (classifierToProcess.HandlesEventType(evt.GetType())) Then
#Region "Tracing"
                        IdentifierGroup.LogVerboseInfo("Evaluating event " & evt.ToString())
#End Region
                        retVal = classifierToProcess.EvaluateEvent(evt)
                    End If
                Next


                If (classifierToProcess.SupportsSnapshots) Then
                    'save the snapshot
                    If (m_snapshotWriter IsNot Nothing) Then
#Region "Tracing"
                        IdentifierGroup.LogVerboseInfo("Saving  classifier state to snapshot")
#End Region
                        m_snapshotWriter.SaveSnapshot(m_streamReader.Key, classifierToProcess.ToSnapshot(Of TClassifier)())
                    End If
                End If

                If forceExclude Then
                    If retVal = IClassifierDataSourceHandler.EvaluationResult.Unchanged Then
                        retVal = IClassifierDataSourceHandler.EvaluationResult.Exclude
                    End If
                End If

                ' Return the evaluation status as at the end of the event stream
                Return retVal
            End If
        Else
#Region "Tracing"
            IdentifierGroup.LogError("Attempt to run classifier but no stream reader passed in")
#End Region
        End If

        'If no classification was performed, leave the result as unchanged..
        Return IClassifierDataSourceHandler.EvaluationResult.Unchanged
    End Function

    ''' <summary>
    ''' Create a new classifer processor that will use the given event stream reader to do its processing
    ''' </summary>
    ''' <param name="readerTouse">
    ''' The event stream processor to use
    ''' </param>
    ''' <param name="classifier">
    ''' (Optional) The classifier class that does the actual evaluation
    ''' </param>
    ''' <param name="snapshotReader">
    ''' (Optional) A class to read snapshots for this classifier running over the instance event stream
    ''' </param>
    ''' <param name="snapshotWriter">
    ''' (Optional) A class to write snapshots for this classifier running over the instance event stream
    ''' </param>
    Friend Sub New(ByVal readerTouse As IEventStreamReader(Of TAggregate, TAggregateKey),
                   Optional classifier As IClassifier(Of TAggregate, TAggregateKey) = Nothing,
                   Optional snapshotReader As IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier) = Nothing,
                   Optional snapshotWriter As IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier) = Nothing)
        m_streamReader = readerTouse
        If (classifier IsNot Nothing) Then
            m_classifier = classifier
        End If
        If (snapshotReader IsNot Nothing) Then
            m_snapshotReader = snapshotReader
        End If
        If (snapshotWriter IsNot Nothing) Then
            m_snapshotWriter = snapshotWriter
        End If
    End Sub

End Class
