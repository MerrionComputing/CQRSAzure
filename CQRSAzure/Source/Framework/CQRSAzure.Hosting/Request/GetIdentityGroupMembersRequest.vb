Option Strict On
Option Explicit On

Imports System
Imports CQRSAzure.Hosting
Imports CQRSAzure.IdentifierGroup

Namespace Request
    ''' <summary>
    ''' Request to get all the members of an aggregate identity group
    ''' </summary>
    Public NotInheritable Class GetIdentityGroupMembersRequest
        Inherits HostRequestBase

        'TODO: Add an identifier for the identity group we are getting the members for

        Private Sub New(originatorIn As IHost,
                       senderIn As IHost,
                       targetIn As IHost,
                       uniqueIdentifierIn As Guid)

            MyBase.New(originatorIn, senderIn, targetIn, uniqueIdentifierIn, RequestCategories.GetIdentityGroupMembers)
        End Sub

    End Class
End Namespace