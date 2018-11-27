Option Strict On
Option Explicit On
Imports System

''' <summary>
''' Base class for any communication between hosts
''' </summary>
Public MustInherit Class HostMessageBase


    Public Enum MessageCategory
        ''' <summary>
        ''' The message is a broadcast notification of state
        ''' </summary>
        Notification = 0
        ''' <summary>
        ''' The message is a request for the target host to perform some action
        ''' </summary>
        Request = 1
        ''' <summary>
        ''' The message is a response to a request
        ''' </summary>
        Response = 2
    End Enum

    Private ReadOnly m_hops As Integer
    ''' <summary>
    ''' The number of times this message has been passed along 
    ''' </summary>
    Public ReadOnly Property Hops As Integer
        Get
            Return m_hops
        End Get
    End Property

    Private ReadOnly m_category As MessageCategory
    ''' <summary>
    ''' The type of inter-host communication that this message is
    ''' </summary>
    Public ReadOnly Property Category As MessageCategory
        Get
            Return m_category
        End Get
    End Property

    Private ReadOnly m_originator As IHost
    ''' <summary>
    ''' The host that originated the message
    ''' </summary>
    ''' <remarks>
    ''' This will be the same as the sender for a new message
    ''' </remarks>
    Public ReadOnly Property Originator As IHost
        Get
            Return m_originator
        End Get
    End Property


    Private ReadOnly m_sender As IHost
    ''' <summary>
    ''' The sender of this instance of the inter-host communication
    ''' </summary>
    Public ReadOnly Property Sender As IHost
        Get
            Return m_sender
        End Get
    End Property


    Private ReadOnly m_target As IHost
    ''' <summary>
    ''' The target that the inter-host communication is sent to
    ''' </summary>
    Public ReadOnly Property Target As IHost
        Get
            Return m_target
        End Get
    End Property

    Private ReadOnly m_messageUniqueIdentifier As Guid
    ''' <summary>
    ''' The unique identifier of this message between hosts
    ''' </summary>
    Public ReadOnly Property MessageUniqueIdentifier As Guid
        Get
            Return m_messageUniqueIdentifier
        End Get
    End Property

    ''' <summary>
    ''' Create a new host message to send
    ''' </summary>
    ''' <param name="categoryIn">
    ''' The type of message to create
    ''' </param>
    ''' <param name="originatorIn">
    ''' The originator of the message
    ''' </param>
    ''' <param name="senderIn">
    ''' The sender of this hop of the message
    ''' </param>
    ''' <param name="targetIn">
    ''' The target of the message
    ''' </param>
    ''' <param name="uniqueIdentifierIn">
    ''' The unique identifier of that message
    ''' </param>
    Protected Friend Sub New(ByVal categoryIn As MessageCategory,
                    ByVal originatorIn As IHost,
                    ByVal senderIn As IHost,
                    ByVal targetIn As IHost,
                    ByVal uniqueIdentifierIn As Guid,
                    Optional ByVal messageHopsIn As Integer = 0)

        m_category = categoryIn
        m_originator = originatorIn
        m_sender = senderIn
        m_target = targetIn
        m_messageUniqueIdentifier = uniqueIdentifierIn
        m_hops = messageHopsIn

    End Sub

    ''' <summary>
    ''' Create a new host message to send
    ''' </summary>
    ''' <param name="categoryIn">
    ''' The type of message to create
    ''' </param>
    ''' <param name="originatorIn">
    ''' The originator of the message
    ''' </param>
    ''' <param name="senderIn">
    ''' The sender of this hop of the message
    ''' </param>
    ''' <param name="targetIn">
    ''' The target of the message
    ''' </param>
    Protected Friend Sub New(ByVal categoryIn As MessageCategory,
                    ByVal originatorIn As IHost,
                    ByVal senderIn As IHost,
                    ByVal targetIn As IHost,
                             Optional ByVal messageHopsIn As Integer = 0)

        Me.New(categoryIn,
               originatorIn,
               senderIn,
               targetIn,
               Guid.NewGuid(),
               messageHopsIn)

    End Sub



End Class
