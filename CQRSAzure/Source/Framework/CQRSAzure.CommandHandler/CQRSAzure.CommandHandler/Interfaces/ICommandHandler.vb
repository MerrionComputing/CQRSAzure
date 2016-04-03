Imports CQRSAzure.CommandDefinition

''' <summary>
''' Base interface for any class that handles a specific command 
''' </summary>
''' <typeparam name="TCommandDefinition">
''' The command definition to perform the action for
''' </typeparam>
Public Interface ICommandHandler(Of TCommandDefinition As ICommandDefinition)

    ''' <summary>
    ''' Handle the command
    ''' </summary>
    ''' <param name="cmdToHandle">
    ''' The instance of the command to handle (with parameters)
    ''' </param>
    Sub HandleCommand(ByVal cmdToHandle As TCommandDefinition)

End Interface
