Imports System
Imports System.Runtime.Serialization

Namespace IdentityGroups
    ''' <summary>
    ''' A point-in-time snapshot of the membership of an identity group was taken
    ''' </summary>
    <Serializable()>
    Public NotInheritable Class IdentityGroupSnapshotWrittenEvent
        Inherits IdentityGroupEventBase
        Implements IIdentityGroupSnapshotWrittenEvent

        Private ReadOnly m_AsOfDate As Date?
        Public ReadOnly Property AsOfDate As Date? Implements IIdentityGroupSnapshotWrittenEvent.AsOfDate
            Get
                Return m_AsOfDate
            End Get
        End Property

        Private ReadOnly m_RequestSource As String
        Public ReadOnly Property RequestSource As String Implements IIdentityGroupSnapshotWrittenEvent.RequestSource
            Get
                Return m_RequestSource
            End Get
        End Property


        Private ReadOnly m_SnapshotLocation As String
        Public ReadOnly Property SnapshotLocation As String Implements IIdentityGroupSnapshotWrittenEvent.SnapshotLocation
            Get
                Return m_SnapshotLocation
            End Get
        End Property

        Private ReadOnly m_WriterType As String
        Public ReadOnly Property WriterType As String Implements IIdentityGroupSnapshotWrittenEvent.WriterType
            Get
                Return m_WriterType
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
                info.AddValue(NameOf(SnapshotLocation), SnapshotLocation)
                info.AddValue(NameOf(WriterType), WriterType)
            End If

        End Sub

        Public Sub New(info As SerializationInfo,
                       context As StreamingContext)
            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_AsOfDate = info.GetDateTime(NameOf(AsOfDate))
                m_RequestSource = info.GetString(NameOf(RequestSource))
                m_SnapshotLocation = info.GetString(NameOf(SnapshotLocation))
                m_WriterType = info.GetString(NameOf(WriterType))
            End If

        End Sub


        Private Sub New(ByVal asOfDateIn As Nullable(Of DateTime),
                        ByVal requestSourceIn As String,
                        ByVal snapshotLocationIn As String,
                        ByVal writerTypeIn As String)

            m_AsOfDate = asOfDateIn
            m_RequestSource = requestSourceIn
            m_SnapshotLocation = snapshotLocationIn
            m_WriterType = writerTypeIn

        End Sub


        Public Shared Function Create(ByVal asOfDateIn As Nullable(Of DateTime),
                        ByVal requestSourceIn As String,
                        ByVal snapshotLocationIn As String,
                        ByVal writerTypeIn As String) As IdentityGroupSnapshotWrittenEvent

            If (Not asOfDateIn.HasValue) Then
                asOfDateIn = DateTime.UtcNow
            End If

            Return New IdentityGroupSnapshotWrittenEvent(asOfDateIn, requestSourceIn, snapshotLocationIn, writerTypeIn)

        End Function
    End Class
End Namespace
