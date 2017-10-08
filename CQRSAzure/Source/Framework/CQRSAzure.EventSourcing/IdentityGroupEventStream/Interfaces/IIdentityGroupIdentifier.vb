Namespace IdentityGroups

    Public Interface IIdentityGroupIdentifier
        Inherits IAggregationIdentifier

        ''' <summary>
        ''' The name of the domain (per DDD) containing this identity group
        ''' </summary>
        ReadOnly Property DomainName As String

        ''' <summary>
        ''' The unique name of this identity group (unique within the domain)
        ''' </summary>
        ReadOnly Property IdentityGroupName As String

    End Interface

End Namespace
