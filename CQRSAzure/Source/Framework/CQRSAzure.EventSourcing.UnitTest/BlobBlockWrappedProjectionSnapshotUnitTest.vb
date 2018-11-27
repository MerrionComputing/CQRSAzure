Imports System.Text
Imports NUnit.Framework

Imports CQRSAzure.EventSourcing.Azure.Blob
Imports CQRSAzure.EventSourcing.UnitTest.Mocking

<TestFixture()>
Public Class BlobBlockWrappedProjectionSnapshotUnitTest

    <TestCase()>
    Public Sub Constructor_TestMethod()

        Dim testProjection As New MockProjectionWithSnapshots()
        Dim testObj As New BlobBlockWrappedProjectionSnapshot(testProjection.ToSnapshot())

        Assert.IsNotNull(testObj)

    End Sub

End Class