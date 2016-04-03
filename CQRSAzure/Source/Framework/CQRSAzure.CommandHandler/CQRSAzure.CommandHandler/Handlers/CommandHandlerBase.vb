Imports CQRSAzure.CommandDefinition

''' <summary>
''' A base class for any command handler implementations
''' </summary>
''' <typeparam name="TCommandDefinition">
''' The specific type of command to handle
''' </typeparam>
''' <remarks>
''' This is to hold common cross-cutting functionality that applies to all command handlers (such as routing, error notifications, logging etc.)
''' </remarks>
Public MustInherit Class CommandHandlerBase(Of TCommandDefinition As ICommandDefinition)
    Implements ICommandHandler(Of TCommandDefinition)

    ''' <summary>
    ''' handle the specific instance of the command passed in
    ''' </summary>
    ''' <param name="cmdToHandle">
    ''' The instance of the command to handle along with its input parameters
    ''' </param>
    Public MustOverride Sub HandleCommand(cmdToHandle As TCommandDefinition) Implements ICommandHandler(Of TCommandDefinition).HandleCommand

End Class
