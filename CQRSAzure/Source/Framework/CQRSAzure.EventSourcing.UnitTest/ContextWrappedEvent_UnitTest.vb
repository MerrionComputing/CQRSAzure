Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class ContextWrappedEvent_UnitTest

    <TestMethod()>
    Public Sub Wrap_NotNull_TestMethod()

        Dim evtTest As New Mocking.MockEventTypeTwo() With {.EventTwoStringProperty = "A test"}
        Dim evtInst = New Mocking.MockEventTypeTwoInstance(evtTest, "123", 1)
        Dim testObj = ContextWrappedEvent(Of String).Wrap(evtInst,
                                                                            27,
                                                                            "Unit test",
                                                                            "Internal",
                                                                            Date.UtcNow,
                                                                            1,
                                                                            "Duncan")

        Assert.IsNotNull(testObj)

    End Sub


    <TestMethod()>
    Public Sub WrapOuter_NotNull_TestMethod()

        Dim evtTest As New Mocking.MockEventTypeTwo() With {.EventTwoStringProperty = "A test"}
        Dim testObj = ContextWrappedEvent(Of String).Wrap("123",
                                                          evtTest,
                                                          27,
                                                         "Unit test",
                                                         "Internal",
                                                         Date.UtcNow,
                                                         1,
                                                         "Duncan")

        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub WrapOuter_RoundTrip_Name()

        Dim expected As String = "Duncan Jones"
        Dim actual As String = "Not set"

        Dim evtTest As New Mocking.MockEventTypeTwo() With {.EventTwoStringProperty = "A test"}
        Dim testObj = ContextWrappedEvent(Of String).Wrap("123",
                                                          evtTest,
                                                          27,
                                                         "Unit test",
                                                         "Internal",
                                                         Date.UtcNow,
                                                         1,
                                                         expected)

        actual = testObj.Who

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub WrapOuter_RoundTrip_Key()

        Dim expected As String = "1234"
        Dim actual As String = "Not set"

        Dim evtTest As New Mocking.MockEventTypeTwo() With {.EventTwoStringProperty = "A test"}
        Dim testObj = ContextWrappedEvent(Of String).Wrap(expected,
                                                          evtTest,
                                                          27,
                                                         "Unit test",
                                                         "Internal",
                                                         Date.UtcNow,
                                                         1,
                                                         "Duncan")

        actual = testObj.AggregateKey

        Assert.AreEqual(expected, actual)

    End Sub

End Class

