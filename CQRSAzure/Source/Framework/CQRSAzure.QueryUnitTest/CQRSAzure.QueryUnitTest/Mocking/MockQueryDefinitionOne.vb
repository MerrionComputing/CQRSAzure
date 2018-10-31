Imports CQRSAzure.QueryDefinition

Public Class MockQueryDefinitionOne
    Inherits QueryDefinitionBase(Of MockQueryResultsOne)

    Public Overrides ReadOnly Property QueryName As String
        Get
            Return NameOf(MockQueryDefinitionOne)
        End Get
    End Property

    Public Property AsOfDateParameter As Nullable(Of DateTime)
        Get
            Return MyBase.GetParameterValue(Of Nullable(Of DateTime))(NameOf(AsOfDateParameter), 0)
        End Get
        Set(value As Nullable(Of DateTime))
            MyBase.SetParameterValue(Of Nullable(Of DateTime))(NameOf(AsOfDateParameter), 0, value)
        End Set
    End Property

End Class
