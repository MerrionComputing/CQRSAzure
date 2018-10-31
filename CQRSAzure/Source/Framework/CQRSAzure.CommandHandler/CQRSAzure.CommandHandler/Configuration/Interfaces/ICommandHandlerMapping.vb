Public Interface ICommandHandlerMapping

    ''' <summary>
    ''' Unique fully qualified class name of the command definition class
    ''' </summary>
    Property DefinitionName As String

    ''' <summary>
    ''' Fully qualified class name of the command handler class
    ''' </summary>
    Property HandlerName As String

End Interface
