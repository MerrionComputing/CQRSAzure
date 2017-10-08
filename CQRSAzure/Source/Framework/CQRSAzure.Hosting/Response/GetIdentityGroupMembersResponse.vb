Option Strict On
Option Explicit On

Imports CQRSAzure.Hosting


Namespace Response
    ''' <summary>
    ''' This host executed the "get identity group" and returned the resulting list
    ''' </summary>
    Public NotInheritable Class GetIdentityGroupMembersResponse
        Inherits HostResponseBase

        Private ReadOnly m_identityGroupMembers As IEnumerable
        Public ReadOnly Property IdentityGroupMembers As IEnumerable
            Get
                Return m_identityGroupMembers
            End Get
        End Property

        Private Sub New(responderIn As IHost,
                       requesterIn As IHost,
                       uniqueIdentifierIn As Guid,
                       requestIdentifierIn As Guid,
                       identityGroupMembersIn As IEnumerable)

            MyBase.New(responderIn, requesterIn, uniqueIdentifierIn, ResponseCategories.Acknowledgement, requestIdentifierIn)

            m_identityGroupMembers = identityGroupMembersIn

        End Sub
    End Class
End Namespace