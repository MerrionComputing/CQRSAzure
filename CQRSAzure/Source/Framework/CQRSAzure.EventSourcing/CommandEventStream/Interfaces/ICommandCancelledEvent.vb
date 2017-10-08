Imports CQRSAzure.EventSourcing

Namespace Commands

    ''' <summary>
    ''' A command execution was cancelled
    ''' </summary>
    Public Interface ICommandCancelledEvent
        Inherits IEvent(Of ICommandAggregateIdentifier)

        ''' <summary>
        ''' The date/time the command was cancelled
        ''' </summary>
        ReadOnly Property CancellationDate As Nullable(Of DateTime)


        ''' <summary>
        ''' The reason the command was cancelled
        ''' </summary>
        ReadOnly Property Reason As String

    End Interface

End Namespace
