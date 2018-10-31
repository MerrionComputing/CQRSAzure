Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing

Namespace Azure.File

    ''' <summary>
    ''' An event instance wrapped up in a way that allows it to be stored in an Azure file
    ''' </summary>
    ''' <remarks>
    ''' The size of the event is stored in the outer wrapper to allow a file reader to skip over and events 
    ''' it doesn't need to process
    ''' </remarks>
    <Serializable()>
    <DataContract()>
    Public NotInheritable Class FileBlockWrappedEvent

        <IgnoreDataMember()>
        <NonSerialized()>
        Private m_formatter As IFormatter = Nothing

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
        Public ReadOnly Property Version As UInteger
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

        <DataMember(Name:="Size", Order:=4)>
        Private ReadOnly m_eventSize As UInteger
        Public ReadOnly Property EventSize As UInteger
            Get
                Return m_eventSize
            End Get
        End Property

        ''' <summary>
        ''' The .NET class used to serialise/deserialise the underlying event blob data
        ''' </summary>
        ''' <remarks>
        ''' It is possible to derive this by a lookup table from the event name and version if you prefer not to save the class name
        ''' in the event record.  Usually any storage space critical systems would do this so as to reduce redundant data stored.
        ''' </remarks>
        <DataMember(Name:="Class", Order:=4)>
        Private ReadOnly m_eventClassName As String
        Public ReadOnly Property ClassName As String
            Get
                Return m_eventClassName
            End Get
        End Property

        <DataMember(Name:="Data", Order:=5)>
        Private ReadOnly m_eventData As Byte()

        Public ReadOnly Property EventInstance As IEvent
            Get
                If (String.IsNullOrWhiteSpace(m_eventClassName)) Then
                    Throw New SerializationException("Unable to determine the event type that wrote this event instance")
                End If

                If (m_eventSize = 0) Then
                    Throw New SerializationException("Unable to return the event data for this event instance - size is zero")
                End If

                Dim evtType As Type = Type.GetType(m_eventClassName, True, True)
                If (evtType IsNot Nothing) Then
                    If (m_formatter Is Nothing) Then
                        m_formatter = New BinaryFormatter()
                    End If
                    Using memStream As New System.IO.MemoryStream(m_eventData)
                        Return CTypeDynamic(m_formatter.Deserialize(memStream), evtType)
                    End Using
                End If

                Return Nothing
            End Get
        End Property


        Public Sub New(ByVal sequenceInit As String,
           ByVal versionInit As UInteger,
           ByVal timestampInit As DateTime,
           ByVal eventInstanceInit As IEvent,
           Optional ByVal formatter As IFormatter = Nothing)

            m_eventName = EventNameAttribute.GetEventName(eventInstanceInit)
            m_sequence = sequenceInit
            m_version = versionInit
            m_timestamp = timestampInit

            m_eventClassName = eventInstanceInit.GetType().AssemblyQualifiedName

            If (formatter IsNot Nothing) Then
                m_formatter = formatter
            Else
                m_formatter = New BinaryFormatter()
            End If

            Using memStream As New System.IO.MemoryStream()
                m_formatter.Serialize(memStream, eventInstanceInit)
                m_eventSize = memStream.Length
                m_eventData = memStream.ToArray()
            End Using

        End Sub

        Public Sub WriteToBinaryStream(ByVal stream As System.IO.Stream,
                                       Optional ByVal formatter As IFormatter = Nothing)

            If (formatter Is Nothing) Then
                formatter = New BinaryFormatter()
            End If

            formatter.Serialize(stream, Me)

        End Sub

    End Class

End Namespace