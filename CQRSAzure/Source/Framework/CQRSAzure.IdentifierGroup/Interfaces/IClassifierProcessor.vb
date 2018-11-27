Imports System
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup

''' <summary>
''' An interface for any processor that can execute classifiers over an event stream
''' </summary>
Public Interface IClassifierProcessor(Of TAggregate As IAggregationIdentifier,
                                          TAggregateKey,
                                          TClassifier As {IClassifier(Of TAggregate, TAggregateKey), New})

    ''' <summary>
    ''' Perform the classification function over the event stream
    ''' </summary>
    ''' <param name="classifierToProcess">
    ''' (Optional) An existing instance of the classifier to use to process the event stream 
    ''' </param>
    ''' <param name="effectiveDateTime">
    ''' The date/time up until which we are running the classification.
    ''' </param>
    ''' <param name="forceExclude">
    ''' If set to true then if no reason is found to classify the aggregate instance as in the group 
    ''' then classify it as excluded from the group
    ''' </param>
    Function Classify(Optional ByVal classifierToProcess As IClassifier(Of TAggregate, TAggregateKey) = Nothing,
                      Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing,
                      Optional ByVal forceExclude As Boolean = False,
                      Optional ByVal projection As IProjection(Of TAggregate, TAggregateKey) = Nothing) As Task(Of IClassifierDataSourceHandler.EvaluationResult)

End Interface


''' <summary>
''' An interface for any processor that can execute classifiers over an untyped event stream
''' </summary>
Public Interface IClassifierProcessorUntyped

    ''' <summary>
    ''' Perform the classification function over the event stream
    ''' </summary>
    ''' <param name="classifierToProcess">
    ''' (Optional) An existing instance of the classifier to use to process the event stream 
    ''' </param>
    ''' <param name="effectiveDateTime">
    ''' The date/time up until which we are running the classification.
    ''' </param>
    ''' <param name="forceExclude">
    ''' If set to true then if no reason is found to classify the aggregate instance as in the group 
    ''' then classify it as excluded from the group
    ''' </param>
    Function Classify(Optional ByVal classifierToProcess As IClassifierUntyped = Nothing,
                      Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing,
                      Optional ByVal forceExclude As Boolean = False,
                      Optional ByVal projection As IProjectionUntyped = Nothing) As Task(Of IClassifierDataSourceHandler.EvaluationResult)

End Interface