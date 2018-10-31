Imports CQRSAzure.EventSourcing

''' <summary>
''' A stock snapshot object that can be used to populate a projection from a snapshot taken at a point in time
''' </summary>
''' <typeparam name="TAggregate"></typeparam>
''' <typeparam name="TAggregateKey"></typeparam>
Public Class ProjectionSnapshot(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Inherits ProjectionSnapshotUntyped
    Implements IProjectionSnapshot(Of TAggregate, TAggregateKey)






    Protected Friend Sub New(ByVal projectionToSnapshot As IProjection(Of TAggregate, TAggregateKey))
        Me.New(projectionToSnapshot.CurrentSequenceNumber,
               projectionToSnapshot.CurrentAsOfDate,
               projectionToSnapshot.CurrentValues)
    End Sub

    Protected Friend Sub New(ByVal projectionToSnapshot As IProjection)
        Me.New(projectionToSnapshot.CurrentSequenceNumber,
               projectionToSnapshot.CurrentAsOfDate,
               projectionToSnapshot.CurrentValues)
    End Sub

    Protected Friend Sub New(ByVal SequenceNumebrInit As UInteger,
                    ByVal AsOfDateInit As Date,
                    ByRef ValuesInit As IEnumerable(Of IProjectionSnapshotProperty))

        MyBase.New(SequenceNumebrInit,
           AsOfDateInit,
           ValuesInit)


    End Sub

End Class


Public Class ProjectionSnapshotUntyped
    Inherits ProjectionSnapshot
    Implements IProjectionSnapshot

    Private ReadOnly m_AsOfDate As Date
    Public ReadOnly Property AsOfDate As Date Implements IProjectionSnapshot.AsOfDate
        Get
            Return m_AsOfDate
        End Get
    End Property

    Private ReadOnly m_Sequence As UInteger
    Public ReadOnly Property Sequence As UInteger Implements IProjectionSnapshot.Sequence
        Get
            Return m_Sequence
        End Get
    End Property

    Private ReadOnly m_values As New List(Of IProjectionSnapshotProperty)
    Public ReadOnly Property Values As IEnumerable(Of IProjectionSnapshotProperty) Implements IProjectionSnapshot.Values
        Get
            Return m_values.AsEnumerable()
        End Get
    End Property

    Private ReadOnly m_maxRow As Integer
    Public ReadOnly Property RowCount As Integer Implements IProjectionSnapshot.RowCount
        Get
            Return m_maxRow + 1
        End Get
    End Property

    Public Sub AddValue(Of TValue)(ByVal rowNumber As Integer,
                    ByVal fieldName As String,
                    ByVal value As TValue)

#Region "Tracing"
        EventSourcing.LogVerboseInfo("Added snapshot value " & fieldName & "(" & rowNumber.ToString() & ") -" & value.ToString())
#End Region

        m_values.Add(ProjectionSnapshotProperty.Create(Of TValue)(fieldName, value, rowNumber))

    End Sub


    Protected Friend Sub New(ByVal projectionToSnapshot As IProjection)
        Me.New(projectionToSnapshot.CurrentSequenceNumber,
               projectionToSnapshot.CurrentAsOfDate,
               projectionToSnapshot.CurrentValues)
    End Sub

    Protected Friend Sub New(ByVal SequenceNumebrInit As UInteger,
                ByVal AsOfDateInit As Date,
                ByRef ValuesInit As IEnumerable(Of IProjectionSnapshotProperty))

#Region "Tracing"
        EventSourcing.LogVerboseInfo("New snapshot created at sequence number " & SequenceNumebrInit.ToString())
#End Region

        m_Sequence = SequenceNumebrInit
        m_AsOfDate = AsOfDateInit
        If (ValuesInit IsNot Nothing) Then
            For Each value In ValuesInit
                m_values.Add(value.Copy())
                If (value.RowNumber > m_maxRow) Then
                    m_maxRow = value.RowNumber
                End If
            Next
        End If


    End Sub

End Class

Public MustInherit Class ProjectionSnapshot

    Public Shared Function Create(ByVal projectionToSnapshot As IProjectionUntyped) As IProjectionSnapshot

        Return New ProjectionSnapshotUntyped(projectionToSnapshot)

    End Function

    Public Shared Function Create(Of TAggregate As IAggregationIdentifier, TAggregateKey)(ByVal projectionToSnapshot As IProjection(Of TAggregate, TAggregateKey)) As IProjectionSnapshot(Of TAggregate, TAggregateKey)

        Return New ProjectionSnapshot(Of TAggregate, TAggregateKey)(projectionToSnapshot)

    End Function

    Public Shared Function Create(Of TAggregate As IAggregationIdentifier, TAggregateKey)(ByVal projectionToSnapshot As IProjectionSnapshot) As IProjectionSnapshot(Of TAggregate, TAggregateKey)

        Return New ProjectionSnapshot(Of TAggregate, TAggregateKey)(projectionToSnapshot.Sequence,
                                                                    projectionToSnapshot.AsOfDate,
                                                                    projectionToSnapshot.Values)

    End Function

    Public Shared Function Create(Of Taggregate As IAggregationIdentifier, TaggregateKey)(ByVal SequenceNumberInit As UInteger,
                    ByVal AsOfDateInit As Date) As IProjectionSnapshot(Of Taggregate, TaggregateKey)

        Return New ProjectionSnapshot(Of Taggregate, TaggregateKey)(SequenceNumberInit,
                                                                    AsOfDateInit,
                                                                    Nothing)
    End Function

End Class