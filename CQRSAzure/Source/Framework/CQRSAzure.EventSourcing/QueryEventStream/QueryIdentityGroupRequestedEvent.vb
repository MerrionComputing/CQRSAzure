Imports System
Imports System.Runtime.Serialization

Namespace Queries
    <Serializable()>
    Public NotInheritable Class QueryIdentityGroupRequestedEvent
        Inherits QueryEventBase
        Implements IQueryIdentityGroupRequestedEvent

        Private ReadOnly m_AsOfDate As Date?
        Public ReadOnly Property AsOfDate As Date? Implements IQueryIdentityGroupRequestedEvent.AsOfDate
            Get
                Return m_AsOfDate
            End Get
        End Property

        Private ReadOnly m_identityGroupName As String
        Public ReadOnly Property IdentityGroupName As String Implements IQueryIdentityGroupRequestedEvent.IdentityGroupName
            Get
                Return m_identityGroupName
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
            End If

        End Sub

        Private Sub New(ByVal AsOfDateIn As Date?,
                 ByVal IdentityGroupNameIn As String)

            m_AsOfDate = AsOfDateIn
            m_identityGroupName = IdentityGroupNameIn
        End Sub


        Public Sub New(info As SerializationInfo,
                context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_AsOfDate = info.GetDateTime(NameOf(AsOfDate))
                m_identityGroupName = info.GetString(NameOf(IdentityGroupName))
            End If

        End Sub


        Public Shared Function Create(ByVal AsOfDateIn As Date?,
                 ByVal IdentityGroupNameIn As String) As QueryIdentityGroupRequestedEvent

            Return New QueryIdentityGroupRequestedEvent(AsOfDateIn,
                                                        IdentityGroupNameIn)
        End Function
    End Class
End Namespace

