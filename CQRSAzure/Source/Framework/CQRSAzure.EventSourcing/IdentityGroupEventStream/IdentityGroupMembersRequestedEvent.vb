Imports System
Imports System.Runtime.Serialization

Namespace IdentityGroups
    <Serializable>
    Public NotInheritable Class IdentityGroupMembersRequestedEvent
        Inherits IdentityGroupEventBase
        Implements IIdentityGroupMembersRequestedEvent

        Private ReadOnly m_AsOfDate As Date?
        Public ReadOnly Property AsOfDate As Date? Implements IIdentityGroupMembersRequestedEvent.AsOfDate
            Get
                Return m_AsOfDate
            End Get
        End Property

        Private ReadOnly m_RequestSource As String
        Public ReadOnly Property RequestSource As String Implements IIdentityGroupMembersRequestedEvent.RequestSource
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
                info.AddValue(NameOf(RequestSource), RequestSource)
            End If

        End Sub

        Public Sub New(info As SerializationInfo,
                       context As StreamingContext)
            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_RequestSource = info.GetString(NameOf(RequestSource))
                m_AsOfDate = info.GetDateTime(NameOf(AsOfDate))
            End If

        End Sub

        Private Sub New(ByVal asOfDateIn As Nullable(Of DateTime),
                        ByVal requestSourceIn As String)

            m_AsOfDate = asOfDateIn
            m_RequestSource = requestSourceIn

        End Sub


        Public Shared Function Create(ByVal asOfDateIn As Nullable(Of DateTime),
                        ByVal requestSourceIn As String) As IdentityGroupMembersRequestedEvent

            If (Not asOfDateIn.HasValue) Then
                asOfDateIn = DateTime.UtcNow
            End If

            Return New IdentityGroupMembersRequestedEvent(asOfDateIn, requestSourceIn)

        End Function
    End Class
End Namespace
