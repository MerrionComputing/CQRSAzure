Imports CQRSAzure.EventSourcing

Namespace Commands
    ''' <summary>
    ''' A fatal error has occured while processing this command that means it cannot (or should not)
    ''' be retried
    ''' </summary>
    Public Interface ICommandFatalErrorOccuredEvent
        Inherits IEvent(Of ICommandAggregateIdentifier)

        ''' <summary>
        ''' The date/time the command step was stopped with a fatal error
        ''' </summary>
        ReadOnly Property ErrorDate As Nullable(Of DateTime)


        ''' <summary>
        ''' If the command has many steps, this is the one that failed
        ''' </summary>
        ReadOnly Property StepNumber As Integer

        ''' <summary>
        ''' The human readable description of the error
        ''' </summary>
        ReadOnly Property ErrorMessage As String

    End Interface
End Namespace
