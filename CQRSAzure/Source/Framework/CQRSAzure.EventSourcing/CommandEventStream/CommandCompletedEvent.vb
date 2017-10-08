Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Commands

    ''' <summary>
    ''' The command completed successfully
    ''' </summary>
    <Serializable()>
    Public NotInheritable Class CommandCompletedEvent
        Inherits CommandEventBase
        Implements ICommandCompletedEvent
        Implements IEvent(Of CommandAggregateIdentifier)

        Private ReadOnly m_completionDate As Date?
        Public ReadOnly Property CompletionDate As Date? Implements ICommandCompletedEvent.CompletionDate
            Get
                Return m_completionDate
            End Get
        End Property




        Private ReadOnly m_successMessage As String
        Public ReadOnly Property SuccessMessage As String Implements ICommandCompletedEvent.SuccessMessage
            Get
                Return m_successMessage
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
                info.AddValue(NameOf(CompletionDate), CompletionDate)
                info.AddValue(NameOf(SuccessMessage), SuccessMessage)
            End If
        End Sub


        Public Sub New(info As SerializationInfo,
       context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_successMessage = info.GetString(NameOf(SuccessMessage))
                m_completionDate = info.GetDateTime(NameOf(CompletionDate))
            End If

        End Sub

        Private Sub New(ByVal completionDateIn As DateTime,
                        ByVal successMessageIn As String,
                        Optional ByVal contextIn As CommandEventContext = Nothing)
            MyBase.New(contextIn)
            m_completionDate = completionDateIn
            m_successMessage = successMessageIn


        End Sub

        Public Overloads Shared Function Create(ByVal completionDateIn As DateTime,
                        ByVal successMessageIn As String,
                        Optional ByVal contextIn As CommandEventContext = Nothing) As CommandCompletedEvent

            Return New CommandCompletedEvent(completionDateIn, successMessageIn, contextIn)

        End Function

    End Class
End Namespace