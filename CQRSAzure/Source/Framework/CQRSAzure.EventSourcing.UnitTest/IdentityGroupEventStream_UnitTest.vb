Imports System.Runtime.Serialization
Imports System.Text
Imports CQRSAzure.EventSourcing.IdentityGroups
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class IdentityGroupSnapshotWrittenEvent_UnitTest

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As IdentityGroupSnapshotWrittenEvent = IdentityGroupSnapshotWrittenEvent.Create(DateTime.Now,
                                                                                                    "Unit test request source",
                                                                                                    "Snapshot location",
                                                                                                    "Unit test writer")

        Assert.IsNotNull(testObj)

    End Sub


    <TestMethod()>
    Public Sub IdentityGroupSnapshotWrittenEvent_RoundTrip_AsOfDate_Serialisation_TestMethod()

        Dim expected As Nullable(Of DateTime) = New DateTime(1971, 12, 22)
        Dim actual As Nullable(Of DateTime) = New DateTime(2012, 12, 20)

        Dim testObj As IIdentityGroupSnapshotWrittenEvent = IdentityGroupSnapshotWrittenEvent.Create(expected,
                                                                                                    "Unit test request source",
                                                                                                    "Snapshot location",
                                                                                                    "Unit test writer")


        Dim cmdObjDeserialised As IdentityGroupSnapshotWrittenEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.AsOfDate

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub IdentityGroupSnapshotWrittenEvent_RoundTrip_RequestSource_Serialisation_TestMethod()

        Dim expected As String = "Unit test source"
        Dim actual As String = "Not set"

        Dim testObj As IIdentityGroupSnapshotWrittenEvent = IdentityGroupSnapshotWrittenEvent.Create(DateTime.UtcNow,
                                                                                                    expected,
                                                                                                    "Snapshot location",
                                                                                                    "Unit test writer")


        Dim cmdObjDeserialised As IdentityGroupSnapshotWrittenEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.RequestSource

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub IdentityGroupSnapshotWrittenEvent_RoundTrip_Location_Serialisation_TestMethod()

        Dim expected As String = "Unit test location"
        Dim actual As String = "Not set"

        Dim testObj As IIdentityGroupSnapshotWrittenEvent = IdentityGroupSnapshotWrittenEvent.Create(DateTime.UtcNow,
                                                                                                    "Test source",
                                                                                                    expected,
                                                                                                    "Unit test writer")


        Dim cmdObjDeserialised As IdentityGroupSnapshotWrittenEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.SnapshotLocation

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub IdentityGroupSnapshotWrittenEvent_RoundTrip_Writer_Serialisation_TestMethod()

        Dim expected As String = "Unit test writer"
        Dim actual As String = "Not set"

        Dim testObj As IIdentityGroupSnapshotWrittenEvent = IdentityGroupSnapshotWrittenEvent.Create(DateTime.UtcNow,
                                                                                                    "Test source",
                                                                                                    "Test location",
                                                                                                    expected)


        Dim cmdObjDeserialised As IdentityGroupSnapshotWrittenEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.WriterType

        Assert.AreEqual(expected, actual)

    End Sub

End Class

