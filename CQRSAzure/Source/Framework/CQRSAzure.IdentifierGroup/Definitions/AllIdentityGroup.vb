Option Explicit On

Imports System
Imports System.Collections.Generic
Imports CQRSAzure.EventSourcing

''' <summary>
''' An identity group that represents all of the instances of a given aggregate identifier known to the system
''' </summary>
''' <typeparam name="TAggregateIdentifier">
''' The type of aggregate identifier for which we are getting the group of all instances
''' </typeparam>
Public NotInheritable Class AllIdentityGroup(Of TAggregateIdentifier As IAggregationIdentifier, TAggregateKey)
    Inherits IdentityGroupBase(Of TAggregateIdentifier, TAggregateKey)

    Public Overrides ReadOnly Property Classifier As IClassifier(Of TAggregateIdentifier, TAggregateKey)
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    ''' <summary>
    ''' Always use the name "All" for the group name of all aggregate identifiers
    ''' </summary>
    ''' <returns></returns>
    Public Overrides ReadOnly Property Name As String
        Get
            Return GROUPNAME_ALL
        End Get
    End Property


    Public Overrides Function GetMembers(Optional AsOfDate As Date = #1/1/0001 12:00:00 AM#) As IEnumerable(Of TAggregateIdentifier)
        Throw New NotImplementedException()
    End Function

#Region "Constructors"
    Public Sub New()
        Me.New(New AllClassifier(Of TAggregateIdentifier, TAggregateKey)(), Nothing)
    End Sub

    ''' <summary>
    ''' Constructor that takes the classifier to use and parent group
    ''' </summary>
    ''' <param name="classifierToUse">
    ''' The classifier process to use to classify members of the group
    ''' </param>
    ''' <param name="parentGroup">
    ''' The parent group of which all members of this group must be members
    ''' </param>
    Public Sub New(Optional ByVal classifierToUse As IClassifier(Of TAggregateIdentifier, TAggregateKey) = Nothing,
                   Optional ByVal parentGroup As IIdentifierGroup(Of TAggregateIdentifier, TAggregateKey) = Nothing)
        MyBase.New(classifierToUse, parentGroup)
    End Sub
#End Region

End Class

''' <summary>
''' A classifier that always includes the members in an event 
''' </summary>
''' <typeparam name="TAggregateIdentifier">
''' The aggregate type which we are classifying
''' </typeparam>
''' <typeparam name="TAggregateKey">
''' The type by which the aggregate is uniquely identified
''' </typeparam>
Public NotInheritable Class AllClassifier(Of TAggregateIdentifier As IAggregationIdentifier, TAggregateKey)
    Implements IClassifier(Of TAggregateIdentifier, TAggregateKey)

    Public ReadOnly Property ClassifierDataSource As IClassifier.ClassifierDataSourceType Implements IClassifier.ClassifierDataSource
        Get
            Return IClassifier.ClassifierDataSourceType.EventHandler
        End Get
    End Property

    ''' <summary>
    ''' This classifier should support snapshots  
    ''' </summary>
    Public ReadOnly Property SupportsSnapshots As Boolean Implements IClassifier.SupportsSnapshots
        Get
            Return True
        End Get
    End Property

    Public Sub LoadFromSnapshot(Of TClassifier As IClassifier)(latestSnapshot As IClassifierSnapshot(Of TAggregateIdentifier, TAggregateKey, TClassifier)) Implements IClassifier(Of TAggregateIdentifier, TAggregateKey).LoadFromSnapshot
        'No need to do anything...
    End Sub

    ''' <summary>
    ''' Everything is included in the group of "All"
    ''' </summary>
    Public Function EvaluateEvent(Of TEvent As IEvent(Of TAggregateIdentifier))(eventToHandle As TEvent) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifier(Of TAggregateIdentifier, TAggregateKey).EvaluateEvent
        Return IClassifierDataSourceHandler.EvaluationResult.Include
    End Function

    ''' <summary>
    ''' Everything is included in the group of "All"
    ''' </summary>
    Public Function EvaluateProjection(Of TProjection As IProjection(Of TAggregateIdentifier, TAggregateKey))(projection As TProjection) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifier(Of TAggregateIdentifier, TAggregateKey).EvaluateProjection
        Return IClassifierDataSourceHandler.EvaluationResult.Include
    End Function



    Public Function ToSnapshot(Of TClassifier As IClassifier)() As IClassifierSnapshot(Of TAggregateIdentifier, TAggregateKey, TClassifier) Implements IClassifier(Of TAggregateIdentifier, TAggregateKey).ToSnapshot
        Throw New NotImplementedException()
    End Function

    Public Function HandlesEventType(eventType As Type) As Boolean Implements IClassifier(Of TAggregateIdentifier, TAggregateKey).HandlesEventType

        Return True

    End Function
End Class
