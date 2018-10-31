''' <summary>
''' Interface to uniquely identify an event
''' </summary>
Public Interface IEventInstance

    ''' <summary>
    ''' Incremental number of the event in its event stream
    ''' </summary>
    ReadOnly Property Version As UInteger

    ''' <summary>
    ''' The data component of the event
    ''' </summary>
    ReadOnly Property EventInstance As IEvent

End Interface

''' <summary>
''' Interface to uniquely identify an event that belongs to a given aggregate identifier that can be 
''' retrieved 
''' </summary>
''' <typeparam name="TAggregateKey">
''' The data type that uniquely identifies the aggregate that this event occured for
''' </typeparam>
''' <remarks>
''' The connection to the aggregate is kept outside the event itself so that we discourage logic from being built
''' that uses this aggregate key (in projections or classifiers)
''' </remarks>
Public Interface IEventInstance(Of TAggregateKey)
    Inherits IEventInstance

    ''' <summary>
    ''' The unique identifier of the aggregate to which this event occured
    ''' </summary>
    ReadOnly Property AggregateKey As TAggregateKey


End Interface