Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.UnitTest

Public Class MockProjectionSnapshot_MockAggregate
    Implements IProjectionSnapshot(Of MockAggregate, String)

    Private ReadOnly m_key As String

    Public ReadOnly Property AsOfDate As Date Implements IProjectionSnapshot(Of MockAggregate, String).AsOfDate
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Sequence As UInteger Implements IProjectionSnapshot(Of MockAggregate, String).Sequence
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Values As IEnumerable(Of IProjectionSnapshotProperty) Implements IProjectionSnapshot(Of MockAggregate, String).Values
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Sub New(ByVal key As String)
        m_key = key
    End Sub

End Class
