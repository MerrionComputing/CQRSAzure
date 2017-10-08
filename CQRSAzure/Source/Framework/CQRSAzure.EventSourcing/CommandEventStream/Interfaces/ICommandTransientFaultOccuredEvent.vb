Imports CQRSAzure.EventSourcing

Namespace Commands
    Public Interface ICommandTransientFaultOccuredEvent
        Inherits IEvent(Of ICommandAggregateIdentifier)

        ''' <summary>
        ''' The date/time the command step was stopped with a transient error
        ''' </summary>
        ReadOnly Property FaultDate As Nullable(Of DateTime)

        ''' <summary>
        ''' If the command has many steps, this is the one that has the fault
        ''' </summary>
        ReadOnly Property StepNumber As Integer

        ''' <summary>
        ''' The human readable description of the fault
        ''' </summary>
        ReadOnly Property FaultMessage As String

    End Interface
End Namespace
