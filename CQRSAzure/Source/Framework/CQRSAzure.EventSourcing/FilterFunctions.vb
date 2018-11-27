Imports System
''' <summary>
''' Functions to allow for fine grained event filtering control
''' </summary>
''' <remarks>
''' This might, for example, allow filtering events by type and version, or by classification
''' </remarks>
Public Module FilterFunctions

#Region "Delegate declarations"

    ''' <summary>
    ''' Function to filter an event type read from an event stream
    ''' </summary>
    ''' <param name="eventType">
    ''' The event type to decide if we should read it or not
    ''' </param>
    ''' <returns>
    ''' True if our defined filter should allow this event through, otherwise false
    ''' </returns>
    Public Delegate Function EventFilterFunction(ByVal eventType As Type) As Boolean

#End Region

End Module
