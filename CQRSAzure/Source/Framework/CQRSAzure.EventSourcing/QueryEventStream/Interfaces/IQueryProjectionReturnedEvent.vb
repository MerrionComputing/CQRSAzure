Imports System

Namespace Queries

    ''' <summary>
    ''' A query requested projection was run and the results returned to the query
    ''' </summary>
    ''' <typeparam name="TAggregateIdentifier">
    ''' The data type of the event stream identifier that the projection was run over
    ''' </typeparam>
    Public Interface IQueryProjectionReturnedEvent(Of TAggregateIdentifier)
        Inherits IEvent(Of IQueryAggregateIdentifier)

        ''' <summary>
        ''' The unique name of the projection that was run
        ''' </summary>
        ReadOnly Property ProjectionName As String

        ''' <summary>
        ''' The unique identifier of the event stream over which the projection was run
        ''' </summary>
        ReadOnly Property AggregateUniqueIdentifier As TAggregateIdentifier

        ''' <summary>
        ''' The efective date as of which was the projection run until
        ''' </summary>
        ReadOnly Property AsOfDate As Nullable(Of DateTime)

        ''' <summary>
        ''' Location from which the results can be read 
        ''' </summary>
        ''' <remarks>
        ''' This could be an URL or the raw results themselves 
        ''' </remarks>
        ReadOnly Property ResultsLocation As String

    End Interface
End Namespace
