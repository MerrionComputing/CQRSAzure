Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.CQRSdsl.CodeGeneration

<TestClass()>
Public Class CommentCodeGeneratorUnitTest

    <TestMethod()>
    Public Sub RemarksCommentSection_Constructor_TestMethod()

        Dim remarksComment = CommentGeneration.RemarksCommentSection(New List(Of String)())

        Assert.IsNotNull(remarksComment)

    End Sub

    <TestMethod()>
    Public Sub SummaryCommentSection_MultiLine_Constructor_TestMethod()

        Dim commentLines As New List(Of String)
        commentLines.Add("Line 1")
        commentLines.Add("Line 2")
        Dim summaryComment = CommentGeneration.SummaryCommentSection(commentLines)

        Assert.IsNotNull(summaryComment)

    End Sub

    <TestMethod()>
    Public Sub RemarksCommentSection_MultiLine_Constructor_TestMethod()

        Dim commentLines As New List(Of String)
        commentLines.Add("Line 1")
        commentLines.Add("Line 2")
        Dim remarksComment = CommentGeneration.RemarksCommentSection(commentLines)

        Assert.IsNotNull(remarksComment)

    End Sub

    <TestMethod>
    Public Sub ParamCommentsSection_Constructor_TestMethod()

        Dim commentLines As New List(Of String)
        commentLines.Add("Line 1")
        commentLines.Add("Line 2")
        Dim paramsComment = CommentGeneration.ParamCommentsSection("myParam", commentLines)

        Assert.IsNotNull(paramsComment)

    End Sub

End Class