Imports CQRSAzure.EventSourcing

Namespace Commands
    ''' <summary>
    ''' Common components that provide context information for all the events that can happen to a command
    ''' </summary>
    ''' <remarks>
    ''' This is to allow a single place to add mandatory fields (such as IP address or security token) if your business
    ''' process should need that
    ''' </remarks>
    Public Interface ICommandEventContext

        ''' <summary>
        ''' Where was the command event request sent from
        ''' </summary>
        ReadOnly Property Source As String

        ''' <summary>
        ''' The user who caused the command event
        ''' </summary>
        ReadOnly Property Username As String

        ''' <summary>
        ''' A token to be used if the command requires authentication
        ''' </summary>
        ReadOnly Property Token As String

    End Interface
End Namespace