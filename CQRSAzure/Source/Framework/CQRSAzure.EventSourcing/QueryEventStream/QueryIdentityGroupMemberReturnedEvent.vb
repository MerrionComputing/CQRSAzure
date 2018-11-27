Imports System
Imports System.Runtime.Serialization

Namespace Queries
    <Serializable()>
    Public NotInheritable Class QueryIdentityGroupMemberReturnedEvent(Of TAggregateIdentifier)
        Inherits QueryEventBase
        Implements IQueryIdentityGroupMemberReturnedEvent(Of TAggregateIdentifier)

        Private ReadOnly m_asOfDate As Date?
        Public ReadOnly Property AsOfDate As Date? Implements IQueryIdentityGroupMemberReturnedEvent(Of TAggregateIdentifier).AsOfDate
            Get
                Return m_asOfDate
            End Get
        End Property

        Private ReadOnly m_identityGroupName As String
        Public ReadOnly Property IdentityGroupName As String Implements IQueryIdentityGroupMemberReturnedEvent(Of TAggregateIdentifier).IdentityGroupName
            Get
                Return m_identityGroupName
            End Get
        End Property


        Private ReadOnly m_memberUniqueIdentifier As TAggregateIdentifier
        Public ReadOnly Property MemberUniqueIdentifier As TAggregateIdentifier Implements IQueryIdentityGroupMemberReturnedEvent(Of TAggregateIdentifier).MemberUniqueIdentifier
            Get
                Return m_memberUniqueIdentifier
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
                info.AddValue(NameOf(IdentityGroupName), IdentityGroupName)
                info.AddValue(NameOf(MemberUniqueIdentifier), MemberUniqueIdentifier)
            End If

        End Sub

        Private Sub New(ByVal AsOfDateIn As Date?,
                         ByVal IdentityGroupNameIn As String,
                         ByVal MemberUniqueIdentifierIn As TAggregateIdentifier)

            m_asOfDate = AsOfDateIn
            m_identityGroupName = IdentityGroupNameIn
            m_memberUniqueIdentifier = MemberUniqueIdentifierIn
        End Sub


        Public Sub New(info As SerializationInfo,
                context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_asOfDate = info.GetDateTime(NameOf(AsOfDate))
                m_identityGroupName = info.GetString(NameOf(IdentityGroupName))
                m_memberUniqueIdentifier = info.GetValue(NameOf(MemberUniqueIdentifier), GetType(TAggregateIdentifier))
            End If

        End Sub

        Public Shared Function Create(ByVal AsOfDateIn As Date?,
                         ByVal IdentityGroupNameIn As String,
                         ByVal MemberUniqueIdentifierIn As TAggregateIdentifier) As QueryIdentityGroupMemberReturnedEvent(Of TAggregateIdentifier)

            Return New QueryIdentityGroupMemberReturnedEvent(Of TAggregateIdentifier)(AsOfDateIn,
                                                                                      IdentityGroupNameIn,
                                                                                      MemberUniqueIdentifierIn)
        End Function
    End Class
End Namespace
