Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.File
Imports CQRSAzure.EventSourcing.UnitTest.Mocking

<TestClass()>
Public Class AzureFileProjectionSnapshotUnitTest

    <TestMethod()>
    Public Sub FileProjectionSnapshotWriter_Constructor_TestMethod()

        Const TEST_KEY As String = "1.snt.234"

        Dim testAggregate As New MockAggregate(TEST_KEY)
        Dim testProjection As New MockProjectionWithSnapshots()

        Dim testObj = FileProjectionSnapshotWriterFactory.Create(testAggregate,
                                                          TEST_KEY,
                                                          testProjection)

        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub FileProjectionSnapshotReader_Constructor_TestMethod()

        Const TEST_KEY As String = "1.snt.234"

        Dim testAggregate As New MockAggregate(TEST_KEY)
        Dim testProjection As New MockProjectionWithSnapshots()

        Dim testObj = FileProjectionSnapshotReaderFactory.Create(testAggregate,
                                                          TEST_KEY,
                                                          testProjection)

        Assert.IsNotNull(testObj)

    End Sub


    <TestMethod()>
    Public Sub FileProjectionSnapshotWriter_SaveSnapshot_TestMethod()

        Const TEST_KEY As String = "1.snt.334"

        Dim testAggregate As New MockAggregate(TEST_KEY)
        Dim testProjection As New MockProjectionWithSnapshots()

        'create a writer to write to the 

        Dim testObj = FileProjectionSnapshotWriterFactory.Create(testAggregate,
                                                          TEST_KEY,
                                                          testProjection)




    End Sub

    <TestMethod()>
    Public Sub FileProjectionSnapshotWriter_FilenameToSequenceNumber_TestMethod()

        Dim expected As UInteger = 17
        Dim actual As UInteger = 44

        Dim filename As String = "0000000017.20160415092207"

        actual = FileProjectionSnapshotBase.FilenameToSequenceNumber(filename)

        Assert.AreEqual(expected, actual)

    End Sub


    <TestMethod()>
    Public Sub FileProjectionSnapshotWriter_FilenameToAsOfDate_TestMethod()

        Dim expected As DateTime? = New DateTime(2016, 1, 17, 9, 30, 0)
        Dim actual As DateTime? = New DateTime(1971, 12, 17, 9, 30, 0)

        Dim filename As String = "0000000017.201601170930000000"

        actual = FileProjectionSnapshotBase.FilenameToAsOfDate(filename)

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub FileProjectionSnapshotBase_ASOFDATE_FORMAT_TestMethod()

        Dim expected As String = "201601170930000000"
        Dim actual As String = "test"

        actual = FileProjectionSnapshotBase.FormatSequenceAsOfDate(New DateTime(2016, 1, 17, 9, 30, 0))

        Assert.AreEqual(expected, actual)

    End Sub


    <TestMethod>
    Public Sub FileProjectionSnapshotReader_NoSnapshots_SequenceIsZero_TestMethod()

        Dim expected As UInteger = 0
        Dim actual As UInteger = 20

        Const TEST_KEY As String = "1.nosuchkey"

        Dim testAggregate As New MockAggregate(TEST_KEY)
        Dim testProjection As New MockProjectionWithSnapshots()

        Dim testObj = FileProjectionSnapshotReaderFactory.Create(testAggregate,
                                                          TEST_KEY,
                                                          testProjection)

        actual = testObj.GetLatestSnapshotSequence(TEST_KEY)

        Assert.AreEqual(expected, actual)

    End Sub




End Class