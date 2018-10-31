Namespace InMemory
    ''' <summary>
    ''' Settings that change how an in-memory event stream operates
    ''' </summary>
    Public Interface IInMemorySettings

        ''' <summary>
        ''' Should the in-memory stream write to the debug message whenever it handles an event
        ''' </summary>
        Property DebugMessages As Boolean

    End Interface
End Namespace