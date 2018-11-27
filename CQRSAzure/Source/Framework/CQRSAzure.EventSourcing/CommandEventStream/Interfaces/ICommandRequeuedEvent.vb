Imports System
Imports CQRSAzure.EventSourcing

Namespace Commands
    Public Interface ICommandRequeuedEvent
        Inherits IEvent(Of ICommandAggregateIdentifier)

        ''' <summary>
        ''' The date/time the command step was re-queued
        ''' </summary>
        ReadOnly Property RequeueDate As Nullable(Of DateTime)


    End Interface
End Namespace
