Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Commands

    ''' <summary>
    ''' A command processor has started executing the command
    ''' </summary>
    <Serializable()>
    Public NotInheritable Class CommandStartedEvent
        Inherits CommandEventBase
        Implements ICommandStartedEvent
        Implements IEvent(Of CommandAggregateIdentifier)


        Private ReadOnly m_ProcessingStartDate As Date?
        Public ReadOnly Property ProcessingStartDate As Date? Implements ICommandStartedEvent.ProcessingStartDate
            Get
                Return m_ProcessingStartDate
            End Get
        End Property

        Public Overrides ReadOnly Property Version As UInteger Implements IEvent(Of CommandAggregateIdentifier).Version
            Get
                Return 1
            End Get
        End Property

        Public Overrides Sub GetObjectData(info As SerializationInfo, context As StreamingContext)
            If Not (info Is Nothing) Then
                MyBase.GetObjectData(info, context)
                info.AddValue(NameOf(ProcessingStartDate), ProcessingStartDate)
            End If
        End Sub

        Public Sub New(info As SerializationInfo,
                  context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_ProcessingStartDate = info.GetDateTime(NameOf(ProcessingStartDate))
            End If

        End Sub

        Private Sub New(ByVal processingStartDateIn As Nullable(Of DateTime),
                        Optional ByVal contextIn As CommandEventContext = Nothing)
            MyBase.New(contextIn)
            m_ProcessingStartDate = processingStartDateIn

        End Sub

        Public Overloads Shared Function Create(ByVal processingStartDateIn As Nullable(Of DateTime),
                        Optional ByVal contextIn As CommandEventContext = Nothing) As CommandStartedEvent

            Return New CommandStartedEvent(processingStartDateIn,
                                           contextIn)

        End Function

    End Class
End Namespace