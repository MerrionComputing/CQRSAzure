Imports System
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Implementation
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
    Public Async Function Classify(Optional ByVal classifierToProcess As IClassifier(Of TAggregate, TAggregateKey) = Nothing,
                             Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing,
                             Optional ByVal forceExclude As Boolean = False,
                             Optional ByVal projection As IProjection(Of TAggregate, TAggregateKey) = Nothing) As Task(Of IClassifierDataSourceHandler.EvaluationResult) Implements IClassifierProcessor(Of TAggregate, TAggregateKey, TClassifier).Classify

        If (classifierToProcess Is Nothing) Then
            If (m_classifier IsNot Nothing) Then
                classifierToProcess = m_classifier
            End If
        End If

#Region "Tracing"
        If (classifierToProcess IsNot Nothing) Then
            IdentifierGroup.LogVerboseInfo("Running classifier for " & classifierToProcess.ToString())
        Else
            IdentifierGroup.LogError("Attempt to run classifier but no classifier passed in")
        End If
#End Region



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
                If (classifierToProcess.ClassifierDataSource = IClassifier.ClassifierDataSourceType.EventHandler) Then
                    For Each evt In Await m_streamReader.GetEvents(startingSequence, effectiveDateTime)
                        If (classifierToProcess.HandlesEventType(evt.GetType())) Then
#Region "Tracing"
                            IdentifierGroup.LogVerboseInfo("Evaluating event " & evt.ToString())
#End Region
                            retVal = classifierToProcess.EvaluateEvent(evt)
                        End If
                    Next
                Else
                    If (projection IsNot Nothing) Then
                        retVal = classifierToProcess.EvaluateProjection(projection)
                    End If
                End If


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


        If (forceExclude) Then
            Return IClassifierDataSourceHandler.EvaluationResult.Exclude
        Else
            'If no classification was performed, leave the result as unchanged..
            Return IClassifierDataSourceHandler.EvaluationResult.Unchanged
        End If

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

''' <summary>
''' Class to run defined classifiers over an event stream to classify an aggregate instance as being 
''' inside or outside of the identity group the classifier pertains to from an untyped event stream reader
''' </summary>
Public NotInheritable Class ClassifierProcessorUntyped
    Implements IClassifierProcessorUntyped


    ''' <summary>
    ''' The stream reader instance that will be used to run the projections
    ''' </summary>
    Private ReadOnly m_streamReader As IEventStreamReaderUntyped
    Private ReadOnly m_snapshotReader As IClassifierSnapshotReaderUntyped
    Private ReadOnly m_snapshotWriter As IClassifierSnapshotWriterUntyped
    Private ReadOnly m_classifier As IClassifierUntyped


    Public Async Function Classify(Optional classifierToProcess As IClassifierUntyped = Nothing,
                             Optional effectiveDateTime As Date? = Nothing,
                             Optional forceExclude As Boolean = False,
                             Optional ByVal projection As IProjectionUntyped = Nothing) As Task(Of IClassifierDataSourceHandler.EvaluationResult) Implements IClassifierProcessorUntyped.Classify


        If (classifierToProcess Is Nothing) Then
            If (m_classifier IsNot Nothing) Then
                classifierToProcess = m_classifier
            End If
        End If

        If (classifierToProcess Is Nothing) Then
            If (forceExclude) Then
                Return IClassifierDataSourceHandler.EvaluationResult.Exclude
            Else
                Return IClassifierDataSourceHandler.EvaluationResult.Unchanged
            End If
        Else
            If m_streamReader IsNot Nothing Then
                If (classifierToProcess IsNot Nothing) Then
                    'Does it support snapshots?
                    Dim startingSequence As UInteger = 0
                    If (classifierToProcess.SupportsSnapshots) Then
                        If (m_snapshotReader IsNot Nothing) Then
                            'load the most recent snapshot for it
                            Dim latestSnapshot As IClassifierSnapshotUntyped = Nothing
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
                    If (classifierToProcess.ClassifierDataSource = IClassifier.ClassifierDataSourceType.EventHandler) Then

                        For Each evt In Await m_streamReader.GetEvents(startingSequence, effectiveDateTime)

                            Dim jsonEvent As IJsonSerialisedEvent = evt
                            If (jsonEvent IsNot Nothing) Then
                                If (classifierToProcess.HandlesEventType(jsonEvent.FullClassName)) Then
#Region "Tracing"
                                    IdentifierGroup.LogVerboseInfo("Evaluating event " & jsonEvent.FullClassName)
#End Region
                                    retVal = classifierToProcess.EvaluateEvent(jsonEvent.FullClassName, jsonEvent.EventInstanceAsJson)
                                End If
                            Else
                                If (classifierToProcess.HandlesEventType(evt.GetType().FullName)) Then
#Region "Tracing"
                                    IdentifierGroup.LogVerboseInfo("Evaluating event " & evt.ToString())
#End Region
                                    retVal = classifierToProcess.EvaluateEvent(evt.GetType().FullName, evt)
                                End If
                            End If


                        Next
                    Else
                        'run the projection 
                        If (projection IsNot Nothing) Then
                            If (m_streamReader IsNot Nothing) Then
                                ' Make a ProjectionProcessorUntyped for it and process it
                                Await ProjectionProcessorUntyped.Create(m_streamReader).Process(projection)
                            End If
                            retVal = classifierToProcess.EvaluateProjection(projection)
                        End If
                    End If

                    If (classifierToProcess.SupportsSnapshots) Then
                        'save the snapshot
                        If (m_snapshotWriter IsNot Nothing) Then
#Region "Tracing"
                            IdentifierGroup.LogVerboseInfo("Saving  classifier state to snapshot")
#End Region
                            m_snapshotWriter.SaveSnapshot(m_streamReader.Key, classifierToProcess.ToSnapshot())
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

            Return IClassifierDataSourceHandler.EvaluationResult.Unchanged

        End If
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
    Friend Sub New(ByVal readerTouse As IEventStreamReaderUntyped,
                   Optional classifier As IClassifierUntyped = Nothing,
                   Optional snapshotReader As IClassifierSnapshotReaderUntyped = Nothing,
                   Optional snapshotWriter As IClassifierSnapshotWriterUntyped = Nothing)
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