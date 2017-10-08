Namespace IdentityGroups

    ''' <summary>
    ''' A query or command asked to know the membership of a given identity group as at a given 
    ''' point in time
    ''' </summary>
    Public Interface IIdentityGroupMembersRequestedEvent

        ''' <summary>
        ''' The effective date/time to get the group membership for
        ''' </summary>
        ''' <remarks>
        ''' If not set then this will run the underlying projection/classifier to the current point in time
        ''' </remarks>
        ReadOnly Property AsOfDate As Nullable(Of DateTime)

        ''' <summary>
        ''' Where the request for the identity group members came from
        ''' </summary>
        ReadOnly Property RequestSource As String

    End Interface
End Namespace
