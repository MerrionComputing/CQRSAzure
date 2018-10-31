Namespace Queries
    ''' <summary>
    ''' A query requested the members of an identity group (to process the underlying 
    ''' projection against)
    ''' </summary>
    Public Interface IQueryIdentityGroupRequestedEvent
        Inherits IEvent(Of IQueryAggregateIdentifier)

        ''' <summary>
        ''' The unique name of the identity group to get the members of
        ''' </summary>
        ReadOnly Property IdentityGroupName As String

        ''' <summary>
        ''' The efective date as of which we want the identity group members
        ''' </summary>
        ''' <remarks>
        ''' If not set then we want the latest possible view of the identity group members
        ''' </remarks>
        ReadOnly Property AsOfDate As Nullable(Of DateTime)

    End Interface
End Namespace