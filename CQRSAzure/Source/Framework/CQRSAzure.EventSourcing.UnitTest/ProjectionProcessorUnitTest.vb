Imports System.Text
Imports NUnit.Framework

Imports CQRSAzure.EventSourcing.Azure.Table
Imports CQRSAzure.EventSourcing.Azure.File
Imports CQRSAzure.EventSourcing.Azure.Blob
Imports CQRSAzure.EventSourcing.InMemory
Imports CQRSAzure.EventSourcing.Local.File
Imports CQRSAzure.EventSourcing.UnitTest.Mocking

<TestFixture()>
Public Class ProjectionProcessorUnitTest

    <TestCase()>
    Public Sub InMemoryProjection_Constructor_UnitTest()

        Dim testObj = InMemoryEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate("123"))
        Assert.IsNotNull(testObj)

    End Sub


    <TestCase()>
    Public Sub InMemoryProjection_NoEvents_UnitTest()

        Dim expected As Integer = 0
        Dim actual As Integer = -1

        Dim testObj = InMemoryEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate("123.imt"))
        Dim myProjection As New MockProjectionMultipleEventsNoSnapshots

        testObj.Process(myProjection)

        actual = myProjection.Total

        Assert.AreEqual(expected, actual)


    End Sub

    <TestCase()>
    Public Sub InMemoryProjection_MultipleEvents_UnitTest()

        Dim expected As Integer = 100
        Dim actual As Integer = -1

        Dim testObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("123"))
        testObj.Reset()
        'add an event
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 50})
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 20})
        testObj.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 20.12, .EventTwoStringProperty = "Event Two"})
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 20})

        Dim testProjectionProcessor = InMemoryEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate("123"))
        Dim myProjection As New MockProjectionNoSnapshots

        testProjectionProcessor.Process(myProjection)

        actual = myProjection.Total

        Assert.AreEqual(expected, actual)


    End Sub


    <TestCase()>
    Public Sub InMemoryProjection_MultipleEvents_TakeSnapshot_UnitTest()


        Dim testStreamWriter As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("123"))
        testStreamWriter.Reset()
        'add an event
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 50})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 20})
        testStreamWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 20.12, .EventTwoStringProperty = "Event Two"})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 20})

        Dim testProjectionProcessor = InMemoryEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate("123"))
        Dim myProjection As New MockProjectionWithSnapshots

        testProjectionProcessor.Process(myProjection)

        Dim testSnapshot = myProjection.ToSnapshot()


        Assert.IsNotNull(testSnapshot)

    End Sub

    <TestCase()>
    Public Sub InMemoryProjection_MultipleEvents_LoadSnapshot_UnitTest()

        Const TEST_UNIQUE_KEY As String = "124"

        Dim expected As Integer = 100
        Dim actual As Integer = -1

        Dim testStreamWriter As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate(TEST_UNIQUE_KEY))

        'add an event
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 50})

        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 20})
        testStreamWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 20.12, .EventTwoStringProperty = "Event Two"})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 20})

        Dim testProjectionProcessor = InMemoryEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate(TEST_UNIQUE_KEY))
        Dim myProjection As New MockProjectionWithSnapshots

        testProjectionProcessor.Process(myProjection)
        Dim testSnapshot = myProjection.ToSnapshot()

        'write it to a snapshot writer
        Dim testSnapshotWriter As InMemoryProjectionSnapshotWriter(Of MockAggregate, String, MockProjectionWithSnapshots) = InMemoryProjectionSnapshotWriter.Create(Of MockAggregate, String, MockProjectionWithSnapshots)(New MockAggregate(TEST_UNIQUE_KEY))
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, testSnapshot)
        End If

        Dim loadedSnapshot As IProjectionSnapshot(Of MockAggregate, String)
        'read it from a snapshot reader
        Dim testSnapshotReader As InMemoryProjectionSnapshotReader(Of MockAggregate, String, MockProjectionWithSnapshots) = InMemoryProjectionSnapshotReader.Create(Of MockAggregate, String, MockProjectionWithSnapshots)(New MockAggregate(TEST_UNIQUE_KEY))
        loadedSnapshot = testSnapshotReader.GetSnapshot(TEST_UNIQUE_KEY)

        Dim mySecondProjection As New MockProjectionWithSnapshots
        mySecondProjection.LoadFromSnapshot(loadedSnapshot)

        actual = mySecondProjection.Total
        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub InMemoryProjection_MultipleEvents_Stepback_LoadSnapshot_UnitTest()

        Const TEST_UNIQUE_KEY As String = "145.T4"

        Dim expected As Integer = 40
        Dim actual As Integer = -1


        'write it to a snapshot writer
        Dim testSnapshotWriter As InMemoryProjectionSnapshotWriter(Of MockAggregate, String, MockProjectionWithSnapshots) = InMemoryProjectionSnapshotWriter.Create(Of MockAggregate, String, MockProjectionWithSnapshots)(New MockAggregate(TEST_UNIQUE_KEY))


        Dim testStreamWriter As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate(TEST_UNIQUE_KEY))
        'add an event
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 20.12, .EventTwoStringProperty = "Event Two"})


        Dim testProjectionProcessor = InMemoryEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate(TEST_UNIQUE_KEY))
        Dim myProjection As New MockProjectionWithSnapshots

        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If

        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})

        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If

        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 30.12, .EventTwoStringProperty = "Event Two the second"})

        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If

        Dim loadedSnapshot As IProjectionSnapshot(Of MockAggregate, String)
        'read it from a snapshot reader
        Dim testSnapshotReader As InMemoryProjectionSnapshotReader(Of MockAggregate, String, MockProjectionWithSnapshots) = InMemoryProjectionSnapshotReader.Create(Of MockAggregate, String, MockProjectionWithSnapshots)(New MockAggregate(TEST_UNIQUE_KEY))
        loadedSnapshot = testSnapshotReader.GetSnapshot(TEST_UNIQUE_KEY, 5)

        Dim mySecondProjection As New MockProjectionWithSnapshots
        mySecondProjection.LoadFromSnapshot(loadedSnapshot)

        actual = mySecondProjection.Total
        Assert.AreEqual(expected, actual)

    End Sub


    <TestCase()>
    Public Sub InMemoryProjection_MultipleMixedEvents_UnitTest()

        Dim expected As String = "My test"
        Dim actual As String = "Not set"

        Dim testObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("123"))
        'add an event
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 50})
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 20, .EventOneStringProperty = "A test"})
        testObj.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 20.12, .EventTwoStringProperty = "Event Two"})
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 20, .EventOneStringProperty = expected})

        Dim testProjectionProcessor = InMemoryEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate("123"))
        Dim myProjection As New MockProjectionMultipleEventsNoSnapshots

        testProjectionProcessor.Process(myProjection)

        actual = myProjection.LastString

        Assert.AreEqual(expected, actual)


    End Sub

    <TestCase()>
    Public Sub AzureBlobProjection_Constructor_UnitTest()

        Dim testObj = BlobEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate("123"))
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub AzureBlobProjection_NoEvents_UnitTest()

        Dim expected As Integer = 0
        Dim actual As Integer = -1

        Dim testObj = BlobEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate("123"))
        Assert.IsNotNull(testObj)

        Dim myProjection As New MockProjectionMultipleEventsNoSnapshots

        testObj.Process(myProjection)

        actual = myProjection.Total

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub AzureBlobProjection_SomeEvents_UnitTest()

        Dim expected As Integer = 0
        Dim actual As Integer = 0


        Dim testAgg = New MockAggregate(AzureBlobEventStreamUnitTest.TEST_AGGREGATE_IDENTIFIER)

        Dim testWriter As BlobEventStreamWriter(Of MockAggregate, String) = BlobEventStreamWriter(Of MockAggregate, String).Create(testAgg)
        testWriter.Reset()
        'add a dummy event
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoNullableDateProperty = DateTime.UtcNow, .EventTwoStringProperty = "My test two", .EventTwoDecimalProperty = 123.45})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoNullableDateProperty = DateTime.UtcNow, .EventTwoStringProperty = "My test three", .EventTwoDecimalProperty = 223.45})
        'MockEventTypeOne
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 73, .EventOneStringProperty = "Event 1 test"})

        Dim testObj = BlobEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(testAgg)
        Assert.IsNotNull(testObj)

        Dim myProjection As New MockProjectionMultipleEventsNoSnapshots

        testObj.Process(myProjection)

        actual = myProjection.Total

        Assert.AreNotEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub AzureBlobProjection_MultipleEvents_Stepback_LoadSnapshot_UnitTest()

        Const TEST_UNIQUE_KEY As String = "ABT.100.T4"

        Dim expected As Integer = 40
        Dim actual As Integer = -1


        'write it to a snapshot writer
        Dim testSnapshotWriter As BlobProjectionSnapshotWriter(Of MockAggregate, String, MockProjectionWithSnapshots) = BlobProjectionSnapshotWriter.Create(Of MockAggregate, String, MockProjectionWithSnapshots)(New MockAggregate(TEST_UNIQUE_KEY))
        'clear out any old snapshots in order to have a clean unit test
        testSnapshotWriter.Reset()


        Dim testStreamWriter As BlobEventStreamWriter(Of MockAggregate, String) = BlobEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate(TEST_UNIQUE_KEY))
        'clear out any old events in order to have a clean test
        testStreamWriter.Reset()
        'add an event
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 20.12, .EventTwoStringProperty = "Event Two"})


        Dim testProjectionProcessor = BlobEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate(TEST_UNIQUE_KEY))
        Dim myProjection As New MockProjectionWithSnapshots

        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If

        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})

        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If

        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 30.12, .EventTwoStringProperty = "Event Two the second"})

        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If

        Dim loadedSnapshot As IProjectionSnapshot(Of MockAggregate, String)
        'read it from a snapshot reader
        Dim testSnapshotReader As BlobProjectionSnapshotReader(Of MockAggregate, String, MockProjectionWithSnapshots) = BlobProjectionSnapshotReader.Create(Of MockAggregate, String, MockProjectionWithSnapshots)(New MockAggregate(TEST_UNIQUE_KEY))
        loadedSnapshot = testSnapshotReader.GetSnapshot(TEST_UNIQUE_KEY, 5)

        Dim mySecondProjection As New MockProjectionWithSnapshots
        mySecondProjection.LoadFromSnapshot(loadedSnapshot)

        actual = mySecondProjection.Total
        Assert.AreEqual(expected, actual)

    End Sub

    ''' <summary>
    ''' Check that running a projection that causes a state change is reflected as such
    ''' </summary>
    <TestCase()>
    Public Sub AzureBlobProjection_SomeEvents_StateChanged_UnitTest()

        Dim expected As Integer = 0
        Dim actual As Integer = 0


        Dim testAgg = New MockAggregate(AzureBlobEventStreamUnitTest.TEST_AGGREGATE_IDENTIFIER)

        Dim testWriter As BlobEventStreamWriter(Of MockAggregate, String) = BlobEventStreamWriter(Of MockAggregate, String).Create(testAgg)
        testWriter.Reset()
        'add a dummy event
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoNullableDateProperty = DateTime.UtcNow, .EventTwoStringProperty = "My test two", .EventTwoDecimalProperty = 123.45})
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoNullableDateProperty = DateTime.UtcNow, .EventTwoStringProperty = "My test three", .EventTwoDecimalProperty = 223.45})
        'MockEventTypeOne
        testWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 73, .EventOneStringProperty = "Event 1 test"})

        Dim testObj = BlobEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(testAgg)
        Assert.IsNotNull(testObj)

        Dim myProjection As New MockProjectionMultipleEventsNoSnapshots

        testObj.Process(myProjection)

        actual = myProjection.StateChanges.Count()

        Assert.AreNotEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub AzureFileProjection_Constructor_Unittest()

        Dim testObj = FileEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate("123"))
        Assert.IsNotNull(testObj)

    End Sub


    <TestCase()>
    Public Sub AzureFileProjection_NoEvents_UnitTest()

        Dim expected As Integer = 0
        Dim actual As Integer = -1

        Dim testObj = FileEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate("123"))
        Assert.IsNotNull(testObj)

        Dim myProjection As New MockProjectionMultipleEventsNoSnapshots

        testObj.Process(myProjection)

        actual = myProjection.Total

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub AzureFileProjection_SomeEvents_UnitTest()

        Dim expected As Integer = 0
        Dim actual As Integer = 0

        Dim testObj = FileEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate(AzureFileEventStreamUnitTest.TEST_AGGREGATE_IDENTIFIER))
        Assert.IsNotNull(testObj)

        Dim myProjection As New MockProjectionMultipleEventsNoSnapshots

        testObj.Process(myProjection)

        actual = myProjection.Total

        Assert.AreNotEqual(expected, actual)

    End Sub


    <TestCase()>
    Public Async Function AzureFileProjection_MultipleEvents_Stepback_LoadSnapshot_UnitTest() As Task

        Const TEST_UNIQUE_KEY As String = "AFT.100.T4"

        Dim expected As Integer = 40
        Dim actual As Integer = -1

        Dim rollbackSequence As UInteger

        'write it to a snapshot writer
        Dim myProjection As New MockProjectionWithSnapshots
        Dim testSnapshotWriter As FileProjectionSnapshotWriter(Of MockAggregate, String, MockProjectionWithSnapshots) = FileProjectionSnapshotWriterFactory.Create(New MockAggregate(TEST_UNIQUE_KEY), TEST_UNIQUE_KEY, myProjection)
        'clear out any old snapshots in order to have a clean unit test
        Await testSnapshotWriter.Reset()


        Dim testStreamWriter As FileEventStreamWriter(Of MockAggregate, String) = FileEventStreamWriterFactory.Create(New MockAggregate(TEST_UNIQUE_KEY), TEST_UNIQUE_KEY)
        'clear out any old events in order to have a clean test
        testStreamWriter.Reset()
        'add an event
        Await testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        Await testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        Await testStreamWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 20.12, .EventTwoStringProperty = "Event Two"})


        Dim testProjectionProcessor = FileEventStreamReaderFactory.CreateProjectionProcessor(New MockAggregate(TEST_UNIQUE_KEY), TEST_UNIQUE_KEY)


        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If

        Await testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        Await testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})

        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If
        rollbackSequence = myProjection.CurrentSequenceNumber

        Await testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        Await testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        Await testStreamWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 30.12, .EventTwoStringProperty = "Event Two the second"})

        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If

        Dim loadedSnapshot As IProjectionSnapshot(Of MockAggregate, String)
        'read it from a snapshot reader
        Dim testSnapshotReader As FileProjectionSnapshotReader(Of MockAggregate, String, MockProjectionWithSnapshots) = FileProjectionSnapshotReaderFactory.Create(New MockAggregate(TEST_UNIQUE_KEY), TEST_UNIQUE_KEY, myProjection)
        loadedSnapshot = testSnapshotReader.GetSnapshot(TEST_UNIQUE_KEY, rollbackSequence)

        Dim mySecondProjection As New MockProjectionWithSnapshots
        mySecondProjection.LoadFromSnapshot(loadedSnapshot)

        actual = mySecondProjection.Total
        Assert.AreEqual(expected, actual)

    End Function


    <TestCase()>
    Public Sub AzureTableProjection_MultipleEvents_Stepback_LoadSnapshot_UnitTest()

        Const TEST_UNIQUE_KEY As String = "145.AzTable"

        Dim expected As Integer = 40
        Dim actual As Integer = -1

        Dim myProjection As New MockProjectionWithSnapshots
        'write it to a snapshot writer
        Dim testSnapshotWriter As TableProjectionSnapshotWriter(Of MockAggregate, String, MockProjectionWithSnapshots) = TableProjectionSnapshotWriterFactory.Create(New MockAggregate(TEST_UNIQUE_KEY), TEST_UNIQUE_KEY, myProjection)


        Dim testStreamWriter As TableEventStreamWriter(Of MockAggregate, String) = TableEventStreamWriterFactory.Create(New MockAggregate(TEST_UNIQUE_KEY), TEST_UNIQUE_KEY)

        'Reset the table contents to allow a clean test
        testStreamWriter.Reset()
        testSnapshotWriter.Reset()

        'add an event
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 20.12, .EventTwoStringProperty = "Event Two"})


        Dim testProjectionProcessor = TableEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate(TEST_UNIQUE_KEY))


        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If

        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})

        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If

        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 30.12, .EventTwoStringProperty = "Event Two the second"})

        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If

        Dim loadedSnapshot As IProjectionSnapshot(Of MockAggregate, String)
        'read it from a snapshot reader
        Dim testSnapshotReader As TableProjectionSnapshotReader(Of MockAggregate, String, MockProjectionWithSnapshots) = TableProjectionSnapshotReaderFactory.Create(New MockAggregate(TEST_UNIQUE_KEY), TEST_UNIQUE_KEY, myProjection)

        Dim latestSnapshot As Integer = testSnapshotReader.GetLatestSnapshotSequence(TEST_UNIQUE_KEY, 5).Result
        If (latestSnapshot >= 4) Then

            loadedSnapshot = testSnapshotReader.GetSnapshot(TEST_UNIQUE_KEY, latestSnapshot)

            Dim mySecondProjection As New MockProjectionWithSnapshots
            mySecondProjection.LoadFromSnapshot(loadedSnapshot)

            actual = mySecondProjection.Total
        End If

        Assert.AreEqual(expected, actual)

    End Sub


    <TestCase()>
    Public Sub LocalFileProjection_MultipleEvents_UnitTest()

        Dim expected As Integer = 100
        Dim actual As Integer = -1

        Dim testObj As LocalFileEventStreamWriter(Of MockAggregate, String) = LocalFileEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("123"))
        testObj.Reset()
        'add an event
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 50})
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 20})
        testObj.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 20.12, .EventTwoStringProperty = "Event Two"})
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 20})

        Dim testProjectionProcessor = LocalFileEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate("123"))
        Dim myProjection As New MockProjectionNoSnapshots

        testProjectionProcessor.Process(myProjection)

        actual = myProjection.Total

        Assert.AreEqual(expected, actual)


    End Sub

#If LOCAL_MACHINE Then
    ' Note: set this path to a real location
    Dim settings As ILocalFileSettings = New CQRSAzureEventSourcingLocalFileSettingsElement() With {.UnderlyingSerialiser = ILocalFileSettings.SerialiserType.Binary,
        .EventStreamRootFolder = "F:\Data\CQRS on Azure\Event Streams",
        .SnapshotsRootFolder = "F:\Data\CQRS on Azure\Snapshots"}

    <TestCase()>
    Public Sub LocalFileProjection_MultipleEvents_Stepback_LoadSnapshot_UnitTest()

        Const TEST_UNIQUE_KEY As String = "LFT.100.Test4"

        Dim expected As Integer = 40
        Dim actual As Integer = -1

        Dim rollbackSequence As UInteger

        'write it to a snapshot writer
        Dim myProjection As New MockProjectionWithSnapshots

        Dim testSnapshotReader As Local.File.LocalFileProjectionSnapshotReader(Of MockAggregate, String, MockProjectionWithSnapshots) = Local.File.LocalFileProjectionSnapshotReaderFactory.Create(New MockAggregate(TEST_UNIQUE_KEY), TEST_UNIQUE_KEY, myProjection, settings)
        Dim testSnapshotWriter As Local.File.LocalFileProjectionSnapshotWriter(Of MockAggregate, String, MockProjectionWithSnapshots) = Local.File.LocalFileProjectionSnapshotWriterFactory.Create(New MockAggregate(TEST_UNIQUE_KEY), TEST_UNIQUE_KEY, myProjection, settings)
        'clear out any old snapshots in order to have a clean unit test
        testSnapshotWriter.Reset()


        Dim testStreamWriter As Local.File.LocalFileEventStreamWriter(Of MockAggregate, String) = Local.File.LocalFileEventStreamWriterFactory.Create(New MockAggregate(TEST_UNIQUE_KEY), TEST_UNIQUE_KEY, settings)
        'clear out any old events in order to have a clean test
        testStreamWriter.Reset()
        'add an event
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 20.12, .EventTwoStringProperty = "Event Two"})


        Dim testProjectionProcessor = Local.File.LocalFileEventStreamReaderFactory.CreateProjectionProcessor(New MockAggregate(TEST_UNIQUE_KEY),
                                                                                                             TEST_UNIQUE_KEY,
                                                                                                             settings,
                                                                                                             ProjectionSnapshotProcessorFactory.Create(Of MockAggregate, String, MockProjectionWithSnapshots)(TEST_UNIQUE_KEY,
                                                                                                                                                                                                               myProjection,
                                                                                                                                                                                                              testSnapshotReader,
                                                                                                                                                                                                              testSnapshotWriter))


        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If

        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10, .EventOneStringProperty = "Event one three"})
        testStreamWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 21.12, .EventTwoStringProperty = "Event Two"})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10, .EventOneStringProperty = "Event one four"})

        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If
        rollbackSequence = myProjection.CurrentSequenceNumber

        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testStreamWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 30.12, .EventTwoStringProperty = "Event Two the second"})

        testProjectionProcessor.Process(myProjection)
        If (testSnapshotWriter IsNot Nothing) Then
            testSnapshotWriter.SaveSnapshot(TEST_UNIQUE_KEY, myProjection.ToSnapshot())
        End If

        Dim loadedSnapshot As IProjectionSnapshot(Of MockAggregate, String)
        'read it from a snapshot reader
        loadedSnapshot = testSnapshotReader.GetSnapshot(TEST_UNIQUE_KEY, rollbackSequence)

        If (loadedSnapshot IsNot Nothing) Then
            Dim mySecondProjection As New MockProjectionWithSnapshots
            mySecondProjection.LoadFromSnapshot(loadedSnapshot)
            actual = mySecondProjection.Total
        End If

        Assert.AreEqual(expected, actual)

    End Sub

#End If

    <TestCase()>
    Public Sub IncrementAssistant_Double_Test()

        Dim expected As Double = 20
        Dim actual As Double = 15

        actual = IncrementAssistant.Increment(actual, 5)


        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub IncrementAssistant_NullableDouble_Test()

        Dim expected As Nullable(Of Double) = 20
        Dim actual As Nullable(Of Double) = 15

        actual = IncrementAssistant.Increment(Of Nullable(Of Double))(actual, 5)


        Assert.AreEqual(expected, actual)

    End Sub


    <TestCase()>
    Public Sub IncrementAssistant_Integer_Test()

        Dim expected As Integer = 20
        Dim actual As Integer = 15

        actual = IncrementAssistant.Increment(actual, 5)


        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub IncrementAssistant_NullableInteger_Test()

        Dim expected As Nullable(Of Integer) = 20
        Dim actual As Nullable(Of Integer) = 15

        actual = IncrementAssistant.Increment(Of Nullable(Of Integer))(actual, 5)


        Assert.AreEqual(expected, actual)

    End Sub


    <TestCase()>
    Public Sub IncrementAssistant_String_Test()

        Dim expected As String = "abcdef"
        Dim actual As String = "abc"

        actual = IncrementAssistant.Increment(actual, "def")


        Assert.AreEqual(expected, actual)

    End Sub


End Class
