Option Strict On
Option Explicit On

Imports CQRSAzure.Hosting
Imports CQRSAzure.QueryDefinition

Namespace Response
    ''' <summary>
    ''' This host executed the query that it was requested to do
    ''' </summary>
    Public NotInheritable Class ExecuteQueryResponse
        Inherits HostResponseBase

        'Query results (?)...

        Private Sub New(responderIn As IHost,
                       requesterIn As IHost,
                       uniqueIdentifierIn As Guid,
                       requestIdentifierIn As Guid)

            MyBase.New(responderIn, requesterIn, uniqueIdentifierIn, ResponseCategories.Acknowledgement, requestIdentifierIn)

        End Sub
    End Class
End Namespace