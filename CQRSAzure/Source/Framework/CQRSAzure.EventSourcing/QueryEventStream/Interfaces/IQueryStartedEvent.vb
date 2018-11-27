Imports System

Namespace Queries
    ''' <summary>
    ''' A query was taken off the queue and processing started on it
    ''' </summary>
    Public Interface IQueryStartedEvent
        Inherits IEvent(Of IQueryAggregateIdentifier)

        ''' <summary>
        ''' The date/time the processing of the query was started
        ''' </summary>
        ReadOnly Property ProcessingStartDate As Nullable(Of DateTime)

        ''' <summary>
        ''' The name of the host processing the query
        ''' </summary>
        ReadOnly Property Processor As String

    End Interface
End Namespace
