Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Commands


Namespace Commands

    ''' <summary>
    ''' The set of all commands that have completed
    ''' </summary>
    <CQRSAzure.EventSourcing.AggregateIdentifier(GetType(CommandAggregateIdentifier))>
    <EventSourcing.IdentityGroup("Commands Completed")>
    Public Class CompletedCommandsIdentityGroup
        Inherits IdentityGroupBase(Of CommandAggregateIdentifier, Guid)

        Public Overrides ReadOnly Property Name As String
            Get
                Return EventSourcing.IdentityGroupAttribute.GetIdentityGroup(GetType(CompletedCommandsIdentityGroup))
            End Get
        End Property


    End Class

    ''' <summary>
    ''' A classifier to use to get the set of completed commands for the completed commands identity group
    ''' </summary>
    Public Class CompletedCommandsIdentityGroupClassifier
        Implements IClassifier(Of CommandAggregateIdentifier, Guid)
        Implements IClassifierProjectionHandler(Of CommandStatusProjection)

        ''' <summary>
        ''' This classifier comes from a projection
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ClassifierDataSource As IClassifier.ClassifierDataSourceType Implements IClassifier.ClassifierDataSource
            Get
                Return IClassifier.ClassifierDataSourceType.Projection
            End Get
        End Property

        Public ReadOnly Property SupportsSnapshots As Boolean Implements IClassifier.SupportsSnapshots
            Get
                Return False
            End Get
        End Property

#Region "Projection processing"
        Public Function EvaluateProjection(Of TProjection As IProjection(Of CommandAggregateIdentifier, Guid))(projection As TProjection) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifier(Of CommandAggregateIdentifier, Guid).EvaluateProjection


            If (projection.GetType() Is GetType(CommandStatusProjection)) Then
                EvaluateCommandStatusProjection(TryCast(projection, CommandStatusProjection))
            End If

            'This projection type is not handled
            Return IClassifierDataSourceHandler.EvaluationResult.Unchanged

        End Function


        Public Function EvaluateCommandStatusProjection(projection As CommandStatusProjection) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifierProjectionHandler(Of CommandStatusProjection).EvaluateProjection

            If (projection.IsComplete) Then
                Return IClassifierDataSourceHandler.EvaluationResult.Include
            Else
                Return IClassifierDataSourceHandler.EvaluationResult.Exclude
            End If

        End Function
#End Region


#Region "Snapshot processing"
        Public Sub LoadFromSnapshot(Of TClassifier As IClassifier) _
            (latestSnapshot As IClassifierSnapshot(Of CommandAggregateIdentifier, Guid, TClassifier)) Implements IClassifier(Of CommandAggregateIdentifier, Guid).LoadFromSnapshot
            Throw New NotImplementedException()
        End Sub

        Public Function ToSnapshot(Of TClassifier As IClassifier)() _
            As IClassifierSnapshot(Of CommandAggregateIdentifier, Guid, TClassifier) Implements IClassifier(Of CommandAggregateIdentifier, Guid).ToSnapshot
            Throw New NotImplementedException()
        End Function
#End Region

#Region "Event Stream Processing"
        Public Function EvaluateEvent(Of TEvent As IEvent(Of CommandAggregateIdentifier)) _
                        (eventToHandle As TEvent) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifier(Of CommandAggregateIdentifier, Guid).EvaluateEvent
            Throw New NotImplementedException()
        End Function


        Public Function HandlesEventType(eventType As Type) As Boolean Implements IClassifier.HandlesEventType
            Return False
        End Function
#End Region

    End Class

End Namespace
