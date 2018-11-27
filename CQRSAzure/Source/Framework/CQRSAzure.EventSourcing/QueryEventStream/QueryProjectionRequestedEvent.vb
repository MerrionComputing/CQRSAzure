Imports System
Imports System.Runtime.Serialization

Namespace Queries
    <Serializable()>
    Public NotInheritable Class QueryProjectionRequestedEvent(Of TAggregateIdentifier)
        Inherits QueryEventBase
        Implements IQueryProjectionRequestedEvent(Of TAggregateIdentifier)

        Private ReadOnly m_aggregateUniqueIdentifier As TAggregateIdentifier
        Public ReadOnly Property AggregateUniqueIdentifier As TAggregateIdentifier Implements IQueryProjectionRequestedEvent(Of TAggregateIdentifier).AggregateUniqueIdentifier
            Get
                Return m_aggregateUniqueIdentifier
            End Get
        End Property

        Private ReadOnly m_AsOfDate As Date?
        Public ReadOnly Property AsOfDate As Date? Implements IQueryProjectionRequestedEvent(Of TAggregateIdentifier).AsOfDate
            Get
                Return m_AsOfDate
            End Get
        End Property

        Private ReadOnly m_projectionName As String
        Public ReadOnly Property ProjectionName As String Implements IQueryProjectionRequestedEvent(Of TAggregateIdentifier).ProjectionpName
            Get
                Return m_projectionName
            End Get
        End Property

        Public Overrides ReadOnly Property Version As UInteger
            Get
                Return 1
            End Get
        End Property

        Public Overrides Sub GetObjectData(info As SerializationInfo, context As StreamingContext)

            If Not (info Is Nothing) Then
                info.AddValue(NameOf(AsOfDate), AsOfDate)
                info.AddValue(NameOf(ProjectionName), ProjectionName)
                info.AddValue(NameOf(AggregateUniqueIdentifier), AggregateUniqueIdentifier)
            End If

        End Sub

        Private Sub New(ByVal AsOfDateIn As Date?,
                        ByVal ProjectionNameIn As String,
                        ByVal AggregateUniqueIdentifierIn As TAggregateIdentifier)

            m_AsOfDate = AsOfDateIn
            m_projectionName = ProjectionNameIn
            m_aggregateUniqueIdentifier = AggregateUniqueIdentifierIn

        End Sub


        Public Sub New(info As SerializationInfo,
                context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_AsOfDate = info.GetDateTime(NameOf(AsOfDate))
                m_projectionName = info.GetString(NameOf(ProjectionName))
                m_aggregateUniqueIdentifier = info.GetValue(NameOf(AggregateUniqueIdentifier), GetType(TAggregateIdentifier))
            End If

        End Sub


        Public Shared Function Create(ByVal AsOfDateIn As Date?,
                 ByVal ProjectionNameIn As String,
                 ByVal MemberUniqueIdentifierIn As TAggregateIdentifier) As QueryProjectionRequestedEvent(Of TAggregateIdentifier)

            Return New QueryProjectionRequestedEvent(Of TAggregateIdentifier)(AsOfDateIn,
                                                                              ProjectionNameIn,
                                                                              MemberUniqueIdentifierIn)
        End Function
    End Class
End Namespace
