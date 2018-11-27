Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup
Imports CQRSAzure.IdentifierGroup.UnitTest
Imports Newtonsoft.Json.Linq

Public Class MockClassifierOddNumber
    Implements IClassifier(Of MockAggregate, String)
    Implements IClassifierEventHandler(Of MockEventTypeOne)
    Implements IClassifierUntyped

    Private m_currentlyOdd As Boolean = False

    Private m_currentSequence As UInteger
    Private m_currentAsOfdate As DateTime

    Public ReadOnly Property SupportsSnapshots As Boolean Implements IClassifier.SupportsSnapshots
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property ClassifierDataSource As IClassifier.ClassifierDataSourceType Implements IClassifier.ClassifierDataSource
        Get
            Return IClassifier.ClassifierDataSourceType.EventHandler
        End Get
    End Property

    Public Function EvaluateMockEventTypeOne(eventToEvaluate As MockEventTypeOne) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifierEventHandler(Of MockEventTypeOne).EvaluateEvent

        If (eventToEvaluate.EventOneIntegerProperty Mod 2 = 1) Then
            m_currentlyOdd = Not m_currentlyOdd
        End If

        Return GetCurrentState()

    End Function

    Public Function EvaluateEvent(Of TEvent As IEvent(Of MockAggregate))(eventToHandle As TEvent) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifier(Of MockAggregate, String).EvaluateEvent

        If (eventToHandle.GetType() = GetType(MockEventTypeOne)) Then
            Return EvaluateMockEventTypeOne(Convert.ChangeType(eventToHandle, GetType(MockEventTypeOne)))
        Else
            Return IClassifierDataSourceHandler.EvaluationResult.Unchanged
        End If

    End Function

    Public Function HandlesEventType(eventType As Type) As Boolean Implements IClassifier(Of MockAggregate, String).HandlesEventType

        If (eventType = GetType(MockEventTypeOne)) Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function GetCurrentState() As IClassifierDataSourceHandler.EvaluationResult

        If (m_currentlyOdd) Then
            Return IClassifierDataSourceHandler.EvaluationResult.Include
        Else
            Return IClassifierDataSourceHandler.EvaluationResult.Exclude
        End If

    End Function

    Public Sub LoadFromSnapshot(Of TClassifier As IClassifier)(latestSnapshot As IClassifierSnapshot(Of MockAggregate, String, TClassifier)) Implements IClassifier(Of MockAggregate, String).LoadFromSnapshot

        'Use the current status to derive if we are currently odd or not...
        If (latestSnapshot.EvaluationState = IClassifierDataSourceHandler.EvaluationResult.Include) Then
            m_currentlyOdd = True
        Else
            m_currentlyOdd = False
        End If

    End Sub

    Public Function ToSnapshot(Of TClassifier As IClassifier)() As IClassifierSnapshot(Of MockAggregate, String, TClassifier) Implements IClassifier(Of MockAggregate, String).ToSnapshot

        Return ClassifierSnapshotFactory.Create(Of MockAggregate, String, MockClassifierOddNumber)(m_key, GetCurrentState(), m_currentSequence, m_currentAsOfdate)

    End Function

    Private ReadOnly m_key As String
    Public Sub New(ByVal key As String)
        m_key = key
    End Sub

    Public Sub New()

    End Sub


    Public Function EvaluateProjection(Of TProjection As IProjection(Of MockAggregate, String))(projection As TProjection) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifier(Of MockAggregate, String).EvaluateProjection
        Throw New NotImplementedException("The data for this projection comes directly from the event stream")
    End Function

    Private Function IClassifierUntyped_EvaluateEvent(ByVal eventClassName As String,
                                                      eventToHandle As Newtonsoft.Json.Linq.JObject) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifierUntyped.EvaluateEvent

        If (eventToHandle IsNot Nothing) Then
            If (eventClassName = GetType(MockEventTypeOne).FullName) Then
                Return EvaluateMockEventTypeOne(eventToHandle.ToObject(Of MockEventTypeOne))
            End If
        End If

        Return IClassifierDataSourceHandler.EvaluationResult.Unchanged
    End Function

    Private Function IClassifierUntyped_EvaluateProjection(Of TProjection As IProjectionUntyped)(projection As TProjection) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifierUntyped.EvaluateProjection
        Throw New NotImplementedException("The data for this projection comes directly from the event stream")
    End Function

    Public Sub LoadFromSnapshot(latestSnapshot As IClassifierSnapshotUntyped) Implements IClassifierUntyped.LoadFromSnapshot
        Throw New NotImplementedException()
    End Sub

    Public Function ToSnapshot() As IClassifierSnapshotUntyped Implements IClassifierUntyped.ToSnapshot
        Throw New NotImplementedException()
    End Function

    Public Function HandlesEventType(eventTypeName As String) As Boolean Implements IClassifierUntyped.HandlesEventType

        If (eventTypeName = GetType(MockEventTypeOne).FullName) Then
            Return True
        Else
            Return False
        End If

    End Function
