Imports CQRSAzure.EventSourcing.Azure.File
Imports CQRSAzure.EventSourcing.UnitTest.Mocking
Imports NUnit.Framework

<TestFixture()>
Public Class AzureFileEventStreamWriterUntypedUnitTest


    Public Const TEST_AGGREGATE_IDENTIFIER As String = "201614599088"
    Public Const TEST_AGGREGATE_GUID As String = "d0107927-c768-39b0-9A00-e0aaffe018bc"

    <TestCase()>
    Public Sub Writer_Constructor_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As FileEventStreamWriterUntyped = New FileEventStreamWriterUntyped(testAgg)
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Async Function Writer_WriteEvent_TestMethod() As Task

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As FileEventStreamWriterUntyped = New FileEventStreamWriterUntyped(testAgg)
        Await testObj.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 123})

        Assert.IsTrue(testObj.RecordCount > 0)

    End Function


    <TestCase()>
    Public Async Function Writer_WriteMultipleEvents_TestMethod() As Task

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As FileEventStreamWriterUntyped = New FileEventStreamWriterUntyped(testAgg)
        Await testObj.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My file test", .EventOneIntegerProperty = 123})
        Await testObj.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My second file test", .EventOneIntegerProperty = 10})

        Assert.IsTrue(testObj.RecordCount > 0)

    End Function

    <TestCase()>
    Public Sub Reader_Constructor_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As FileEventStreamReaderUntyped = New FileEventStreamReaderUntyped(testAgg)
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub Reader_ReadEvents_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As FileEventStreamReaderUntyped = New FileEventStreamReaderUntyped(testAgg)
        Dim ret = testObj.GetEvents()

        Assert.IsNotNull(ret)

    End Sub

    <TestCase()>
    Public Async Function WriterToReader_RoundTrip_TestMethodAsync() As Task

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testReader As FileEventStreamReaderUntyped = New FileEventStreamReaderUntyped(testAgg)
        Dim testWriter As FileEventStreamWriterUntyped = New FileEventStreamWriterUntyped(testAgg)
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoNullableDateProperty = DateTime.UtcNow, .EventTwoStringProperty = "My file test two", .EventTwoDecimalProperty = 123.45})

        Dim lastEvent As IEvent = testReader.GetEvents.Result.LastOrDefault()

        Assert.IsNotNull(lastEvent)

    End Function


End Class
