
Namespace Queries
    ''' <summary>
    ''' The members of an identity group were returned to a query that requested them
    ''' </summary>
    Public Interface IQueryIdentityGroupReturnedEvent
        Inherits IEvent(Of IQueryAggregateIdentifier)

        ''' <summary>
        ''' The unique name of the identity group that was requested
        ''' </summary>
        ReadOnly Property IdentityGroupName As String


        ''' <summary>
        ''' The efective date as of which we got the identity group members
        ''' </summary>
        ''' <remarks>
        ''' This can be used to determine if the group membership is too aged to use
        ''' </remarks>
        ReadOnly Property AsOfDate As Nullable(Of DateTime)

    End Interface
End Namespace