Option Strict On
Option Explicit On

Namespace Request

    ''' <summary>
    ''' Request to run a single projection for an aggregate instance
    ''' </summary>
    Public NotInheritable Class RunProjectionRequest
        Inherits HostRequestBase

        'TODO: Identification for the aggregate 

        'TODO: Identification of the projection to execute

        Private Sub New(originatorIn As IHost,
                       senderIn As IHost,
                       targetIn As IHost,
                       uniqueIdentifierIn As Guid)

            MyBase.New(originatorIn, senderIn, targetIn, uniqueIdentifierIn, RequestCategories.RunProjection)

        End Sub

    End Class
End Namespace