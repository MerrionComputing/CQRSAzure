Imports CQRSAzure.QueryDefinition
Imports CQRSAzure.QueryHandler
Imports CQRSAzure.QueryUnitTest

Public Class MockQueryHandlerOne
    Inherits QueryHandlerBase(Of MockQueryDefinitionOne, MockQueryResultsOne)

    Public Overrides Function HandleQuery(qryToHandle As MockQueryDefinitionOne) As MockQueryResultsOne

        If (qryToHandle.AsOfDateParameter.HasValue) Then
            Return New MockQueryResultsOne() With {.StringResultProperty = qryToHandle.AsOfDateParameter.ToString()}
        Else
            Return New MockQueryResultsOne() With {.StringResultProperty = "No date set"}
        End If

    End Function
End Class
