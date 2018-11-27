Imports System
Imports CQRSAzure.EventSourcing

Namespace Queries

    ''' <summary>
    ''' Identifier for an individual instance of a query
    ''' </summary>
    Public Interface IQueryAggregateIdentifier
        Inherits IAggregationIdentifier(Of Guid)

    End Interface
End Namespace
