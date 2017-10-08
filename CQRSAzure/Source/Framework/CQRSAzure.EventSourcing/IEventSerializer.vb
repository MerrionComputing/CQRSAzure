Imports System.Runtime.Serialization

''' <summary>
''' Specialised class that can serialize an event to or from a backing store either as name:value pairs or 
''' as a binary stream
''' </summary>
Public Interface IEventSerializer


    ''' <summary>
    ''' The different ways this serialiser can serialise/deserialise an event
    ''' </summary>
    <Flags()>
    Enum SerialiserCapability
        ''' <summary>
        ''' None - fall back on the system default serialisation
        ''' </summary>
        None = &H0
        ''' <summary>
        ''' Name/Value pair based serialisation
        ''' </summary>
        ''' <remarks>
        ''' This is good for JSON or NoSQL serialisation
        ''' </remarks>
        NameValuePairs = &H1
        ''' <summary>
        ''' Serialise to/from a binary stream
        ''' </summary>
        Stream = &H2
    End Enum

    ''' <summary>
    ''' What capabilities does the serialiser support
    ''' </summary>
    ReadOnly Property Capabilities As SerialiserCapability

    ''' <summary>
    ''' Turn the event into a dictionary of name/value pairs describing its properties
    ''' </summary>
    Function ToNameValuePairs() As IDictionary(Of String, Object)

    ''' <summary>
    ''' Populate the properties of the event from a dictionary of name/value pairs
    ''' </summary>
    ''' <param name="valueDictionary">
    ''' A dictionary of name/value pairs describing the properties of the event
    ''' </param>
    Sub FromNameValuePairs(ByVal valueDictionary As IDictionary(Of String, Object))

    ''' <summary>
    ''' Save the properties of this event instance into the given stream
    ''' </summary>
    ''' <param name="streamToWriteTo">
    ''' The stream into which to write the properties of this event
    ''' </param>
    Function SaveToStream(ByVal streamToWriteTo As System.IO.Stream) As Long

    ''' <summary>
    ''' Read the event properties from the stream into this instance
    ''' </summary>
    ''' <param name="streamToRead">
    ''' The stream from which to read the properties of the event
    ''' </param>
    Sub FromStream(ByVal streamToRead As System.IO.Stream)

End Interface
