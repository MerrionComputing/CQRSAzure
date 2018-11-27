Imports System
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup


Public Class ClassifierSnapshot(Of TAggregate As IAggregationIdentifier,
                                         TAggregateKey,
                                         TClassifier As IClassifier)
    Inherits ClassifierSnapshot
    Implements IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier)


    Private ReadOnly m_aggregateKey As TAggregateKey
    ''' <summary>
    ''' The unique identifier of the aggregate to which this is a classifier snapshot
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Key As TAggregateKey Implements IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier).Key
        Get
            Return m_aggregateKey
        End Get
    End Property


    Public Sub New(ByVal aggregateKey As TAggregateKey,
                   snapshotEvaluationState As IClassifierDataSourceHandler.EvaluationResult,
                   snapshotAsOfSequenceNumber As UInteger,
                   snapshotAsOfDateTime As Date)
        MyBase.New(snapshotEvaluationState, snapshotAsOfSequenceNumber, snapshotAsOfDateTime)

    End Sub

End Class

Public Class ClassifierSnapshot
    Implements IClassifierSnapshot

    Private ReadOnly m_effectiveDateTime As Date
    Public ReadOnly Property EffectiveDateTime As Date Implements IClassifierSnapshot.EffectiveDateTime
        Get
            Return m_effectiveDateTime
        End Get
    End Property

    Private ReadOnly m_effectiveSequenceNumber As UInteger
    ''' <summary>
    ''' The event stream sequence number as of which the snapshot was taken
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property EffectiveSequenceNumber As UInteger Implements IClassifierSnapshot.EffectiveSequenceNumber
        Get
            Return m_effectiveSequenceNumber
        End Get
    End Property

    Private ReadOnly m_evaluationState As IClassifierDataSourceHandler.EvaluationResult
    ''' <summary>
    ''' The evaluation state of the classifier as-at the time the snapshot was taken
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property EvaluationState As IClassifierDataSourceHandler.EvaluationResult Implements IClassifierSnapshot.EvaluationState
        Get
            Return m_evaluationState
        End Get
    End Property

    Protected Sub New(ByVal snapshotEvaluationState As IClassifierDataSourceHandler.EvaluationResult,
                   ByVal snapshotAsOfSequenceNumber As UInteger,
                   ByVal snapshotAsOfDateTime As Date)

        m_evaluationState = snapshotEvaluationState
        m_effectiveSequenceNumber = snapshotAsOfSequenceNumber
        If (snapshotAsOfDateTime.Year > 1900) Then
            m_effectiveDateTime = snapshotAsOfDateTime
        Else
            'Force the snapshot effective date/time to be as-of-now
            m_effectiveDateTime = DateTime.UtcNow
        End If

    End Sub

End Class

Public Module ClassifierSnapshotFactory

    Public Function Create(Of TAggregate As IAggregationIdentifier,
                              TAggregateKey,
                              TClassifier As IClassifier)(
                                        ByVal key As TAggregateKey,
                                        ByVal snapshotEvaluationState As IClassifierDataSourceHandler.EvaluationResult,
                                        ByVal snapshotAsOfSequenceNumber As UInteger,
                                        ByVal snapshotAsOfDateTime As Date) As IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier)


        Return New ClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier)(key, snapshotEvaluationState, snapshotAsOfSequenceNumber, snapshotAsOfDateTime)

    End Function

End Module