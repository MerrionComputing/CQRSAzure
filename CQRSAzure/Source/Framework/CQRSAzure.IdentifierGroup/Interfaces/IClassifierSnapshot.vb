Imports System
Imports CQRSAzure.EventSourcing

''' <summary>
''' A snapshot of the state of a particular classifier for an aggregate as at a given point in time
''' </summary>
''' <typeparam name="TAggregate">
''' The data type that has the event stream being evaluated
''' </typeparam>
''' <typeparam name="TAggregateKey">
''' The data type by which unique instances of the aggregate are identified
''' </typeparam>
''' <typeparam name="TClassifier">
''' The classifier function being used to classify the unique instance iof the aggregate
''' </typeparam>
Public Interface IClassifierSnapshot(Of TAggregate As IAggregationIdentifier,
                                         TAggregateKey,
                                         TClassifier As IClassifier)
    Inherits IClassifierSnapshot

    ''' <summary>
    ''' The unique identifier by which the aggregate identifier is known
    ''' </summary>
    ReadOnly Property Key As TAggregateKey


End Interface


Public Interface IClassifierSnapshotUntyped
    Inherits IClassifierSnapshot

    ''' <summary>
    ''' The unique identifier by which the aggregate identifier is known
    ''' </summary>
    ReadOnly Property Key As String

End Interface

Public Interface IClassifierSnapshot

    ''' <summary>
    ''' The effective date/time of the classifier snapshot
    ''' </summary>
    ReadOnly Property EffectiveDateTime As DateTime

    ''' <summary>
    ''' The sequence number of the last event included in the classification of which this was the 
    ''' resulting snapshot state
    ''' </summary>
    ReadOnly Property EffectiveSequenceNumber As UInteger

    ''' <summary>
    ''' The classifier evaluation state as at the point in time that the snapshot was taken
    ''' </summary>
    ReadOnly Property EvaluationState As IClassifierDataSourceHandler.EvaluationResult

End Interface
