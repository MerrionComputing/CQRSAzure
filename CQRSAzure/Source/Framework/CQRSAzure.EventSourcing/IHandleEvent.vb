''' <summary>
''' The projection handles this particular type of event
''' </summary>
''' <typeparam name="TEvent">
''' The event being handled 
''' </typeparam>
Public Interface IHandleEvent(Of In TEvent As IEvent)

    ''' <summary>
    ''' Handle the particular event as part of a projection
    ''' </summary>
    ''' <param name="eventHandled">
    ''' The specific event to handle
    ''' </param>
    Sub HandleEvent(ByVal eventHandled As TEvent)

End Interface
