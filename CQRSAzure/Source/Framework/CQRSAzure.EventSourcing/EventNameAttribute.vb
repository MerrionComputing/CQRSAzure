''' <summary>
''' Attribute to tag an IEvent based class with a specific event name
''' </summary>
''' <remarks>
''' This is useful if we want to use event names that are not valid .NET class names
''' </remarks>
<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
Public NotInheritable Class EventNameAttribute
    Inherits Attribute

    Private ReadOnly m_eventName As String
    ''' <summary>
    ''' The name of this event
    ''' </summary>
    Public ReadOnly Property EventName As String
        Get
            Return m_eventName
        End Get
    End Property

#Region "Constructors"
    Public Sub New(ByVal eventNameIn As String)
        m_eventName = eventNameIn
    End Sub
#End Region

    Public Shared Function GetEventName(ByVal eventObjectType As Type) As String

        For Each eventNameAttr As EventNameAttribute In eventObjectType.GetCustomAttributes(GetType(EventNameAttribute), True)
#Region "Tracing"
            EventSourcing.LogVerboseInfo(eventObjectType.ToString() & " has the event name attribute set to " & eventNameAttr.EventName)
#End Region
            Return eventNameAttr.EventName
        Next

#Region "Tracing"
        EventSourcing.LogVerboseInfo(eventObjectType.ToString() & " has no event name attribute set - defaulting to class name")
#End Region

        ' No attribute - return the class name
        Return eventObjectType.Name

    End Function

    Public Shared Function GetEventName(ByVal eventObject As Object) As String

        If (eventObject IsNot Nothing) Then
            Return GetEventName(eventObject.GetType())
        Else
            Return String.Empty
        End If

    End Function

End Class
