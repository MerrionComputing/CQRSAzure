Option Strict On
Option Explicit On

Imports CQRSAzure.Hosting

Namespace Response
    ''' <summary>
    ''' This host executed the command that it was requested to do
    ''' </summary>
    Public NotInheritable Class ExecuteCommandResponse
        Inherits HostResponseBase

        'TODO : Any playload to add information about the command being executed (?)

        Private Sub New(responderIn As IHost,
               requesterIn As IHost,
               uniqueIdentifierIn As Guid,
               requestIdentifierIn As Guid)

            MyBase.New(responderIn, requesterIn, uniqueIdentifierIn, ResponseCategories.Acknowledgement, requestIdentifierIn)

        End Sub
    End Class
End Namespace