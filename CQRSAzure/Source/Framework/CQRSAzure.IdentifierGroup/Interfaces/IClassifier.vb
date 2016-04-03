Imports CQRSAzure.EventSourcing

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
    Function Evaluate(Of TEvent As IEvent(Of TAggregate))(ByVal eventToHandle As TEvent) As IClassifierEventHandler.EvaluationResult

    ''' <summary>
    ''' Load the snapshot of this projection as the starting point for populating this classification
    ''' </summary>
    ''' <param name="snapshotToLoad">
    ''' The stored snapshot for the given classifier
    ''' </param>
    Sub LoadSnapshot(ByVal snapshotToLoad As IClassifierSnapshot(Of TAggregate, TAggregateKey))

End Interface

Public Interface IClassifier

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

End Interface

''' <summary>
''' An interface for a class that handles a specific event to decide if an aggregate is in or out of an identity group
''' </summary>
''' <typeparam name="TEvent">
''' The specific event being evaluated
''' </typeparam>
Public Interface IClassifierEventHandler(Of TEvent As IEvent)
    Inherits IClassifierEventHandler



    Function Evaluate(ByVal eventToEvaluate As TEvent) As EvaluationResult

End Interface

Public Interface IClassifierEventHandler

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

