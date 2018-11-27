Imports System.Text
Imports NUnit.Framework

<TestFixture()>
Public Class HistoryQueueUnitTest

    <TestCase()>
    Public Sub Constructor_Zero_TestMethod()

        Dim testObj As HistoryQueue(Of String) = New HistoryQueue(Of String)(0)
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub Constructor_IntMax_TestMethod()

        Dim testObj As HistoryQueue(Of String) = New HistoryQueue(Of String)(Integer.MaxValue)
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub Constructor_Negative_TestMethod()

        Dim testObj As HistoryQueue(Of String) = New HistoryQueue(Of String)(-9)
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub Undercount_TestMethod()

        Dim expected As Integer = 3
        Dim actual As Integer = 0

        Dim testObj As HistoryQueue(Of String) = New HistoryQueue(Of String)(10)
        testObj.Enqueue("Item 1")
        testObj.Enqueue("Item 2")
        testObj.Enqueue("Item 3")
        actual = testObj.LongCount()

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub Undercount_ZeroMax_TestMethod()

        Dim expected As Integer = 3
        Dim actual As Integer = 0

        Dim testObj As HistoryQueue(Of String) = New HistoryQueue(Of String)(0)
        testObj.Enqueue("Item 1")
        testObj.Enqueue("Item 2")
        testObj.Enqueue("Item 3")
        actual = testObj.LongCount()

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub Overcount_TestMethod()

        Dim expected As Integer = 3
        Dim actual As Integer = 0

        Dim testObj As HistoryQueue(Of String) = New HistoryQueue(Of String)(3)
        testObj.Enqueue("Item 1")
        testObj.Enqueue("Item 2")
        testObj.Enqueue("Item 3")
        testObj.Enqueue("Item 4")
        testObj.Enqueue("Item 5")
        actual = testObj.LongCount()

        Assert.AreEqual(expected, actual)

    End Sub



End Class