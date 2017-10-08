Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class KeyConverterFactoryUnitTest

    <TestMethod()>
    Public Sub StringConverter_TestMethod()

        Dim expected As String = "expected"
        Dim actual As String = "actual"

        Dim converter = KeyConverterFactory.CreateKeyConverter(Of String)
        actual = converter.FromString(converter.ToUniqueString(expected))

        Assert.AreEqual(actual, expected)

    End Sub

    <TestMethod()>
    Public Sub IntegerConverter_TestMethod()

        Dim expected As Integer = 1912
        Dim actual As Integer = -29

        Dim converter = KeyConverterFactory.CreateKeyConverter(Of Integer)
        actual = converter.FromString(converter.ToUniqueString(expected))

        Assert.AreEqual(actual, expected)

    End Sub

    <TestMethod()>
    Public Sub DoubleConverter_TestMethod()

        Dim expected As Double = 1912.234
        Dim actual As Double = -29.88

        Dim converter = KeyConverterFactory.CreateKeyConverter(Of Double)
        actual = converter.FromString(converter.ToUniqueString(expected))

        Assert.AreEqual(actual, expected)

    End Sub

    <TestMethod()>
    Public Sub DecimalConverter_TestMethod()

        Dim expected As Decimal = 1912.234
        Dim actual As Decimal = -29.88

        Dim converter = KeyConverterFactory.CreateKeyConverter(Of Decimal)
        actual = converter.FromString(converter.ToUniqueString(expected))

        Assert.AreEqual(actual, expected)

    End Sub

End Class