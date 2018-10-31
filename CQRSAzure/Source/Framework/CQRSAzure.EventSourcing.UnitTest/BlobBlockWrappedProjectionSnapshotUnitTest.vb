Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.EventSourcing.Azure.Blob
Imports CQRSAzure.EventSourcing.UnitTest.Mocking

<TestClass()>
Public Class BlobBlockWrappedProjectionSnapshotUnitTest

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testProjection As New MockProjectionWithSnapshots()
        Dim testObj As New BlobBlockWrappedProjectionSnapshot(testProjection.ToSnapshot())

        Assert.IsNotNull(testObj)

    End Sub

End Class