<TestClass>
Public Class IdentityGroupMemberExcludedEvent_UnitTest

    <TestMethod>
    Public Sub Constructor_OfString_TestMethod()

        Dim testObj As IdentityGroupMemberExcludedEvent(Of SerializableMockAggregate) = IdentityGroupMemberExcludedEvent(Of SerializableMockAggregate).Create(New SerializableMockAggregate("My identity"), DateTime.UtcNow)

        Assert.IsNotNull(testObj)
    End Sub

    <TestMethod>
    Public Sub Constructor_MockGuidAggregate_TestMethod()

        Dim testObj As IdentityGroupMemberExcludedEvent(Of SerializableMockGuidAggregate) = IdentityGroupMemberExcludedEvent(Of SerializableMockGuidAggregate).Create(New SerializableMockGuidAggregate(Guid.NewGuid), DateTime.UtcNow)

        Assert.IsNotNull(testObj)
    End Sub

    <Ignore()>
    <TestMethod()>
    Public Sub IdentityGroupMemberExcludedEvent_MockAggregatee_RoundTrip_Member_Serialisation_TestMethod()

        Dim expected As SerializableMockAggregate = New SerializableMockAggregate("Expected Id")
        Dim actual As SerializableMockAggregate = New SerializableMockAggregate("Not set")

        Dim testObj As IdentityGroupMemberExcludedEvent(Of SerializableMockAggregate) = IdentityGroupMemberExcludedEvent(Of SerializableMockAggregate).Create(expected, DateTime.UtcNow)



        Dim cmdObjDeserialised As IdentityGroupMemberExcludedEvent(Of SerializableMockAggregate)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.MemberUniqueIdentifier

        'TODO: Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub IdentityGroupMemberExcludedEvent_MockAggregatee_RoundTrip_AsOfDate_Serialisation_TestMethod()

        Dim expected As Nullable(Of DateTime) = New DateTime(2017, 3, 12)
        Dim actual As Nullable(Of DateTime) = New DateTime(2027, 3, 12)

        Dim testObj As IdentityGroupMemberExcludedEvent(Of SerializableMockAggregate) = IdentityGroupMemberExcludedEvent(Of SerializableMockAggregate).Create(New SerializableMockAggregate("Expected Id"), expected)



        Dim cmdObjDeserialised As IdentityGroupMemberExcludedEvent(Of SerializableMockAggregate)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.AsOfDate

        Assert.AreEqual(expected, actual)

    End Sub

    <Ignore()>
    <TestMethod()>
    Public Sub IdentityGroupMemberExcludedEvent_MockGuidAggregatee_RoundTrip_Member_Serialisation_TestMethod()

        Dim expected As SerializableMockGuidAggregate = New SerializableMockGuidAggregate(Guid.NewGuid)
        Dim actual As SerializableMockGuidAggregate = New SerializableMockGuidAggregate(Guid.Empty)

        Dim testObj As IdentityGroupMemberExcludedEvent(Of SerializableMockGuidAggregate) = IdentityGroupMemberExcludedEvent(Of SerializableMockGuidAggregate).Create(expected, DateTime.UtcNow)



        Dim cmdObjDeserialised As IdentityGroupMemberExcludedEvent(Of SerializableMockGuidAggregate)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.MemberUniqueIdentifier

        'TODO: Assert.AreEqual(expected, actual)

    End Sub
End Class

<TestClass>
Public Class IdentityGroupMemberIncludedEvent_UnitTest

    <TestMethod>
    Public Sub Constructor_OfString_TestMethod()

        Dim testObj As IdentityGroupMemberIncludedEvent(Of SerializableMockAggregate) = IdentityGroupMemberIncludedEvent(Of SerializableMockAggregate).Create(New SerializableMockAggregate("My identity"), DateTime.UtcNow)

        Assert.IsNotNull(testObj)
    End Sub

    <TestMethod>
    Public Sub Constructor_MockGuidAggregate_TestMethod()

        Dim testObj As IdentityGroupMemberIncludedEvent(Of SerializableMockGuidAggregate) = IdentityGroupMemberIncludedEvent(Of SerializableMockGuidAggregate).Create(New SerializableMockGuidAggregate(Guid.NewGuid), DateTime.UtcNow)

        Assert.IsNotNull(testObj)
    End Sub

    <Ignore()>
    <TestMethod()>
    Public Sub IdentityGroupMemberIncludedEvent_MockAggregatee_RoundTrip_Member_Serialisation_TestMethod()

        Dim expected As SerializableMockAggregate = New SerializableMockAggregate("Expected Id")
        Dim actual As SerializableMockAggregate = New SerializableMockAggregate("Not set")

        Dim testObj As IdentityGroupMemberIncludedEvent(Of SerializableMockAggregate) = IdentityGroupMemberIncludedEvent(Of SerializableMockAggregate).Create(expected, DateTime.UtcNow)



        Dim cmdObjDeserialised As IdentityGroupMemberIncludedEvent(Of SerializableMockAggregate)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.MemberUniqueIdentifier

        'TODO: Assert.AreEqual(expected, actual)

    End Sub

    <Ignore()>
    <TestMethod()>
    Public Sub IdentityGroupMemberIncludedEvent_MockAggregatee_RoundTrip_AsOfDate_Serialisation_TestMethod()

        Dim expected As Nullable(Of DateTime) = New DateTime(2017, 3, 12)
        Dim actual As Nullable(Of DateTime) = New DateTime(2027, 3, 12)

        Dim testObj As IdentityGroupMemberIncludedEvent(Of SerializableMockAggregate) = IdentityGroupMemberIncludedEvent(Of SerializableMockAggregate).Create(New SerializableMockAggregate("Expected Id"), expected)



        Dim cmdObjDeserialised As IdentityGroupMemberIncludedEvent(Of SerializableMockAggregate)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.AsOfDate

        'TODO: Assert.AreEqual(expected, actual)

    End Sub

End Class

