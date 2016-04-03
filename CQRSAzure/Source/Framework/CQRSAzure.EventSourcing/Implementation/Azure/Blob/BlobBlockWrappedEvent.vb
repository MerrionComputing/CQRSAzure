Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

Namespace Azure.Blob
    ''' <summary>
    ''' An event instance wrapped up in a way that allows it to be stored in an Azure blob
    ''' </summary>
    ''' <remarks>
    ''' Each block in an append blob can be a different size, up to a maximum of 4 MB, and an append blob can include a maximum of 50,000 blocks.
    ''' </remarks>
    <Serializable()>
    <DataContract()>
    Public Class BlobBlockWrappedEvent
        Implements IEventInstance

        <DataMember(Name:="EventName", Order:=0)>
        Private ReadOnly m_eventName As String
        ''' <summary>
        ''' The class name of the event
        ''' </summary>
        Public ReadOnly Property EventName As String
            Get
                Return m_eventName
            End Get
        End Property

        <DataMember(Name:="Sequence", Order:=1)>
        Private ReadOnly m_sequence As Long
        ''' <summary>
        ''' The sequence number of this record
        ''' </summary>
        Public ReadOnly Property Sequence As Long
            Get
                Return m_sequence
            End Get
        End Property

        <DataMember(Name:="Version", Order:=2)>
        Private ReadOnly m_version As UInteger
        Public ReadOnly Property Version As UInteger Implements IEventInstance.Version
            Get
                Return m_version
            End Get
        End Property


        <DataMember(Name:="Timestamp", Order:=3)>
        Private ReadOnly m_timestamp As DateTime
        Public ReadOnly Property Timestamp As DateTime
            Get
                Return m_timestamp
            End Get
        End Property

        <DataMember(Name:="Instance", Order:=4)>
        Private ReadOnly m_eventInstance As IEvent
        Public ReadOnly Property EventInstance As IEvent Implements IEventInstance.EventInstance
            Get
                Return m_eventInstance
            End Get
        End Property

        Public Sub New(ByVal sequenceInit As String,
                   ByVal versionInit As UInteger,
                   ByVal timestampInit As DateTime,
                   ByVal eventInstanceInit As IEvent)

            m_eventName = EventNameAttribute.GetEventName(eventInstanceInit)
            m_sequence = sequenceInit
            m_version = versionInit
            m_timestamp = timestampInit
            m_eventInstance = eventInstanceInit

        End Sub


        Public Function ToBinaryStream() As System.IO.Stream

            Dim ms As New System.IO.MemoryStream()

            Dim bf As New BinaryFormatter()
            bf.Serialize(ms, Me)
            'Move to the start of the binary stream so we can write it to an event
            ms.Seek(0, IO.SeekOrigin.Begin)
            Return ms

        End Function

        Public Shared Function FromBinaryStream(ByVal binaryStream As System.IO.Stream) As BlobBlockWrappedEvent

            Dim bf As New BinaryFormatter()
            Return CTypeDynamic(Of BlobBlockWrappedEvent)(bf.Deserialize(binaryStream))

        End Function

    End Class

End Namespace