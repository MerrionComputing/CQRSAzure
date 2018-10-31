Option Explicit On
Option Strict On


Imports CQRSAzure.EventSourcing
''' <summary>
''' An event wrapped with the context information used to provide an audit trail
''' </summary>
''' <typeparam name="TAggregateKey">
''' The type of the key which uniquely identifies the aggregate that this event is wrapped for
''' </typeparam>
Public Class ContextWrappedEvent(Of TAggregateKey)
    Inherits ContextWrappedEventUntyped
    Implements IEventContext(Of TAggregateKey)




    Private ReadOnly m_key As TAggregateKey
    Public ReadOnly Property AggregateKey As TAggregateKey Implements IEventInstance(Of TAggregateKey).AggregateKey
        Get
            Return m_key
        End Get
    End Property


    Private Sub New(ByVal key_init As TAggregateKey,
                    ByVal eventInstance_init As IEvent,
                    ByVal sequence_init As Long,
                    Optional ByVal commentary_init As String = DEFAULT_COMMENTARY,
                    Optional ByVal source_init As String = DEFAULT_SOURCE,
                    Optional ByVal timestamp_init As Date = DEFAULT_TIMESTAMP,
                    Optional ByVal version_init As UInteger = DEFAULT_VERSION,
                    Optional ByVal who_init As String = DEFAULT_WHO
                    )

        MyBase.New(key_init.ToString(),
                   eventInstance_init,
                   sequence_init,
                   commentary_init,
                   source_init,
                   timestamp_init,
                   version_init,
                   who_init)

        m_key = key_init


    End Sub


#Region "Factory methods"

    Public Shared Function Wrap(ByVal key As TAggregateKey,
                    ByVal eventInstance As IEvent,
                    ByVal sequence As Long,
                    ByVal commentary As String,
                    ByVal source As String,
                    ByVal timestamp As Date,
                    ByVal version As UInteger,
                    ByVal who As String) As IEventContext(Of TAggregateKey)

#Region "Tracing"
        EventSourcing.LogVerboseInfo("Wrapping event of type " & eventInstance.GetType.ToString() & " with unique key " & key.ToString())
#End Region

        Return New ContextWrappedEvent(Of TAggregateKey)(
            key,
            eventInstance,
            sequence,
            commentary,
            source,
            timestamp,
            version,
            who
            )

    End Function

    Public Shared Function Wrap(ByVal eventInstance As IEventInstance(Of TAggregateKey),
                                ByVal sequence As Long,
                ByVal commentary As String,
                ByVal source As String,
                ByVal timestamp As Date,
                ByVal version As UInteger,
                ByVal who As String) As IEventContext(Of TAggregateKey)

#Region "Tracing"
        EventSourcing.LogVerboseInfo("Wrapping event of type " & eventInstance.GetType.ToString() & " at sequence number " & sequence.ToString())
#End Region

        Return New ContextWrappedEvent(Of TAggregateKey)(
            eventInstance.AggregateKey,
            eventInstance.EventInstance,
            sequence,
            commentary,
            source,
            timestamp,
            version,
            who
            )

    End Function
#End Region

End Class


Public Class ContextWrappedEventUntyped
    Implements IEventContext

#Region "Default values"
    'Note that there is no default for sequence number or event instance as it does not ever make sense not to supply these
    Public Const DEFAULT_COMMENTARY As String = ""
    Public Const DEFAULT_TIMESTAMP As DateTime = #1900/1/1#
    Public Const DEFAULT_SOURCE As String = ""
    'I would caution against using a default version as it prevents event definition changing, but it may be OK in some cases
    Public Const DEFAULT_VERSION As UInteger = 0
    Public Const DEFAULT_WHO As String = ""
    Public Const DEFAULT_CORRELATION_IDENTIFIER As String = ""
