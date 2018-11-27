Imports System.Text
Imports NUnit.Framework
Imports CQRSAzure.EventSourcing.Projections

<TestFixture()>
Public Class ProjectionSnapshotWrittenEvent_UnitTest

    <TestCase()>
    Public Sub Constructor_TestMethod()

        Dim testObj As IProjectionSnapshotWrittenEvent = ProjectionSnapshotWrittenEvent.Create(
            DateTime.Now,
            123,
            "Unit test snapshot",
            "InMemory")

        Assert.IsNotNull(testObj)
    End Sub

    <TestCase()>
    Public Sub ProjectionSnapshotWrittenEvent_RoundTrip_AsOfDate_Serialisation_TestMethod()

        Dim expected As Nullable(Of DateTime) = New DateTime(1971, 12, 31)
        Dim actual As Nullable(Of DateTime) = New DateTime(2017, 2, 3)

        Dim testObj As IProjectionSnapshotWrittenEvent = ProjectionSnapshotWrittenEvent.Create(
            expected,
            101,
            "Location",
            "InMemory")


        Dim cmdObjDeserialised As ProjectionSnapshotWrittenEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.AsOfDate

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub ProjectionSnapshotWrittenEvent_RoundTrip_AsOfSequence_Serialisation_TestMethod()

        Dim expected As Integer = 123
        Dim actual As Integer = -3

        Dim testObj As IProjectionSnapshotWrittenEvent = ProjectionSnapshotWrittenEvent.Create(
            DateTime.Now,
            expected,
            "Location",
            "InMemory")


        Dim cmdObjDeserialised As ProjectionSnapshotWrittenEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.AsOfSequence

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub ProjectionSnapshotWrittenEvent_RoundTrip_SnapshotLocation_Serialisation_TestMethod()

        Dim expected As String = "Unit test query name"
        Dim actual As String = "Not set"

        Dim testObj As IProjectionSnapshotWrittenEvent = ProjectionSnapshotWrittenEvent.Create(
            DateTime.Now,
            123,
            expected,
            "InMemory")


        Dim cmdObjDeserialised As ProjectionSnapshotWrittenEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.SnapshotLocation

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub ProjectionSnapshotWrittenEvent_RoundTrip_WriterType_Serialisation_TestMethod()

        Dim expected As String = "Unit test writer"
        Dim actual As String = "Not set"

        Dim testObj As IProjectionSnapshotWrittenEvent = ProjectionSnapshotWrittenEvent.Create(
            DateTime.Now,
            123,
            "A test",
            expected)


        Dim cmdObjDeserialised As ProjectionSnapshotWrittenEvent

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

<TestFixture>
Public Class ProjectionRequestedEvent_UnitTest


    <TestCase()>
    Public Sub Constructor_TestMethod()

        Dim testObj As IProjectionRequestedEvent = ProjectionRequestedEvent.Create("Unit test source",
        DateTime.Now)

        Assert.IsNotNull(testObj)
    End Sub

    <TestCase()>
    Public Sub ProjectionRequestedEvent_RoundTrip_Source_Serialisation_TestMethod()

        Dim expected As String = "Unit test query name"
        Dim actual As String = "Not set"

        Dim testObj As IProjectionRequestedEvent = ProjectionRequestedEvent.Create(expected,
            DateTime.Now)


        Dim cmdObjDeserialised As ProjectionRequestedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.RequestSource

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub ProjectionRequestedEvent_RoundTrip_AsOfDate_Serialisation_TestMethod()

        Dim expected As Nullable(Of DateTime) = New DateTime(2017, 12, 17)
        Dim actual As Nullable(Of DateTime) = New DateTime(1988, 10, 19)

        Dim testObj As IProjectionRequestedEvent = ProjectionRequestedEvent.Create("test source",
            expected)


        Dim cmdObjDeserialised As ProjectionRequestedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.AsOfDate

        Assert.AreEqual(expected, actual)
    End Sub

End Class

<TestFixture>
Public Class ProjectionResultsReturnedEvent_UnitTest

    <TestCase()>
    Public Sub Constructor_TestMethod()

        Dim testObj As IProjectionResultsReturnedEvent = ProjectionResultsReturnedEvent.Create(
            DateTime.Now,
            123,
            "Unit test snapshot")

        Assert.IsNotNull(testObj)
    End Sub

    <TestCase()>
    Public Sub ProjectionResultsReturnedEvent_RoundTrip_AsOfDate_Serialisation_TestMethod()

        Dim expected As Nullable(Of DateTime) = New DateTime(2017, 12, 17)
        Dim actual As Nullable(Of DateTime) = New DateTime(1988, 10, 19)

        Dim testObj As IProjectionResultsReturnedEvent = ProjectionResultsReturnedEvent.Create(expected,
            123,
            "Unit test snapshot")


        Dim cmdObjDeserialised As IProjectionResultsReturnedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.AsOfDate

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub ProjectionResultsReturnedEvent_RoundTrip_AsOfSequence_Serialisation_TestMethod()

        Dim expected As Int32 = 123
        Dim actual As Int32 = 998

        Dim testObj As IProjectionResultsReturnedEvent = ProjectionResultsReturnedEvent.Create(DateTime.Now,
                                                                                               expected,
            "Unit test snapshot")


        Dim cmdObjDeserialised As IProjectionResultsReturnedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.AsOfSequence

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub ProjectionResultsReturnedEvent_RoundTrip_Location_Serialisation_TestMethod()

        Dim expected As String = "Unit test location"
        Dim actual As String = "Not set"

        Dim testObj As IProjectionResultsReturnedEvent = ProjectionResultsReturnedEvent.Create(DateTime.Now,
                                                                                               199, expected)


        Dim cmdObjDeserialised As IProjectionResultsReturnedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.ProjectionLocation

        Assert.AreEqual(expected, actual)
    End Sub
End Class