<TestClass()>
Public Class IdentityGroupMembersRequestedEvent_UnitTest

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As IdentityGroupMembersRequestedEvent = IdentityGroupMembersRequestedEvent.Create(DateTime.Now,
                                                                                                    "Unit test request source")

        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub IdentityGroupMembersRequestedEvent_RoundTrip_AsOfDate_TestMethod()

        Dim expected As Nullable(Of DateTime) = New DateTime(1971, 12, 22)
        Dim actual As Nullable(Of DateTime) = New DateTime(2012, 12, 20)

        Dim testObj As IdentityGroupMembersRequestedEvent = IdentityGroupMembersRequestedEvent.Create(expected,
                                                                                                    "Unit test request source")


        Dim cmdObjDeserialised As IdentityGroupMembersRequestedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.AsOfDate

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub IdentityGroupMembersRequestedEvent_RoundTrip_RequestSource_TestMethod()

        Dim expected As String = "Request Source"
        Dim actual As String = "Not set"

        Dim testObj As IdentityGroupMembersRequestedEvent = IdentityGroupMembersRequestedEvent.Create(Nothing, expected)


        Dim cmdObjDeserialised As IdentityGroupMembersRequestedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.RequestSource

        Assert.AreEqual(expected, actual)

    End Sub

End Class

<TestClass()>
Public Class IdentityGroupMembersReturnedEvent_UnitTest

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As IIdentityGroupMembersReturnedEvent = IdentityGroupMembersReturnedEvent.Create(DateTime.Now,
                                                                                                    "Unit test location",
                                                                                                    "Unit test request source")

        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub IIdentityGroupMembersReturnedEventt_RoundTrip_RequestSource_TestMethod()

        Dim expected As String = "Request Source"
        Dim actual As String = "Not set"

        Dim testObj As IIdentityGroupMembersReturnedEvent = IdentityGroupMembersReturnedEvent.Create(Nothing, "Unit test location", expected)


        Dim cmdObjDeserialised As IIdentityGroupMembersReturnedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.RequestSource

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub IdentityGroupMembersReturnedEventt_RoundTrip_Location_TestMethod()

        Dim expected As String = "Unit test location"
        Dim actual As String = "Not set"

        Dim testObj As IIdentityGroupMembersReturnedEvent = IdentityGroupMembersReturnedEvent.Create(Nothing, expected, "Request source")


        Dim cmdObjDeserialised As IIdentityGroupMembersReturnedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.GroupMembersLocation

        Assert.AreEqual(expected, actual)

    End Sub

End Class

<DomainName("UnitTest")>
<Serializable()>
Public Class SerializableMockAggregate
    Implements IAggregationIdentifier(Of String)
    Implements ISerializable

    Private m_key As String
    Public Sub SetKey(key As String) Implements IAggregationIdentifier(Of String).SetKey
        m_key = key
    End Sub

    Public Function GetKey() As String Implements IAggregationIdentifier(Of String).GetKey
        Return m_key
    End Function

    Public Function GetAggregateIdentifier() As String Implements IAggregationIdentifier.GetAggregateIdentifier
        Return m_key
    End Function

    Public Sub New(ByVal uniqueid As String)
        m_key = uniqueid
    End Sub

    Public Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData

        If Not (info Is Nothing) Then
            info.AddValue("Key", GetKey)
        End If

    End Sub

    Public Sub New(info As SerializationInfo,
               context As StreamingContext)

        If Not (info Is Nothing) Then
            'Populate the members of the event from the context
            m_key = info.GetString("Key")
        End If

    End Sub

End Class


<DomainName("UnitTest")>
<Serializable()>
Public Class SerializableMockGuidAggregate
    Implements IAggregationIdentifier(Of Guid)
    Implements ISerializable

    Private converter As New GUIDKeyConverter()

    <DataMember(Name:="Key")>
    Private m_key As Guid
    Public Sub SetKey(key As Guid) Implements IAggregationIdentifier(Of Guid).SetKey
        m_key = key
    End Sub

    Public Function GetAggregateIdentifier() As String Implements IAggregationIdentifier.GetAggregateIdentifier
        Return converter.ToUniqueString(m_key)
    End Function

    Public Function GetKey() As Guid Implements IAggregationIdentifier(Of Guid).GetKey
        Return m_key
    End Function

    Public Sub New(ByVal uniquekey As Guid)
        m_key = uniquekey
    End Sub

    Public Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData

        If Not (info Is Nothing) Then
            info.AddValue("Key", GetKey)
        End If

    End Sub

    Public Sub New(info As SerializationInfo,
           context As StreamingContext)

        If Not (info Is Nothing) Then
            'Populate the members of the event from the context
            m_key = info.GetValue("Key", GetType(Guid))
        End If

    End Sub
End Class