Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Commands


    <Serializable()>
    Public Class CommandStepCompletedEvent
        Inherits CommandEventBase
        Implements ICommandStepCompletedEvent
        Implements IEvent(Of CommandAggregateIdentifier)


        Private ReadOnly m_StatusMessage As String
        Public ReadOnly Property StatusMessage As String Implements ICommandStepCompletedEvent.StatusMessage
            Get
                Return m_StatusMessage
            End Get
        End Property

        Private ReadOnly m_StepCompletionDate As Date?
        Public ReadOnly Property StepCompletionDate As Date? Implements ICommandStepCompletedEvent.StepCompletionDate
            Get
                Return m_StepCompletionDate
            End Get
        End Property


        Private ReadOnly m_StepNumber As Integer
        ''' <summary>
        ''' The ordinal number of the step that was completed
        ''' </summary>
        Public ReadOnly Property StepNumber As Integer Implements ICommandStepCompletedEvent.StepNumber
            Get
                Return m_StepNumber
            End Get
        End Property

        Public Overrides ReadOnly Property Version As UInteger Implements IEvent(Of CommandAggregateIdentifier).Version
            Get
                Return 1
            End Get
        End Property

        Public Overrides Sub GetObjectData(info As SerializationInfo, context As StreamingContext)
            If Not (info Is Nothing) Then
                MyBase.GetObjectData(info, context)
                info.AddValue(NameOf(StepCompletionDate), StepCompletionDate)
                info.AddValue(NameOf(StatusMessage), StatusMessage)
                info.AddValue(NameOf(StepNumber), StepNumber)
            End If
        End Sub

        Public Sub New(info As SerializationInfo,
          context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_StepCompletionDate = info.GetDateTime(NameOf(StepCompletionDate))
                m_StatusMessage = info.GetString(NameOf(StatusMessage))
                m_StepNumber = info.GetInt32(NameOf(StepNumber))
            End If

        End Sub

        Private Sub New(ByVal stepCompletionDateIn As Nullable(Of DateTime),
                        ByVal stepNumberIn As Integer,
                        ByVal statusMessageIn As String,
                Optional ByVal contextIn As CommandEventContext = Nothing)
            MyBase.New(contextIn)
            m_StepCompletionDate = stepCompletionDateIn
            m_StepNumber = stepNumberIn
            m_StatusMessage = statusMessageIn


        End Sub

        Public Overloads Shared Function Create(ByVal stepCompletionDateIn As Nullable(Of DateTime),
                        ByVal stepNumberIn As Integer,
                        ByVal statusMessageIn As String,
                Optional ByVal contextIn As CommandEventContext = Nothing) As CommandStepCompletedEvent

            Return New CommandStepCompletedEvent(stepCompletionDateIn,
                                                 stepNumberIn,
                                                 statusMessageIn,
                                                 contextIn)

        End Function

    End Class

End Namespace
