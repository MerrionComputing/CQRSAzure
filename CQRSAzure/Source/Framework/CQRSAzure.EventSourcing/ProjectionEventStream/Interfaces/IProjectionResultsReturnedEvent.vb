Namespace Projections

    ''' <summary>
    ''' The results for a projection were returned to a query or identity group classifier that asked for them
    ''' </summary>
    Public Interface IProjectionResultsReturnedEvent

        ''' <summary>
        ''' The sequence number as of which the returned result is applicable
        ''' </summary>
        ReadOnly Property AsOfSequence As Integer


        ''' <summary>
        ''' The effective date of the last event of the result projection
        ''' </summary>
        ReadOnly Property AsOfDate As Nullable(Of DateTime)

        ''' <summary>
        ''' Where the returned data are stored
        ''' </summary>
        ''' <remarks>
        ''' This could be an URL or the raw results themselves 
        ''' </remarks>
        ReadOnly Property ProjectionLocation As String

    End Interface
End Namespace
