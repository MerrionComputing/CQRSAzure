Option Strict On
Option Explicit On

Imports CQRSAzure.Hosting

Namespace Notification
    ''' <summary>
    ''' A notification sent from one host to another
    ''' </summary>
    ''' <remarks>
    ''' Because each specific type of host notification can have different payload requirements, this 
    ''' class is an abstract base class
    ''' </remarks>
    Public MustInherit Class HostNotificationBase
        Inherits HostMessageBase


        Public Enum NotificationCategories
            ''' <summary>
            ''' This notification message can be ignored
            ''' </summary>
            NotSet = 0
            ''' <summary>
            ''' A new host has started up and can be used going forward
            ''' </summary>
            HostJoining = 1
            ''' <summary>
            ''' A host is disconnecting and no new requests should be sent its way
            ''' </summary>
            HostClosing = 2
            ''' <summary>
            ''' A host is very busy and should not be sent any new requests for a while
            ''' </summary>
            HighUsage = 3
            ''' <summary>
            ''' A host should be regarded as unreliable and not sent any new requests
            ''' </summary>
            Unreliable = 4
        End Enum

        Private ReadOnly m_notificationCategory As NotificationCategories
        ''' <summary>
        ''' The type of notification message this is
        ''' </summary>
        Public ReadOnly Property NotificationCategory As NotificationCategories
            Get
                Return m_notificationCategory
            End Get
        End Property

        Protected Friend Sub New(originatorIn As IHost,
                             senderIn As IHost,
                             targetIn As IHost,
                             uniqueIdentifierIn As Guid,
                              notificationCategoryIn As NotificationCategories)

            MyBase.New(MessageCategory.Notification,
                   originatorIn,
                   senderIn,
                   targetIn,
                   uniqueIdentifierIn)

            m_notificationCategory = notificationCategoryIn

        End Sub


    End Class
End Namespace