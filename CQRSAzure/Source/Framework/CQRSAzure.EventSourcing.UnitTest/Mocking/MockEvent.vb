Imports System.IO
Imports System.Runtime.Serialization
Imports System.Security.Permissions
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.UnitTest

Namespace Mocking


    <Serializable()>
    <DomainName("UnitTest")>
    Public Class MockEventTypeOne
        Implements IEvent(Of MockAggregate)
        Implements IEventSerializer

        Public Property EventOneStringProperty As String

        Public Property EventOneIntegerProperty As Integer

        Public ReadOnly Property Version As UInteger Implements IEvent(Of MockAggregate).Version
            Get
                Return 0
            End Get
        End Property

        Public ReadOnly Property Capabilities As IEventSerializer.SerialiserCapability Implements IEventSerializer.Capabilities
            Get
                Return (IEventSerializer.SerialiserCapability.Stream Or IEventSerializer.SerialiserCapability.NameValuePairs)
            End Get
        End Property

        Public Sub New()

        End Sub

        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            EventOneStringProperty = info.GetValue("EventOneStringProperty", GetType(String))
            EventOneIntegerProperty = info.GetInt32("EventOneIntegerProperty")

        End Sub

        <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
        Public Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData

            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            info.AddValue("EventOneStringProperty", EventOneStringProperty)
            info.AddValue("EventOneIntegerProperty", EventOneIntegerProperty)

        End Sub

        Public Function ToNameValuePairs() As IDictionary(Of String, Object) Implements IEventSerializer.ToNameValuePairs

            Dim retValues As New Dictionary(Of String, Object)()
            retValues.Add(NameOf(EventOneIntegerProperty), EventOneIntegerProperty)
            retValues.Add(NameOf(EventOneStringProperty), EventOneStringProperty)
            Return retValues

        End Function

        Public Sub FromNameValuePairs(valueDictionary As IDictionary(Of String, Object)) Implements IEventSerializer.FromNameValuePairs

            If (valueDictionary IsNot Nothing) Then
                If (valueDictionary.ContainsKey(NameOf(EventOneIntegerProperty))) Then
                    EventOneIntegerProperty = valueDictionary(NameOf(EventOneIntegerProperty))
                End If
                If (valueDictionary.ContainsKey(NameOf(EventOneStringProperty))) Then
                    EventOneStringProperty = valueDictionary(NameOf(EventOneStringProperty))
                End If
            End If

        End Sub

        Public Function SaveToStream(streamToWriteTo As Stream) As Long Implements IEventSerializer.SaveToStream

            If (streamToWriteTo IsNot Nothing) Then
                Dim startPosition As Long = streamToWriteTo.Position

                Using writer As New System.IO.BinaryWriter(streamToWriteTo, System.Text.UTF32Encoding.UTF32)
                    writer.Seek(startPosition, SeekOrigin.Begin)
                    writer.Write(EventOneIntegerProperty)
                    If Not String.IsNullOrEmpty(EventOneStringProperty) Then
                        writer.Write(EventOneStringProperty)
                    Else
                        writer.Write("")
                    End If
                    Return (writer.BaseStream.Position - startPosition)
                End Using
            End If

            Return Nothing

        End Function

        Public Sub FromStream(streamToRead As Stream) Implements IEventSerializer.FromStream

            If (streamToRead IsNot Nothing) Then
                Using reader As New System.IO.BinaryReader(streamToRead, System.Text.UTF32Encoding.UTF32)
                    EventOneIntegerProperty = reader.ReadInt32()
                    EventOneStringProperty = reader.ReadString()
                End Using
            End If

        End Sub
    End Class

    <Serializable()>
    <DomainName("UnitTest")>
    <EventAsOfDateAttribute(NameOf(MockEventTypeTwo.EventTwoNullableDateProperty))>
    Public Class MockEventTypeTwo
        Implements IEvent(Of MockAggregate)

        Public Property EventTwoStringProperty As String

        Public Property EventTwoDecimalProperty As Decimal

        Public Property EventTwoNullableDateProperty As Nullable(Of DateTime)

        Public ReadOnly Property Version As UInteger Implements IEvent(Of MockAggregate).Version
            Get
                Return 1
            End Get
        End Property

        Public Sub New()

        End Sub

        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            EventTwoStringProperty = info.GetValue("EventTwoStringProperty", GetType(String))
            EventTwoDecimalProperty = info.GetValue("EventTwoDecimalProperty", GetType(Decimal))
            EventTwoNullableDateProperty = info.GetValue("EventTwoNullableDateProperty", GetType(Nullable(Of DateTime)))

        End Sub

        <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
        Public Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData

            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            info.AddValue("EventTwoStringProperty", EventTwoStringProperty)
            info.AddValue("EventTwoDecimalProperty", EventTwoDecimalProperty)
            info.AddValue("EventTwoNullableDateProperty", EventTwoNullableDateProperty)

        End Sub

    End Class

    <Serializable()>
    <DomainName("UnitTest")>
    <EventAsOfDateAttribute(NameOf(MockEventTypeTwo.EventTwoNullableDateProperty))>
    Public Class MockGuidEventTypeTwo
        Implements IEvent(Of MockGuidAggregate)

        Public Property EventTwoStringProperty As String

        Public Property EventTwoDecimalProperty As Decimal

        Public Property EventTwoNullableDateProperty As Nullable(Of DateTime)

        Public ReadOnly Property Version As UInteger Implements IEvent(Of MockGuidAggregate).Version
            Get
                Return 1
            End Get
        End Property

        Public Sub New()

        End Sub

        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            EventTwoStringProperty = info.GetValue("EventTwoStringProperty", GetType(String))
            EventTwoDecimalProperty = info.GetValue("EventTwoDecimalProperty", GetType(Decimal))
            EventTwoNullableDateProperty = info.GetValue("EventTwoNullableDateProperty", GetType(Nullable(Of DateTime)))

        End Sub

        <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
        Public Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData

            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            info.AddValue("EventTwoStringProperty", EventTwoStringProperty)
            info.AddValue("EventTwoDecimalProperty", EventTwoDecimalProperty)
            info.AddValue("EventTwoNullableDateProperty", EventTwoNullableDateProperty)

        End Sub

    End Class

End Namespace