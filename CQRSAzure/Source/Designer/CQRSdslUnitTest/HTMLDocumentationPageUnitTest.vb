Imports System.Text
Imports CQRSAzure.CQRSdsl.DocumentationGeneration
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class HTMLDocumentationPageUnitTest

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New HTMLDocumentationPage("My page")
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub PageName_RoundTrip_TestMethod()

        Dim expected As String = "My page"
        Dim actual As String = "foo"

        Dim testObj As New HTMLDocumentationPage(expected)

        actual = testObj.PageName

        Assert.AreEqual(expected, actual)

    End Sub



End Class