Imports CQRSAzure.CommandDefinition

''' <summary>
''' Factory methods for spinning-up instances of command handlers for given command definitions
''' </summary>
Module CommandHandlerFactory


    ''' <summary>
    ''' Perform the start-up initialisations neccessary to link command handlers up to their definitions
    ''' </summary>
    ''' <remarks>
    ''' These may be declared in a config file or may be discovered by reflection as desired
    ''' </remarks>
    Public Sub Initialise()



    End Sub

    ''' <summary>
    ''' Spin up an instance of a command handler to handle a given command
    ''' </summary>
    ''' <typeparam name="TCommandDefinition">
    ''' The type of the command to handle
    ''' </typeparam>
    ''' <param name="commandInstance">
    ''' The instance of the command, with its specific parameters, to handle
    ''' </param>
    Public Function Create(Of TCommandDefinition As ICommandDefinition)(ByVal commandInstance As TCommandDefinition) As ICommandHandler(Of TCommandDefinition)

        Throw New NotImplementedException("Command handler functionality not yet implemenmted")

    End Function

End Module
