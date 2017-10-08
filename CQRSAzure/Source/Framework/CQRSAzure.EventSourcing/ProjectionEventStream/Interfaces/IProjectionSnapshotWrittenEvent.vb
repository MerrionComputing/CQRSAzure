Namespace Projections

    ''' <summary>
    ''' A snapshot of the state of this projection as at a particular event sequence was taken
    ''' </summary>
    Public Interface IProjectionSnapshotWrittenEvent

        ''' <summary>
        ''' The sequence number as of which the snapshot was written
        ''' </summary>
        ReadOnly Property AsOfSequence As Integer

        ''' <summary>
        ''' What technology was used to persiste the snapshot
        ''' </summary>
        ReadOnly Property WriterType As String

        ''' <summary>
        ''' The effective date of the snapshot
        ''' </summary>
        ReadOnly Property AsOfDate As Nullable(Of DateTime)

        ''' <summary>
        ''' Where the snapshot data are stored
        ''' </summary>
        ''' <remarks>
        ''' This could be an URL or the raw results themselves 
        ''' </remarks>
        ReadOnly Property SnapshotLocation As String

    End Interface
End Namespace
