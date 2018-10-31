Imports CQRSAzure.EventSourcing
Imports Newtonsoft.Json.Linq
Imports Newtonsoft
Imports System.Linq.Expressions

Public MustInherit Class ProjectionBase(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Inherits ProjectionBase
    Implements IProjection(Of TAggregate, TAggregateKey)


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

Public MustInherit Class ProjectionBaseUntyped
    Inherits ProjectionBase
    Implements IProjectionUntyped

    Public MustOverride Sub HandleEventJSon(eventFullName As String, eventToHandle As JObject) Implements IProjectionUntyped.HandleEventJSon

    Public MustOverride Function HandlesEventTypeByName(eventTypeFullName As String) As Boolean Implements IProjectionUntyped.HandlesEventTypeByName

    ''' <summary>
    ''' Load the state of this projection from a saved snapshot
    ''' </summary>
    ''' <param name="snapshotToLoad">
    ''' The snapshot to load the projection state from
    ''' </param>
    Public Overridable Sub LoadFromSnapshot(snapshotToLoad As IProjectionSnapshot) Implements IProjectionUntyped.LoadFromSnapshot

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





    ''' <summary>
    ''' Turn the current state of this projection to a snapshot
    ''' </summary>
    Public Overridable Function ToSnapshot() As IProjectionSnapshot Implements IProjectionUntyped.ToSnapshot

#Region "Tracing"
        EventSourcing.LogVerboseInfo("Creating projection snapshot at " & CurrentSequenceNumber.ToString())
#End Region

        Return ProjectionSnapshot.Create(Me)

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
    Public Overridable ReadOnly Property CurrentValues As IEnumerable(Of ProjectionSnapshotProperty) Implements IProjection.CurrentValues
        Get
            Return m_currentValues.Cast(Of ProjectionSnapshotProperty)
        End Get
    End Property

    Protected Sub AddOrUpdateValue(Of TValue)(ByVal propertyName As String, ByVal rowNumber As Integer, ByVal value As TValue)

        InitValues()

        If (m_currentValues.Exists(Function(ByVal t As IProjectionSnapshotProperty) As Boolean
                                       Return t.Name = propertyName AndAlso t.RowNumber = rowNumber
                                   End Function)) Then
            'Update the value of that snapshot item
            Dim currentValue As IProjectionSnapshotProperty = m_currentValues.First(Function(ByVal t As IProjectionSnapshotProperty) As Boolean
                                                                                        Return t.Name = propertyName AndAlso t.RowNumber = rowNumber
                                                                                    End Function)

            'store the old and new value
            Dim oldValue As TValue = currentValue.Value

            'update the value
            currentValue.UpdateValue(value)


            If (oldValue IsNot Nothing) AndAlso (value IsNot Nothing) Then
                If Not (oldValue.Equals(value)) Then
                    'if the value has changed, note that
                    StateValueChanged(MakePropertyName(propertyName, rowNumber), oldValue, value)
                End If
            Else
                If (value IsNot Nothing) Then
                    StateValueSet(MakePropertyName(propertyName, rowNumber), value)
                ElseIf (oldValue IsNot Nothing) Then
                    StateValueUnset(MakePropertyName(propertyName, rowNumber), oldValue)
                End If
                'If they are both "Nothing" the value is unchanged
            End If

        Else
            'Add a new snapshot item
            m_currentValues.Add(ProjectionSnapshotProperty.Create(Of TValue)(propertyName, value, rowNumber))
            StateValueChanged(MakePropertyName(propertyName, rowNumber), Nothing, value)
        End If

    End Sub

    Public MustOverride Sub HandleEvent(Of TEvent As IEvent)(eventToHandle As TEvent) Implements IProjection.HandleEvent

    Public Sub OnEventRead(sequenceNumber As UInteger, Optional ByVal asOfDate As Nullable(Of Date) = Nothing) Implements IProjection.OnEventRead

#Region "Tracing"
        EventSourcing.LogVerboseInfo("Event read in to projection - sequence number " & sequenceNumber.ToString())
#End Region

        SetCurrentSequenceNumber(sequenceNumber)
        If (asOfDate.HasValue) Then
            SetCurrentAsOfDate(asOfDate.Value)
        End If
    End Sub

    Private Sub InitValues()
        If (m_currentValues Is Nothing) Then
            m_currentValues = New List(Of IProjectionSnapshotProperty)
        End If
    End Sub

    ''' <summary>
    ''' If the property exists, increment it by the value given, otherwise add it with the value given
    ''' </summary>
    ''' <typeparam name="TValue">
    ''' The underlying data type of the property
    ''' </typeparam>
    ''' <param name="propertyName">
    ''' The name of the property to increment
    ''' </param>
    ''' <param name="incrementByValue">
    ''' The amount by which to increment the value
    ''' </param>
    ''' <param name="rowNumber">
    ''' (Optional) The row of data to update the property for by incrfementing it
    ''' </param>
    Protected Sub IncrementValue(Of TValue)(ByVal propertyName As String,
                                            ByVal incrementByValue As TValue,
                                            Optional ByVal rowNumber As Integer = ProjectionSnapshotProperty.NO_ROW_NUMBER)

        InitValues()

        If (m_currentValues.Exists(Function(ByVal t As IProjectionSnapshotProperty) As Boolean
                                       Return t.Name = propertyName AndAlso t.RowNumber = rowNumber
                                   End Function)) Then

            'Update the value of that snapshot item
            Dim currentValue As IProjectionSnapshotProperty = m_currentValues.First(Function(ByVal t As IProjectionSnapshotProperty) As Boolean
                                                                                        Return t.Name = propertyName AndAlso t.RowNumber = rowNumber
                                                                                    End Function)

            'store the old and new value
            Dim oldValue As TValue = currentValue.Value

            'Use the increment function...
            AddOrUpdateValue(Of TValue)(propertyName,
                                        rowNumber,
                                        IncrementAssistant.Increment(oldValue, incrementByValue))

        Else
            AddOrUpdateValue(Of TValue)(propertyName,
                                        rowNumber,
                                        incrementByValue)
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

        If (m_currentValues.Count = 0) Then
            Return Nothing
        End If

        Return CType(m_currentValues.FirstOrDefault(Function(ByVal t As IProjectionSnapshotProperty) As Boolean
                                                        Return t.Name = propertyName AndAlso t.RowNumber = rowNumber
                                                    End Function).Value, TValue)

    End Function


    Public MustOverride ReadOnly Property SupportsSnapshots As Boolean Implements IProjection.SupportsSnapshots

#Region "State event change tracking"
    <Newtonsoft.Json.JsonIgnore()>
    Private m_StateChanges As Dictionary(Of String, StateChange) = New Dictionary(Of String, StateChange)
    Public ReadOnly Property StateChanges As Dictionary(Of String, StateChange) Implements IStateChangeTracking.StateChanges
        Get
            Return m_StateChanges
        End Get
    End Property

    Private Sub StateValueChanged(Of TValue)(propertyName As String, oldValue As TValue, value As TValue)

        If (m_StateChanges.ContainsKey(propertyName)) Then
            'update the new value of the state change..
            m_StateChanges(propertyName) = StateChange.Create(oldValue, value)
        Else
            'add the new state change
            m_StateChanges.Add(propertyName, StateChange.Create(oldValue, value))
        End If

    End Sub

    Private Sub StateValueUnset(Of TValue)(propertyName As String, oldValue As TValue)

        StateValueChanged(Of TValue)(propertyName, oldValue, Nothing)

    End Sub

    Private Sub StateValueSet(Of TValue)(propertyName As String, value As TValue)

        StateValueChanged(Of TValue)(propertyName, Nothing, value)

    End Sub

    ''' <summary>
    ''' Have any of the values of this projection changed
    ''' </summary>
    Public Function ProjectionValuesChanged() As Boolean

        If (m_StateChanges IsNot Nothing) Then
            Return (m_StateChanges.Count > 0)
        Else
            Return False
        End If

    End Function

    Private Function MakePropertyName(propertyName As String, rowNumber As Integer) As String

        If (rowNumber = ProjectionSnapshotProperty.NO_ROW_NUMBER) Then
            Return propertyName
        Else
            Return propertyName & "(" & rowNumber.ToString() & ")"
        End If

    End Function

#End Region

    Public MustOverride Function HandlesEventType(eventType As Type) As Boolean Implements IProjection.HandlesEventType

    Public Sub MarkEventHandled(handledEventSequenceNumber As UInteger) Implements IProjection.MarkEventHandled

        If handledEventSequenceNumber >= m_CurrentSequenceNumber Then
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

Public Class IncrementAssistant

    Public Overloads Shared Function Increment(ByVal source As Integer, ByVal target As Integer) As Integer
        Return source + target
    End Function

    Public Overloads Shared Function Increment(ByVal source As Double, ByVal target As Double) As Double
        Return source + target
    End Function

    Public Overloads Shared Function Increment(ByVal source As Decimal, ByVal target As Decimal) As Decimal
        Return source + target
    End Function

    Public Overloads Shared Function Increment(ByVal source As String, ByVal target As String) As String
        Return source & target
    End Function

    Public Overloads Shared Function Increment(Of TValue)(ByVal source As TValue, ByVal target As TValue) As TValue

        '// declare the parameters
        Dim paramSource As ParameterExpression = Expression.Parameter(GetType(TValue), NameOf(source))
        Dim paramTarget As ParameterExpression = Expression.Parameter(GetType(TValue), NameOf(target))
        '// add the parameters together
        Dim body As BinaryExpression = Expression.Add(paramSource, paramTarget)
        '// compile it
        Dim add As Func(Of TValue, TValue, TValue) = Expression.Lambda(Of Func(Of TValue, TValue, TValue))(body, paramSource, paramTarget).Compile()
        '// call it
        Return add(source, target)

    End Function

End Class