Option Explicit On
Option Strict On


Imports CQRSAzure.EventSourcing
''' <summary>
''' An event wrapped with the context information used to provide an audit trail
''' </summary>
''' <typeparam name="TAggregationKey">
''' The type of the key which uniquely identifies the aggregate that this event is wrapped for
''' </typeparam>
Public Class ContextWrappedEvent(Of TAggregationKey)
    Implements IEventContext(Of TAggregationKey)


#Region "Default values"
    'Note that there is no default for sequence number or event instance as it does not ever make sense not to supply these
    Public Const DEFAULT_COMMENTARY As String = ""
    Public Const DEFAULT_TIMESTAMP As DateTime = #1900/1/1#
    Public Const DEFAULT_SOURCE As String = ""
    'I would caution against using a default version as it prevents event definitiosn changing, but it may be OK in some cases
    Public Const DEFAULT_VERSION As UInteger = 0
    Public Const DEFAULT_WHO As String = ""
#End Region

    Private ReadOnly m_key As TAggregationKey
    Public ReadOnly Property AggregateKey As TAggregationKey Implements IEventInstance(Of TAggregationKey).AggregateKey
        Get
            Return m_key
        End Get
    End Property

    Private ReadOnly m_commentary As String
    Public ReadOnly Property Commentary As String Implements IEventContext.Commentary
        Get
            Return m_commentary
        End Get
    End Property

    Private ReadOnly m_eventInstance As IEvent
    Public ReadOnly Property EventInstance As IEvent Implements IEventInstance.EventInstance
        Get
            Return m_eventInstance
        End Get
    End Property

    Private ReadOnly m_sequence As Long
    Public ReadOnly Property SequenceNumber As Long Implements IEventContext.SequenceNumber
        Get
            Return m_sequence
        End Get
    End Property

    Private ReadOnly m_source As String
    Public ReadOnly Property Source As String Implements IEventContext.Source
        Get
            Return m_source
        End Get
    End Property

    Private ReadOnly m_timestamp As Date
    Public ReadOnly Property Timestamp As Date Implements IEventContext.Timestamp
        Get
            Return m_timestamp
        End Get
    End Property

    Private ReadOnly m_version As UInteger
    Public ReadOnly Property Version As UInteger Implements IEventInstance.Version
        Get
            Return m_version
        End Get
    End Property

    Private ReadOnly m_who As String
    Public ReadOnly Property Who As String Implements IEventContext.Who
        Get
            Return m_who
        End Get
    End Property

    Private Sub New(ByVal key_init As TAggregationKey,
                    ByVal eventInstance_init As IEvent,
                    ByVal sequence_init As Long,
                    Optional ByVal commentary_init As String = DEFAULT_COMMENTARY,
                    Optional ByVal source_init As String = DEFAULT_SOURCE,
                    Optional ByVal timestamp_init As Date = DEFAULT_TIMESTAMP,
                    Optional ByVal version_init As UInteger = DEFAULT_VERSION,
                    Optional ByVal who_init As String = DEFAULT_WHO
                    )

        m_key = key_init
        m_eventInstance = eventInstance_init
        m_commentary = commentary_init
        m_sequence = sequence_init
        m_source = source_init
        m_timestamp = timestamp_init
        m_version = version_init
        m_who = who_init

    End Sub


#Region "Factory methods"

    Public Shared Function Wrap(ByVal key As TAggregationKey,
                    ByVal eventInstance As IEvent,
                    ByVal sequence As Long,
                    ByVal commentary As String,
                    ByVal source As String,
                    ByVal timestamp As Date,
                    ByVal version As UInteger,
                    ByVal who As String) As IEventContext(Of TAggregationKey)

        Return New ContextWrappedEvent(Of TAggregationKey)(
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

    Public Shared Function Wrap(ByVal eventInstance As IEventInstance(Of TAggregationKey),
                                ByVal sequence As Long,
                ByVal commentary As String,
                ByVal source As String,
                ByVal timestamp As Date,
                ByVal version As UInteger,
                ByVal who As String) As IEventContext(Of TAggregationKey)

        Return New ContextWrappedEvent(Of TAggregationKey)(
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
