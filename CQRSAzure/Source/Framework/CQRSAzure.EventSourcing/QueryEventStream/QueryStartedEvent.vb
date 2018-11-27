Imports System
Imports System.Runtime.Serialization

Namespace Queries


    <Serializable()>
    Public NotInheritable Class QueryStartedEvent
        Inherits QueryEventBase
        Implements IQueryStartedEvent

        Private ReadOnly m_ProcessingStartDate As Date?
        Public ReadOnly Property ProcessingStartDate As Date? Implements IQueryStartedEvent.ProcessingStartDate
            Get
                Return m_ProcessingStartDate
            End Get
        End Property

        Private ReadOnly m_Processor As String
        Public ReadOnly Property Processor As String Implements IQueryStartedEvent.Processor
            Get
                Return m_Processor
            End Get
        End Property

        Public Overrides ReadOnly Property Version As UInteger
            Get
                Return 1
            End Get
        End Property

        Public Overrides Sub GetObjectData(info As SerializationInfo, context As StreamingContext)

            If Not (info Is Nothing) Then
                info.AddValue(NameOf(ProcessingStartDate), ProcessingStartDate)
                info.AddValue(NameOf(Processor), Processor)
            End If

        End Sub

        Private Sub New(ByVal ProcessingStartDateIn As Date?,
                 ByVal ProcessorIn As String)

            m_ProcessingStartDate = ProcessingStartDateIn
            m_Processor = ProcessorIn

        End Sub

        Public Sub New(info As SerializationInfo,
           context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_ProcessingStartDate = info.GetDateTime(NameOf(ProcessingStartDate))
                m_Processor = info.GetString(NameOf(Processor))
            End If

        End Sub


        Public Shared Function Create(ByVal ProcessingStartDateIn As Date?,
                 ByVal ProcessorIn As String) As QueryStartedEvent

            If (Not ProcessingStartDateIn.HasValue) Then
                ProcessingStartDateIn = DateTime.UtcNow
            End If

            Return New QueryStartedEvent(ProcessingStartDateIn, ProcessorIn)
        End Function

    End Class
End Namespace
