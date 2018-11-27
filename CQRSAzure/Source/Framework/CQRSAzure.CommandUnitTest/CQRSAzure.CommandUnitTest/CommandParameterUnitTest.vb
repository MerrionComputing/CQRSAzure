
Imports CQRSAzure.CommandDefinition
Imports NUnit.Framework

<TestFixture()>
Public Class CommandParameterUnitTest

    <TestCase()>
    Public Sub Constructor_Integer_TestMethod()

        Dim cmdObj As CommandParameter(Of Integer) =
            CommandParameter(Of Integer).Create("Test", 0, 123)

        Assert.IsNotNull(cmdObj)

    End Sub

    <TestCase()>
    Public Sub Constructor_String_TestMethod()

        Dim cmdObj As CommandParameter(Of String) =
            CommandParameter(Of String).Create("Test", 0, "Test value")

        Assert.IsNotNull(cmdObj)

    End Sub

    <TestCase()>
    Public Sub Constructor_Guid_TestMethod()

        Dim cmdObj As CommandParameter(Of Guid) =
            CommandParameter(Of Guid).Create("Test", 0, Guid.Empty)

        Assert.IsNotNull(cmdObj)

    End Sub

    <TestCase()>
    Public Sub Value_Integer_RoundTrip()

        Dim expected As Integer = 3345
        Dim actual As Integer = -22

        Dim cmdObj As CommandParameter(Of Integer) =
            CommandParameter(Of Integer).Create("Test", 0, expected)

        actual = cmdObj.Value

        Assert.AreEqual(expected, actual)

    End Sub


    <TestCase()>
    Public Sub Value_Guid_RoundTrip()

        Dim expected As Guid = New Guid("81b64d0a-a4e5-450d-977e-ab2f3802370c")
        Dim actual As Guid = Guid.Empty

        Dim cmdObj As CommandParameter(Of Guid) =
            CommandParameter(Of Guid).Create("Test", 0, expected)

        actual = cmdObj.Value

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub Value_Guid_Serialisation_RoundTrip()

        Dim expected As Guid = New Guid("81b64d0a-a4e5-450d-977e-ab2f3802370c")
        Dim actual As Guid = Guid.Empty

        Dim cmdObj As CommandParameter(Of Guid) =
            CommandParameter(Of Guid).Create("Test", 0, expected)

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(cmdObj.GetType())
        Dim ms As New System.IO.MemoryStream(5000)
        ser.WriteObject(ms, cmdObj)
        ms.Seek(0, IO.SeekOrigin.Begin)

        Dim cmdObjDeserialised As CommandParameter(Of Guid)
        cmdObjDeserialised = ser.ReadObject(ms)

        actual = cmdObjDeserialised.Value

        Assert.AreEqual(expected, actual)

    End Sub


    <TestCase()>
    Public Sub Name_Guid_Serialisation_RoundTrip()

        Dim expected As String = "Expected name"
        Dim actual As String = "Actual name"

        Dim cmdObj As CommandParameter(Of Guid) =
            CommandParameter(Of Guid).Create(expected, 0, New Guid("81b64d0a-a4e5-450d-977e-ab2f3802370c"))

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(cmdObj.GetType())
        Dim ms As New System.IO.MemoryStream(5000)
        ser.WriteObject(ms, cmdObj)
        ms.Seek(0, IO.SeekOrigin.Begin)

        Dim cmdObjDeserialised As CommandParameter(Of Guid)
        cmdObjDeserialised = ser.ReadObject(ms)

        actual = cmdObjDeserialised.Name

        Assert.AreEqual(expected, actual)

    End Sub


    <TestCase()>
    Public Sub Index_Guid_Serialisation_RoundTrip()

        Dim expected As Integer = 33
        Dim actual As Integer = 2

        Dim cmdObj As CommandParameter(Of Guid) =
            CommandParameter(Of Guid).Create("Command parameter name", expected, New Guid("81b64d0a-a4e5-450d-977e-ab2f3802370c"))

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(cmdObj.GetType())
        Dim ms As New System.IO.MemoryStream(5000)
        ser.WriteObject(ms, cmdObj)
        ms.Seek(0, IO.SeekOrigin.Begin)

        Dim cmdObjDeserialised As CommandParameter(Of Guid)
        cmdObjDeserialised = ser.ReadObject(ms)

        actual = cmdObjDeserialised.Index

        Assert.AreEqual(expected, actual)

    End Sub

End Class