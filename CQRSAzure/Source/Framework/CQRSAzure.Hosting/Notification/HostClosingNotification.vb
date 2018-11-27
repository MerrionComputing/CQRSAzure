Imports System

Namespace Notification

    ''' <summary>
    ''' A host is having to shut down
    ''' </summary>
    Public NotInheritable Class HostClosingNotification
        Inherits HostNotificationBase

        Private ReadOnly m_graceful As Boolean
        ''' <summary>
        ''' Is the host shutting down gracefully 
        ''' </summary>
        ''' <remarks>
        ''' Graceful shutdown means any requests in flight will be serviced but no new requests can be accepted
        ''' </remarks> 
        Public ReadOnly Property Graceful As Boolean
            Get
                Return m_graceful
            End Get
        End Property

        Private Sub New(originatorIn As IHost,
                       senderIn As IHost,
                       targetIn As IHost,
                       uniqueIdentifierIn As Guid,
                       ByVal gracefulIn As Boolean)
            MyBase.New(originatorIn, senderIn, targetIn, uniqueIdentifierIn, NotificationCategories.HostClosing)

            m_graceful = gracefulIn

        End Sub

    End Class

End Namespace