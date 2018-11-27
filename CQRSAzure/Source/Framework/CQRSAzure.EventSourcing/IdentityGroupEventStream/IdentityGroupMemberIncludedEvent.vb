Imports System
Imports System.Runtime.Serialization

Namespace IdentityGroups
    <Serializable>
    Public NotInheritable Class IdentityGroupMemberIncludedEvent(Of TAggregateIdentifier As {IAggregationIdentifier, ISerializable})
        Inherits IdentityGroupEventBase
        Implements IIdentityGroupMemberIncludedEvent(Of TAggregateIdentifier)

        Private ReadOnly m_AsOfDate As Date?
        Public ReadOnly Property AsOfDate As Date? Implements IIdentityGroupMemberIncludedEvent(Of TAggregateIdentifier).AsOfDate
            Get
                Return m_AsOfDate
            End Get
        End Property

        Private ReadOnly m_MemberUniqueIdentifier As TAggregateIdentifier
        Public ReadOnly Property MemberUniqueIdentifier As TAggregateIdentifier Implements IIdentityGroupMemberIncludedEvent(Of TAggregateIdentifier).MemberUniqueIdentifier
            Get
                Return m_MemberUniqueIdentifier
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
                info.AddValue(NameOf(MemberUniqueIdentifier), MemberUniqueIdentifier)
            End If

        End Sub

        Public Sub New(info As SerializationInfo,
                       context As StreamingContext)
            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_MemberUniqueIdentifier = info.GetValue(NameOf(MemberUniqueIdentifier), GetType(TAggregateIdentifier))
                m_AsOfDate = info.GetDateTime(NameOf(AsOfDate))
            End If

        End Sub

        Public Sub New(ByVal memberToInclude As TAggregateIdentifier,
                       ByVal asOfDateIn As Nullable(Of DateTime))

            m_MemberUniqueIdentifier = memberToInclude
            m_AsOfDate = asOfDateIn

        End Sub


        Public Shared Function Create(ByVal memberToInclude As TAggregateIdentifier,
                       ByVal asOfDateIn As Nullable(Of DateTime)) As IdentityGroupMemberIncludedEvent(Of TAggregateIdentifier)

            If (Not asOfDateIn.HasValue) Then
                asOfDateIn = DateTime.UtcNow
            End If

            Return New IdentityGroupMemberIncludedEvent(Of TAggregateIdentifier)(memberToInclude, asOfDateIn)

        End Function
    End Class
End Namespace
