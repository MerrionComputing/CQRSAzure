
''' <summary>
''' Interface describing a data transfer object for reporting the status of a command that has been sent for processing
''' </summary>
Public Interface ICommandStatusResult


    ''' <summary>
    ''' The different statuses a command can have
    ''' </summary>
    Enum CommandExecutionStatus
        ''' <summary>
        ''' The command for which we are seeming the status is not known to the system
        ''' </summary>
        ''' <remarks>
        ''' This may occur if the command has not yet been received but the query gets asked
        ''' before it
        ''' </remarks>
        UnknownCommand = 0
        ''' <summary>
        ''' The command has completed successfully 
        ''' </summary>
        SuccessfulCompletion = 1
        ''' <summary>
        ''' The command is being processed
        ''' </summary>
        InProgress = 2
        ''' <summary>
        ''' The command is in the queue to process but has not yet started
        ''' </summary>
        Queued = 3
        ''' <summary>
        ''' The command failed (or only partially completed) but has been sent for a retry
        ''' </summary>
        ''' <remarks>
        ''' This might be used if a handler was overloaded or offline when the command was in progress
        ''' </remarks>
        Retrying = 4
        ''' <summary>
        ''' The command failed fatally and will not be retried
        ''' </summary>
        Failed = 5
    End Enum

    ''' <summary>
    ''' Underlying reason why the command failed 
    ''' </summary>
    Enum CommandFailureCause
        ''' <summary>
        ''' The underlying reason is not known 
        ''' </summary>
        NotKnown = 0
        ''' <summary>
        ''' The account that sent the command does not have permission to do so
        ''' </summary>
        InsufficientPermission = 1
        ''' <summary>
        ''' The business state of the data makes the command not valid
        ''' </summary>
        InvalidBusinessDataState = 2
        ''' <summary>
        ''' Nothing was able to handle the given command
        ''' </summary>
        UnavailableHanlder = 3
        ''' <summary>
        ''' The input parameters of the command were wrong
        ''' </summary>
        InvalidCommandParameter = 4
        ''' <summary>
        ''' The context (machine, execute time etc.) for the command was invalid
        ''' </summary>
        InvalidCommandContext = 5
    End Enum

    ''' <summary>
    ''' The unique instance identifier of the command to which this status pertains
    ''' </summary>
    ReadOnly Property CommandInstanceIdentifier As Guid

    ''' <summary>
    ''' The name of the command type that this status pertains to
    ''' </summary>
    ReadOnly Property CommandName As String

    ''' <summary>
    ''' The status returned from the command when queried
    ''' </summary>
    ReadOnly Property ExecutationStatus As CommandExecutionStatus

    ''' <summary>
    ''' If the command has failed, this gives the reason
    ''' </summary>
    ReadOnly Property FailureCause As CommandFailureCause

    ''' <summary>
    ''' The text message from a command 
    ''' </summary>
    ReadOnly Property StatusMessage As String



End Interface
