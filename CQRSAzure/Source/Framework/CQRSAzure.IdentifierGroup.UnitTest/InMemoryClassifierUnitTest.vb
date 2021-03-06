﻿Imports System.Text
Imports CQRSAzure.EventSourcing.InMemory
Imports NUnit.Framework

<TestFixture()>
Public Class InMemoryClassifierUnitTest

    Public Const MY_TEST_KEY As String = "my.test"

    <TestCase()>
    Public Sub Constructor_TestMethod()

        Dim testObj As ClassifierProcessor(Of MockAggregate, String, MockClassifierOddNumber)
        testObj = InMemory.InMemoryClassifier(Of MockAggregate, String, MockClassifierOddNumber).CreateClassifierProcessor(MY_TEST_KEY)
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Async Function IsOddClassifier_Include_TestMethod() As Task

        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Exclude
        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Include

        Dim myAggregate As MockAggregate = New MockAggregate(MY_TEST_KEY)

        'Add a number of events to the event stream
        InMemoryEventStreamWriter(Of MockAggregate, String).ResetStream(MY_TEST_KEY)
        Dim testWriter As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(myAggregate)
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 9, .EventOneStringProperty = "test event one"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 3, .EventOneStringProperty = "test event two"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 2, .EventOneStringProperty = "test event three"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 6, .EventOneStringProperty = "test event four"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 6.89, .EventTwoStringProperty = "test two event one"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = -9.89, .EventTwoStringProperty = "test two event two"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 1, .EventOneStringProperty = "test event five"})

        Dim testClassifier As ClassifierProcessor(Of MockAggregate, String, MockClassifierOddNumber)
        testClassifier = InMemory.InMemoryClassifier(Of MockAggregate, String, MockClassifierOddNumber).CreateClassifierProcessor(MY_TEST_KEY)

        actual = Await testClassifier.Classify(New MockClassifierOddNumber(MY_TEST_KEY))

        Assert.AreEqual(actual, expected)

    End Function

    <TestCase()>
    Public Async Function IsOddClassifier_Exclue_TestMethod() As Task

        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged
        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Exclude

        Dim myAggregate As MockAggregate = New MockAggregate(MY_TEST_KEY)

        'Add a number of events to the event stream
        InMemoryEventStreamWriter(Of MockAggregate, String).ResetStream(MY_TEST_KEY)
        Dim testWriter As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(myAggregate)
        Task.WaitAll(
            testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 9, .EventOneStringProperty = "test event one"}),
            testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 3, .EventOneStringProperty = "test event two"}),
            testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 2, .EventOneStringProperty = "test event three"}),
            testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 6, .EventOneStringProperty = "test event four"}),
            testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 6.89, .EventTwoStringProperty = "test two event one"}),
            testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = -9.89, .EventTwoStringProperty = "test two event two"}),
            testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 2, .EventOneStringProperty = "test event five"})
        )

        Dim testClassifier As ClassifierProcessor(Of MockAggregate, String, MockClassifierOddNumber)
        testClassifier = InMemory.InMemoryClassifier(Of MockAggregate, String, MockClassifierOddNumber).CreateClassifierProcessor(MY_TEST_KEY)

        actual = Await testClassifier.Classify(New MockClassifierOddNumber(MY_TEST_KEY))

        Assert.AreEqual(actual, expected)

    End Function



    <TestCase()>
    Public Async Function Write_100Events_ThenClassify_WithSnapshots_Exclude_TestMethod() As Task

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Exclude
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        '1 - Write 100 events to the event stream...
        Dim testAgg As New MockAggregate(MY_TEST_KEY)
        Dim testWriter As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(testAgg)

#Region "load the  first 50 event streams"
        '1 - clean down the stream between test runs
        testWriter.Reset()
        '2 - Add 100 events ov various types
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 123})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 124})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 3"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 4"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 125})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 126})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 8"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 9"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 223})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 224})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 13"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 14"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 225})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 226})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 18"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 19"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 323})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 324})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 23"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 24"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 325})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 326})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 28"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 29"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 423})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 424})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 33"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 34"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 425})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 426})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 38"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 39"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 523})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 524})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 43"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 44"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 525})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 526})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 48"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 49"})
#End Region

        Dim testClassfier = New MockClassifierOddNumber(MY_TEST_KEY)

        Dim snapshotWriter = InMemory.InMemoryClassifierSnapshotWriterFactory.Create(testAgg, MY_TEST_KEY, testClassfier)
        Dim snapshotReader = InMemory.InMemoryClassifierSnapshotReaderFactory.Create(testAgg, MY_TEST_KEY, testClassfier)

        Dim testProcessor As ClassifierProcessor(Of MockAggregate, String, MockClassifierOddNumber)
        testProcessor = InMemory.InMemoryClassifier(Of MockAggregate, String, MockClassifierOddNumber).CreateClassifierProcessor(MY_TEST_KEY,
                                                                                                                                 settings:=Nothing,
                                                                                                                                 classifier:=testClassfier,
                                                                                                                                 snapshotReader:=snapshotReader,
                                                                                                                                 snapshotWriter:=snapshotWriter)


        Dim firstHalf = testProcessor.Classify()
        'at this point the classifier should have saved a snapshot of the first 50 events

        Dim testProcessorFromSnapshot As ClassifierProcessor(Of MockAggregate, String, MockClassifierOddNumber)
        testProcessorFromSnapshot = InMemory.InMemoryClassifier(Of MockAggregate, String, MockClassifierOddNumber).CreateClassifierProcessor(MY_TEST_KEY,
                                                                                                                                 settings:=Nothing,
                                                                                                                                 classifier:=testClassfier,
                                                                                                                                 snapshotReader:=snapshotReader,
                                                                                                                                 snapshotWriter:=snapshotWriter)


#Region "Second half of the event stream"
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 623})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 624})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 53"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 54"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 625})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 626})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 58"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 59"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 723})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 724})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 63"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 64"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 725})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 726})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 68"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 69"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 823})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 824})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 73"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 74"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 825})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 826})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 78"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 79"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 923})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 924})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 83"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 84"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 925})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 926})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 88"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 89"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 123})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 124})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 93"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 94"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 125})
        Await testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 126})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 98"})
        Await testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 99"})
        Await testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
#End Region


        'run the 'is even' classifier 
        actual = Await testProcessorFromSnapshot.Classify(New MockClassifierOddNumber(MY_TEST_KEY))

        Assert.AreEqual(expected, actual)

    End Function
End Class