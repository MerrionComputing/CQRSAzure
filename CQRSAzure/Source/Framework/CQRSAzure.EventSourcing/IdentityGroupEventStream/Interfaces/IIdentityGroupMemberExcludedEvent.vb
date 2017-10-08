Namespace IdentityGroups
    Public Interface IIdentityGroupMemberExcludedEvent(Of TAggregateIdentifier As IAggregationIdentifier)

        ''' <summary>
        ''' The effective date of the last event of the classification that counted this member
        ''' as being out of the group
        ''' </summary>
        ReadOnly Property AsOfDate As Nullable(Of DateTime)

        ''' <summary>
        ''' The unique identifier of the member excluded from the group
        ''' </summary>
        ReadOnly Property MemberUniqueIdentifier As TAggregateIdentifier

    End Interface
End Namespace
