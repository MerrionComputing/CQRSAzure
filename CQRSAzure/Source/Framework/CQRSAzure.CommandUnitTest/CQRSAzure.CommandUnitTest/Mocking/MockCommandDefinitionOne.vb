Imports CQRSAzure.CommandDefinition

''' <summary>
''' Mock command definition class for use in unit testing
''' </summary>
Public Class MockCommandDefinitionOne
    Inherits CommandDefinitionBase
    Implements ICommandDefinition

    Public Overrides ReadOnly Property CommandName As String
        Get
            Return NameOf(MockCommandDefinitionOne)
        End Get
    End Property

    Public Property FirstNamesParameter As String
        Get
            Return MyBase.GetParameterValue(Of String)(NameOf(FirstNamesParameter), 0)
        End Get
        Set(value As String)
            MyBase.SetParameterValue(Of String)(NameOf(FirstNamesParameter), 0, value)
        End Set
    End Property


    Public Property SurnameParameter As String
        Get
            Return MyBase.GetParameterValue(Of String)(NameOf(SurnameParameter), 0)
        End Get
        Set(value As String)
            MyBase.SetParameterValue(Of String)(NameOf(SurnameParameter), 0, value)
        End Set
    End Property


End Class
