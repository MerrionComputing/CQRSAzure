Imports System
Imports CQRSAzure.EventSourcing

Namespace Commands
    ''' <summary>
    ''' The command completed successfully
    ''' </summary>
    Public Interface ICommandCompletedEvent
        Inherits IEvent(Of ICommandAggregateIdentifier)

        ''' <summary>
        ''' The date/time the command completed
        ''' </summary>
        ReadOnly Property CompletionDate As Nullable(Of DateTime)



        ''' <summary>
        ''' The human readable description of the completion state
        ''' </summary>
        ReadOnly Property SuccessMessage As String

    End Interface
End Namespace
