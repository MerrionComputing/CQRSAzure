Option Strict On
Option Explicit On

Imports CQRSAzure.Hosting

Namespace Request
    ''' <summary>
    ''' Base class for any request sent from one host to another
    ''' </summary>
    Public MustInherit Class HostRequestBase
        Inherits HostMessageBase

        Public Enum RequestCategories
            ''' <summary>
            ''' Unknown request typoe - should be ignored
            ''' </summary>
            NotSet = 0
            ''' <summary>
            ''' Run a given command
            ''' </summary>
            ExecuteCommand = 1
            ''' <summary>
            ''' Run a query and send the results back to me
            ''' </summary>
            ExecuteQuery = 2
            ''' <summary>
            ''' Get the set of members of a given identety group as of now
            ''' </summary>
            GetIdentityGroupMembers = 3
            ''' <summary>
            ''' Run a single projection only
            ''' </summary>
            RunProjection = 4
            ''' <summary>
            ''' Run a single classifier only
            ''' </summary>
            RunClassifier = 5
        End Enum

        Private ReadOnly m_category As RequestCategories
        ''' <summary>
        ''' The type of request this host message pertains to
        ''' </summary>
        Public ReadOnly Property RequestCategory As RequestCategories
            Get
                Return m_category
            End Get
        End Property

        ''' <summary>
        ''' The unique identifier of an individual request
        ''' </summary>
        ''' <remarks>
        ''' This is used in any response in orderto uniquely identify the request being responded to
        ''' </remarks>
        ReadOnly Property RequestIdentifier As Guid
            Get
                Return MyBase.MessageUniqueIdentifier
            End Get
        End Property

        Protected Friend Sub New(originatorIn As IHost,
                   senderIn As IHost,
                   targetIn As IHost,
                   uniqueIdentifierIn As Guid,
                   ByVal categoryIn As RequestCategories)

            MyBase.New(MessageCategory.Request,
                   originatorIn,
                   senderIn,
                   targetIn,
                   uniqueIdentifierIn)

            m_category = categoryIn

        End Sub

    End Class
End Namespace