﻿Imports System.Runtime.Serialization
Imports System.Security.Permissions
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace Azure.Table.Untyped

    ''' <summary>
    ''' Turns an event serialised in an Azure table to a JSON version that can be processed 
    ''' by the "untyped" event stream processors
    ''' </summary>
    <JsonObject(Title:="Table Json Wrapped Event")>
    Public Class TableJsonWrappedEvent
        Implements IEventInstance

        Private m_eventName As String
        ''' <summary>
        ''' The class name of the event
        ''' </summary>
        Public Property EventName As String
            Get
                Return m_eventName
            End Get
            Set(value As String)
                m_eventName = value
            End Set
        End Property


        Private m_sequence As Long
        ''' <summary>
        ''' The sequence number of this record
        ''' </summary>
        Public Property Sequence As Long
            Get
                Return m_sequence
            End Get
            Set(value As Long)
                m_sequence = value
            End Set
        End Property

        Private m_version As UInteger
        Public Property Version As UInteger Implements IEventInstance.Version
            Get
                Return m_version
            End Get
            Set(value As UInteger)
                m_version = value
            End Set
        End Property



        Private m_timestamp As DateTime
        Public Property Timestamp As DateTime
            Get
                Return m_timestamp
            End Get
            Set(value As DateTime)
                m_timestamp = value
            End Set
        End Property


        Private m_eventInstance As IEvent
        <JsonIgnore()>
        Public ReadOnly Property EventInstance As IEvent Implements IEventInstance.EventInstance
            Get
                If (m_eventInstance Is Nothing) Then
                    m_eventInstance = New Blob.JSonWrappedEventInstance() With {.FullClassName = EventInstanceClassName, .EventInstanceAsJson = EventInstanceAsJson}
                End If
                Return m_eventInstance
            End Get
        End Property

        Public Property EventInstanceAsJson As Newtonsoft.Json.Linq.JObject

        Public Property EventInstanceClassName As String


        Public Sub New(ByVal sequenceInit As Long,
                ByVal versionInit As UInteger,
                ByVal timestampInit As DateTime,
                ByVal eventInstanceInit As IEvent)

            If (eventInstanceInit IsNot Nothing) Then
                ' Save the full class name so we can deserialise it later
                EventInstanceClassName = eventInstanceInit.GetType().FullName
                ' and write it o JSON string
                EventInstanceAsJson = ToJSon(eventInstanceInit)
            End If

            m_eventName = EventNameAttribute.GetEventName(eventInstanceInit)
            m_sequence = sequenceInit
            m_version = versionInit
            m_timestamp = timestampInit
            m_eventInstance = eventInstanceInit

        End Sub

        Private Function ToJSon(eventInstanceInit As IEvent) As Newtonsoft.Json.Linq.JObject

            Return Newtonsoft.Json.Linq.JObject.FromObject(eventInstanceInit,
                                                           New JsonSerializer() With {.TypeNameHandling = TypeNameHandling.All})

        End Function

        Private Overloads Function FromJSon(ByVal jsonString As String) As IEvent

            If (Not String.IsNullOrWhiteSpace(jsonString)) Then
                If (Not String.IsNullOrWhiteSpace(Me.EventInstanceClassName)) Then
                    Dim tRet As Type = Type.GetType(Me.EventInstanceClassName)
                    If (tRet IsNot Nothing) Then
                        Return JsonConvert.DeserializeObject(jsonString, tRet)
                    End If
                End If
            End If

            Return Nothing

        End Function

        Private Overloads Function FromJson(ByVal jsonObject As Newtonsoft.Json.Linq.JObject) As IEvent

            If (Not String.IsNullOrWhiteSpace(Me.EventInstanceClassName)) Then
                Dim tRet As Type = Type.GetType(Me.EventInstanceClassName)
                If (tRet IsNot Nothing) Then
                    Return jsonObject
                End If
            End If

            Return Nothing

        End Function

        ''' <summary>
        ''' Empty constructor for serialisation
        ''' </summary>
        Public Sub New()

        End Sub


        Public Function ToJSonText() As String

            Return JsonConvert.SerializeObject(Me, DefaultJSonSerialiserSettings())

        End Function

        Public Shared Function DefaultJSonSerialiserSettings() As JsonSerializerSettings

            Return New JsonSerializerSettings() With {.TypeNameHandling = TypeNameHandling.All,
                .Formatting = Formatting.None}

        End Function

    End Class


End Namespace