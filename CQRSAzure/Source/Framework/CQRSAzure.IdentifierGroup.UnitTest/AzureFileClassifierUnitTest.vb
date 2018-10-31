Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.File

<TestClass()>
Public Class AzureFileClassifierUnitTest

    Public Const MY_TEST_KEY As String = "my.file.test"

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As ClassifierProcessor(Of MockAggregate, String, MockClassifierOddNumber)
        testObj = Azure.File.AzureFileClassifier(Of MockAggregate, String, MockClassifierOddNumber).CreateClassifierProcessor(New MockAggregate(MY_TEST_KEY))
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub IsOddClassifier_Include_TestMethod()

        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Exclude
        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Include

        Dim myAggregate As MockAggregate = New MockAggregate(MY_TEST_KEY)

        'Add a number of events to the event stream
        Dim testWriter As FileEventStreamWriter(Of MockAggregate, String) = FileEventStreamWriter(Of MockAggregate, String).Create(myAggregate)
        testWriter.Reset()
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 9, .EventOneStringProperty = "test event one"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 3, .EventOneStringProperty = "test event two"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 2, .EventOneStringProperty = "test event three"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 6, .EventOneStringProperty = "test event four"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 6.89, .EventTwoStringProperty = "test two event one"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = -9.89, .EventTwoStringProperty = "test two event two"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 1, .EventOneStringProperty = "test event five"})

        Dim testClassifier As ClassifierProcessor(Of MockAggregate, String, MockClassifierOddNumber)
        testClassifier = Azure.File.AzureFileClassifier(Of MockAggregate, String, MockClassifierOddNumber).CreateClassifierProcessor(myAggregate)

        actual = testClassifier.Classify(New MockClassifierOddNumber(MY_TEST_KEY))

        Assert.AreEqual(actual, expected)

    End Sub

    <TestMethod()>
    Public Sub IsOddClassifier_Exclue_TestMethod()

        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged
        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Exclude

        Dim myAggregate As MockAggregate = New MockAggregate(MY_TEST_KEY)

        'Add a number of events to the event stream
        Dim testWriter As FileEventStreamWriter(Of MockAggregate, String) = FileEventStreamWriter(Of MockAggregate, String).Create(myAggregate)
        testWriter.Reset()
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 9, .EventOneStringProperty = "test event one"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 3, .EventOneStringProperty = "test event two"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 2, .EventOneStringProperty = "test event three"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 6, .EventOneStringProperty = "test event four"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 6.89, .EventTwoStringProperty = "test two event one"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = -9.89, .EventTwoStringProperty = "test two event two"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 2, .EventOneStringProperty = "test event five"})

        Dim testClassifier As ClassifierProcessor(Of MockAggregate, String, MockClassifierOddNumber)
        testClassifier = Azure.File.AzureFileClassifier(Of MockAggregate, String, MockClassifierOddNumber).CreateClassifierProcessor(myAggregate)

        actual = testClassifier.Classify(New MockClassifierOddNumber(MY_TEST_KEY))

        Assert.AreEqual(actual, expected)

    End Sub

    <TestMethod()>
    Public Sub Write_100Events_ThenClassify_Include_TestMethod()

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Include
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        '1 - Write 100 events to the event stream...
        Dim testAgg As New MockAggregate(MY_TEST_KEY)
        Dim testWriter As FileEventStreamWriter(Of MockAggregate, String) = FileEventStreamWriter(Of MockAggregate, String).Create(testAgg)

#Region "load the event stream"
        '1 - clean down the stream between test runs
        testWriter.Reset()
        '2 - Add 100 events ov various types
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 122})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 124})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 3"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 4"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 125})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 126})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 8"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 9"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 223})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 224})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 13"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 14"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 225})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 226})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 18"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 19"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 323})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 324})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 23"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 24"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 325})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 326})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 28"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 29"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 423})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 424})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 33"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 34"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 425})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 426})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 38"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 39"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 523})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 524})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 43"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 44"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 525})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 526})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 48"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 49"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 623})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 624})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 53"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 54"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 625})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 626})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 58"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 59"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 723})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 724})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 63"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 64"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 725})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 726})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 68"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 69"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 823})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 824})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 73"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 74"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 825})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 826})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 78"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 79"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 923})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 924})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 83"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 84"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 925})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 926})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 88"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 89"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 123})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 124})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 93"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 94"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 125})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 126})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 98"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 99"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
#End Region

        Dim testProcessor As ClassifierProcessor(Of MockAggregate, String, MockClassifierOddNumber)
        testProcessor = Azure.File.AzureFileClassifier(Of MockAggregate, String, MockClassifierOddNumber).CreateClassifierProcessor(New MockAggregate(MY_TEST_KEY))

        'run the 'is even' classifier 
        actual = testProcessor.Classify(New MockClassifierOddNumber(MY_TEST_KEY))

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub Write_100EventsInBlock_ThenClassify_Include_TestMethod()

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Include
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        '1 - Write 100 events to the event stream...
        Dim testAgg As New MockAggregate(MY_TEST_KEY)
        Dim testWriter As FileEventStreamWriter(Of MockAggregate, String) = FileEventStreamWriter(Of MockAggregate, String).Create(testAgg)

