
Imports System.Runtime.Serialization

Namespace IdentityGroups

    <Serializable()>
    Public NotInheritable Class IdentityGroupMembersReturnedEvent
        Inherits IdentityGroupEventBase
        Implements IIdentityGroupMembersReturnedEvent

        Private ReadOnly m_AsOfDate As Date?
        Public ReadOnly Property AsOfDate As Date? Implements IIdentityGroupMembersReturnedEvent.AsOfDate
            Get
                Return m_AsOfDate
            End Get
        End Property

        Private ReadOnly m_GroupMembersLocation As String
        Public ReadOnly Property GroupMembersLocation As String Implements IIdentityGroupMembersReturnedEvent.GroupMembersLocation
            Get
                Return m_GroupMembersLocation
            End Get
        End Property


        Private ReadOnly m_RequestSource As String
        Public ReadOnly Property RequestSource As String Implements IIdentityGroupMembersReturnedEvent.RequestSource
            Get
                Return m_RequestSource
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
                info.AddValue(NameOf(GroupMembersLocation), GroupMembersLocation)
                info.AddValue(NameOf(RequestSource), RequestSource)
            End If

        End Sub

        Public Sub New(info As SerializationInfo,
                       context As StreamingContext)
            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_AsOfDate = info.GetDateTime(NameOf(AsOfDate))
                m_GroupMembersLocation = info.GetString(NameOf(GroupMembersLocation))
                m_RequestSource = info.GetString(NameOf(RequestSource))
            End If

        End Sub

        Private Sub New(ByVal asOfDateIn As Nullable(Of DateTime),
                        ByVal groupMembersLocationIn As String,
                        ByVal requestSourceIn As String)

            m_AsOfDate = asOfDateIn
            m_GroupMembersLocation = groupMembersLocationIn
            m_RequestSource = requestSourceIn

        End Sub


        Public Shared Function Create(ByVal asOfDateIn As Nullable(Of DateTime),
                        ByVal groupMembersLocationIn As String,
                        ByVal requestSourceIn As String) As IdentityGroupMembersReturnedEvent

            If (Not asOfDateIn.HasValue) Then
                asOfDateIn = DateTime.UtcNow
            End If

            Return New IdentityGroupMembersReturnedEvent(asOfDateIn,
                                                         groupMembersLocationIn,
                                                         requestSourceIn)

        End Function

    End Class
End Namespace