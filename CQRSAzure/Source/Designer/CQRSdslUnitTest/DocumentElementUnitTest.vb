Imports System.Text
Imports CQRSAzure.CQRSdsl.DocumentationGeneration
Imports Microsoft.VisualStudio.TestTools.UnitTesting


<TestClass()> Public Class DocumentElementUnitTest

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New DocumentElement("Test", IDocumentationWriter.DocumentationLevel.Heading)
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub ElementName_RoundTrip_TestMethod()

        Dim expected As String = "Expected element name"
        Dim actual As String = "not what was expected"

        Dim testObj As New DocumentElement(expected, IDocumentationWriter.DocumentationLevel.Heading)
        actual = testObj.Content

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub Level_RoundTrip_Heading_TestMethod()

        Dim expected As IDocumentationWriter.DocumentationLevel = IDocumentationWriter.DocumentationLevel.Heading
        Dim actual As IDocumentationWriter.DocumentationLevel = IDocumentationWriter.DocumentationLevel.ListItem

        Dim testObj As New DocumentElement("test", expected)
        actual = testObj.Level

        Assert.AreEqual(expected, actual)


    End Sub

    <TestMethod>
    Public Sub Level_RoundTrip_ListItem_TestMethod()

        Dim expected As IDocumentationWriter.DocumentationLevel = IDocumentationWriter.DocumentationLevel.ListItem
        Dim actual As IDocumentationWriter.DocumentationLevel = IDocumentationWriter.DocumentationLevel.Normal

        Dim testObj As New DocumentElement("test", expected)
        actual = testObj.Level

        Assert.AreEqual(expected, actual)


    End Sub

    <TestMethod>
    Public Sub Level_RoundTrip_Normal_TestMethod()

        Dim expected As IDocumentationWriter.DocumentationLevel = IDocumentationWriter.DocumentationLevel.Normal
        Dim actual As IDocumentationWriter.DocumentationLevel = IDocumentationWriter.DocumentationLevel.SubHeading

        Dim testObj As New DocumentElement("test", expected)
        actual = testObj.Level

        Assert.AreEqual(expected, actual)


    End Sub

    <TestMethod>
    Public Sub Level_RoundTrip_SubHeading_TestMethod()

        Dim expected As IDocumentationWriter.DocumentationLevel = IDocumentationWriter.DocumentationLevel.SubHeading
        Dim actual As IDocumentationWriter.DocumentationLevel = IDocumentationWriter.DocumentationLevel.Heading

        Dim testObj As New DocumentElement("test", expected)
        actual = testObj.Level

        Assert.AreEqual(expected, actual)


    End Sub

End Class