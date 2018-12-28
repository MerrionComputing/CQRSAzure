Imports System.Text
Imports NUnit.Framework
Imports CQRSAzure.IdentifierGroup.Azure.Blob.Untyped


<TestFixture()>
Public Class AzureBlobIdentifierGroupProcessorUntyped_UnitTest

    <TestCase()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New AzureBlobIdentifierGroupProcessorUntyped("Leagues", "Command", "UnitTestStorageConnectionString")
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Async Function GetAll_TestMethod() As Task

        Dim testObj As New AzureBlobIdentifierGroupProcessorUntyped("Query", "Get League Summary", "QueryConnectionString")
        Dim values = Await testObj.GetAll()

        Assert.IsNotNull(values)

    End Function

    <TestCase()>
    Public Async Function GetAll_WithDate_TestMethod() As Task

        Dim testObj As New AzureBlobIdentifierGroupProcessorUntyped("UnitTest", "MockAggregate", "UnitTestStorageConnectionString")
        Dim values = Await testObj.GetAll(New DateTime(2018, 11, 1))

        Assert.IsNotNull(values)

    End Function
End Class
