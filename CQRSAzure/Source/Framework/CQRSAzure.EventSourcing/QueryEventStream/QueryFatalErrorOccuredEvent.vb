Imports System
Imports System.Runtime.Serialization

Namespace Queries
    <Serializable()>
    Public NotInheritable Class QueryFatalErrorOccuredEvent
        Inherits QueryEventBase
        Implements IQueryFatalErrorOccuredEvent


        Private ReadOnly m_errorDate As Date?
        Public ReadOnly Property ErrorDate As Date? Implements IQueryFatalErrorOccuredEvent.ErrorDate
            Get
                Return m_errorDate
            End Get
        End Property

        Private ReadOnly m_errorMessage As String
        Public ReadOnly Property ErrorMessage As String Implements IQueryFatalErrorOccuredEvent.ErrorMessage
            Get
                Return m_errorMessage
            End Get
        End Property

        Public Overrides ReadOnly Property Version As UInteger
            Get
                Return 1
            End Get
        End Property

        Public Overrides Sub GetObjectData(info As SerializationInfo, context As StreamingContext)

            If Not (info Is Nothing) Then
                info.AddValue(NameOf(ErrorDate), ErrorDate)
                info.AddValue(NameOf(ErrorMessage), ErrorMessage)
            End If

        End Sub

        Private Sub New(ByVal errorDateIn As Nullable(Of DateTime),
                         ByVal errorMessageIn As String)
            m_errorDate = errorDateIn
            m_errorMessage = errorMessageIn
        End Sub

        Public Sub New(info As SerializationInfo,
   context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_errorDate = info.GetDateTime(NameOf(ErrorDate))
                m_errorMessage = info.GetString(NameOf(ErrorMessage))
            End If

        End Sub

        Public Shared Function Create(ByVal errorDateIn As Nullable(Of DateTime),
                         ByVal errorMessageIn As String) As QueryFatalErrorOccuredEvent

            If (Not errorDateIn.HasValue) Then
                errorDateIn = DateTime.UtcNow
            End If
            Return New QueryFatalErrorOccuredEvent(errorDateIn, errorMessageIn)

        End Function

    End Class
End Namespace
