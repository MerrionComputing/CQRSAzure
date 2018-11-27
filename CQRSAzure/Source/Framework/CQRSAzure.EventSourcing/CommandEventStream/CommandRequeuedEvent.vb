Imports System
Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Commands

    <Serializable()>
    Public NotInheritable Class CommandRequeuedEvent
        Inherits CommandEventBase
        Implements ICommandRequeuedEvent
        Implements IEvent(Of CommandAggregateIdentifier)


        Private ReadOnly m_requeueDate As Date?
        Public ReadOnly Property RequeueDate As Date? Implements ICommandRequeuedEvent.RequeueDate
            Get
                Return m_requeueDate
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
                info.AddValue(NameOf(RequeueDate), RequeueDate)
            End If

        End Sub

        Public Sub New(info As SerializationInfo,
                        context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_requeueDate = info.GetDateTime(NameOf(RequeueDate))
            End If

        End Sub

        Private Sub New(ByVal requeueDateIn As DateTime,
                        Optional ByVal contextIn As CommandEventContext = Nothing)

            MyBase.New(contextIn)
            m_requeueDate = requeueDateIn

        End Sub

        Public Overloads Shared Function Create(ByVal requeueDateIn As DateTime,
                        Optional ByVal contextIn As CommandEventContext = Nothing) As CommandRequeuedEvent

            Return New CommandRequeuedEvent(requeueDateIn, contextIn)

        End Function

    End Class
End Namespace
