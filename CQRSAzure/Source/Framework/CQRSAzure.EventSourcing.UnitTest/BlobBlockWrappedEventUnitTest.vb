Imports System.Text
Imports NUnit.Framework

Imports CQRSAzure.EventSourcing.Azure.Blob
Imports CQRSAzure.EventSourcing.UnitTest.Mocking

<TestFixture()>
Public Class BlobBlockWrappedEventUnitTest

    <TestCase()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New BlobBlockWrappedEvent(12, 0, DateTime.UtcNow, New MockEventTypeOne() With {.EventOneStringProperty = "Test Case"})
        Assert.IsNotNull(testObj)

    End Sub


    <TestCase()>
    Public Sub ToBinaryStream_NonZeroSize_TestMethod()

        Dim testObj As New BlobBlockWrappedEvent(12, 0, DateTime.UtcNow, New MockEventTypeOne() With {.EventOneStringProperty = "Test Case"})
        Dim testStream As System.IO.Stream = testObj.ToBinaryStream()

        Assert.IsTrue(testStream.Length > 0)

    End Sub

    <TestCase()>
    Public Sub ToBinaryStream_RoundTrip_TestMethod()

        Dim actual As String = ""
        Dim expected As String = "Test Case"

        Dim testObj As New BlobBlockWrappedEvent(12, 0, DateTime.UtcNow, New MockEventTypeOne() With {.EventOneStringProperty = expected})
        Dim testStream As System.IO.Stream = testObj.ToBinaryStream()
        testStream.Seek(0, IO.SeekOrigin.Begin)
        Dim testObj2 As BlobBlockWrappedEvent = BlobBlockWrappedEvent.FromBinaryStream(testStream)
        actual = CType(testObj2.EventInstance, MockEventTypeOne).EventOneStringProperty

        Assert.AreEqual(expected, actual)

    End Sub



End Class