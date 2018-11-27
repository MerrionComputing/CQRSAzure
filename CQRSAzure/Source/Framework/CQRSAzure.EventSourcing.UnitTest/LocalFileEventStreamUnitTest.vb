Imports System.Text
Imports NUnit.Framework

Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Local.File

#If LOCAL_MACHINE Then

<TestFixture()>
Public Class LocalFileEventStreamUnitTest


    Public Const TEST_AGGREGATE_IDENTIFIER As String = "LOCALFILE.OK.331.987"
    Public Const TEST_AGGREGATE_GUID As String = "d0107927-c778-49b0-9490-e0aaffe018bc"

    Dim settings_nvp As ILocalFileSettings = New CQRSAzureEventSourcingLocalFileSettingsElement() With {.UnderlyingSerialiser = ILocalFileSettings.SerialiserType.NameValuePairs,
        .EventStreamRootFolder = "D:\Data\CQRS on Azure\Event Streams\NVP",
        .SnapshotsRootFolder = "D:\Data\CQRS on Azure\Snapshots\NVP"}

    Dim settings_bin As ILocalFileSettings = New CQRSAzureEventSourcingLocalFileSettingsElement() With {.UnderlyingSerialiser = ILocalFileSettings.SerialiserType.Binary,
        .EventStreamRootFolder = "D:\Data\CQRS on Azure\Event Streams",
        .SnapshotsRootFolder = "D:\Data\CQRS on Azure\Snapshots"}

    <TestCase()>
    Public Sub Reader_Constructor_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As IEventStreamReader(Of MockAggregate, String) = LocalFileEventStreamReader(Of MockAggregate, String).Create(testAgg)
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub Reader_KeyRoundTrip_TestMethod()

        Dim expected As String = TEST_AGGREGATE_IDENTIFIER
        Dim actual As String = "not set"

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As IEventStreamReader(Of MockAggregate, String) = LocalFileEventStreamReader(Of MockAggregate, String).Create(testAgg)

        actual = testObj.Key

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub Reader_Filename_TestMethod()

        Dim actual As String = "not set"

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As LocalFileEventStreamReader(Of MockAggregate, String) = LocalFileEventStreamReader(Of MockAggregate, String).Create(testAgg)

        actual = testObj.Filename

        Assert.AreNotEqual("not set", actual)

    End Sub


    <TestCase()>
    Public Sub Writer_WriteEvent_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As LocalFileEventStreamWriter(Of MockAggregate, String) = LocalFileEventStreamWriter(Of MockAggregate, String).Create(testAgg, settings_bin)
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 123})

        Assert.IsTrue(testObj.RecordCount > 0)

    End Sub

    <TestCase()>
    Public Sub Writer_NVP_WriteEvent_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As LocalFileEventStreamWriter(Of MockAggregate, String) = LocalFileEventStreamWriter(Of MockAggregate, String).Create(testAgg, settings_nvp)
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 123})

        Assert.IsTrue(testObj.RecordCount > 0)

    End Sub

    <TestCase()>
    Public Sub Writer_WriteMultipleEvent_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As LocalFileEventStreamWriter(Of MockAggregate, String) = LocalFileEventStreamWriter(Of MockAggregate, String).Create(testAgg, settings_bin)

        Dim events As New List(Of IEvent(Of MockAggregate))

        events.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 123})
        events.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test 2", .EventOneIntegerProperty = 124})
        events.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test 3", .EventOneIntegerProperty = 125})
        events.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test 4", .EventOneIntegerProperty = 126})

        testObj.AppendEvents(0, events)

        Assert.IsTrue(testObj.RecordCount > 0)

    End Sub

    <TestCase()>
    Public Sub Writer_NVP_WriteMultipleEvent_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As LocalFileEventStreamWriter(Of MockAggregate, String) = LocalFileEventStreamWriter(Of MockAggregate, String).Create(testAgg, settings_nvp)

        Dim events As New List(Of IEvent(Of MockAggregate))

        events.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 123})
        events.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test 2", .EventOneIntegerProperty = 124})
        events.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test 3", .EventOneIntegerProperty = 125})
        events.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test 4", .EventOneIntegerProperty = 126})

        testObj.AppendEvents(0, events)

        Assert.IsTrue(testObj.RecordCount > 0)

    End Sub

    <TestCase()>
    Public Sub Reader_ReadEvents_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As LocalFileEventStreamReader(Of MockAggregate, String) = LocalFileEventStreamReader(Of MockAggregate, String).Create(testAgg, settings_bin)
        Dim ret = testObj.GetEvents()

        Assert.IsNotNull(ret)

    End Sub

    <TestCase()>
    Public Sub Reader_NVP_ReadEvents_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As LocalFileEventStreamReader(Of MockAggregate, String) = LocalFileEventStreamReader(Of MockAggregate, String).Create(testAgg, settings_nvp)
        Dim ret = testObj.GetEvents()

        Assert.IsNotNull(ret)

    End Sub

    <TestCase()>
    Public Sub Reader_GetAllKeys_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As LocalFileEventStreamReader(Of MockAggregate, String) = LocalFileEventStreamReader(Of MockAggregate, String).Create(testAgg, settings_bin)
        Dim allKeys = testObj.GetAllStreamKeys()

        Assert.IsTrue(allKeys.Contains(TEST_AGGREGATE_IDENTIFIER))

    End Sub

    <TestCase()>
    Public Sub Reader_NVP_GetAllKeys_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As LocalFileEventStreamReader(Of MockAggregate, String) = LocalFileEventStreamReader(Of MockAggregate, String).Create(testAgg, settings_nvp)
        Dim allKeys = testObj.GetAllStreamKeys()

        Assert.IsTrue(allKeys.Contains(TEST_AGGREGATE_IDENTIFIER))

    End Sub

    <TestCase()>
    Public Sub Reader_Writer_RoundTrip_TestMethod()

        Dim expected As String = "Mock event test 2"
        Dim actual As String = "not set"

        Dim testAggregate As New MockGuidAggregate(New Guid(TEST_AGGREGATE_GUID))

        Dim testwriter As LocalFileEventStreamWriter(Of MockGuidAggregate, Guid) = LocalFileEventStreamWriter(Of MockGuidAggregate, Guid).Create(testAggregate, settings_bin)
        testwriter.AppendEvent(New MockGuidEventTypeTwo() With {.EventTwoStringProperty = expected, .EventTwoDecimalProperty = 13.97})

        Dim testreader As LocalFileEventStreamReader(Of MockGuidAggregate, Guid) = LocalFileEventStreamReader(Of MockGuidAggregate, Guid).Create(testAggregate, settings_bin)

        Dim allEvents = testreader.GetEvents()
        If (allEvents.Count > 0) Then
            Dim returnedEvent = TryCast(allEvents.LastOrDefault(), MockGuidEventTypeTwo)
            actual = returnedEvent.EventTwoStringProperty
        End If

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub Reader_NVP_Writer_RoundTrip_TestMethod()

        Dim expected As String = "Mock event test 2"
        Dim actual As String = "not set"

        Dim testAggregate As New MockGuidAggregate(New Guid(TEST_AGGREGATE_GUID))

        Dim testwriter As LocalFileEventStreamWriter(Of MockGuidAggregate, Guid) = LocalFileEventStreamWriter(Of MockGuidAggregate, Guid).Create(testAggregate, settings_nvp)
        testwriter.AppendEvent(New MockGuidEventTypeTwo() With {.EventTwoStringProperty = expected, .EventTwoDecimalProperty = 13.97})

        Dim testreader As LocalFileEventStreamReader(Of MockGuidAggregate, Guid) = LocalFileEventStreamReader(Of MockGuidAggregate, Guid).Create(testAggregate, settings_nvp)

        Dim allEvents = testreader.GetEvents()
        If (allEvents.Count > 0) Then
            Dim returnedEvent = TryCast(allEvents.LastOrDefault(), MockGuidEventTypeTwo)
            actual = returnedEvent.EventTwoStringProperty
        End If

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub Guid_Allkeys_TestMethod()

        Dim expected As Guid = New Guid(TEST_AGGREGATE_GUID)


        Dim testAggregate As New MockGuidAggregate(expected)

        Dim testwriter As LocalFileEventStreamWriter(Of MockGuidAggregate, Guid) = LocalFileEventStreamWriter(Of MockGuidAggregate, Guid).Create(testAggregate)
        testwriter.AppendEvent(New MockGuidEventTypeTwo() With {.EventTwoStringProperty = "Test with guid", .EventTwoDecimalProperty = 13.97})

        Dim testreader As LocalFileEventStreamReader(Of MockGuidAggregate, Guid) = LocalFileEventStreamReader(Of MockGuidAggregate, Guid).Create(testAggregate)

        Dim allKeys = testreader.GetAllStreamKeys()

        Assert.IsTrue(allKeys.Contains(expected))

    End Sub

    <TestCase()>
    Public Sub LocalFileProjectionSnapshotReader_Constructor_NoSettings_TestMethod()

        Dim testAggregate As New MockGuidAggregate(New Guid(TEST_AGGREGATE_GUID))
        Dim testprojection As New MockProjectionWithSnapshots()


        Dim testObj As IProjectionSnapshotReader(Of MockGuidAggregate, Guid, MockProjectionWithSnapshots) = LocalFileProjectionSnapshotReaderFactory.Create(Of MockGuidAggregate, Guid, MockProjectionWithSnapshots)(testAggregate, testAggregate.GetKey(), testprojection)

        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub LocalFileProjectionSnapshotReader_Constructor_DefaultSettings_TestMethod()

        Dim testAggregate As New MockGuidAggregate(New Guid(TEST_AGGREGATE_GUID))
        Dim testprojection As New MockProjectionWithSnapshots()

        Dim testSettings As ILocalFileSettings = New CQRSAzureEventSourcingLocalFileSettingsElement() With {.UnderlyingSerialiser = ILocalFileSettings.SerialiserType.Binary}

        Dim testObj As IProjectionSnapshotReader(Of MockGuidAggregate, Guid, MockProjectionWithSnapshots) = LocalFileProjectionSnapshotReaderFactory.Create(Of MockGuidAggregate, Guid, MockProjectionWithSnapshots)(testAggregate, testAggregate.GetKey(), testprojection, testSettings)

        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub LocalFileProjectionSnapshotWriter_Constructor_NoSettings_TestMethod()

        Dim testAggregate As New MockGuidAggregate(New Guid(TEST_AGGREGATE_GUID))
        Dim testprojection As New MockProjectionWithSnapshots()


        Dim testObj As IProjectionSnapshotWriter(Of MockGuidAggregate, Guid, MockProjectionWithSnapshots) = LocalFileProjectionSnapshotWriterFactory.Create(Of MockGuidAggregate, Guid, MockProjectionWithSnapshots)(testAggregate, testAggregate.GetKey(), testprojection)

        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub LocalFileProjectionSnapshotBase_MakeFilename_TestMethod()

        Dim expected As String = "duncan-s.snapshot.1234"
        Dim actual As String = "not set"

        Dim testObj As LocalFileProjectionSnapshotReader(Of MockAggregate, String, MockProjectionWithSnapshots)
        testObj = LocalFileProjectionSnapshotReaderFactory.Create(New MockAggregate("duncan's"), "duncan's", New MockProjectionWithSnapshots())

        actual = testObj.MakeFilename(1234)

        Assert.AreEqual(expected, actual)

    End Sub

End Class

#End If