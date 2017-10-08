Imports CQRSAzure.EventSourcing

Public MustInherit Class ProjectionBase(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Inherits ProjectionBase
    Implements IProjection(Of TAggregate, TAggregateKey)

    Public MustOverride Sub HandleEvent(Of TEvent As IEvent)(eventToHandle As TEvent) Implements IProjection(Of TAggregate, TAggregateKey).HandleEvent

    ''' <summary>
    ''' Load the state of this projection from a saved snapshot
    ''' </summary>
    ''' <param name="snapshotToLoad">
    ''' The snapshot to load the projection state from
    ''' </param>
    Public Overridable Sub LoadFromSnapshot(snapshotToLoad As IProjectionSnapshot(Of TAggregate, TAggregateKey)) Implements IProjection(Of TAggregate, TAggregateKey).LoadFromSnapshot

        If (snapshotToLoad IsNot Nothing) Then
#Region "Tracing"
            EventSourcing.LogInfo("Loading projection snapshot from sequence " & snapshotToLoad.Sequence.ToString())
#End Region
            MyBase.SetCurrentSequenceNumber(snapshotToLoad.Sequence)
            If (snapshotToLoad.AsOfDate > m_CurrentAsOfDate) Then
                MyBase.SetCurrentAsOfDate(snapshotToLoad.AsOfDate)
            End If
            'clear any existing values
            m_currentValues.Clear()
            For Each value In snapshotToLoad.Values
#Region "Tracing"
                If (value.Value IsNot Nothing) Then
                    EventSourcing.LogVerboseInfo("Setting " & value.Name & " to " & value.Value.ToString())
                Else
                    EventSourcing.LogVerboseInfo("Setting " & value.Name & " to [Null]")
                End If
#End Region
                MyBase.AddOrUpdateValue(value.Name, value.RowNumber, value.Value)
            Next
        Else
#Region "Tracing"
            EventSourcing.LogError("Attempt to load snapshot but no snapshot passed in")
#End Region
        End If
    End Sub

    Public Sub OnEventRead(sequenceNumber As UInteger, Optional ByVal asOfDate As Nullable(Of Date) = Nothing) Implements IProjection(Of TAggregate, TAggregateKey).OnEventRead

#Region "Tracing"
        EventSourcing.LogVerboseInfo("Event read in to projection - sequence number " & sequenceNumber.ToString())
#End Region

        MyBase.SetCurrentSequenceNumber(sequenceNumber)
        If (asOfDate.HasValue) Then
            MyBase.SetCurrentAsOfDate(asOfDate.Value)
        End If
    End Sub

    ''' <summary>
    ''' Turn the current state of this projection to a snapshot
    ''' </summary>
    Public Overridable Function ToSnapshot() As IProjectionSnapshot(Of TAggregate, TAggregateKey) Implements IProjection(Of TAggregate, TAggregateKey).ToSnapshot

#Region "Tracing"
        EventSourcing.LogVerboseInfo("Creating projection snapshot at " & CurrentSequenceNumber.ToString())
#End Region

        Return ProjectionSnapshot.Create(Of TAggregate, TAggregateKey)(Me)

    End Function

End Class

''' <summary>
''' Base class providing common functionality for any projection
''' </summary>
Public MustInherit Class ProjectionBase
    Implements IProjection

    Protected m_CurrentAsOfDate As Date
    Public Overridable ReadOnly Property CurrentAsOfDate As Date Implements IProjection.CurrentAsOfDate
        Get
            Return m_CurrentAsOfDate
        End Get
    End Property

    Friend Sub SetCurrentAsOfDate(ByVal asOfDate As Date)
        If asOfDate > m_CurrentAsOfDate Then
            m_CurrentAsOfDate = asOfDate
        End If
    End Sub

    Protected m_CurrentSequenceNumber As UInteger
    Public Overridable ReadOnly Property CurrentSequenceNumber As UInteger Implements IProjection.CurrentSequenceNumber
        Get
            Return m_CurrentSequenceNumber
        End Get
    End Property

    Friend Sub SetCurrentSequenceNumber(ByVal currentSequence As UInteger)
        m_CurrentSequenceNumber = currentSequence
    End Sub


    Protected m_currentValues As New List(Of IProjectionSnapshotProperty)
    Public Overridable ReadOnly Property CurrentValues As IEnumerable(Of IProjectionSnapshotProperty) Implements IProjection.CurrentValues
        Get
            Return m_currentValues.AsEnumerable()
        End Get
    End Property

    Protected Sub AddOrUpdateValue(Of TValue)(ByVal propertyName As String, ByVal rowNumber As Integer, ByVal value As TValue)

        If (m_currentValues Is Nothing) Then
            m_currentValues = New List(Of IProjectionSnapshotProperty)
        End If

        If (m_currentValues.Exists(Function(ByVal t As IProjectionSnapshotProperty) As Boolean
                                       Return t.Name = propertyName AndAlso t.RowNumber = rowNumber
                                   End Function)) Then
            'Update the value of that snapshot item
            m_currentValues.First(Function(ByVal t As IProjectionSnapshotProperty) As Boolean
                                      Return t.Name = propertyName AndAlso t.RowNumber = rowNumber
                                  End Function).UpdateValue(value)
        Else
            'Add a new snapshot item
            m_currentValues.Add(ProjectionSnapshotProperty.Create(Of TValue)(propertyName, value, rowNumber))
        End If

    End Sub


    ''' <summary>
    ''' Create a property for use in the projection processing
    ''' </summary>
    ''' <typeparam name="TValue">
    ''' The underlying data type to use to store the property
    ''' </typeparam>
    ''' <param name="propertyName">
    ''' The unique name of the property
    ''' </param>
    Protected Sub CreateProperty(Of TValue)(ByVal propertyName As String)

#Region "Tracing"
        EventSourcing.LogVerboseInfo("Creating projection property " & propertyName & " of type " & GetType(TValue).Name)
#End Region

        If (m_currentValues Is Nothing) Then
            m_currentValues = New List(Of IProjectionSnapshotProperty)
        End If
        AddOrUpdateValue(Of TValue)(propertyName, ProjectionSnapshotProperty.NO_ROW_NUMBER, Nothing)

    End Sub

    Protected Function GetPropertyValue(Of TValue)(ByVal propertyName As String, Optional ByVal rowNumber As Integer = ProjectionSnapshotProperty.NO_ROW_NUMBER) As TValue

        If (m_currentValues Is Nothing) Then
            Return Nothing
        End If

        Return m_currentValues.FirstOrDefault(Function(ByVal t As IProjectionSnapshotProperty) As Boolean
                                                  Return t.Name = propertyName AndAlso t.RowNumber = rowNumber
                                              End Function).Value

    End Function


    Public MustOverride ReadOnly Property SupportsSnapshots As Boolean Implements IProjection.SupportsSnapshots

    Public MustOverride Function HandlesEventType(eventType As Type) As Boolean Implements IProjection.HandlesEventType

    Public Sub MarkEventHandled(handledEventSequenceNumber As UInteger) Implements IProjection.MarkEventHandled

        If handledEventSequenceNumber > m_CurrentSequenceNumber Then
#Region "Tracing"
            EventSourcing.LogVerboseInfo("Projection handled event with sequence number " & handledEventSequenceNumber.ToString())
#End Region
            m_CurrentSequenceNumber = handledEventSequenceNumber
        Else
#Region "Tracing"
            EventSourcing.LogError("Out of sequence event has occured - " & handledEventSequenceNumber.ToString() & " is before " & m_CurrentSequenceNumber.ToString())
#End Region
            'TODO - notify that an out of sequence event has occured ?
        End If

    End Sub
End Class
