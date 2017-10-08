
Namespace Projections
    ''' <summary>
    ''' A  specific projection was requested (by a query or identity group classifier for example)
    ''' </summary>
    Public Interface IProjectionRequestedEvent


        ''' <summary>
        ''' Where did the source of this projection request come from
        ''' </summary>
        ''' <remarks>
        ''' This can be used to decide where to post the result
        ''' </remarks>
        ReadOnly Property RequestSource As String

        ''' <summary>
        ''' The effective date to run the projection up until
        ''' </summary>
        ''' <returns>
        ''' If not set this will run to the current head of the aggregate event stream
        ''' as identified by the unique identifier
        ''' </returns>
        ReadOnly Property AsOfDate As Nullable(Of DateTime)

    End Interface
End Namespace