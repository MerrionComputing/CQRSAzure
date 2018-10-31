Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.CQRSdsl.DocumentationGeneration

<TestClass()>
Public Class HTMLDocumentationWriterUnitTest

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New HTMLDocumentationWriter()
        Assert.IsNotNull(testObj)



    End Sub

    <TestMethod()>
    Public Sub CreatePage_RoundTrip_TestMethod()

        Dim expected As String = "New page"
        Dim actual As String = "Old page"

        Dim testObj As New HTMLDocumentationWriter()


        testObj.CreatePage(expected)

        actual = testObj.CurrentPage

        Assert.AreEqual(expected, actual)



    End Sub

End Class