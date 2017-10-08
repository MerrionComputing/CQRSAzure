Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup
Imports CQRSAzure.IdentifierGroup.UnitTest

Public Class MockClassifierOddNumber
    Implements IClassifier(Of MockAggregate, String)
    Implements IClassifierEventHandler(Of MockEventTypeOne)

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
            Return EvaluateMockEventTypeOne(CTypeDynamic(Of MockEventTypeOne)(eventToHandle))
        Else
            Return IClassifierDataSourceHandler.EvaluationResult.Unchanged
        End If

    End Function

    Public Function HandlesEventType(eventType As Type) As Boolean Implements IClassifier.HandlesEventType

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

End Class
