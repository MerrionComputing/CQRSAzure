
Imports System.Reflection
''' <summary>
''' An attribute to mark one property of an event as being the "as-of date" of the event 
''' (This being the real-world date and time as of which the event occured)
''' </summary>
''' <remarks>
''' The named property is the one that will provide teh as-of date for any given instance
''' </remarks>
<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=False)>
Public NotInheritable Class EventAsOfDateAttribute
    Inherits Attribute

    Private ReadOnly m_propertyName As String
    ''' <summary>
    ''' The name of this event
    ''' </summary>
    Public ReadOnly Property PropertyName As String
        Get
            Return m_propertyName
        End Get
    End Property

#Region "Constructors"
    Public Sub New(ByVal propertyNameIn As String)
        m_propertyName = propertyNameIn
    End Sub
#End Region

    Public Shared Function GetAsOfDate(ByVal eventInstance As IEvent) As Nullable(Of DateTime)


        Dim eventType As Type = eventInstance.GetType()

        For Each eventNameAttr As EventAsOfDateAttribute In eventType.GetCustomAttributes(GetType(EventAsOfDateAttribute), False)
            If (Not String.IsNullOrWhiteSpace(eventNameAttr.PropertyName)) Then
                Dim pi As PropertyInfo = eventType.GetProperty(eventNameAttr.PropertyName)
                If (pi IsNot Nothing) Then
#Region "Tracing"
                    EventSourcing.LogVerboseInfo(eventInstance.GetType().ToString() & " has As-Of Date attribute set to use " & pi.Name)
#End Region
                    'make sure the property type is valid
                    If IsValidDateType(pi.PropertyType) Then
                        Dim objValue = pi.GetValue(eventInstance)
                        If (objValue IsNot Nothing) Then
#Region "Tracing"
                            EventSourcing.LogVerboseInfo(" As-Of Date set to " & objValue.ToString())
#End Region
                            Return objValue
                        End If
                    End If
                End If
            End If
            Exit For
        Next

#Region "Tracing"
        EventSourcing.LogVerboseInfo(eventInstance.GetType().ToString() & " has no As-Of Date attribute set  ")
#End Region

        'If not found, no as-of date property is set
        Return Nothing

    End Function

    Private Shared Function IsValidDateType(propertyType As Type) As Boolean

        If (propertyType Is GetType(Date)) Then
            Return True
        End If

        If (propertyType Is GetType(DateTime)) Then
            Return True
        End If

        If (propertyType Is GetType(Nullable(Of Date))) Then
            Return True
        End If

        If (propertyType Is GetType(Nullable(Of DateTime))) Then
            Return True
        End If

#Region "Tracing"
        EventSourcing.LogError(propertyType.ToString() & " is not a valid date type for setting the event As-Of Date")
#End Region

        Return False

    End Function
End Class
