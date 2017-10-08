Namespace Queries
    ''' <summary>
    ''' A transient fault (unavaliable resource etc.) has occured while processing a 
    ''' query.  It may be possible to resume from this fault - or, depending on the 
    ''' business need it may need to be reissued as a new query.
    ''' </summary>
    Public Interface IQueryTransientFaultOccuredEvent
        Inherits IEvent(Of IQueryAggregateIdentifier)

        ''' <summary>
        ''' The date/time the query was stopped with a transient error
        ''' </summary>
        ReadOnly Property FaultDate As Nullable(Of DateTime)

        ''' <summary>
        ''' The human readable description of the fault
        ''' </summary>
        ReadOnly Property FaultMessage As String

    End Interface
End Namespace
