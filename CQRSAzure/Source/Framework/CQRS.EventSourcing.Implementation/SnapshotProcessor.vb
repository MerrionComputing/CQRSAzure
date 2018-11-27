Imports System
''' <summary>
''' Processor to load or save snapshots for a projection
''' </summary>
Public NotInheritable Class SnapshotProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey)


    ''' <summary>
    ''' Save the given projection snapshot to the underlying storage technology
    ''' </summary>
    ''' <param name="snapshotToSave">
    ''' The projection snapshot to be saved to the backing storage
    ''' </param>
    Public Sub SaveSnapshot(ByVal snapshotToSave As IProjectionSnapshot(Of TAggregate, TAggregateKey))
        Throw New NotImplementedException
    End Sub

    ''' <summary>
    ''' Get the most recent projection snapshot from the underlying storage technology
    ''' </summary>
    ''' <param name="priorToSequence">
    ''' if this is non zero get the snapshot record closest to the prior-to sequence number (inclusive)
    ''' </param>
    ''' <returns>
    ''' The most recent snapshot available if one has been saved - or Nothing (null) if none exists
    ''' </returns>
    Public Function GetLatestSnapshot(Optional ByVal priorToSequence As UInteger = 0) As IProjectionSnapshot(Of TAggregate, TAggregateKey)
        Throw New NotImplementedException
    End Function


    ''' <summary>
    ''' Turn the current state of a projection into a snapshot record that can be saved to a backing store
    ''' </summary>
    ''' <param name="projectionToSave">
    ''' The projection that has been run over an event stream
    ''' </param>
    ''' <param name="CurrentSequence">
    ''' The current sequence number we have got to in that event stream
    ''' </param>
    ''' <param name="CurrentAsOfDate">
    ''' The as-of date of the most recent event in the event stream (Optional)
    ''' </param>
    Public Function GenerateSnapshot(ByVal projectionToSave As IProjection(Of TAggregate, TAggregateKey),
                                     ByVal CurrentSequence As UInteger,
                                     Optional ByVal CurrentAsOfDate As DateTime = Nothing) As IProjectionSnapshot(Of TAggregate, TAggregateKey)
        Throw New NotImplementedException
    End Function

End Class
