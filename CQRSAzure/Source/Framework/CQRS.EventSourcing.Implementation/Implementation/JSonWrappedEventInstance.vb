
Imports System
Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Public Class JSonWrappedEventInstance
    Implements IJsonSerialisedEvent

    Public Property FullClassName As String Implements IJsonSerialisedEvent.FullClassName

    Public Property EventInstanceAsJson As Newtonsoft.Json.Linq.JObject Implements IJsonSerialisedEvent.EventInstanceAsJson


    Public Sub New()

    End Sub

    Public Sub New(ByVal eventClassName As String,
                   ByVal eventInstance As IEvent)

        FullClassName = eventClassName
        EventInstanceAsJson = Newtonsoft.Json.Linq.JObject.FromObject(eventInstance, New JsonSerializer() With {.TypeNameHandling = TypeNameHandling.All})


    End Sub

    Public Sub New(ByVal eventInstance As IEvent)
        Me.New(eventInstance.GetType().FullName, eventInstance)
    End Sub

    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        FullClassName = info.GetValue(NameOf(FullClassName), GetType(String))
        EventInstanceAsJson = info.GetValue(NameOf(EventInstanceAsJson), GetType(Newtonsoft.Json.Linq.JObject))

    End Sub

    Public Shared Function DefaultJSonSerialiserSettings() As JsonSerializerSettings

        Return New JsonSerializerSettings() With {.TypeNameHandling = TypeNameHandling.All,
            .Formatting = Formatting.None}

    End Function


End Class
