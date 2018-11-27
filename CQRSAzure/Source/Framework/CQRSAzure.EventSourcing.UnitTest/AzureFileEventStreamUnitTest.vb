Imports System.Text
Imports NUnit.Framework

Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.File
Imports CQRSAzure.EventSourcing.UnitTest.Mocking

<TestFixture()>
Public Class AzureFileEventStreamUnitTest

    Public Const TEST_AGGREGATE_IDENTIFIER As String = "201614569083"
    Public Const TEST_AGGREGATE_GUID As String = "d0107927-c778-39b0-9590-e0aaffe018bc"

    <TestCase()>
    Public Sub Reader_Constructor_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As FileEventStreamReader(Of MockAggregate, String) = FileEventStreamReader(Of MockAggregate, String).Create(testAgg)
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub Writer_Constructor_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As FileEventStreamWriter(Of MockAggregate, String) = FileEventStreamWriter(Of MockAggregate, String).Create(testAgg)
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub Writer_WriteEvent_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As FileEventStreamWriter(Of MockAggregate, String) = FileEventStreamWriter(Of MockAggregate, String).Create(testAgg)
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My file test", .EventOneIntegerProperty = 123})
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My second file test", .EventOneIntegerProperty = 10})

        Assert.IsTrue(testObj.RecordCount > 0)

    End Sub

    <TestCase()>
    Public Sub Writer_WriteEventTwo_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As FileEventStreamWriter(Of MockAggregate, String) = FileEventStreamWriter(Of MockAggregate, String).Create(testAgg)
        testObj.AppendEvent(New MockEventTypeTwo() With {.EventTwoNullableDateProperty = DateTime.UtcNow, .EventTwoStringProperty = "My file test two", .EventTwoDecimalProperty = 123.45})

        Assert.IsTrue(testObj.RecordCount > 0)

    End Sub

    <TestCase()>
    Public Sub Reader_ReadEvents_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As FileEventStreamReader(Of MockAggregate, String) = FileEventStreamReader(Of MockAggregate, String).Create(testAgg)
        Dim ret = testObj.GetEvents()

        Assert.IsNotNull(ret)

    End Sub

    <TestCase()>
    Public Sub Reader_ReadEventsWithContext_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As FileEventStreamReader(Of MockAggregate, String) = FileEventStreamReader(Of MockAggregate, String).Create(testAgg)
        Dim ret = testObj.GetEventsWithContext()

        Assert.IsNotNull(ret)

    End Sub

    <TestCase()>
    Public Async Function WriterToReader_RoundTrip_TestMethod() As Task

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testReader As FileEventStreamReader(Of MockAggregate, String) = FileEventStreamReader(Of MockAggregate, String).Create(testAgg)
        Dim testWriter As FileEventStreamWriter(Of MockAggregate, String) = FileEventStreamWriter(Of MockAggregate, String).Create(testAgg)

        Await testWriter.Reset()
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoNullableDateProperty = DateTime.UtcNow, .EventTwoStringProperty = "My file test two", .EventTwoDecimalProperty = 123.45})

        Dim evetList = Await testReader.GetEvents()
        Dim lastEvent As IEvent(Of MockAggregate) = evetList.LastOrDefault()

        Assert.IsNotNull(lastEvent)

    End Function


    <TestCase()>
    Public Async Function GetAllStreamKeys_NoDate_TestMethod() As Task

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        Dim testObj = FileEventStreamReader(Of MockAggregate, String).Create(New MockAggregate(AzureBlobEventStreamUnitTest.TEST_AGGREGATE_IDENTIFIER))
        Dim testProvider As IEventStreamProvider(Of MockAggregate, String) = FileEventStreamProviderFactory.Create(Of MockAggregate, String)()

        Dim values As IEnumerable(Of String) = Await testProvider.GetAllStreamKeys()
        actual = values.Contains(AzureBlobEventStreamUnitTest.TEST_AGGREGATE_IDENTIFIER)

        Assert.AreEqual(expected, actual)

    End Function

    Public GUIDKEY As Guid = New Guid("2c67ce7f-3349-4fd2-94c2-d0fb98e0028f")

    <TestCase()>
    Public Async Function GetAllStreamKeys_GUIDKey_NoDate_TestMethod() As Task

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        Dim key As Guid = GUIDKEY

        'create the stream and write an event to it
        Dim testAgg As New MockGuidAggregate(key)
        Dim testWriter = FileEventStreamWriterFactory.Create(testAgg, key)
        Await testWriter.AppendEvent(New MockGuidEventTypeTwo() With {.EventTwoDecimalProperty = 75, .EventTwoStringProperty = DateTime.UtcNow.ToShortDateString()})

        Dim testProvider = FileEventStreamProviderFactory.Create(Of MockGuidAggregate, Guid)()

        Dim values As IEnumerable(Of Guid) = Await testProvider.GetAllStreamKeys()
        actual = values.Contains(key)

        Assert.AreEqual(expected, actual)

    End Function

    <TestCase()>
    Public Async Function GetAllStreamKeys_GUIDKey_FutureDate_TestMethod() As Task

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        Dim asOfdate As DateTime = New DateTime(2037, 1, 1)

        Dim key As Guid = GUIDKEY
        Dim testAgg As New MockGuidAggregate(key)
        Dim testWriter = FileEventStreamWriterFactory.Create(testAgg, key)
        Await testWriter.AppendEvent(New MockGuidEventTypeTwo() With {.EventTwoDecimalProperty = 75, .EventTwoStringProperty = DateTime.UtcNow.ToShortDateString()})

        Dim testObj = FileEventStreamReader(Of MockGuidAggregate, Guid).Create(New MockGuidAggregate(key))
        Dim testProvider As IEventStreamProvider(Of MockGuidAggregate, Guid) = FileEventStreamProviderFactory.Create(Of MockGuidAggregate, Guid)()

        Dim values As IEnumerable(Of Guid) = Await testProvider.GetAllStreamKeys(asOfdate)
        actual = values.Contains(key)

        Assert.AreEqual(expected, actual)

    End Function
End Class