End Class


''' <summary>
''' A classifier that runs off a projection
''' </summary>
Public Class MockClassifierProjectionBased
    Implements IClassifierUntyped

    Public ReadOnly Property SupportsSnapshots As Boolean Implements IClassifier.SupportsSnapshots
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property ClassifierDataSource As IClassifier.ClassifierDataSourceType Implements IClassifier.ClassifierDataSource
        Get
            Return IClassifier.ClassifierDataSourceType.Projection
        End Get
    End Property

    Public Sub LoadFromSnapshot(latestSnapshot As IClassifierSnapshotUntyped) Implements IClassifierUntyped.LoadFromSnapshot
        Throw New NotImplementedException()
    End Sub

    Public Function HandlesEventType(eventTypeName As String) As Boolean Implements IClassifierUntyped.HandlesEventType
        Throw New NotImplementedException()
    End Function

    Public Function EvaluateEvent(eventClassName As String, eventToHandle As JObject) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifierUntyped.EvaluateEvent
        Throw New NotImplementedException()
    End Function

    Public Function EvaluateProjection(Of TProjection As IProjectionUntyped)(projection As TProjection) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifierUntyped.EvaluateProjection

        Dim prj As MockClassifierProjectionInstance = Convert.ChangeType(projection, GetType(MockClassifierProjectionInstance))
        If (prj IsNot Nothing) Then
            If (prj.WasLastValueOdd) Then
                Return IClassifierDataSourceHandler.EvaluationResult.Exclude
            End If
        End If

        Return IClassifierDataSourceHandler.EvaluationResult.Unchanged

    End Function

    Public Function ToSnapshot() As IClassifierSnapshotUntyped Implements IClassifierUntyped.ToSnapshot
        Throw New NotImplementedException()
    End Function
End Class

''' <summary>
''' The projection that is used in the mock classifier projection based
''' </summary>
Public Class MockClassifierProjectionInstance
    Inherits ProjectionBaseUntyped
    Implements IHandleEvent(Of MockEventTypeOne)
    Implements IProjectionUntyped


    Private lastValue As Integer

    Public Function WasLastValueOdd() As Boolean
        If ((lastValue Mod 2) = 0) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Overrides ReadOnly Property SupportsSnapshots As Boolean
        Get
            Return False
        End Get
    End Property



    Public Overrides Function HandlesEventType(eventType As Type) As Boolean

        Return HandlesEventTypeByName(eventType.FullName)

    End Function

    Public Overrides Sub HandleEvent(Of TEvent As IEvent)(eventToHandle As TEvent)


        Select Case eventToHandle.GetType()
            Case GetType(MockEventTypeOne)
                HandleMockEventTypeOne(Convert.ChangeType(eventToHandle, GetType(MockEventTypeOne)))
        End Select


    End Sub

    Public Overrides Function HandlesEventTypeByName(eventTypeFullName As String) As Boolean

        If (eventTypeFullName = GetType(MockEventTypeOne).FullName) Then
            Return True
        End If

        Return False
    End Function

    Public Overrides Sub HandleEventJSon(eventFullName As String,
                                     eventToHandle As JObject)

        If (eventFullName = GetType(MockEventTypeOne).FullName) Then
            HandleMockEventTypeOne(eventToHandle.ToObject(Of MockEventTypeOne))
        End If

    End Sub

    Public Shadows Sub HandleMockEventTypeOne(eventHandled As MockEventTypeOne) Implements IHandleEvent(Of MockEventTypeOne).HandleEvent

        If (eventHandled IsNot Nothing) Then
            lastValue = eventHandled.EventOneIntegerProperty
        End If

    End Sub
End Class