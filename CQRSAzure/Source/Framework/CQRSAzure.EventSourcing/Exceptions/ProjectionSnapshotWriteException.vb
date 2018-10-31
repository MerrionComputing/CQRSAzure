Imports System.Security.Permissions
Imports System.Runtime.Serialization


''' <summary>
''' An exception has occured writing to a projection snapshot
''' </summary>
<Serializable()>
Public Class ProjectionSnapshotWriteException
    Inherits ProjectionSnapshotExceptionBase

    Public Sub New(AggregateNameIn As String,
                   AggregateKeyIn As String,
                   ProjectionNameIn As String,
                   Optional MessageIn As String = "",
                   Optional innerExceptionIn As Exception = Nothing)
        MyBase.New(AggregateNameIn, AggregateKeyIn, ProjectionNameIn, MessageIn, innerExceptionIn)
    End Sub

End Class
