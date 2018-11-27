Option Strict On
Option Explicit On

Imports System
Imports CQRSAzure.Hosting
Imports CQRSAzure.IdentifierGroup

Namespace Response
    ''' <summary>
    ''' This host executed the classifier and returned the resulting list
    ''' </summary>
    Public NotInheritable Class RunClassifierResponse
        Inherits HostResponseBase

        'Classifier results...
        Private ReadOnly m_classificationResult As IClassifierDataSourceHandler.EvaluationResult
        Public ReadOnly Property ClassificationResult As IClassifierDataSourceHandler.EvaluationResult
            Get
                Return m_classificationResult
            End Get
        End Property

        Private Sub New(responderIn As IHost,
                       requesterIn As IHost,
                       uniqueIdentifierIn As Guid,
                       requestIdentifierIn As Guid,
                       classificationResultIn As IClassifierDataSourceHandler.EvaluationResult)

            MyBase.New(responderIn, requesterIn, uniqueIdentifierIn, ResponseCategories.Acknowledgement, requestIdentifierIn)

            m_classificationResult = classificationResultIn

        End Sub
    End Class
End Namespace