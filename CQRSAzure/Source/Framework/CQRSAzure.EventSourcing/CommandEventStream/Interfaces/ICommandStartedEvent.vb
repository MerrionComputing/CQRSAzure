Imports CQRSAzure.EventSourcing

Namespace Commands

    ''' <summary>
    ''' A command processor has started executing the command
    ''' </summary>
    Public Interface ICommandStartedEvent
        Inherits IEvent(Of ICommandAggregateIdentifier)

        ''' <summary>
        ''' The date/time the processing of the command was started
        ''' </summary>
        ReadOnly Property ProcessingStartDate As Nullable(Of DateTime)


    End Interface
End Namespace
