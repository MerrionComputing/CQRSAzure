''' <summary>
''' Interface for any implementation class that writes untyped cached projection snapshot records
''' </summary>
Public Interface IProjectionSnapshotWriterUntyped


    ''' <summary>
    ''' Save the snapshot data to the backing storage technology
    ''' </summary>
    ''' <param name="key">
    ''' The unique key of the aggregate we are storing a projection snapshot for
    ''' </param>
    ''' <param name="snapshotToSave">
    ''' The specific projection snapshot to save
    ''' </param>
    Sub SaveSnapshot(ByVal key As String, ByVal snapshotToSave As IProjectionSnapshot)

End Interface
