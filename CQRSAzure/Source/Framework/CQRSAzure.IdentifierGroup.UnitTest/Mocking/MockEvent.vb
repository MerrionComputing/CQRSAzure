Imports CQRSAzure.EventSourcing
Imports System.Runtime.Serialization
Imports System.Security.Permissions
Imports CQRSAzure.IdentifierGroup.UnitTest

<Serializable()>
<DomainName("UnitTest")>
Public Class MockEventTypeOne
    Implements IEvent(Of MockAggregate)

    Public Property EventOneStringProperty As String

    Public Property EventOneIntegerProperty As Integer

    Public Property EventOneEffective As DateTime

    Public ReadOnly Property Version As UInteger Implements IEvent(Of MockAggregate).Version
        Get
            Return 7
        End Get
    End Property

    Public Sub New()

    End Sub

    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        EventOneStringProperty = info.GetValue("EventOneStringProperty", GetType(String))
        EventOneIntegerProperty = info.GetValue("EventOneIntegerProperty", GetType(Integer))

    End Sub

    <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData

        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        info.AddValue("EventOneStringProperty", EventOneStringProperty)
        info.AddValue("EventOneIntegerProperty", EventOneIntegerProperty)

    End Sub

End Class

<Serializable()>
<DomainName("UnitTest")>
<EventAsOfDate(NameOf(MockEventTypeTwo.EventTwoNullableDateProperty))>
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
Public Class MockEventTypeThree
    Implements IEvent(Of MockAggregate)

    Public Property CountryCode As String

    Public Property InternationalDialingCode As String

    Public ReadOnly Property Version As UInteger Implements IEvent(Of MockAggregate).Version
        Get
            Return 3
        End Get
    End Property

    Public Sub New()

    End Sub

    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        CountryCode = info.GetValue("CountryCode", GetType(String))
        InternationalDialingCode = info.GetValue("InternationalDialingCode", GetType(String))

    End Sub

    <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData

        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        info.AddValue("CountryCode", CountryCode)
        info.AddValue("InternationalDialingCode", InternationalDialingCode)

    End Sub
End Class