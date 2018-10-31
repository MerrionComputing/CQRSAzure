Imports CQRSAzure.QueryDefinition

''' <summary>
''' A query handler to get the status of the given command
''' </summary>
''' <remarks>
''' Commands are held in a a specially named event stream 
''' </remarks>
Public Class GetCommandStatusQueryHandler
    Inherits QueryHandlerBase(Of GetCommandStatusQueryDefinition, ICommandStatusResult)


    Public Overrides Function HandleQuery(qryToHandle As GetCommandStatusQueryDefinition) As ICommandStatusResult

        'Unique identifier is - qryToHandle.CommandIdentifier

    End Function

End Class
