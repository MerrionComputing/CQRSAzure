Imports CQRSAzure.EventSourcing
''' <summary>
''' An interface for a class that handles a specific event to decide if an aggregate is in or out of an identity group
''' </summary>
''' <typeparam name="TEvent">
''' The specific event being evaluated
''' </typeparam>
Public Interface IClassifierEventHandler(Of TEvent As IEvent)
    Inherits IClassifierDataSourceHandler

    Function EvaluateEvent(ByVal eventToEvaluate As TEvent) As EvaluationResult

End Interface