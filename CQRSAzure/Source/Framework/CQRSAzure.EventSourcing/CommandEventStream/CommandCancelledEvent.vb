
Imports System
Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Commands

    ''' <summary>
    ''' A command execution was cancelled
    ''' </summary>
    <Serializable()>
    Public NotInheritable Class CommandCancelledEvent
        Inherits CommandEventBase
        Implements ICommandCancelledEvent
        Implements IEvent(Of CommandAggregateIdentifier)

        Private ReadOnly m_cancellationDate As Date?
        Public ReadOnly Property CancellationDate As Date? Implements ICommandCancelledEvent.CancellationDate
            Get
                Return m_cancellationDate
            End Get
        End Property



        Private ReadOnly m_reason As String
        Public ReadOnly Property Reason As String Implements ICommandCancelledEvent.Reason
            Get
                Return m_reason
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
                info.AddValue(NameOf(CancellationDate), CancellationDate)
                info.AddValue(NameOf(Reason), Reason)
                'Add the context parts

            End If
        End Sub

        Public Sub New(info As SerializationInfo,
               context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_reason = info.GetString(NameOf(Reason))
                m_cancellationDate = info.GetDateTime(NameOf(CancellationDate))
            End If

        End Sub

        Private Sub New(ByVal cancellationDateIn As DateTime,
                        ByVal reasonIn As String,
                        Optional ByVal contextIn As CommandEventContext = Nothing)
            MyBase.New(contextIn)
            m_cancellationDate = cancellationDateIn
            m_reason = reasonIn


        End Sub

        Public Overloads Shared Function Create(ByVal cancellationDateIn As DateTime,
                        ByVal reasonIn As String,
                        Optional ByVal contextIn As CommandEventContext = Nothing) As CommandCancelledEvent

            Return New CommandCancelledEvent(cancellationDateIn, reasonIn, contextIn)

        End Function

    End Class

End Namespace

