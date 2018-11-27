Option Strict On
Option Explicit On

Imports System
Imports CQRSAzure.Hosting
Imports CQRSAzure.QueryDefinition

Namespace Request

    ''' <summary>
    ''' A request to execute a given query
    ''' </summary>
    Public NotInheritable Class ExecuteQueryRequest
        Inherits HostRequestBase

        'TODO: Add payload to identify the query and its input parameters
        Private ReadOnly m_queryDefintion As IQueryDefinition

        Private Sub New(originatorIn As IHost,
                       senderIn As IHost,
                       targetIn As IHost,
                       uniqueIdentifierIn As Guid,
                       queryDefintionIn As IQueryDefinition)

            MyBase.New(originatorIn, senderIn, targetIn, uniqueIdentifierIn, RequestCategories.ExecuteQuery)

            m_queryDefintion = queryDefintionIn

        End Sub
    End Class

End Namespace