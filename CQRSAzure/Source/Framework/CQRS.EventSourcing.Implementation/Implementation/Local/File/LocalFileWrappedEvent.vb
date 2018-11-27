Imports System
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Local.File

Namespace Local.File

    ''' <summary>
    ''' An event instance wrapped up in a way that allows it to be stored in an local file
    ''' </summary>
    ''' <remarks>
    ''' The size of the event is stored in the outer wrapper to allow a file reader to skip over and events 
    ''' it doesn't need to process
    ''' </remarks>
    <Serializable()>
    <DataContract()>
    Public NotInheritable Class LocalFileWrappedEvent

        <IgnoreDataMember()>
        <NonSerialized()>
        Private ReadOnly m_formatter As ILocalFileSettings.SerialiserType

        <DataMember(Name:=NameOf(EventClassName), Order:=0)>
        Private ReadOnly m_eventName As String
        ''' <summary>
        ''' The class name of the event
        ''' </summary>
        Public ReadOnly Property EventClassName As String
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

        <DataMember(Name:="SerialiserCapability", Order:=5)>
        Private m_serialiserUsed As IEventSerializer.SerialiserCapability

        <DataMember(Name:="Data", Order:=6)>
        Private ReadOnly m_eventData As Byte()

        Public ReadOnly Property EventInstance As IEvent
            Get
                If (String.IsNullOrWhiteSpace(m_eventClassName)) Then
                    Throw New SerializationException("Unable to determine the event type that wrote this event instance")
                End If


                Dim evtType As Type = Type.GetType(m_eventClassName, True, True)
                If (evtType IsNot Nothing) Then

                    'Is there an already known serialiser set for this event type?

                    'If not - try and make one


                    If evtType.GetInterface(NameOf(IEventSerializer)) IsNot Nothing Then
                        '1 Instantiate the event
                        Dim evtInstance = Activator.CreateInstance(evtType)
                        Dim serialiser = EventSerializerFactory.GetSerialiserByType(evtType)
                        If (serialiser IsNot Nothing) Then
                            '2 Now do we know what serialiser..?
                            If (m_serialiserUsed = IEventSerializer.SerialiserCapability.NameValuePairs) Then
                                'populate the name/value pairs
                                Dim nvprops As New Dictionary(Of String, Object)
                                evtInstance = serialiser.FromNameValuePairs(nvprops)
                            Else
                                Using memStream As New System.IO.MemoryStream(m_eventData)
                                    evtInstance = serialiser.FromStream(memStream)
                                End Using
                            End If
                            Return CType(evtInstance, IEvent)
                        Else
                            Throw New EventStreamReadException(DomainNameAttribute.GetAggregateDomainQualifiedName(evtType), evtInstance.ToString(), "", Sequence, "Serialiser not set")
                        End If
                    Else
                        'Try and use its serialised
                        Using memStream As New System.IO.MemoryStream(m_eventData)
                            If (m_formatter = ILocalFileSettings.SerialiserType.Binary) Then
                                Dim bf As New BinaryFormatter
                                Return CType(bf.Deserialize(memStream), IEvent)
                            Else
                                'Try with JSON
                                Dim jf As New Json.DataContractJsonSerializer(evtType)
                                Return CType(jf.ReadObject(memStream), IEvent)
                            End If
                        End Using
                    End If
                End If


                Return Nothing
            End Get
        End Property


        Public Sub New(ByVal sequenceInit As Long,
           ByVal versionInit As UInteger,
           ByVal timestampInit As DateTime,
           ByVal eventInstanceInit As IEvent,
           Optional formatter As ILocalFileSettings.SerialiserType = ILocalFileSettings.SerialiserType.Binary)

            m_formatter = formatter
            m_eventName = EventNameAttribute.GetEventName(eventInstanceInit)
            m_sequence = sequenceInit
            m_version = versionInit
            m_timestamp = timestampInit


            Using memStream As New System.IO.MemoryStream()
                Dim serialiser = EventSerializerFactory.GetSerialiserByType(eventInstanceInit.GetType)

                If (formatter = ILocalFileSettings.SerialiserType.Binary) Then
                    m_serialiserUsed = IEventSerializer.SerialiserCapability.Stream
                    If (serialiser IsNot Nothing) Then
                        serialiser.SaveToStream(memStream, eventInstanceInit)
                    Else
                        'Fall back on the standard binary serialiser
                        Dim bf As New BinaryFormatter
                        bf.Serialize(memStream, eventInstanceInit)
                    End If
                Else
                    m_serialiserUsed = IEventSerializer.SerialiserCapability.NameValuePairs
                    If (serialiser IsNot Nothing) Then
                        Using writer As New System.IO.StreamWriter(memStream)
                            For Each pair As KeyValuePair(Of String, Object) In serialiser.ToNameValuePairs(eventInstanceInit)
                                writer.WriteLine("[[" & pair.Key & "::" & pair.Value.ToString() & "]]")
                            Next
                        End Using
                    Else
                        'Fall back on JSON
                        Dim js As New Json.DataContractJsonSerializer(eventInstanceInit.GetType())
                        js.WriteObject(memStream, eventInstanceInit)
                    End If
                End If

                m_eventData = memStream.ToArray()
                m_eventSize = CUInt(m_eventData.LongLength)
            End Using
            m_eventClassName = eventInstanceInit.GetType().AssemblyQualifiedName

        End Sub


        Public Sub WriteToBinaryStream(ByVal stream As System.IO.Stream,
                                       Optional formatter As ILocalFileSettings.SerialiserType = ILocalFileSettings.SerialiserType.Binary)

            If (formatter = ILocalFileSettings.SerialiserType.Binary) Then
                Dim bf As New BinaryFormatter
                bf.Serialize(stream, Me)
            Else
                Dim jf As New Json.DataContractJsonSerializer(Me.GetType())
                jf.WriteObject(stream, Me)
            End If

        End Sub




    End Class
End Namespace