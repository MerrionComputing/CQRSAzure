Option Strict On
Option Explicit On

Imports System
Imports CQRSAzure.Hosting

Namespace Response
    ''' <summary>
    ''' This host was unable to perform the request passed to it
    ''' </summary>
    Public NotInheritable Class FailureResponse
        Inherits HostResponseBase

        ''' <summary>
        ''' The differentpossible causes of a failure response
        ''' </summary>
        Public Enum FailureClassifications
            ''' <summary>
            ''' The host processing the request has an error 
            ''' </summary>
            ''' <remarks>
            ''' The request should be passed on to another host to process
            ''' </remarks>
            HostError = 0
            ''' <summary>
            ''' The request itself had an error
            ''' </summary>
            ''' <remarks>
            ''' It probably doesn't make sense to try another host
            ''' </remarks>
            RequestError = 1
            ''' <summary>
            ''' The request has already been done by some other process
            ''' </summary>
            ''' <remarks>
            ''' No action needs to be taken otehr than logging this
            ''' </remarks>
            RequestAlreadyComplete = 2
        End Enum

        Private ReadOnly m_failureClass As FailureClassifications
        ''' <summary>
        ''' What type of a failure report this is
        ''' </summary>
        Public ReadOnly Property FailureClassification As FailureClassifications
            Get
                Return m_failureClass
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
                       failureClassificationsIn As FailureClassifications)

            MyBase.New(responderIn, requesterIn, uniqueIdentifierIn, ResponseCategories.Acknowledgement, requestIdentifierIn)

            m_failureClass = failureClassificationsIn

        End Sub

    End Class
End Namespace