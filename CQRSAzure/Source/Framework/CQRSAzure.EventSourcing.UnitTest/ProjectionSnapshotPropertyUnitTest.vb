Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports CQRSAzure.EventSourcing

<TestClass()> Public Class ProjectionSnapshotPropertyUnitTest

    <TestMethod()>
    Public Sub Constructor_Integer_TestMethod()

        Dim testObj As IProjectionSnapshotProperty = ProjectionSnapshotProperty.Create("Test", 1)
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub Constructor_String_TestMethod()

        Dim testObj As IProjectionSnapshotProperty = ProjectionSnapshotProperty.Create("Test", "test")
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub Constructor_Object_Empty_TestMethod()

        Dim testObj As IProjectionSnapshotProperty = ProjectionSnapshotProperty.Create(Of Object)("Test", Nothing)
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub Constructor_DateTime_TestMethod()

        Dim testObj As IProjectionSnapshotProperty = ProjectionSnapshotProperty.Create("Test", DateTime.MinValue)
        Assert.IsNotNull(testObj)

    End Sub

End Class