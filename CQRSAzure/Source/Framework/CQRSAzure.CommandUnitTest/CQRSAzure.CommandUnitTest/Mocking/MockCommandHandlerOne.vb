
Imports CQRSAzure.CommandUnitTest

''' <summary>
''' Mock command handler class for use in unit testing
''' </summary>
Public Class MockCommandHandlerOne
    Inherits CommandHandler.CommandHandlerBase(Of MockCommandDefinitionOne)

    Private _mostRecentName As String

    Public ReadOnly Property MostRecentName As String
        Get
            Return _mostRecentName
        End Get
    End Property

    Public Overrides Sub HandleCommand(cmdToHandle As MockCommandDefinitionOne)

        _mostRecentName = cmdToHandle.FirstNamesParameter & " " & cmdToHandle.SurnameParameter

    End Sub
End Class
