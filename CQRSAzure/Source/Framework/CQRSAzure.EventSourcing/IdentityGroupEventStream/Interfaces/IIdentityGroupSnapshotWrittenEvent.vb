Namespace IdentityGroups
    ''' <summary>
    ''' A point-in-time snapshot of the membership of an identity group was taken
    ''' </summary>
    Public Interface IIdentityGroupSnapshotWrittenEvent

        ''' <summary>
        ''' The effective date/time we got the group membership for
        ''' </summary>
        ReadOnly Property AsOfDate As Nullable(Of DateTime)

        ''' <summary>
        ''' Where the request for the identity group members came from
        ''' </summary>
        ReadOnly Property RequestSource As String

        ''' <summary>
        ''' Where the group members data are stored
        ''' </summary>
        ''' <remarks>
        ''' This could be an URL or the raw results themselves 
        ''' </remarks>
        ReadOnly Property SnapshotLocation As String

        ''' <summary>
        ''' What technology was used to persiste the snapshot
        ''' </summary>
        ReadOnly Property WriterType As String


    End Interface
End Namespace
