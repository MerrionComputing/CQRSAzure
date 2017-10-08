Option Strict On
Option Explicit On

Imports CQRSAzure.Hosting
Imports CQRSAzure.IdentifierGroup


Namespace Request

    ''' <summary>
    ''' Request to run a single classifier for an aggregate instance
    ''' </summary>
    Public NotInheritable Class RunClassifierRequest
        Inherits HostRequestBase

        'TODO: Identification for the aggregate 

        'TODO: Identification of the classifier to execute

        Private Sub New(originatorIn As IHost,
                        senderIn As IHost,
                        targetIn As IHost,
                        uniqueIdentifierIn As Guid)

            MyBase.New(originatorIn, senderIn, targetIn, uniqueIdentifierIn, RequestCategories.RunClassifier)

        End Sub
    End Class
End Namespace