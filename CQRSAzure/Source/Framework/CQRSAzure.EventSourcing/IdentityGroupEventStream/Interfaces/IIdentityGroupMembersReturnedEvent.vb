Namespace IdentityGroups

    ''' <summary>
    ''' The set of members for an identity group were identified as at a given point in time
    ''' </summary>
    Public Interface IIdentityGroupMembersReturnedEvent


        ''' <summary>
        ''' The effective date/time we got the group membership for
        ''' </summary>
        ReadOnly Property AsOfDate As Nullable(Of DateTime)

        ''' <summary>
        ''' Where the request for the identity group members came from
        ''' </summary>
        ReadOnly Property RequestSource As String

        ''' <summary>
        ''' Where the group members data are stored
        ''' </summary>
        ''' <remarks>
        ''' This could be an URL or the raw results themselves 
        ''' </remarks>
        ReadOnly Property GroupMembersLocation As String

    End Interface
End Namespace
