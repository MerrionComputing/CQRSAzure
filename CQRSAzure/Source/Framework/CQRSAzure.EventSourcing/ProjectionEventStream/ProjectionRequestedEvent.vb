Imports System.Runtime.Serialization

Namespace Projections
    ''' <summary>
    ''' A  specific projection was requested (by a query or identity group classifier for example)
    ''' </summary>
    <Serializable()>
    Public NotInheritable Class ProjectionRequestedEvent
        Inherits ProjectionEventBase
        Implements IProjectionRequestedEvent

        Public ReadOnly m_AsOfDate As Date?
        ''' <summary>
        ''' The effective date to run the projection up until
        ''' </summary>
        ''' <returns>
        ''' If not set this will run to the current head of the aggregate event stream
        ''' as identified by the unique identifier
        ''' </returns>
        Public ReadOnly Property AsOfDate As Date? Implements IProjectionRequestedEvent.AsOfDate
            Get
                Return m_AsOfDate
            End Get
        End Property

        Private ReadOnly m_RequestSource As String
        ''' <summary>
        ''' Where did the source of this projection request come from
        ''' </summary>
        ''' <remarks>
        ''' This can be used to decide where to post the result
        ''' </remarks>
        Public ReadOnly Property RequestSource As String Implements IProjectionRequestedEvent.RequestSource
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

        Private Sub New(ByVal requestSourceIn As String,
                        ByVal asOfDateIn As Nullable(Of DateTime))

            m_RequestSource = requestSourceIn
            m_AsOfDate = asOfDateIn

        End Sub


        Public Shared Function Create(ByVal requestSourceIn As String,
                        ByVal asOfDateIn As Nullable(Of DateTime)) As ProjectionRequestedEvent

            Return New ProjectionRequestedEvent(requestSourceIn, asOfDateIn)

        End Function
    End Class
End Namespace