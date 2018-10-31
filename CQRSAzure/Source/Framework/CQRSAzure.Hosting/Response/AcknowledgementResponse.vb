Option Strict On
Option Explicit On

Imports CQRSAzure.Hosting

Namespace Response
    ''' <summary>
    ''' A response to acknowledge receipt of a request
    ''' </summary>
    ''' <remarks>
    ''' An acknowledgement is always sent straight to the originator :. no need to have a separate "Sender" in the notification
    ''' </remarks>
    Public NotInheritable Class AcknowledgementResponse
        Inherits HostResponseBase

        ''' <summary>
        ''' The different responses acknowledging a request
        ''' </summary>
        Public Enum AcknowledgementStatuses
            ''' <summary>
            ''' This host will process the request and get the results back to you
            ''' </summary>
            Processing = 0
            ''' <summary>
            ''' This host is passing the request on to another host which will get back to you
            ''' </summary>
            HandingOn = 1
            ''' <summary>
            ''' This host will not process the request - you need to try a different host (or throw an error)
            ''' </summary>
            WillNotProcess = 2
        End Enum

        Private ReadOnly m_ackStatus As AcknowledgementStatuses
        Public ReadOnly Property AcknowledgementStatus As AcknowledgementStatuses
            Get
                Return m_ackStatus
            End Get
        End Property

        ''' <summary>
        ''' Create an acknowledgement that we received a request
        ''' </summary>
        ''' <param name="responderIn">
        ''' The host (me) responding to acknowledge the request
        ''' </param>
        ''' <param name="requesterIn">
        ''' The host which originated the request
        ''' </param>
        ''' <param name="uniqueIdentifierIn">
        ''' Unique identifier of the acknowledgement
        ''' </param>
        ''' <param name="requestIdentifierIn">
        ''' The unique identifier of the request being responded to
        ''' </param>
        Private Sub New(responderIn As IHost,
                       requesterIn As IHost,
                       uniqueIdentifierIn As Guid,
                       requestIdentifierIn As Guid,
                        acknowledgementStatusIn As AcknowledgementStatuses)

            MyBase.New(responderIn, requesterIn, uniqueIdentifierIn, ResponseCategories.Acknowledgement, requestIdentifierIn)

            m_ackStatus = acknowledgementStatusIn

        End Sub
    End Class

End Namespace