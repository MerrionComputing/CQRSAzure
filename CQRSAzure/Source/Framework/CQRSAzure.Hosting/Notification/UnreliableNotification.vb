Option Strict On
Option Explicit On

Imports CQRSAzure.Hosting

Namespace Notification
    ''' <summary>
    ''' A host is to be considered unreliable 
    ''' </summary>
    Public NotInheritable Class UnreliableNotification
        Inherits HostNotificationBase




        Private Sub New(originatorIn As IHost, senderIn As IHost, targetIn As IHost, uniqueIdentifierIn As Guid)
            MyBase.New(originatorIn, senderIn, targetIn, uniqueIdentifierIn, NotificationCategories.Unreliable)
        End Sub

    End Class
End Namespace