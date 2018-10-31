Imports System.Text
Imports CQRSAzure.EventSourcing
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.EventSourcing.InMemory
Imports CQRSAzure.EventSourcing.UnitTest.Mocking

<TestClass()>
Public Class InMemoryEventStreamUnitTest

    <TestMethod()>
    Public Sub Reader_Constructor_TestMethod()

        Dim testObj As InMemoryEventStreamReader(Of MockAggregate, String) = InMemoryEventStreamReader(Of MockAggregate, String).Create(New MockAggregate("123"))
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub Reader_FactoryConstructor_TestMethod()

        Dim testObj As InMemoryEventStreamReader(Of MockAggregate, String) = InMemoryEventStreamReaderFactory.Create(New MockAggregate("123"),
                                                                                                                     "123",
                                                                                                                     CQRSAzureEventSourcingInMemorySettingsElement.DefaultSettings)
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub Writer_Constructor_TestMethod()

        Dim testObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("113"))
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub Writer_FactoryConstructor_TestMethod()

        Dim testObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriterFactory.Create(New MockAggregate("123"),
                                                                                                                     "123",
                                                                                                                     CQRSAzureEventSourcingInMemorySettingsElement.DefaultSettings)
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub Writer_Add_RecordCount_TestMethod()

        Dim expected As Integer = 1
        Dim actual As Integer = 0

        Dim testObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("133"))
        'add an event
        testObj.AppendEvent(New MockEventTypeOne())
        actual = testObj.RecordCount

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub Writer_Add_Reader_Read_TestMethod()

        Dim expected As Integer = 1
        Dim actual As Integer = 0

        Dim testEvt As New MockEventTypeOne()
        testEvt.EventOneIntegerProperty = expected

        Dim testWriteObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("912"))
        'add an event
        testWriteObj.AppendEvent(testEvt)

        Dim testReadObj As InMemoryEventStreamReader(Of MockAggregate, String) = InMemoryEventStreamReader(Of MockAggregate, String).Create(New MockAggregate("912"))
        Dim readObj As MockEventTypeOne = testReadObj.GetEvents().LastOrDefault()

        actual = readObj.EventOneIntegerProperty

        Assert.AreEqual(expected, actual)

    End Sub


    <TestMethod()>
    Public Sub Writer_AddMultiple_Reader_Read_TestMethod()

        Dim expected As Integer = 1
        Dim actual As Integer = 0

        Dim testEvt As New MockEventTypeOne()
        testEvt.EventOneIntegerProperty = expected

        Dim testWriteObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("9992"))

        testWriteObj.AppendEvent(New MockEventTypeOne())
        testWriteObj.AppendEvent(New MockEventTypeTwo())
        testWriteObj.AppendEvent(New MockEventTypeOne())
        testWriteObj.AppendEvent(New MockEventTypeTwo())

        'add our test event
        testWriteObj.AppendEvent(testEvt)

        Dim testReadObj As InMemoryEventStreamReader(Of MockAggregate, String) = InMemoryEventStreamReader(Of MockAggregate, String).Create(New MockAggregate("9992"))
        Dim readObj As MockEventTypeOne = testReadObj.GetEvents().LastOrDefault()

        actual = readObj.EventOneIntegerProperty

        Assert.AreEqual(expected, actual)

    End Sub


    <TestMethod()>
    Public Sub Writer_AddMultiple_FilteredReader_Read_TestMethod()

        Dim expected As Integer = 1
        Dim actual As Integer = 0

        Dim testEvt As New MockEventTypeOne()
        testEvt.EventOneIntegerProperty = expected

        Dim testWriteObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("9992"))

        testWriteObj.AppendEvent(New MockEventTypeOne())
        testWriteObj.AppendEvent(New MockEventTypeTwo())
        testWriteObj.AppendEvent(New MockEventTypeOne())
        testWriteObj.AppendEvent(New MockEventTypeTwo())

        'add our test event
        testWriteObj.AppendEvent(testEvt)

        Dim filterEvents As IEnumerable(Of Type) = {GetType(MockEventTypeOne)}

        Dim testReadObj As InMemoryEventStreamReader(Of MockAggregate, String) = InMemoryEventStreamReader(Of MockAggregate, String).Create(New MockAggregate("9992"), filterEvents)
        Dim readObj As MockEventTypeOne = testReadObj.GetEvents().LastOrDefault()

        actual = readObj.EventOneIntegerProperty

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub Writer_AddMultiple_FilteredFunctionReader_Read_TestMethod()

        Dim expected As Integer = 1
        Dim actual As Integer = 0

        Dim testEvt As New MockEventTypeOne()
        testEvt.EventOneIntegerProperty = expected

        Dim testWriteObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("9992"))
        testWriteObj.Reset()
        testWriteObj.AppendEvent(New MockEventTypeOne())
        testWriteObj.AppendEvent(New MockEventTypeTwo())
        testWriteObj.AppendEvent(New MockEventTypeOne())
        testWriteObj.AppendEvent(New MockEventTypeTwo())

        'add our test event
        testWriteObj.AppendEvent(testEvt)



        Dim filterEventFunction As FilterFunctions.EventFilterFunction = Function(ByVal et As Type)
                                                                             Return (et.Equals(GetType(MockEventTypeOne)))
                                                                         End Function

        Dim testReadObj As InMemoryEventStreamReader(Of MockAggregate, String) = InMemoryEventStreamReader(Of MockAggregate, String).Create(New MockAggregate("9992"), eventFilterFunction:=filterEventFunction)
        Dim readObj As MockEventTypeOne = testReadObj.GetEvents().LastOrDefault()

        actual = readObj.EventOneIntegerProperty

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub Writer_AddMultiple_Reader_ReadWrapped_TestMethod()

        Dim expected As Integer = 1
        Dim actual As Integer = 0

        Dim testEvt As New MockEventTypeOne()
        testEvt.EventOneIntegerProperty = expected

        Dim testWriteObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("9992"))
        testWriteObj.Reset()
        testWriteObj.AppendEvent(New MockEventTypeOne())
        testWriteObj.AppendEvent(New MockEventTypeTwo())
        testWriteObj.AppendEvent(New MockEventTypeOne())
        testWriteObj.AppendEvent(New MockEventTypeTwo())

        'add our test event
        testWriteObj.AppendEvent(testEvt)

        Dim testReadObj As InMemoryEventStreamReader(Of MockAggregate, String) = InMemoryEventStreamReader(Of MockAggregate, String).Create(New MockAggregate("9992"))
        Dim readObj = testReadObj.GetEventsWithContext().LastOrDefault()

        actual = CTypeDynamic(Of MockEventTypeOne)(readObj.EventInstance).EventOneIntegerProperty

        Assert.AreEqual(expected, actual)

    End Sub


    <TestMethod>
    Public Sub Projection_MultipleEvents_UnitTest()

        Dim expected As Integer = 30
        Dim actual As Integer = -1

        InMemoryEventStreamWriter(Of MockAggregate, String).ResetStream("9992")
        Dim testWriteObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("9992"))
        testWriteObj.Reset()
        testWriteObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testWriteObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = -10})
        testWriteObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testWriteObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testWriteObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = -10})
        testWriteObj.AppendEvent(New MockEventTypeTwo() With {.EventTwoNullableDateProperty = New DateTime(1971, 12, 19)})
        testWriteObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 20, .EventOneStringProperty = "Test case"})
        testWriteObj.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 37.3})

        Dim testReadObj As ProjectionProcessor(Of MockAggregate, String) = InMemoryEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(New MockAggregate("9992"))
        Dim projObj As New MockProjection_Simple()
        testReadObj.Process(projObj)

        actual = projObj.Total

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub GetAllStreamKeys_NoDate_TestMethod()

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        InMemoryEventStreamWriter(Of MockAggregate, String).ResetStream("9995")
        Dim testWriteObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("9995"))

        actual = testWriteObj.GetAllStreamKeys().Contains("9995")
        Assert.AreEqual(expected, actual)


    End Sub

    <TestMethod>
    Public Sub GetAllStreamKeys_PastDate_TestMethod()

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        InMemoryEventStreamWriter(Of MockAggregate, String).ResetStream("9995")
        Dim testWriteObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("9995"))
        testWriteObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 77})
        actual = testWriteObj.GetAllStreamKeys(New DateTime(2012, 12, 19)).Contains("9995")

        Assert.AreEqual(expected, actual)


    End Sub

    <TestMethod>
    Public Sub GetAllStreamKeys_FutureDate_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        InMemoryEventStreamWriter(Of MockAggregate, String).ResetStream("9995")
        Dim testWriteObj As InMemoryEventStreamWriter(Of MockAggregate, String) = InMemoryEventStreamWriter(Of MockAggregate, String).Create(New MockAggregate("9995"))

        actual = testWriteObj.GetAllStreamKeys(New DateTime(2092, 12, 19)).Contains("9995")
        Assert.AreEqual(expected, actual)


    End Sub

    <TestMethod>
    Public Sub Factory_GenerateCreationFunctionDelegate_TestMethod()

        Dim testDelegate = InMemoryEventStreamReaderFactory.GenerateCreationFunctionDelegate(Of MockAggregate, String)

        Assert.IsNotNull(testDelegate)

    End Sub

    <TestMethod>
    Public Sub Factory_Reader_GenerateCreationFunctionDelegate_Invoke_TestMethod()

        Dim testDelegate = InMemoryEventStreamReaderFactory.GenerateCreationFunctionDelegate(Of MockAggregate, String)

        Dim testReader = testDelegate.Invoke(New MockAggregate("9996"), "9996", CQRSAzureEventSourcingInMemorySettingsElement.DefaultSettings)

        Assert.IsNotNull(testReader)

    End Sub

    <TestMethod>
    Public Sub Factory_Writer_GenerateCreationFunctionDelegate_Invoke_TestMethod()

        Dim testDelegate = InMemoryEventStreamWriterFactory.GenerateCreationFunctionDelegate(Of MockAggregate, String)

        Dim testWriter = testDelegate.Invoke(New MockAggregate("9996"), "9996", CQRSAzureEventSourcingInMemorySettingsElement.DefaultSettings)

        Assert.IsNotNull(testWriter)

    End Sub

    <TestMethod>
    Public Sub Factory_Reader_GenerateCreationFunctionDelegate_Invoke_WithSettings_TestMethod()

        Dim testDelegate = InMemoryEventStreamWriterFactory.GenerateCreationFunctionDelegate(Of MockAggregate, String)

        Dim testWriter = testDelegate.Invoke(New MockAggregate("9996"),
                                             "9996",
                                             New CQRSAzureEventSourcingInMemorySettingsElement() With {.DebugMessages = False})

        Assert.IsNotNull(testWriter)

    End Sub


    <TestMethod>
    Public Sub Factory_Writer_Projection_MultipleEvents_UnitTest()

        Dim expected As Integer = 30
        Dim actual As Integer = -1

        Dim testReaderDelegate = InMemoryEventStreamReaderFactory.GenerateCreationFunctionDelegate(Of MockAggregate, String)
        Dim testWriterDelegate = InMemoryEventStreamWriterFactory.GenerateCreationFunctionDelegate(Of MockAggregate, String)

        InMemoryEventStreamWriter(Of MockAggregate, String).ResetStream("9992")
        Dim testWriteObj As InMemoryEventStreamWriter(Of MockAggregate, String) = testWriterDelegate.Invoke(New MockAggregate("9997"),
                                                                                                            "9997",
                                                                                                            CQRSAzureEventSourcingInMemorySettingsElement.DefaultSettings)
        testWriteObj.Reset()
        testWriteObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testWriteObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = -10})
        testWriteObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testWriteObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 10})
        testWriteObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = -10})
        testWriteObj.AppendEvent(New MockEventTypeTwo() With {.EventTwoNullableDateProperty = New DateTime(1971, 12, 19)})
        testWriteObj.AppendEvent(New MockEventTypeOne() With {.EventOneIntegerProperty = 20, .EventOneStringProperty = "Test case"})
        testWriteObj.AppendEvent(New MockEventTypeTwo() With {.EventTwoDecimalProperty = 37.3})

        Dim testReadObj As ProjectionProcessor(Of MockAggregate, String) = InMemoryEventStreamReader(Of MockAggregate, String).CreateProjectionProcessor(testReaderDelegate.Invoke(New MockAggregate("9997"),
                                                                                                            "9997",
                                                                                                            CQRSAzureEventSourcingInMemorySettingsElement.DefaultSettings))
        Dim projObj As New MockProjection_Simple()
        testReadObj.Process(projObj)

        actual = projObj.Total

        Assert.AreEqual(expected, actual)

    End Sub
End Class

<TestClass()>
Public Class InMemoryEventStreamProjectionSnapshotUnitTest

    <TestMethod()>
    Public Sub SnapshotReader_Constructor_TestMethod()

        Dim testObj As InMemoryProjectionSnapshotReader(Of MockAggregate, String, MockProjection_Simple) = InMemoryProjectionSnapshotReader(Of MockAggregate, String, MockProjection_Simple).Create(New MockAggregate("123"))
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub SnapshotWriter_Constructor_TestMethod()

        Dim testObj As InMemoryProjectionSnapshotWriter(Of MockAggregate, String, MockProjection_Simple) = InMemoryProjectionSnapshotWriter(Of MockAggregate, String, MockProjection_Simple).Create(New MockAggregate("123"))
        Assert.IsNotNull(testObj)

    End Sub

End Class