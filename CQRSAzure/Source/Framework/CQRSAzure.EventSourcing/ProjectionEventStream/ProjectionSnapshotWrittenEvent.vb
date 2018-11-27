Imports System
Imports System.Runtime.Serialization

Namespace Projections

    ''' <summary>
    ''' A snapshot of the state of this projection as at a particular event sequence was taken
    ''' </summary>
    <Serializable()>
    Public NotInheritable Class ProjectionSnapshotWrittenEvent
        Inherits ProjectionEventBase
        Implements IProjectionSnapshotWrittenEvent


        Private ReadOnly m_AsOfDate As Date?
        Public ReadOnly Property AsOfDate As Date? Implements IProjectionSnapshotWrittenEvent.AsOfDate
            Get
                Return m_AsOfDate
            End Get
        End Property

        Private ReadOnly m_AsOfSequence As Integer
        Public ReadOnly Property AsOfSequence As Integer Implements IProjectionSnapshotWrittenEvent.AsOfSequence
            Get
                Return m_AsOfSequence
            End Get
        End Property

        Private ReadOnly m_SnapshotLocation As String
        Public ReadOnly Property SnapshotLocation As String Implements IProjectionSnapshotWrittenEvent.SnapshotLocation
            Get
                Return m_SnapshotLocation
            End Get
        End Property

        Private ReadOnly m_WriterType As String
        Public ReadOnly Property WriterType As String Implements IProjectionSnapshotWrittenEvent.WriterType
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
                info.AddValue(NameOf(AsOfSequence), AsOfSequence)
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
                m_AsOfSequence = info.GetInt32(NameOf(AsOfSequence))
                m_SnapshotLocation = info.GetString(NameOf(SnapshotLocation))
                m_WriterType = info.GetString(NameOf(WriterType))
            End If

        End Sub

        Private Sub New(ByVal asOfDateIn As Nullable(Of DateTime),
                ByVal asOfSequenceIn As Int32,
                ByVal snapshotLocationIn As String,
                ByVal writerTypeIn As String)

            m_AsOfDate = asOfDateIn
            m_AsOfSequence = asOfSequenceIn
            m_SnapshotLocation = snapshotLocationIn
            m_WriterType = writerTypeIn

        End Sub

        Public Shared Function Create(ByVal asOfDateIn As Nullable(Of DateTime),
                        ByVal asOfSequenceIn As Int32,
                        ByVal snapshotLocationIn As String,
                        ByVal writerTypeIn As String) As ProjectionSnapshotWrittenEvent

            If (Not asOfDateIn.HasValue) Then
                asOfDateIn = DateTime.UtcNow
            End If

            Return New ProjectionSnapshotWrittenEvent(asOfDateIn, asOfSequenceIn, snapshotLocationIn, writerTypeIn)

        End Function

    End Class
End Namespace
