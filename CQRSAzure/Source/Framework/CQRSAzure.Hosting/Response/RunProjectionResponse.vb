Option Strict On
Option Explicit On

Imports System
Imports CQRSAzure.Hosting

Namespace Response
    ''' <summary>
    ''' This host executed the projection and returned the resulting list
    ''' </summary>
    Public NotInheritable Class RunProjectionResponse
        Inherits HostResponseBase

        'Projection results...

        Private Sub New(responderIn As IHost,
               requesterIn As IHost,
               uniqueIdentifierIn As Guid,
               requestIdentifierIn As Guid)

            MyBase.New(responderIn, requesterIn, uniqueIdentifierIn, ResponseCategories.Acknowledgement, requestIdentifierIn)

        End Sub
    End Class
End Namespace