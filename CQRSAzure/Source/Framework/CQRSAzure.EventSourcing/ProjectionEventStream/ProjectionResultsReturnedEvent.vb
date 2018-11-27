Imports System
Imports System.Runtime.Serialization

Namespace Projections

    ''' <summary>
    ''' The results for a projection were returned to a query or identity group classifier that asked for them
    ''' </summary>
    <Serializable()>
    Public NotInheritable Class ProjectionResultsReturnedEvent
        Inherits ProjectionEventBase
        Implements IProjectionResultsReturnedEvent

        Private ReadOnly m_AsOfDate As Date?
        Public ReadOnly Property AsOfDate As Date? Implements IProjectionResultsReturnedEvent.AsOfDate
            Get
                Return m_AsOfDate
            End Get
        End Property

        Private ReadOnly m_AsOfSequence As Integer
        Public ReadOnly Property AsOfSequence As Integer Implements IProjectionResultsReturnedEvent.AsOfSequence
            Get
                Return m_AsOfSequence
            End Get
        End Property

        Private ReadOnly m_SnapshotLocation As String
        ''' <summary>
        ''' Where the returned data are stored
        ''' </summary>
        ''' <remarks>
        ''' This could be an URL or the raw results themselves 
        ''' </remarks>
        Public ReadOnly Property ProjectionLocation As String Implements IProjectionResultsReturnedEvent.ProjectionLocation
            Get
                Return m_SnapshotLocation
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
                info.AddValue(NameOf(ProjectionLocation), ProjectionLocation)
            End If
        End Sub

        Public Sub New(info As SerializationInfo,
                       context As StreamingContext)
            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_AsOfSequence = info.GetInt32(NameOf(AsOfSequence))
                m_SnapshotLocation = info.GetString(NameOf(ProjectionLocation))
                m_AsOfDate = info.GetDateTime(NameOf(AsOfDate))
            End If

        End Sub

        Private Sub New(ByVal asOfDateIn As Nullable(Of DateTime),
                        ByVal asOfSequenceIn As Int32,
                        ByVal projectionLocationIn As String)

            m_AsOfDate = asOfDateIn
            m_AsOfSequence = asOfSequenceIn
            m_SnapshotLocation = projectionLocationIn

        End Sub

        Public Shared Function Create(ByVal asOfDateIn As Nullable(Of DateTime),
                        ByVal asOfSequenceIn As Int32,
                        ByVal projectionLocationIn As String) As ProjectionResultsReturnedEvent

            If (Not asOfDateIn.HasValue) Then
                asOfDateIn = DateTime.UtcNow
            End If

            Return New ProjectionResultsReturnedEvent(asOfDateIn, asOfSequenceIn, projectionLocationIn)

        End Function

    End Class
End Namespace
