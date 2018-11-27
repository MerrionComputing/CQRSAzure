Imports System

Namespace Queries

    ''' <summary>
    ''' The query requested to run the given projection for the given aggregate instance
    ''' </summary>
    ''' <typeparam name="TAggregateIdentifier">
    ''' The data type by which the aggregate over which the projection will run is uniquely identified
    ''' </typeparam>
    Public Interface IQueryProjectionRequestedEvent(Of TAggregateIdentifier)
        Inherits IEvent(Of IQueryAggregateIdentifier)

        ''' <summary>
        ''' The unique name of the projection to run
        ''' </summary>
        ReadOnly Property ProjectionpName As String

        ''' <summary>
        ''' The unique identifier of the event stream over which the projection will be run
        ''' </summary>
        ReadOnly Property AggregateUniqueIdentifier As TAggregateIdentifier

        ''' <summary>
        ''' The efective date as of which we want the projection run until
        ''' </summary>
        ''' <remarks>
        ''' If not set then we want the latest possible view of the projection
        ''' </remarks>
        ReadOnly Property AsOfDate As Nullable(Of DateTime)

    End Interface
End Namespace