#Region "load the event stream"
        '1 - clean down the stream between test runs
        testWriter.Reset()

        Dim eventlist As New List(Of IEvent(Of MockAggregate))

        '2 - Add 100 events ov various types
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 122})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 124})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 3"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 4"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 125})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 126})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 8"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 9"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 223})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 224})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 13"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 14"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 225})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 226})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 18"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 19"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 323})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 324})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 23"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 24"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 325})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 326})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 28"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 29"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 423})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 424})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 33"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 34"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 425})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 426})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 38"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 39"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 523})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 524})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 43"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 44"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 525})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 526})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 48"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 49"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 623})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 624})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 53"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 54"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 625})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 626})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 58"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 59"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 723})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 724})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 63"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 64"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 725})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 726})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 68"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 69"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 823})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 824})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 73"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 74"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 825})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 826})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 78"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 79"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 923})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 924})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 83"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 84"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 925})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 926})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 88"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 89"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 123})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 124})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 93"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 94"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 125})
        eventlist.Add(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 126})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 98"})
        eventlist.Add(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 99"})
        eventlist.Add(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})

        testWriter.AppendEvents(0, eventlist)
#End Region

        Dim testProcessor As ClassifierProcessor(Of MockAggregate, String, MockClassifierOddNumber)
        testProcessor = Azure.File.AzureFileClassifier(Of MockAggregate, String, MockClassifierOddNumber).CreateClassifierProcessor(New MockAggregate(MY_TEST_KEY))

        'run the 'is even' classifier 
        actual = testProcessor.Classify(New MockClassifierOddNumber(MY_TEST_KEY))

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub Write_100Events_ThenClassify_Exclude_TestMethod()

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Exclude
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        '1 - Write 100 events to the event stream...
        Dim testAgg As New MockAggregate(MY_TEST_KEY)
        Dim testWriter As FileEventStreamWriter(Of MockAggregate, String) = FileEventStreamWriter(Of MockAggregate, String).Create(testAgg)

#Region "load the event stream"
        '1 - clean down the stream between test runs
        testWriter.Reset()
        '2 - Add 100 events ov various types
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 123})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 124})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 3"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 4"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 125})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 126})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 8"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 9"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 223})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 224})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 13"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 14"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 225})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 226})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 18"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 19"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 323})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 324})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 23"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 24"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 325})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 326})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 28"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 29"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 423})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 424})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 33"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 34"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 425})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 426})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 38"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 39"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 523})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 524})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 43"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 44"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 525})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 526})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 48"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 49"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 623})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 624})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 53"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 54"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 625})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 626})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 58"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 59"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 723})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 724})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 63"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 64"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 725})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 726})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 68"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 69"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 823})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 824})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 73"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 74"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 825})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 826})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 78"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 79"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 923})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 924})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 83"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 84"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 925})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 926})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 88"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 89"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 123})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 124})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 93"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 94"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "UK", .InternationalDialingCode = "0044"})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A test", .EventOneIntegerProperty = 125})
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "A further test", .EventOneIntegerProperty = 126})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 98"})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoStringProperty = "Test 99"})
        testWriter.AppendEvent(New MockEventTypeThree() With {.CountryCode = "IRL", .InternationalDialingCode = "00353"})
#End Region

        Dim testProcessor As ClassifierProcessor(Of MockAggregate, String, MockClassifierOddNumber)
        testProcessor = Azure.File.AzureFileClassifier(Of MockAggregate, String, MockClassifierOddNumber).CreateClassifierProcessor(New MockAggregate(MY_TEST_KEY))

        'run the 'is even' classifier 
        actual = testProcessor.Classify(New MockClassifierOddNumber(MY_TEST_KEY))

        Assert.AreEqual(expected, actual)

    End Sub



End Class