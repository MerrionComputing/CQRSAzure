Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup

''' <summary>
''' Base class for a classifier the decides if an aggregate is in or out of a business meaningful identity group
''' </summary>
''' <typeparam name="TAggregate">
''' The type of the aggregate we are grouping
''' </typeparam>
''' <typeparam name="TAggregateKey">
''' The data type that is used to uniquely identify an instance of the aggregate
''' </typeparam>
''' <remarks>
''' The class derived from this will need to implement the IClassifierEventHandler interface for each event it handles
''' </remarks>
Public MustInherit Class ClassifierBase(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Implements IClassifier(Of TAggregate, TAggregateKey)

#Region "private members"
    'Event stream reader to use to evaluate this classifier
    Private ReadOnly m_eventStream As IEventStreamReader(Of TAggregate, TAggregateKey)
#End Region

    Public MustOverride Function EvaluateEvent(Of TEvent As IEvent(Of TAggregate))(eventToHandle As TEvent) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifier(Of TAggregate, TAggregateKey).EvaluateEvent

    Public MustOverride Function EvaluateProjection(Of TProjection As IProjection(Of TAggregate, TAggregateKey))(projection As TProjection) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifier(Of TAggregate, TAggregateKey).EvaluateProjection

    Public MustOverride ReadOnly Property SupportsSnapshots As Boolean Implements IClassifier.SupportsSnapshots

    ''' <summary>
    ''' How does the classifier get the data it uses to perform a classification
    ''' </summary>
    Public MustOverride ReadOnly Property ClassifierDataSource As IClassifier.ClassifierDataSourceType Implements IClassifier.ClassifierDataSource

    Public MustOverride Sub LoadFromSnapshot(Of TClassifier As IClassifier)(latestSnapshot As IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier)) Implements IClassifier(Of TAggregate, TAggregateKey).LoadFromSnapshot

    Public MustOverride Function ToSnapshot(Of TClassifier As IClassifier)() As IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier) Implements IClassifier(Of TAggregate, TAggregateKey).ToSnapshot

    Public MustOverride Function HandlesEventType(eventType As Type) As Boolean Implements IClassifier.HandlesEventType

    ''' <summary>
    ''' Create a new classifier that can use the given event stream reader to decide if an aggregate is in or out of an identity group
    ''' </summary>
    ''' <param name="streamReader">
    ''' The aggregate event stream reader to use to ghet teh events in order to evaluate if an aggregate is in or out of the identity group
    ''' </param>
    Public Sub New(ByVal streamReader As IEventStreamReader(Of TAggregate, TAggregateKey))
        m_eventStream = streamReader

    End Sub


End Class