#End Region

    Private ReadOnly m_key As String

    Private ReadOnly m_eventInstance As IEvent
    Private ReadOnly m_sequence As Long
    Private ReadOnly m_commentary As String
    Private ReadOnly m_source As String
    Private ReadOnly m_timestamp As Date
    Private ReadOnly m_version As UInteger
    Private ReadOnly m_who As String
    Private ReadOnly m_correlationIdentifier As String

    Public ReadOnly Property Who As String Implements IEventContext.Who
        Get
            Return m_who
        End Get
    End Property

    Public ReadOnly Property Timestamp As Date Implements IEventContext.Timestamp
        Get
            Return m_timestamp
        End Get
    End Property

    Public ReadOnly Property Source As String Implements IEventContext.Source
        Get
            Return m_source
        End Get
    End Property

    Public ReadOnly Property SequenceNumber As Long Implements IEventContext.SequenceNumber
        Get
            Return m_sequence
        End Get
    End Property

    Public ReadOnly Property Commentary As String Implements IEventContext.Commentary
        Get
            Return m_commentary
        End Get
    End Property

    Public ReadOnly Property Version As UInteger Implements IEventInstance.Version
        Get
            Return m_version
        End Get
    End Property

    Public ReadOnly Property CorrelationIdentifier As String Implements IEventContext.CorrelationIdentifier
        Get
            Return m_correlationIdentifier
        End Get
    End Property

    Public ReadOnly Property EventInstance As IEvent Implements IEventInstance.EventInstance
        Get
            Return m_eventInstance
        End Get
    End Property

    Protected Friend Sub New(ByVal key_init As String,
                ByVal eventInstance_init As IEvent,
                ByVal sequence_init As Long,
                Optional ByVal commentary_init As String = DEFAULT_COMMENTARY,
                Optional ByVal source_init As String = DEFAULT_SOURCE,
                Optional ByVal timestamp_init As Date = DEFAULT_TIMESTAMP,
                Optional ByVal version_init As UInteger = DEFAULT_VERSION,
                Optional ByVal who_init As String = DEFAULT_WHO,
                Optional ByVal correlationIdentifier_init As String = DEFAULT_CORRELATION_IDENTIFIER
                )

        m_key = key_init
        m_eventInstance = eventInstance_init
        m_commentary = commentary_init
        m_sequence = sequence_init
        m_source = source_init
        m_timestamp = timestamp_init
        m_version = version_init
        m_who = who_init
        m_correlationIdentifier = correlationIdentifier_init

    End Sub


#Region "Factory methods"

    Public Shared Function Wrap(ByVal key As String,
                    ByVal eventInstance As IEvent,
                    ByVal sequence As Long,
                    ByVal commentary As String,
                    ByVal source As String,
                    ByVal timestamp As Date,
                    ByVal version As UInteger,
                    ByVal who As String,
                    ByVal correlationIdentifier As String) As IEventContext

#Region "Tracing"
        EventSourcing.LogVerboseInfo("Wrapping event of type " & eventInstance.GetType.ToString() & " with unique key " & key)
#End Region

        Return New ContextWrappedEventUntyped(
            key,
            eventInstance,
            sequence,
            commentary,
            source,
            timestamp,
            version,
            who,
            correlationIdentifier
            )

    End Function


    '
    Public Shared Function Wrap(ByVal eventInstance As InstanceWrappedEvent(Of String),
                    ByVal sequence As Long,
                    ByVal commentary As String,
                    ByVal source As String,
                    ByVal timestamp As Date,
                    ByVal who As String,
                    ByVal correlationIdentifier As String) As IEventContext

#Region "Tracing"
        EventSourcing.LogVerboseInfo("Wrapping event of type " & eventInstance.GetType.ToString() & " with unique key " & eventInstance.AggregateKey)
#End Region

        Return New ContextWrappedEventUntyped(
            eventInstance.AggregateKey,
            eventInstance.EventInstance,
            sequence,
            commentary,
            source,
            timestamp,
            eventInstance.Version,
            who,
            correlationIdentifier
            )

    End Function

#End Region
End Class

Partial Public Module EventSerializerFactory
    Private m_EventSerialisers As Dictionary(Of Type, IEventSerializer) = New Dictionary(Of Type, IEventSerializer)()

    ''' <summary>
    ''' Add the event serialiser to the known event serialisers
    ''' </summary>
    ''' <param name="eventTypeToSerialise">
    ''' The data type of the event to be serialised
    ''' </param>
    ''' <param name="serialiserToUse">
    ''' The class that can serialise the given event
    ''' </param>
    Friend Sub AddOrSetSerialiser(ByVal eventTypeToSerialise As Type, ByVal serialiserToUse As IEventSerializer)

        If (m_EventSerialisers.ContainsKey(eventTypeToSerialise)) Then
            m_EventSerialisers(eventTypeToSerialise) = serialiserToUse
        Else
            m_EventSerialisers.Add(eventTypeToSerialise, serialiserToUse)
        End If

    End Sub

    ''' <summary>
    ''' Add the event serialiser to the known event serialisers
    ''' </summary>
    ''' <param name="serialiser">
    ''' </param>
    Public Sub AddOrSetSerialiser(Of TEvent As {New, IEvent})(ByVal serialiser As EventSerializer(Of TEvent))

        AddOrSetSerialiser(GetType(TEvent), serialiser)

    End Sub



    Public Function GetSerialiserByType(ByVal eventTypeToSerialise As Type) As IEventSerializer

        If (m_EventSerialisers.ContainsKey(eventTypeToSerialise)) Then
            Return m_EventSerialisers(eventTypeToSerialise)
        Else
            Return Nothing
        End If


    End Function

    Public Function GetSerialiserByType(Of TEvent)(ByVal eventTypeToSerialise As Type) As IEventSerializer(Of TEvent)

        Return GetSerialiserByType(Of TEvent)()


    End Function

    Public Function GetSerialiserByType(Of TEvent)() As IEventSerializer(Of TEvent)

        If (m_EventSerialisers.ContainsKey(GetType(TEvent))) Then
            Return DirectCast(m_EventSerialisers(GetType(TEvent)), IEventSerializer(Of TEvent))
        Else
            Return Nothing
        End If

    End Function


End Module
