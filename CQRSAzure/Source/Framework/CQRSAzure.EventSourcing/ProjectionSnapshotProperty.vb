Imports CQRSAzure.EventSourcing

Public Class ProjectionSnapshotProperty(Of TValue)
    Inherits ProjectionSnapshotProperty
    Implements IProjectionSnapshotProperty(Of TValue)

    Public ReadOnly Property Value As TValue Implements IProjectionSnapshotProperty(Of TValue).Value
        Get
            Return ValueAsObject
        End Get
    End Property

    Protected Friend Sub New(ByVal nameIn As String,
                             ByVal rowNumberIn As Integer,
                             ByVal valueIn As TValue)
        MyBase.New(nameIn,
                   rowNumberIn,
                   valueIn)
    End Sub

    Public Overloads Function Copy() As IProjectionSnapshotProperty(Of TValue) Implements IProjectionSnapshotProperty(Of TValue).Copy

        Return Create(Of TValue)(Name, Value, RowNumber)

    End Function
End Class

Public Class ProjectionSnapshotProperty
    Implements IProjectionSnapshotProperty

    Public Const NO_ROW_NUMBER = 0

    Private ReadOnly m_Name As String
    Public ReadOnly Property Name As String Implements IProjectionSnapshotProperty.Name
        Get
            Return m_Name
        End Get
    End Property

    Private ReadOnly m_RowNumber As Integer
    Public ReadOnly Property RowNumber As Integer Implements IProjectionSnapshotProperty.RowNumber
        Get
            Return m_RowNumber
        End Get
    End Property

    Private m_ValueAsObject As Object
    Public ReadOnly Property ValueAsObject As Object Implements IProjectionSnapshotProperty.Value
        Get
            Return m_ValueAsObject
        End Get
    End Property

    Protected Friend Sub New(ByVal nameIn As String,
                             Optional ByVal rowNumberIn As Integer = NO_ROW_NUMBER,
                             Optional ByVal valueIn As Object = Nothing)


        m_Name = nameIn
        If (rowNumberIn >= 0) Then
            m_RowNumber = rowNumberIn
        Else
            m_RowNumber = NO_ROW_NUMBER
        End If
        If (valueIn IsNot Nothing) Then
#Region "Tracing"
            EventSourcing.LogVerboseInfo("Projection snapshot property " & nameIn & "(" & rowNumberIn.ToString() & ") set to " & valueIn.ToString())
#End Region
            m_ValueAsObject = valueIn
        Else
#Region "Tracing"
            EventSourcing.LogVerboseInfo("Projection snapshot property " & nameIn & "(" & rowNumberIn.ToString() & ") set to [Null]")
#End Region
        End If

    End Sub

    Public Shared Function Create(Of Tvalue)(ByVal nameIn As String,
                                             ByVal valueIn As Tvalue,
                                             Optional ByVal rowNumberIn As Integer = NO_ROW_NUMBER
                            ) As ProjectionSnapshotProperty(Of Tvalue)

        Return New ProjectionSnapshotProperty(Of Tvalue)(nameIn, rowNumberIn, valueIn)

    End Function

    Public Sub UpdateValue(newValue As Object) Implements IProjectionSnapshotProperty.UpdateValue
        If (newValue IsNot Nothing) Then
            m_ValueAsObject = newValue
        Else
            m_ValueAsObject = Nothing
        End If
    End Sub

    Public Function Copy() As IProjectionSnapshotProperty Implements IProjectionSnapshotProperty.Copy

        Return Create(m_Name, m_ValueAsObject, m_RowNumber)

    End Function
End Class