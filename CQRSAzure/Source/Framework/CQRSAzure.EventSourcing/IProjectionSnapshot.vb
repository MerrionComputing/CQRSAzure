''' <summary>
''' A snapshot of a projection as at a particular point in time
''' </summary>
Public Interface IProjectionSnapshot(Of TAggregate As IAggregationIdentifier, TAggregateKey)

    ''' <summary>
    ''' The effective date/time for the snapshot
    ''' </summary>
    ''' <returns>
    ''' 
    ''' </returns>
    ReadOnly Property AsOfDate As DateTime

    ''' <summary>
    ''' The sequence number of the last event that was part of the snapshot
    ''' </summary>
    ''' <returns>
    ''' If the event stream current max sequence is higher than this then the snapshot is not up to date and the 
    ''' snapshot processor should continue from this point
    ''' </returns>
    ReadOnly Property Sequence As UInteger

End Interface
