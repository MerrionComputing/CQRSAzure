Imports System.Runtime.Serialization

Namespace Queries

    <Serializable()>
    Public NotInheritable Class QueryTransientFaultOccuredEvent
        Inherits QueryEventBase
        Implements IQueryTransientFaultOccuredEvent


        Private ReadOnly m_FaultDate As Date?
        Public ReadOnly Property FaultDate As Date? Implements IQueryTransientFaultOccuredEvent.FaultDate
            Get
                Return m_FaultDate
            End Get
        End Property

        Private ReadOnly m_FaultMessage As String
        Public ReadOnly Property FaultMessage As String Implements IQueryTransientFaultOccuredEvent.FaultMessage
            Get
                Return m_FaultMessage
            End Get
        End Property

        Public Overrides ReadOnly Property Version As UInteger
            Get
                Return 1
            End Get
        End Property

        Public Overrides Sub GetObjectData(info As SerializationInfo, context As StreamingContext)

            If Not (info Is Nothing) Then
                info.AddValue(NameOf(FaultDate), FaultDate)
                info.AddValue(NameOf(FaultMessage), FaultMessage)
            End If

        End Sub

        Private Sub New(ByVal FaultDateIn As Date?,
         ByVal FaultMessageIn As String)

            m_FaultDate = FaultDateIn
            m_FaultMessage = FaultMessageIn

        End Sub

        Public Sub New(info As SerializationInfo,
           context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_FaultDate = info.GetDateTime(NameOf(FaultDate))
                m_FaultMessage = info.GetString(NameOf(FaultMessage))
            End If

        End Sub

        Public Shared Function Create(ByVal FaultDateIn As Date?,
         ByVal FaultMessageIn As String) As QueryTransientFaultOccuredEvent

            If (Not FaultDateIn.HasValue) Then
                FaultDateIn = DateTime.UtcNow
            End If

            Return New QueryTransientFaultOccuredEvent(FaultDateIn,
                                                       FaultMessageIn)

        End Function

    End Class
End Namespace
