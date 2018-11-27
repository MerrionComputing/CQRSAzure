Option Strict On
Option Explicit On

Imports System
Imports CQRSAzure.Hosting

Namespace Notification

    ''' <summary>
    ''' A host is starting up and joining the hosts network
    ''' </summary>
    Public NotInheritable Class HostJoiningNotification
        Inherits HostNotificationBase

        ''' <summary>
        ''' The types if request this host is able to respond to
        ''' </summary>
        <Flags()>
        Public Enum RequestCapabilities
            ''' <summary>
            ''' Host has no capabilities (used for message testing)
            ''' </summary>
            None = 0
            ''' <summary>
            ''' Host can execute commands
            ''' </summary>
            ExecuteCommands = &H1
            ''' <summary>
            ''' Host can run queries and return results
            ''' </summary>
            ExecuteQueries = &H2
            ''' <summary>
            ''' Host can return the members of an identity group
            ''' </summary>
            GetIdentityGroupMembers = &H4
            ''' <summary>
            ''' Host can run specific projections
            ''' </summary>
            RunProjection = &H8
            ''' <summary>
            ''' Host can run a specific classifier
            ''' </summary>
            RunClassifier = &H10
        End Enum


        Private ReadOnly m_capability As RequestCapabilities
        Public ReadOnly Property RequestCapability As RequestCapabilities
            Get
                Return m_capability
            End Get
        End Property


        Private Sub New(originatorIn As IHost,
                        senderIn As IHost,
                        targetIn As IHost,
                        uniqueIdentifierIn As Guid,
                        capabilityIn As RequestCapabilities)
            MyBase.New(originatorIn, senderIn, targetIn, uniqueIdentifierIn, NotificationCategories.HostJoining)

            m_capability = capabilityIn

        End Sub

    End Class
End Namespace