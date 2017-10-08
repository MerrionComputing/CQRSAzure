Public Interface IQueryHandlerMapping

    ''' <summary>
    ''' Unique fully qualified class name of the query definition class
    ''' </summary>
    Property DefinitionName As String

    ''' <summary>
    ''' Fully qualified class name of the query handler class
    ''' </summary>
    Property HandlerName As String

End Interface