<TestClass()>
Public Class EventSerialiserFactory_UnitTest

    <TestMethod>
    Public Sub GetSerialiser_NotNull_TestMethod()

        EventSerializerFactory.AddOrSetSerialiser(Of Mocking.MockEventTypeTwo)(EventSerializerFactory.Create(Of Mocking.MockEventTypeTwo)())
        Dim testObj = EventSerializerFactory.GetSerialiserByType(Of Mocking.MockEventTypeTwo)(GetType(Mocking.MockEventTypeTwo))
        Assert.IsNotNull(testObj)

    End Sub


    <TestMethod>
    Public Sub Create_NoParams_TestMethod()

        Dim testObj = EventSerializerFactory.Create(Of Mocking.MockEventTypeTwo)
        Assert.IsNotNull(testObj)

    End Sub


    <TestMethod>
    Public Sub CreateAndUsetStreamFormatterFunction_TestMethod()

        Dim unexpected As Long = 0
        Dim actual As Long = 0

        Dim testObj = EventSerializerFactory.CreateDefaultStreamFormatterFunction(Of Mocking.MockEventTypeTwo)()

        Dim stream As New System.IO.MemoryStream
        Dim foo As New Mocking.MockEventTypeTwo() With {.EventTwoDecimalProperty = 17.3, .EventTwoStringProperty = "Test"}

        Dim formatter = testObj.Invoke()
        If (formatter IsNot Nothing) Then
            formatter.Serialize(stream, foo)
        End If
        actual = stream.Position

        Assert.AreNotEqual(unexpected, actual)

    End Sub



    <TestMethod>
    Public Sub CreateDefaultStreamFormatterFunction_TestMethod()

        Dim testObj = EventSerializerFactory.CreateDefaultStreamFormatterFunction(Of Mocking.MockEventTypeTwo)()
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub RoundTripEvent_StreamFunction_TestMethod()

        Dim expected As String = "Expected Output"
        Dim actual As String = "Actual Output"

        Dim testSerialise = EventSerializerFactory.CreateDefaultStreamFormatterFunction(Of Mocking.MockEventTypeTwo)()

        Dim foo As New Mocking.MockEventTypeTwo() With {.EventTwoDecimalProperty = 17.3, .EventTwoStringProperty = expected}

        Dim stream As New System.IO.MemoryStream()

        Dim formatter = testSerialise.Invoke()
        If (formatter IsNot Nothing) Then
            formatter.Serialize(stream, foo)
            stream.Seek(0, IO.SeekOrigin.Begin)
            Dim fooRet As Mocking.MockEventTypeTwo = formatter.Deserialize(stream)
            actual = fooRet.EventTwoStringProperty
        End If


        Assert.AreEqual(expected, actual)


    End Sub


    <TestMethod>
    Public Sub RoundTripEvent_StreamFunction_AttributedSubstitute_TestMethod()

        Dim expected As String = "Expected Output"
        Dim actual As String = "Actual Output"

        Dim testSerialise = EventSerializerFactory.CreateDefaultStreamFormatterFunction(Of Mocking.MockEventTypeTwoWithSerialser)()

        Dim foo As New Mocking.MockEventTypeTwoWithSerialser() With {.EventTwoDecimalProperty = 17.3, .EventTwoStringProperty = expected}

        Dim stream As New System.IO.MemoryStream

        Dim formatter = testSerialise.Invoke()
        If (formatter IsNot Nothing) Then
            formatter.Serialize(stream, foo)
            stream.Seek(0, IO.SeekOrigin.Begin)
            Dim fooRet As Mocking.MockEventTypeTwoWithSerialser = formatter.Deserialize(stream)
            actual = fooRet.EventTwoStringProperty
        End If

        Assert.AreEqual(expected, actual)


    End Sub

    <TestMethod>
    Public Sub CreateDefaultSaveNameValuePairsFunction_TestMethod()

        Dim testObj = EventSerializerFactory.CreateDefaultSaveNameValuePairsFunction(Of Mocking.MockEventTypeTwo)()
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub CreateAndUseDefaultSaveNameValuePairsFunction_TestMethod()

        Dim testObj = EventSerializerFactory.CreateDefaultSaveNameValuePairsFunction(Of Mocking.MockEventTypeTwo)()

        Dim foo As New Mocking.MockEventTypeTwo() With {.EventTwoDecimalProperty = 17.3, .EventTwoStringProperty = "Test"}
        Dim fooDictionary = testObj.Invoke(foo)

        Assert.IsNotNull(fooDictionary)


    End Sub

    <TestMethod>
    Public Sub CreateAndUseDefaultSaveNameValuePairsFunction_AttributedSubstitute_TestMethod()

        Dim testObj = EventSerializerFactory.CreateDefaultSaveNameValuePairsFunction(Of Mocking.MockEventTypeTwoWithSerialser)()

        Dim foo As New Mocking.MockEventTypeTwoWithSerialser() With {.EventTwoDecimalProperty = 17.3, .EventTwoStringProperty = "Test"}
        Dim fooDictionary = testObj.Invoke(foo)

        Assert.IsNotNull(fooDictionary)


    End Sub

    <TestMethod>
    Public Sub RoundTripEvent_NameValuePairsFunction_AttributedSubstitute_TestMethod()

        Dim expected As String = "Expected Output"
        Dim actual As String = "Actual Output"

        Dim testSerialise = EventSerializerFactory.CreateDefaultSaveNameValuePairsFunction(Of Mocking.MockEventTypeTwoWithSerialser)()
        Dim testDeserialise = EventSerializerFactory.CreateDefaultReadNameValuePairsFunction(Of Mocking.MockEventTypeTwoWithSerialser)()

        Dim foo As New Mocking.MockEventTypeTwoWithSerialser() With {.EventTwoDecimalProperty = 17.3, .EventTwoStringProperty = expected}
        Dim fooDictionary = testSerialise.Invoke(foo)
        Dim ret As Mocking.MockEventTypeTwoWithSerialser = testDeserialise.Invoke(fooDictionary)

        actual = ret.EventTwoStringProperty

        Assert.AreEqual(expected, actual)


    End Sub

    <TestMethod>
    Public Sub CreateDefaultReadNameValuePairsFunction_TestMethod()

        Dim testObj = EventSerializerFactory.CreateDefaultReadNameValuePairsFunction(Of Mocking.MockEventTypeTwo)()
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub CreateAnduseDefaultReadNameValuePairsFunction_TestMethod()

        Dim testObj = EventSerializerFactory.CreateDefaultReadNameValuePairsFunction(Of Mocking.MockEventTypeTwo)()
        Dim fooDictionary As New Dictionary(Of String, Object)
        fooDictionary.Add("EventTwoDecimalProperty", 18.37D)
        fooDictionary.Add("EventTwoStringProperty", "This is a test")
        fooDictionary.Add("NoSuchProperty", "This is a negative test")

        Dim foo = testObj.Invoke(fooDictionary)
        Assert.IsNotNull(foo)

    End Sub

    <TestMethod>
    Public Sub RoundTrip_SerialiserFactory_UnitTest()

        'load the serialisers
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Opened) _
            (OpenedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Closed) _
            (ClosedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Money_Deposited) _
            (DepositedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Money_Withdrawn) _
            (WithdrawnEventSerialiser.Create())

        Dim obj = EventSerializerFactory.GetSerialiserByType(Of Accounts.Account.eventDefinition.Money_Deposited) _
        (GetType(Accounts.Account.eventDefinition.Money_Deposited))

        Assert.IsNotNull(obj)

    End Sub


    <TestMethod>
    Public Sub SerialiserFactory_Untyped_Get_UnitTest()

        'load the serialisers
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Opened) _
            (OpenedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Closed) _
            (ClosedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Money_Deposited) _
            (DepositedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Money_Withdrawn) _
            (WithdrawnEventSerialiser.Create())

        Dim eventType As Type = GetType(Accounts.Account.eventDefinition.Closed)

        Dim deserialiser As IEventSerializer = EventSerializerFactory.GetSerialiserByType(eventType)

        Assert.IsNotNull(deserialiser)

    End Sub

End Class