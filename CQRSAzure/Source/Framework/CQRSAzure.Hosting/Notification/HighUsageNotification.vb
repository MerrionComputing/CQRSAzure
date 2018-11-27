Imports System

Namespace Notification
    ''' <summary>
    ''' Notify that this host node is currently under high usage and should be left alone for a while
    ''' </summary>
    Public NotInheritable Class HighUsageNotification
        Inherits HostNotificationBase


        Private Sub New(originatorIn As IHost, senderIn As IHost, targetIn As IHost, uniqueIdentifierIn As Guid)
            MyBase.New(originatorIn, senderIn, targetIn, uniqueIdentifierIn, NotificationCategories.HighUsage)
        End Sub

    End Class
End Namespace