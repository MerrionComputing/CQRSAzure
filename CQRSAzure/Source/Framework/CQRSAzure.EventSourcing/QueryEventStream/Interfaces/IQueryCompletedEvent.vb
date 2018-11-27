Imports System

Namespace Queries

    ''' <summary>
    ''' The query processing completed
    ''' </summary>
    Public Interface IQueryCompletedEvent
        Inherits IEvent(Of IQueryAggregateIdentifier)

        ''' <summary>
        ''' The date/time the query processing completed
        ''' </summary>
        ReadOnly Property CompletionDate As Nullable(Of DateTime)

        ''' <summary>
        ''' The human readable description of the completion state
        ''' </summary>
        ReadOnly Property SuccessMessage As String

        ''' <summary>
        ''' The number of results from the query
        ''' </summary>
        ReadOnly Property ResultRecordCount As Integer


    End Interface
End Namespace
