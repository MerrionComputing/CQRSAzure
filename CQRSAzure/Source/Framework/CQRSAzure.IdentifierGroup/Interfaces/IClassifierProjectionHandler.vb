Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup

''' <summary>
''' Interface for any classifier that gets its source data from a projection rather than directly by handling events
''' </summary>
''' <typeparam name="TProjection">
''' The type of the projection the classifier can handle
''' </typeparam>
Public Interface IClassifierProjectionHandler(Of TProjection As IProjection)
    Inherits IClassifierDataSourceHandler

    Function EvaluateProjection(ByVal projection As TProjection) As IClassifierDataSourceHandler.EvaluationResult

End Interface
