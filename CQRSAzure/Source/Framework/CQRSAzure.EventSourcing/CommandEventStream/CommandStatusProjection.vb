Imports System
Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Commands
    ''' <summary>
    ''' A projection to get the status of a specified command
    ''' </summary>
    <DataContract>
    Public NotInheritable Class CommandStatusProjection
        Inherits ProjectionBase(Of CommandAggregateIdentifier, Guid)
        Implements IHandleEvent(Of CommandCreatedEvent)
        Implements IHandleEvent(Of CommandCancelledEvent)
        Implements IHandleEvent(Of CommandCompletedEvent)
        Implements IHandleEvent(Of CommandFatalErrorOccuredEvent)
        Implements IHandleEvent(Of CommandTransientFaultOccuredEvent)
        Implements IHandleEvent(Of CommandRequeuedEvent)
        Implements IHandleEvent(Of CommandStepCompletedEvent)
        Implements IHandleEvent(Of CommandStartedEvent)

        <DataMember(Name:=NameOf(StatusMessage))>
        Private m_statusMessage As String

        ''' <summary>
        ''' The text status of the command based on the most recent event that occured to it
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property StatusMessage As String
            Get
                Return m_statusMessage
            End Get
        End Property

        <DataMember(Name:=NameOf(LastStepComplete))>
        Private m_lastStepComplete As Integer = 0
        ''' <summary>
        ''' The last step in the command that was completed
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property LastStepComplete As Integer
            Get
                Return m_lastStepComplete
            End Get
        End Property

        ''' <summary>
        ''' The status of a command can be stored in a snapshot
        ''' </summary>
        Public Overrides ReadOnly Property SupportsSnapshots As Boolean
            Get
                Return True
            End Get
        End Property

        <DataMember(Name:=NameOf(CommandName), IsRequired:=True)>
        Private m_commandName As String
        Public ReadOnly Property CommandName As String
            Get
                Return m_commandName
            End Get
        End Property




        'Different statuses the command can have
        <DataMember(Name:=NameOf(IsCancelled))>
        Private m_cancelled As Boolean = False
        ''' <summary>
        ''' The command has been cancelled
        ''' </summary>
        Public ReadOnly Property IsCancelled As Boolean
            Get
                Return m_cancelled
            End Get
        End Property

        <DataMember(Name:=NameOf(IsComplete))>
        Private m_complete As Boolean = False
        ''' <summary>
        ''' The command has completed successfully without error
        ''' </summary>
        Public ReadOnly Property IsComplete As Boolean
            Get
                Return m_complete
            End Get
        End Property

        <DataMember(Name:=NameOf(IsFatalError))>
        Private m_fatalError As Boolean = False
        ''' <summary>
        ''' The command ened with a fatal error
        ''' </summary>
        ''' <remarks>
        ''' It probably does not make sense to retry the command automatically
        ''' </remarks>
        Public ReadOnly Property IsFatalError As Boolean
            Get
                Return m_fatalError
            End Get
        End Property

        <DataMember(Name:=NameOf(IsTransientError))>
        Private m_transientError As Boolean = False
        ''' <summary>
        ''' A transient error occured while processing this command
        ''' </summary>
        Public ReadOnly Property IsTransientError As Boolean
            Get
                Return m_transientError
            End Get
        End Property

        'Duration tracking
        <DataMember(Name:=NameOf(CreatedDate))>
        Private m_createdDate As Nullable(Of DateTime)
        Public ReadOnly Property CreatedDate As Nullable(Of DateTime)
            Get
                Return m_createdDate
            End Get
        End Property

        Public Overrides Sub HandleEvent(Of TEvent As IEvent)(eventToHandle As TEvent)

            If (eventToHandle.GetType() Is GetType(CommandCreatedEvent)) Then
                HandleCommandCreatedEvent(TryCast(eventToHandle, CommandCreatedEvent))
            End If

            If (eventToHandle.GetType() Is GetType(CommandCancelledEvent)) Then
                HandleCommandCancelledEvent(TryCast(eventToHandle, CommandCancelledEvent))
            End If

            If (eventToHandle.GetType() Is GetType(CommandCompletedEvent)) Then
                HandleCommandCompletedEvent(TryCast(eventToHandle, CommandCompletedEvent))
            End If

            If (eventToHandle.GetType() Is GetType(CommandFatalErrorOccuredEvent)) Then
                HandleCommandFatalErrorOccuredEvent(TryCast(eventToHandle, CommandFatalErrorOccuredEvent))
            End If

            If (eventToHandle.GetType() Is GetType(CommandRequeuedEvent)) Then
                HandleCommandRequeuedEvent(TryCast(eventToHandle, CommandRequeuedEvent))
            End If

            If (eventToHandle.GetType() Is GetType(CommandStartedEvent)) Then
                HandleCommandStartedEvent(TryCast(eventToHandle, CommandStartedEvent))
            End If

            If (eventToHandle.GetType() Is GetType(CommandStepCompletedEvent)) Then
                HandleCommandStepCompletedEvent(TryCast(eventToHandle, CommandStepCompletedEvent))
            End If

            If (eventToHandle.GetType() Is GetType(CommandTransientFaultOccuredEvent)) Then
                HandleCommandTransientFaultOccuredEvent(TryCast(eventToHandle, CommandTransientFaultOccuredEvent))
            End If

        End Sub



        Public Overrides Function HandlesEventType(eventType As Type) As Boolean

            If (eventType Is GetType(CommandCreatedEvent)) Then
                Return True
            End If

            If (eventType Is GetType(CommandCancelledEvent)) Then
                Return True
            End If

            If (eventType Is GetType(CommandCompletedEvent)) Then
                Return True
            End If

            If (eventType Is GetType(CommandFatalErrorOccuredEvent)) Then
                Return True
            End If

            If (eventType Is GetType(CommandRequeuedEvent)) Then
                Return True
            End If

            If (eventType Is GetType(CommandStartedEvent)) Then
                Return True
            End If

            If (eventType Is GetType(CommandStepCompletedEvent)) Then
                Return True
            End If

            If (eventType Is GetType(CommandTransientFaultOccuredEvent)) Then
                Return True
            End If

            Return False

        End Function

        Public Sub HandleCommandCancelledEvent(eventHandled As CommandCancelledEvent) Implements IHandleEvent(Of CommandCancelledEvent).HandleEvent

            m_statusMessage = eventHandled.Reason
            m_cancelled = True

        End Sub

        Public Sub HandleCommandCompletedEvent(eventHandled As CommandCompletedEvent) Implements IHandleEvent(Of CommandCompletedEvent).HandleEvent

            m_statusMessage = eventHandled.SuccessMessage
            m_complete = True

        End Sub

        Public Sub HandleCommandCreatedEvent(eventHandled As CommandCreatedEvent) Implements IHandleEvent(Of CommandCreatedEvent).HandleEvent

            m_commandName = eventHandled.CommandName
            m_statusMessage = "New " & eventHandled.CommandName & " command created "
            m_createdDate = eventHandled.CreationDate

        End Sub

        Public Sub HandleCommandFatalErrorOccuredEvent(eventHandled As CommandFatalErrorOccuredEvent) Implements IHandleEvent(Of CommandFatalErrorOccuredEvent).HandleEvent

            m_statusMessage = eventHandled.ErrorMessage
            m_fatalError = True

        End Sub

        Public Sub HandleCommandTransientFaultOccuredEvent(eventHandled As CommandTransientFaultOccuredEvent) Implements IHandleEvent(Of CommandTransientFaultOccuredEvent).HandleEvent

            m_statusMessage = eventHandled.FaultMessage
            m_transientError = True

        End Sub

        Public Sub HandleCommandRequeuedEvent(eventHandled As CommandRequeuedEvent) Implements IHandleEvent(Of CommandRequeuedEvent).HandleEvent

            m_transientError = False
            m_statusMessage = "Requeued"

        End Sub

        Public Sub HandleCommandStepCompletedEvent(eventHandled As CommandStepCompletedEvent) Implements IHandleEvent(Of CommandStepCompletedEvent).HandleEvent

            m_statusMessage = eventHandled.StatusMessage
            m_lastStepComplete = eventHandled.StepNumber

        End Sub

        Public Sub HandleCommandStartedEvent(eventHandled As CommandStartedEvent) Implements IHandleEvent(Of CommandStartedEvent).HandleEvent

            m_statusMessage = "Command started"

        End Sub
    End Class
End Namespace