Imports System.Text
Imports CQRSAzure.IdentifierGroup.Local.File
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class LocalFileClassifierSnapshotReaderUnitTest

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As LocalFileClassifierSnapshotReader(Of MockAggregate, String, MockClassifierOddNumber)
        testObj = LocalFileClassifierSnapshotReader(Of MockAggregate, String, MockClassifierOddNumber).Create(New MockAggregate("123"),
                                                                                                              New MockClassifierOddNumber("123"))

        Assert.IsNotNull(testObj)

    End Sub

End Class