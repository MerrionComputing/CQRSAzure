﻿Imports System.Text
Imports NUnit.Framework

Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.Table.Untyped
Imports CQRSAzure.EventSourcing.UnitTest.Mocking
Imports CQRSAzure.IdentifierGroup

<TestFixture()>
Public Class AzureTableEventStreamUntypedUnitTest

    Public Const TEST_AGGREGATE_IDENTIFIER As String = "TABLE.OK.551.007"

    <TestCase()>
    Public Sub Reader_Constructor_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As IEventStreamReaderUntyped = TableEventStreamReaderUntyped.Create(testAgg)
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub Writer_Constructor_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As IEventStreamWriterUntyped = TableEventStreamWriterUntyped.Create(testAgg)
        Assert.IsNotNull(testObj)

    End Sub


    <TestCase()>
    Public Sub Writer_WriteMultipleEvent_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As IEventStreamWriterUntyped = TableEventStreamWriterUntyped.Create(testAgg)
        testObj.SetContext(New MockContext("First event", "Unit testing", "Duncan", "Correlation.123"))
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test 2", .EventOneIntegerProperty = 132})
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test 3", .EventOneIntegerProperty = 133})
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test 4", .EventOneIntegerProperty = 134})
        testObj.SetContext(New MockContext("Last event", "Unit testing", "Christina", "Correlation.456"))
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test 5", .EventOneIntegerProperty = 135})

        Assert.IsTrue(testObj.RecordCount > 3)

    End Sub


    <TestCase()>
    Public Sub Reader_ReadEvents_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As TableEventStreamReaderUntyped = TableEventStreamReaderUntyped.Create(testAgg)
        Dim ret = testObj.GetEvents()

        Assert.IsNotNull(ret)

    End Sub


    <TestCase()>
    Public Sub Reader_ReadEventsWithContext_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As TableEventStreamReaderUntyped = TableEventStreamReaderUntyped.Create(testAgg)
        Dim ret = testObj.GetEventsWithContext()

        Assert.IsNotNull(ret)

    End Sub



    <TestCase()>
    Public Async Function Write_100Events_Then_RunProjection_WithSnapshots_TestMethod() As Task

        Dim expected As Integer = 19380
        Dim actual As Integer = -1

        '1 - Write 100 events to the event stream...
        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testWriter As TableEventStreamWriterUntyped = TableEventStreamWriterUntyped.Create(testAgg)

#Region "load the event stream"
        '1 - clean down the stream between test runs
        Await testWriter.Reset()
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

        'create a projection processor
        Dim projectionProcessor = TableEventStreamReaderUntyped.CreateProjectionProcessor(testAgg)

        If (projectionProcessor IsNot Nothing) Then
            Dim projectionToProcess As MockProjection_Untyped_Snapshots = New MockProjection_Untyped_Snapshots()
            Await projectionProcessor.Process(projectionToProcess)
            actual = projectionToProcess.Total
        End If

        Assert.AreEqual(expected, actual)

    End Function


End Class