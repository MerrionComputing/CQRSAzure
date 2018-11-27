Imports System

Namespace IdentityGroups
    Public Interface IIdentityGroupMemberIncludedEvent(Of TAggregateIdentifier As IAggregationIdentifier)

        ''' <summary>
        ''' The effective date of the last event of the classification that counted this member
        ''' as being in the group
        ''' </summary>
        ReadOnly Property AsOfDate As Nullable(Of DateTime)

        ''' <summary>
        ''' The unique identifier of the member included in the group
        ''' </summary>
        ReadOnly Property MemberUniqueIdentifier As TAggregateIdentifier

    End Interface
End Namespace