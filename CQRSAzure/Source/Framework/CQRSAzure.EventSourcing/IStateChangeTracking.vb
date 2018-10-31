''' <summary>
''' An interface for implementing by any entity types for which we wish to track whether or not the state has changed
''' as a result of events processed
''' </summary>
''' <remarks>
''' This is to enable an architecture based on the propagation of changes
''' </remarks>
Public Interface IStateChangeTracking

    ''' <summary>
    ''' The set of state changes occuring in the entity when event(s) processed
    ''' </summary>
    ''' <returns>
    ''' Dictionary key is the property name that has changed
    ''' </returns>
    ReadOnly Property StateChanges As Dictionary(Of String, StateChange)

End Interface

Public Interface IStateChange(Of TFieldDataType)
    Inherits IStateChange

    ''' <summary>
    ''' The value in the field prior to event(s) being processed
    ''' </summary>
    Overloads ReadOnly Property OldValue As TFieldDataType

    ''' <summary>
    ''' The value in the field subsequent to event(s) being processed
    ''' </summary>
    Overloads ReadOnly Property NewValue As TFieldDataType

End Interface

Public Interface IStateChange

    ''' <summary>
    ''' The value in the field prior to event(s) being processed
    ''' </summary>
    ReadOnly Property OldValue As Object

    ''' <summary>
    ''' The value in the field subsequent to event(s) being processed
    ''' </summary>
    ReadOnly Property NewValue As Object

End Interface