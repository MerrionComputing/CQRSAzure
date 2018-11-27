Imports System
Imports CQRSAzure.EventSourcing

Namespace Commands
    Public Interface ICommandAggregateIdentifier
        Inherits IAggregationIdentifier(Of Guid)

    End Interface
End Namespace