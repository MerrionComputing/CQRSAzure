Option Strict On
Option Explicit On

Imports CQRSAzure.Hosting

Namespace Response
    ''' <summary>
    ''' Base class for any request sent from one host to another
    ''' </summary>
    Public MustInherit Class HostResponseBase
        Inherits HostMessageBase

        Public Enum ResponseCategories
            ''' <summary>
            ''' Acknowledge that a request has been received
            ''' </summary>
            Acknowledgement = 0
            ''' <summary>
            ''' An error occured while trying to process this request
            ''' </summary>
            Failure = 1
            ''' <summary>
            ''' A command or query was executed and these are the results
            ''' </summary>
            ''' <remarks>
            ''' For a command this will just return the command execution status
            ''' </remarks>
            Results = 2
        End Enum

        Private ReadOnly m_category As ResponseCategories
        Public ReadOnly Property ResponseCategory As ResponseCategories
            Get
                Return m_category
            End Get
        End Property

        ''' <summary>
        ''' Unique identifier of this host response
        ''' </summary>
        Public ReadOnly Property ResponseUniqueIdentifier As Guid
            Get
                Return MyBase.MessageUniqueIdentifier
            End Get
        End Property


        Private ReadOnly m_requestIdentifier As Guid
        ''' <summary>
        ''' The unique identifier of the request to which this the response
        ''' </summary>
        ''' <remarks>
        ''' This is used in any response in orderto uniquely identify the request being responded to
        ''' </remarks>
        ReadOnly Property RequestIdentifier As Guid
            Get
                Return m_requestIdentifier
            End Get
        End Property


        Protected Friend Sub New(originatorIn As IHost,
                             targetIn As IHost,
                             uniqueIdentifierIn As Guid,
                             responseCategoryIn As ResponseCategories,
                             requestIdentifierIn As Guid)
            MyBase.New(MessageCategory.Response,
                   originatorIn,
                   Nothing,
                   targetIn,
                   uniqueIdentifierIn)

            m_category = responseCategoryIn
            m_requestIdentifier = requestIdentifierIn

        End Sub

    End Class
End Namespace