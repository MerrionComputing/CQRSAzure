Imports System.Text
Imports NUnit.Framework

Imports CQRSAzure.QueryDefinition

<TestFixture()>
Public Class QueryParameterUnitTest

    <TestCase()>
    Public Sub Constructor_Integer_TestMethod()

        Dim paramObj As QueryParameter(Of Integer) =
            QueryParameter(Of Integer).Create("test", 0, 1234)

        Assert.IsNotNull(paramObj)

    End Sub

    <TestCase()>
    Public Sub Constructor_String_TestMethod()

        Dim paramObj As QueryParameter(Of String) =
            QueryParameter(Of String).Create("test", 0, "test value")

        Assert.IsNotNull(paramObj)

    End Sub

    <TestCase()>
    Public Sub Constructor_Guid_TestMethod()

        Dim paramObj As QueryParameter(Of Guid) =
            QueryParameter(Of Guid).Create("test", 0, Guid.Empty)

        Assert.IsNotNull(paramObj)

    End Sub

    <TestCase>
    Public Sub Value_Integer_RoundTrip()

        Dim expected As Integer = 123
        Dim actual As Integer = -7

        Dim paramObj As QueryParameter(Of Integer) =
            QueryParameter(Of Integer).Create("test", 0, expected)

        actual = paramObj.Value

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase>
    Public Sub Value_String_RoundTrip()

        Dim expected As String = "This is a test"
        Dim actual As String = "Actual value"

        Dim paramObj As QueryParameter(Of String) =
            QueryParameter(Of String).Create("test", 0, expected)

        actual = paramObj.Value

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase>
    Public Sub Value_String_Serialisation_RoundTrip()

        Dim expected As String = "This is a test"
        Dim actual As String = "Actual value"

        Dim paramObj As QueryParameter(Of String) =
            QueryParameter(Of String).Create("test", 0, expected)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(paramObj.GetType())
        Dim ms As New System.IO.MemoryStream(5000)
        ser.WriteObject(ms, paramObj)
        ms.Seek(0, IO.SeekOrigin.Begin)

        Dim paramObjDeserialised As QueryParameter(Of String)
        paramObjDeserialised = ser.ReadObject(ms)

        actual = paramObjDeserialised.Value

        Assert.AreEqual(expected, actual)

    End Sub


    <TestCase>
    Public Sub Name_String_Serialisation_RoundTrip()

        Dim expected As String = "expected name"
        Dim actual As String = "Actual name"

        Dim paramObj As QueryParameter(Of String) =
            QueryParameter(Of String).Create(expected, 0, "This is a test value")

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(paramObj.GetType())
        Dim ms As New System.IO.MemoryStream(5000)
        ser.WriteObject(ms, paramObj)
        ms.Seek(0, IO.SeekOrigin.Begin)

        Dim paramObjDeserialised As QueryParameter(Of String)
        paramObjDeserialised = ser.ReadObject(ms)

        actual = paramObjDeserialised.Name

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase>
    Public Sub Value_Integer_Serialisation_RoundTrip()

        Dim expected As Integer = 123
        Dim actual As Integer = 456

        Dim paramObj As QueryParameter(Of Integer) =
            QueryParameter(Of Integer).Create("test", 0, expected)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(paramObj.GetType())
        Dim ms As New System.IO.MemoryStream(5000)
        ser.WriteObject(ms, paramObj)
        ms.Seek(0, IO.SeekOrigin.Begin)

        Dim paramObjDeserialised As QueryParameter(Of Integer)
        paramObjDeserialised = ser.ReadObject(ms)

        actual = paramObjDeserialised.Value

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase>
    Public Sub Index_Integer_Serialisation_RoundTrip()

        Dim expected As Integer = 123
        Dim actual As Integer = 456

        Dim paramObj As QueryParameter(Of Integer) =
            QueryParameter(Of Integer).Create("test", expected, 9876)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(paramObj.GetType())
        Dim ms As New System.IO.MemoryStream(5000)
        ser.WriteObject(ms, paramObj)
        ms.Seek(0, IO.SeekOrigin.Begin)

        Dim paramObjDeserialised As QueryParameter(Of Integer)
        paramObjDeserialised = ser.ReadObject(ms)

        actual = paramObjDeserialised.Index

        Assert.AreEqual(expected, actual)

    End Sub

End Class