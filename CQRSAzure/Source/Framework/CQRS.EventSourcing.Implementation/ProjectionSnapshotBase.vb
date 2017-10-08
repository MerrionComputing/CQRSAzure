''' <summary>
''' Base class for all projection snapshots
''' </summary>
''' <typeparam name="TProjection">
''' The type of the projection that is being snapshotted
''' </typeparam>
Public MustInherit Class ProjectionSnapshotBase(Of TProjection As IProjection)

    Public ReadOnly Property ProjectionName As String
        Get
            Dim typeName As String = GetType(TProjection).Name
            Return typeName
        End Get
    End Property

End Class
