''' <summary>
''' Interface for any implementation class that finds and reads untyped cached projection snapshot records
''' </summary>
Public Interface IProjectionSnapshotReaderUntyped

    ''' <summary>
    ''' Load the snapshot data to the backing storage technology
    ''' </summary>
    ''' <param name="key">
    ''' The unique key of the aggregate we are retrieving a projection snapshot for
    ''' </param>
    ''' <param name="OnOrBeforeSequence">
    ''' if specified, get the latest snapshot prior to the given sequence number
    ''' </param>
    Function GetSnapshot(ByVal key As String, Optional ByVal OnOrBeforeSequence As UInteger = 0) As IProjectionSnapshot


    ''' <summary>
    ''' Gets the sequence number of the latest snapshot held for a given aggregate instance
    ''' </summary>
    ''' <param name="key">
    ''' The unique key of the aggregate we are retrieving a projection snapshot for
    ''' </param>
    ''' <param name="OnOrBeforeSequence">
    ''' if specified, get the latest snapshot prior to the given sequence number
    ''' </param>
    ''' <returns>
    ''' If zero, there are no snapshots found for this aggregate key
    ''' </returns>
    Function GetLatestSnapshotSequence(ByVal key As String, Optional ByVal OnOrBeforeSequence As UInteger = 0) As UInteger

End Interface
