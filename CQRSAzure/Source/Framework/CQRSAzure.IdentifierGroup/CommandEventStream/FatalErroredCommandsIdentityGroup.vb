Imports System
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Commands


Namespace Commands

    ''' <summary>
    ''' The set of all commands that have failed due to a fatal error
    ''' </summary>
    ''' <remarks>
    ''' Because we consider any re-issue of a command after a fatal error to be a new command this means we can use an
    ''' evenyt stream classifier to select commands in fatal error state
    ''' </remarks>
    <CQRSAzure.EventSourcing.AggregateIdentifier(GetType(FatalErroredCommandsIdentityGroupClassifier))>
    <EventSourcing.IdentityGroup("Fatal Error Commands")>
    Public Class FatalErroredCommandsIdentityGroup
        Inherits IdentityGroupBase(Of CommandAggregateIdentifier, Guid)

        Public Overrides ReadOnly Property Name As String
            Get
                Return EventSourcing.IdentityGroupAttribute.GetIdentityGroup(GetType(FatalErroredCommandsIdentityGroup))
            End Get
        End Property

    End Class


    ''' <summary>
    ''' A classifier to use to get the set of fatally failed commands for the completed commands identity group
    ''' </summary>
    Public Class FatalErroredCommandsIdentityGroupClassifier
        Implements IClassifier(Of CommandAggregateIdentifier, Guid)
        Implements IClassifierEventHandler(Of CommandFatalErrorOccuredEvent)


        Public ReadOnly Property ClassifierDataSource As IClassifier.ClassifierDataSourceType Implements IClassifier.ClassifierDataSource
            Get
                Return IClassifier.ClassifierDataSourceType.EventHandler
            End Get
        End Property


        Public ReadOnly Property SupportsSnapshots As Boolean Implements IClassifier.SupportsSnapshots
            Get
                Return False
            End Get
        End Property

#Region "Snapshot processing"
        Public Sub LoadFromSnapshot(Of TClassifier As IClassifier)(latestSnapshot As IClassifierSnapshot(Of CommandAggregateIdentifier, Guid, TClassifier)) Implements IClassifier(Of CommandAggregateIdentifier, Guid).LoadFromSnapshot
            Throw New NotImplementedException()
        End Sub

        Public Function ToSnapshot(Of TClassifier As IClassifier)() As IClassifierSnapshot(Of CommandAggregateIdentifier, Guid, TClassifier) Implements IClassifier(Of CommandAggregateIdentifier, Guid).ToSnapshot
            Throw New NotImplementedException()
        End Function
#End Region

        Public Function EvaluateCommandFatalErrorOccuredEvent(eventToEvaluate As CommandFatalErrorOccuredEvent) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifierEventHandler(Of CommandFatalErrorOccuredEvent).EvaluateEvent

            Return IClassifierDataSourceHandler.EvaluationResult.Include

        End Function

        Public Function EvaluateEvent(Of TEvent As IEvent(Of CommandAggregateIdentifier))(eventToHandle As TEvent) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifier(Of CommandAggregateIdentifier, Guid).EvaluateEvent

            If (eventToHandle.GetType() Is GetType(CommandFatalErrorOccuredEvent)) Then
                Return EvaluateCommandFatalErrorOccuredEvent(TryCast(eventToHandle, CommandFatalErrorOccuredEvent))
            End If

            Return IClassifierDataSourceHandler.EvaluationResult.Unchanged
        End Function

        Public Function HandlesEventType(eventType As Type) As Boolean Implements IClassifier(Of CommandAggregateIdentifier, Guid).HandlesEventType


            If (eventType Is GetType(CommandFatalErrorOccuredEvent)) Then
                Return True
            End If

            Return False

        End Function

#Region "Projection processing"
        Public Function EvaluateProjection(Of TProjection As IProjection(Of CommandAggregateIdentifier, Guid))(projection As TProjection) As IClassifierDataSourceHandler.EvaluationResult Implements IClassifier(Of CommandAggregateIdentifier, Guid).EvaluateProjection
            Throw New NotImplementedException()
        End Function
#End Region

    End Class

End Namespace

