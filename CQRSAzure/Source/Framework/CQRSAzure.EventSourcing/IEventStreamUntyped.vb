Imports System
''' <summary>
''' A stream of zero or more events that have occured for a particular unique instance of an aggregate 
''' identifier
''' </summary>
Public Interface IEventStreamUntyped
    Inherits IEventStreamUntypedIdentity

    ''' <summary>
    ''' The number of record in this event stream
    ''' </summary>
    ReadOnly Property RecordCount As UInt64

    ''' <summary>
    ''' When was the last record written to this event stream
    ''' </summary>
    ReadOnly Property LastAddition As Nullable(Of DateTime)

End Interface

''' <summary>
''' An interface that can be used to uniquely identify an event stream without knowing its aggregate class
''' </summary>
''' <remarks>
''' This can be used to access an event stream from a language or system that does not understand the .NET type system
''' </remarks>
Public Interface IEventStreamUntypedIdentity

    ''' <summary>
    ''' The domain to which the aggregate belongs
    ''' </summary>
    ReadOnly Property DomainName As String

    ''' <summary>
    ''' The type of aggregate class the event stream belongs
    ''' </summary>
    ReadOnly Property AggregateTypeName As String

    ''' <summary>
    ''' The unique identifier of the aggregate instance to which the event stream pertains
    ''' </summary>
    ReadOnly Property InstanceKey As String

End Interface