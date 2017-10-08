''' <summary>
''' An event wrapped in the information to connect it with an aggregation identifier
''' </summary>
''' <typeparam name="TAggregateKey">
''' The data type of the key that uniquely identifies the aggregation this wrapped event is linked ot
''' </typeparam>
Public Class InstanceWrappedEvent(Of TAggregateKey)
    Implements IEventInstance(Of TAggregateKey)

    Private ReadOnly m_AggregateKey As TAggregateKey
    Public ReadOnly Property AggregateKey As TAggregateKey Implements IEventInstance(Of TAggregateKey).AggregateKey
        Get
            Return m_AggregateKey
        End Get
    End Property

    Private ReadOnly m_EventInstance As IEvent
    Public ReadOnly Property EventInstance As IEvent Implements IEventInstance.EventInstance
        Get
            Return m_EventInstance
        End Get
    End Property

    Private ReadOnly m_Version As UInteger
    Public ReadOnly Property Version As UInteger Implements IEventInstance.Version
        Get
            Return m_Version
        End Get
    End Property

    Private Sub New(ByVal Key_init As TAggregateKey,
                    ByVal EventInstance_init As IEvent,
                    ByVal version_init As UInteger)

        m_AggregateKey = Key_init
        m_EventInstance = EventInstance_init
        m_Version = version_init

    End Sub


#Region "Factory methods"

    Public Shared Function Wrap(ByVal Key As TAggregateKey,
                    ByVal EventInstance As IEvent,
                    ByVal version As UInteger) As IEventInstance(Of TAggregateKey)

#Region "Tracing"
        EventSourcing.LogVerboseInfo("Wrapping event instance of " & EventInstance.GetType().ToString() & " - key = " & Key.ToString())
#End Region

        Return New InstanceWrappedEvent(Of TAggregateKey)(
            Key,
            EventInstance,
            version)

    End Function

#End Region

End Class
