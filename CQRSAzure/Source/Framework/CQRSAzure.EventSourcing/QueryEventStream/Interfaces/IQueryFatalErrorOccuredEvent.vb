Namespace Queries

    ''' <summary>
    ''' A query was terminated with an unrecoverable error 
    ''' </summary>
    Public Interface IQueryFatalErrorOccuredEvent
        Inherits IEvent(Of IQueryAggregateIdentifier)

        ''' <summary>
        ''' The date/time the query was stopped with a fatal error
        ''' </summary>
        ReadOnly Property ErrorDate As Nullable(Of DateTime)


        ''' <summary>
        ''' The human readable description of the error
        ''' </summary>
        ReadOnly Property ErrorMessage As String

    End Interface
End Namespace
