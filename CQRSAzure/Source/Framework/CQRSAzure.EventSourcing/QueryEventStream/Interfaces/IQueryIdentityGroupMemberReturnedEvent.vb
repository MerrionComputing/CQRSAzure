Namespace Queries
    ''' <summary>
    ''' A single member of an identity group was returned to a query that requested them
    ''' </summary>
    ''' <typeparam name="TAggregateIdentifier">
    ''' The data type by which members of the aggregate group are uniquely identified
    ''' </typeparam>
    Public Interface IQueryIdentityGroupMemberReturnedEvent(Of TAggregateIdentifier)
        Inherits IEvent(Of IQueryAggregateIdentifier)

        ''' <summary>
        ''' The unique name of the identity group that was requested
        ''' </summary>
        ReadOnly Property IdentityGroupName As String

        ''' <summary>
        ''' The unique identifier of the member of the identity group
        ''' </summary>
        ReadOnly Property MemberUniqueIdentifier As TAggregateIdentifier

        ''' <summary>
        ''' The efective date as of which we got this identity group membership
        ''' </summary>
        ReadOnly Property AsOfDate As Nullable(Of DateTime)

    End Interface
End Namespace