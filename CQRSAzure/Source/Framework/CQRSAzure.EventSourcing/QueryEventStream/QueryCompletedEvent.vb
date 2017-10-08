Imports System.Runtime.Serialization

Namespace Queries

    <Serializable()>
    Public NotInheritable Class QueryCompletedEvent
        Inherits QueryEventBase
        Implements IQueryCompletedEvent


        Private ReadOnly m_CompletionDate As Date?
        Public ReadOnly Property CompletionDate As Date? Implements IQueryCompletedEvent.CompletionDate
            Get
                Return m_CompletionDate
            End Get
        End Property

        Private ReadOnly m_resultRecordCount As Integer
        Public ReadOnly Property ResultRecordCount As Integer Implements IQueryCompletedEvent.ResultRecordCount
            Get
                Return m_resultRecordCount
            End Get
        End Property

        Private m_SuccessMessage As String
        Public ReadOnly Property SuccessMessage As String Implements IQueryCompletedEvent.SuccessMessage
            Get
                Return m_SuccessMessage
            End Get
        End Property

        Public Overrides ReadOnly Property Version As UInteger
            Get
                Return 1
            End Get
        End Property

        Public Overrides Sub GetObjectData(info As SerializationInfo, context As StreamingContext)

            If Not (info Is Nothing) Then
                info.AddValue(NameOf(CompletionDate), CompletionDate)
                info.AddValue(NameOf(ResultRecordCount), ResultRecordCount)
                info.AddValue(NameOf(SuccessMessage), SuccessMessage)
            End If

        End Sub


        Private Sub New(ByVal CompletionDateIn As Date?,
                         ByVal RecordCountIn As Integer,
                         ByVal SuccessMessageIn As String)

            m_CompletionDate = CompletionDateIn
            m_resultRecordCount = RecordCountIn
            m_SuccessMessage = SuccessMessageIn

        End Sub

        Public Sub New(info As SerializationInfo,
           context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_CompletionDate = info.GetDateTime(NameOf(CompletionDate))
                m_resultRecordCount = info.GetInt32(NameOf(ResultRecordCount))
                m_SuccessMessage = info.GetString(NameOf(SuccessMessage))
            End If

        End Sub

        Public Shared Function Create(ByVal CompletionDateIn As Date?,
                         ByVal RecordCountIn As Integer,
                         ByVal SuccessMessageIn As String) As QueryCompletedEvent

            If (Not CompletionDateIn.HasValue) Then
                CompletionDateIn = DateTime.UtcNow
            End If

            Return New QueryCompletedEvent(CompletionDateIn,
                                           RecordCountIn,
                                           SuccessMessageIn)

        End Function
    End Class
End Namespace