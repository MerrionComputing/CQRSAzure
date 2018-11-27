Imports System
Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Commands

    <Serializable()>
    Public NotInheritable Class CommandTransientFaultOccuredEvent
        Inherits CommandEventBase
        Implements ICommandTransientFaultOccuredEvent
        Implements IEvent(Of CommandAggregateIdentifier)


        ''' <summary>
        ''' The date/time the command step was stopped with a transient error
        ''' </summary>
        Private ReadOnly m_FaultDate As Date?
        Public ReadOnly Property FaultDate As Date? Implements ICommandTransientFaultOccuredEvent.FaultDate
            Get
                Return m_FaultDate
            End Get
        End Property

        Private m_FaultMessage As String
        Public ReadOnly Property FaultMessage As String Implements ICommandTransientFaultOccuredEvent.FaultMessage
            Get
                Return m_FaultMessage
            End Get
        End Property

        Private ReadOnly m_StepNumber As Integer
        Public ReadOnly Property StepNumber As Integer Implements ICommandTransientFaultOccuredEvent.StepNumber
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
                info.AddValue(NameOf(FaultDate), FaultDate)
                info.AddValue(NameOf(FaultMessage), FaultMessage)
                info.AddValue(NameOf(StepNumber), StepNumber)
            End If
        End Sub

        Public Sub New(info As SerializationInfo,
          context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_FaultDate = info.GetDateTime(NameOf(FaultDate))
                m_FaultMessage = info.GetString(NameOf(FaultMessage))
                m_StepNumber = info.GetInt32(NameOf(StepNumber))
            End If

        End Sub

        Private Sub New(ByVal faultDateIn As Nullable(Of Date),
                        ByVal faultMessageIn As String,
                        ByVal stepNumberIn As Integer,
                        Optional ByVal contextIn As CommandEventContext = Nothing)

            MyBase.New(contextIn)
            m_FaultDate = faultDateIn
            m_FaultMessage = faultMessageIn

            m_StepNumber = stepNumberIn



        End Sub

        Public Overloads Shared Function Create(ByVal faultDateIn As Nullable(Of Date),
                        ByVal faultMessageIn As String,
                        ByVal stepNumberIn As Integer,
                        Optional ByVal contextIn As CommandEventContext = Nothing) As CommandTransientFaultOccuredEvent

            Return New CommandTransientFaultOccuredEvent(faultDateIn,
                                                         faultMessageIn,
                                                         stepNumberIn,
                                                         contextIn)

        End Function

    End Class
End Namespace
