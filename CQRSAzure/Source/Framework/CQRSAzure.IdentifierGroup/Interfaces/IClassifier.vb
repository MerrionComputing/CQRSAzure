Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup

''' <summary>
''' An interface for any class that decides if an aggregate is in or our of an identity group
''' </summary>
''' <typeparam name="TAggregate">
''' The type of the aggregate identifier which is to be evaluated
''' </typeparam>
Public Interface IClassifier(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Inherits IClassifier



    ''' <summary>
    ''' Perform whatever evaluation is required to handle the specific event
    ''' </summary>
    ''' <param name="eventToHandle">
    ''' The specific event to handle and perform whatever processing is required in order to 
    ''' evaluate the status of the aggregate instance in relation to the identity group
    ''' </param>
    Function EvaluateEvent(Of TEvent As IEvent(Of TAggregate))(ByVal eventToHandle As TEvent) As IClassifierDataSourceHandler.EvaluationResult

    Function EvaluateProjection(Of TProjection As IProjection(Of TAggregate, TAggregateKey))(ByVal projection As TProjection) As IClassifierDataSourceHandler.EvaluationResult

    ''' <summary>
    ''' Load the starting point of this classifier from the given classifier snapshot
    ''' </summary>
    ''' <typeparam name="TClassifier">
    ''' Th eclassifier type (the type of this interface's implementing class)
    ''' </typeparam>
    ''' <param name="latestSnapshot">
    ''' The classifier snapshot saved away by an earlier evaluation
    ''' </param>
    Sub LoadFromSnapshot(Of TClassifier As IClassifier)(latestSnapshot As IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier))

    ''' <summary>
    ''' Turn the current state of this projection to a snapshot
    ''' </summary>
    Function ToSnapshot(Of TClassifier As IClassifier)() As IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier)

End Interface

Public Interface IClassifier

    ''' <summary>
    ''' How does the classifier get the data it uses to perform a classification
    ''' </summary>
    Enum ClassifierDataSourceType
        ''' <summary>
        ''' Directly processing the event stream with an event handler
        ''' </summary>
        EventHandler = 0
        ''' <summary>
        ''' Running a projection and analysing the result from that
        ''' </summary>
        Projection = 1
    End Enum

    ''' <summary>
    ''' Does this classifier use snapshots to save the latest state or does it need to evaluate the entire 
    ''' event stream every time?
    ''' </summary>
    ''' <returns>
    ''' True if the classifier supports snapshots
    ''' </returns>
    ReadOnly Property SupportsSnapshots As Boolean

    ''' <summary>
    ''' Does the projection handle the data for the given event type
    ''' </summary>
    ''' <param name="eventType">
    ''' The type of the event containing the data that may or may not be handled
    ''' </param>
    ''' <returns>
    ''' True if this event type should get processed
    ''' </returns>
    Function HandlesEventType(ByVal eventType As Type) As Boolean

    ''' <summary>
    ''' How does the classifier get the data it uses to perform a classification
    ''' </summary>
    ReadOnly Property ClassifierDataSource As ClassifierDataSourceType

End Interface



Public Interface IClassifierDataSourceHandler

    ''' <summary>
    ''' The evaluation result of evaluating this given event
    ''' </summary>
    Enum EvaluationResult
        ''' <summary>
        ''' The in/out analysis is not changed by the evaluation of this event
        ''' </summary>
        Unchanged = 0
        ''' <summary>
        ''' The in/out analysis is set to include this item in the group
        ''' </summary>
        Include = 1
        ''' <summary>
        ''' The in/out analysis is set to exclude this item according to this event
        ''' </summary>
        Exclude = 2
    End Enum
End Interface

