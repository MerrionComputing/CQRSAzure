Imports System.Text
Imports CQRSAzure.EventSourcing.UnitTest.Mocking
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class EventAsOfDateAttributeUnitTest

    <TestMethod()>
    Public Sub NoAttribute_NoDate_TestMethod()

        Dim expected As Nullable(Of DateTime) = Nothing
        Dim actual As Nullable(Of DateTime) = New DateTime(1971, 12, 19, 3, 45, 18)


        Dim testEvent As New MockEventTypeOne() With {.EventOneIntegerProperty = 17, .EventOneStringProperty = "My test"}
        actual = EventAsOfDateAttribute.GetAsOfDate(testEvent)

        Assert.AreEqual(expected.HasValue, actual.HasValue)

    End Sub

    <TestMethod()>
    Public Sub HasAttribute_NoDate_TestMethod()

        Dim expected As Nullable(Of DateTime) = Nothing
        Dim actual As Nullable(Of DateTime) = New DateTime(1971, 12, 19, 3, 45, 18)


        Dim testEvent As New MockEventTypeTwo() With {.EventTwoDecimalProperty = 17.2, .EventTwoStringProperty = "My test"}
        actual = EventAsOfDateAttribute.GetAsOfDate(testEvent)

        Assert.AreEqual(expected.HasValue, actual.HasValue)

    End Sub

    <TestMethod()>
    Public Sub HasAttribute_HasDate_TestMethod()

        Dim expected As Nullable(Of DateTime) = New DateTime(2021, 12, 19, 3, 45, 18)
        Dim actual As Nullable(Of DateTime) = New DateTime(1971, 12, 19, 3, 45, 18)


        Dim testEvent As New MockEventTypeTwo() With {.EventTwoDecimalProperty = 17.2, .EventTwoStringProperty = "My test", .EventTwoNullableDateProperty = expected}
        actual = EventAsOfDateAttribute.GetAsOfDate(testEvent)

        Assert.AreEqual(expected.Value, actual.Value)

    End Sub

End Class