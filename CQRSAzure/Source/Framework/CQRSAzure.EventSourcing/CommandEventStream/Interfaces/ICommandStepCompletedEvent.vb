Imports CQRSAzure.EventSourcing

Namespace Commands
    Public Interface ICommandStepCompletedEvent
        Inherits IEvent(Of ICommandAggregateIdentifier)

        ''' <summary>
        ''' The date/time the command step was completed
        ''' </summary>
        ReadOnly Property StepCompletionDate As Nullable(Of DateTime)


        ''' <summary>
        ''' The ordinal number of the step that was completed
        ''' </summary>
        ReadOnly Property StepNumber As Integer

        ''' <summary>
        ''' Message accompanying the step completion
        ''' </summary>
        ReadOnly Property StatusMessage As String

    End Interface
End Namespace
