Imports System.Text
Imports NUnit.Framework

Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Commands
Imports CQRSAzure.EventSourcing.InMemory

<TestFixture()>
Public Class CommandStatusProjection_UnitTest

    <TestCase()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New CommandStatusProjection()
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub CommandCreatedEvent_Constructor_TestMethod()

        Dim testObj As CommandCreatedEvent = CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now)
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub CommandCreatedEvent_CommandName_RoundTrip_TestMethod()

        Dim expected As String = "My command"
        Dim actual As String = "Not set"

        Dim testObj As CommandCreatedEvent = CommandCreatedEvent.Create(Guid.NewGuid, expected, DateTime.Now)
        actual = testObj.CommandName

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub CommandStatusProjection_CreatedEvent_TestMethod()

        Dim expected As String = "My command"
        Dim actual As String = "Not set"

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, expected, DateTime.Now))

        'Run the projection
        Dim testReadObj As ProjectionProcessor(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid).CreateProjectionProcessor(cmdAgg)

        Dim projObj As New CommandStatusProjection()
        testReadObj.Process(projObj)

        actual = projObj.CommandName

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub CommandStatusProjection_CancelledEvent_TestMethod()

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        testEventStream.AppendEvent(CommandCancelledEvent.Create(DateTime.Now, "Test cancel"))

        'Run the projection
        Dim testReadObj As ProjectionProcessor(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid).CreateProjectionProcessor(cmdAgg)

        Dim projObj As New CommandStatusProjection()
        testReadObj.Process(projObj)

        actual = projObj.IsCancelled

        Assert.AreEqual(expected, actual)

    End Sub


    <TestCase()>
    Public Sub CommandStatusProjection_CompleteEvent_TestMethod()

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now, "Parameters location"))
        testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete"))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))
        testEventStream.AppendEvent(CommandCompletedEvent.Create(DateTime.Now, "Command complete"))

        'Run the projection
        Dim testReadObj As ProjectionProcessor(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid).CreateProjectionProcessor(cmdAgg)

        Dim projObj As New CommandStatusProjection()
        testReadObj.Process(projObj)

        actual = projObj.IsComplete

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub CommandStatusProjection_FatalErrorEvent_TestMethod()

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete"))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))
        testEventStream.AppendEvent(CommandFatalErrorOccuredEvent.Create(DateTime.Now, "Command failed", 1))

        'Run the projection
        Dim testReadObj As ProjectionProcessor(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid).CreateProjectionProcessor(cmdAgg)

        Dim projObj As New CommandStatusProjection()
        testReadObj.Process(projObj)

        actual = projObj.IsFatalError

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub CommandStatusProjection_TransientErrorEvent_TestMethod()

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete"))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))
        testEventStream.AppendEvent(CommandTransientFaultOccuredEvent.Create(DateTime.Now, "Command stuck", 1))

        'Run the projection
        Dim testReadObj As ProjectionProcessor(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid).CreateProjectionProcessor(cmdAgg)

        Dim projObj As New CommandStatusProjection()
        testReadObj.Process(projObj)

        actual = projObj.IsTransientError

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub CommandStatusProjection_TransientErrorEvent_Requeued_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete"))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))
        testEventStream.AppendEvent(CommandTransientFaultOccuredEvent.Create(DateTime.Now, "Command stuck", 1))
        testEventStream.AppendEvent(CommandRequeuedEvent.Create(DateTime.Now))

        'Run the projection
        Dim testReadObj As ProjectionProcessor(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid).CreateProjectionProcessor(cmdAgg)

        Dim projObj As New CommandStatusProjection()
        testReadObj.Process(projObj)

        actual = projObj.IsTransientError

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub CommandStatusProjection_StepCompletedrEvent_TestMethod()

        Dim expected As Integer = 1
        Dim actual As Integer = 77

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete"))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))


        'Run the projection
        Dim testReadObj As ProjectionProcessor(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid).CreateProjectionProcessor(cmdAgg)

        Dim projObj As New CommandStatusProjection()
        testReadObj.Process(projObj)

        actual = projObj.LastStepComplete

        Assert.AreEqual(expected, actual)

    End Sub

End Class

<TestFixture()>
Public Class CommandCancelledEvent_UnitTest

    <TestCase()>
    Public Sub CommandCancelledEvent_Constructor_TestMethod()

        Dim testObj As CommandCancelledEvent = CommandCancelledEvent.Create(DateTime.Now, "Unit test")

        Assert.IsNotNull(testObj)

    End Sub


    <TestCase()>
    Public Sub CommandCancelledEvent_RoundTrip_Reason_Serialisation_TestMethod()

        Dim expected As String = "fault reason"
        Dim actual As String = "Not set"

        Dim testObj As CommandCancelledEvent = CommandCancelledEvent.Create(DateTime.UtcNow,
            expected)

        Dim cmdObjDeserialised As CommandCancelledEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.Reason

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub CommandCancelledEvent_RoundTrip_Date_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2016, 3, 4)
        Dim actual As DateTime = New DateTime(1987, 12, 4)

        Dim testObj As CommandCancelledEvent = CommandCancelledEvent.Create(expected,
                                                                             "Unit testing",
                                                                             CommandEventContext.Create("source", "username", "token"))

        Dim cmdObjDeserialised As CommandCancelledEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.CancellationDate

        Assert.AreEqual(expected, actual)
    End Sub

End Class

<TestFixture()>
Public Class CommandCompletedEvent_UnitTest

    <TestCase()>
    Public Sub CommandCompletedEvent_Constructor_TestMethod()

        Dim testObj As CommandCompletedEvent = CommandCompletedEvent.Create(DateTime.Now, "Unit test")

        Assert.IsNotNull(testObj)

    End Sub


    <TestCase()>
    Public Sub CommandCompletedEvent_RoundTrip_SuccessMessage_Serialisation_TestMethod()

        Dim expected As String = "unit test command succeeded"
        Dim actual As String = "Not set"

        Dim testObj As CommandCompletedEvent = CommandCompletedEvent.Create(DateTime.UtcNow,
            expected)

        Dim cmdObjDeserialised As CommandCompletedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.SuccessMessage

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub CommandCompletedEvent_RoundTrip_Date_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2016, 3, 4)
        Dim actual As DateTime = New DateTime(1987, 12, 4)

        Dim testObj As CommandCompletedEvent = CommandCompletedEvent.Create(expected,
                                                                             "Unit testing",
                                                                             CommandEventContext.Create("source", "username", "token"))

        Dim cmdObjDeserialised As CommandCompletedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.CompletionDate

        Assert.AreEqual(expected, actual)
    End Sub

End Class

<TestFixture()>
Public Class CommandCreatedEvent_UnitTest


    <TestCase()>
    Public Sub CommandCreatedEvent_Constructor_TestMethod()

        Dim testObj As CommandCreatedEvent = CommandCreatedEvent.Create(Guid.NewGuid, "Unit Test Command", Nothing, "")

        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub CommandCreatedEvent_RoundTrip_CommandName_Serialisation_TestMethod()

        Dim expected As String = "unit test command"
        Dim actual As String = "Not set"

        Dim testObj As CommandCreatedEvent = CommandCreatedEvent.Create(Guid.NewGuid, expected, Nothing, "")

        Dim cmdObjDeserialised As CommandCreatedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.CommandName

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub CommandCreatedEvent_RoundTrip_CommandIdentifier_Serialisation_TestMethod()

        Dim expected As Guid = Guid.NewGuid()
        Dim actual As Guid = Guid.Empty

        Dim testObj As CommandCreatedEvent = CommandCreatedEvent.Create(expected, "uniy test command", Nothing, "")

        Dim cmdObjDeserialised As CommandCreatedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.CommandUniqueIdentifier

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub CommandCreatedEvent_RoundTrip_CreateDate_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2017, 3, 12)
        Dim actual As DateTime = New DateTime(1988, 12, 3)

        Dim testObj As CommandCreatedEvent = CommandCreatedEvent.Create(Guid.NewGuid, "uniy test command", expected, "")

        Dim cmdObjDeserialised As CommandCreatedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.CreationDate.Value

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub CommandCreatedEvent_RoundTrip_Parameters_Serialisation_TestMethod()

        Dim expected As String = "Unit test command parameters"
        Dim actual As String = "Not set"

        Dim testObj As CommandCreatedEvent = CommandCreatedEvent.Create(Guid.NewGuid, "uniy test command", Nothing, expected)

        Dim cmdObjDeserialised As CommandCreatedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.CommandParameters

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub CommandCreatedEvent_RoundTrip_IdentityGroupName_Serialisation_TestMethod()

        Dim expected As String = "unit test identity group"
        Dim actual As String = "Not set"

        Dim testObj As CommandCreatedEvent = CommandCreatedEvent.Create(Guid.NewGuid, "unit test command", Nothing, "", identityGroupNameIn:=expected)

        Dim cmdObjDeserialised As CommandCreatedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.IdentityGroupName

        Assert.AreEqual(expected, actual)
    End Sub

End Class

<TestFixture()>
Public Class CommandFatalErrorOccuredEvent_UnitTest


    <TestCase()>
    Public Sub CommandFatalErrorOccuredEvent_Constructor_TestMethod()

        Dim testObj As CommandFatalErrorOccuredEvent = CommandFatalErrorOccuredEvent.Create(DateTime.UtcNow,
                                                                                            "Unit test fatal error",
                                                                                            0)


        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub CommandFatalErrorOccuredEvent_RoundTrip_Error_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2017, 3, 12)
        Dim actual As DateTime = New DateTime(1988, 12, 3)

        Dim testObj As CommandFatalErrorOccuredEvent = CommandFatalErrorOccuredEvent.Create(expected,
                                                                                            "Unit test fatal error",
                                                                                            2)

        Dim cmdObjDeserialised As CommandFatalErrorOccuredEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.ErrorDate.Value

        Assert.AreEqual(expected, actual)
    End Sub


End Class

<TestFixture>
Public Class CommandRequeuedEvent_UnitTest


    <TestCase()>
    Public Sub CommandRequeuedEvent_Constructor_TestMethod()

        Dim testObj As CommandRequeuedEvent = CommandRequeuedEvent.Create(DateTime.Now,
                                                                          CommandEventContext.Create("source",
                                                                                                     "username",
                                                                                                     "token"))


        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub CommandRequeuedEvent_RoundTrip_RequeueDate_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2017, 3, 12)
        Dim actual As DateTime = New DateTime(1988, 12, 3)

        Dim testObj As CommandRequeuedEvent = CommandRequeuedEvent.Create(expected,
                                                                          CommandEventContext.Create("source",
                                                                                                     "username",
                                                                                                     "token"))

        Dim cmdObjDeserialised As CommandRequeuedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.RequeueDate

        Assert.AreEqual(expected, actual)
    End Sub


    <TestCase()>
    Public Sub CommandRequeuedEvent_RoundTrip_Token_Serialisation_TestMethod()

        Dim expected As String = "Unit test token"
        Dim actual As String = "Not set"

        Dim testObj As CommandRequeuedEvent = CommandRequeuedEvent.Create(DateTime.UtcNow,
                                                                          CommandEventContext.Create("source",
                                                                                                     "username",
                                                                                                     expected))

        Dim cmdObjDeserialised As CommandRequeuedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.Token

        Assert.AreEqual(expected, actual)
    End Sub

End Class

<TestFixture>
Public Class CommandStartedEvent_UnitTest


    <TestCase()>
    Public Sub CommandStartedEvent_Constructor_TestMethod()

        Dim testObj As CommandStartedEvent = CommandStartedEvent.Create(DateTime.Now,
                                                                          CommandEventContext.Create("source",
                                                                                                     "username",
                                                                                                     "token"))


        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub CommandStartedEvent_RoundTrip_ProcessingStartDate_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2017, 3, 14)
        Dim actual As DateTime = New DateTime(1988, 12, 3)

        Dim testObj As CommandStartedEvent = CommandStartedEvent.Create(expected,
                                                                          CommandEventContext.Create("source",
                                                                                                     "username",
                                                                                                     "token"))

        Dim cmdObjDeserialised As CommandStartedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.ProcessingStartDate

        Assert.AreEqual(expected, actual)
    End Sub




End Class

<TestFixture>
Public Class CommandStepCompletedEvent_UniTest

    <TestCase()>
    Public Sub CommandStepCompletedEvent_Constructor_TestMethod()

        Dim testObj As CommandStepCompletedEvent = CommandStepCompletedEvent.Create(DateTime.Now,
                                                                                    3,
                                                                                    "Unit test step 3 completed",
                                                                          CommandEventContext.Create("source",
                                                                                                     "username",
                                                                                                     "token"))


        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub CommandStartedEvent_RoundTrip_StepCompletionDate_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2017, 3, 14)
        Dim actual As DateTime = New DateTime(1988, 12, 3)

        Dim testObj As CommandStepCompletedEvent = CommandStepCompletedEvent.Create(expected,
                                                                                3,
                                                                               "Unit test step 3 completed",
                                                                               CommandEventContext.Create("source",
                                                                                                     "username",
                                                                                                     "token"))

        Dim cmdObjDeserialised As CommandStepCompletedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.StepCompletionDate

        Assert.AreEqual(expected, actual)
    End Sub


    <TestCase()>
    Public Sub CommandStartedEvent_RoundTrip_StepNumber_Serialisation_TestMethod()

        Dim expected As Integer = 7
        Dim actual As Integer = 3

        Dim testObj As CommandStepCompletedEvent = CommandStepCompletedEvent.Create(DateTime.UtcNow,
                                                                                expected,
                                                                               "Unit test step 3 completed",
                                                                               CommandEventContext.Create("source",
                                                                                                     "username",
                                                                                                     "token"))

        Dim cmdObjDeserialised As CommandStepCompletedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.StepNumber

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub CommandStartedEvent_RoundTrip_StepMessage_Serialisation_TestMethod()

        Dim expected As String = "Unit test step complete"
        Dim actual As String = "Not set"

        Dim testObj As CommandStepCompletedEvent = CommandStepCompletedEvent.Create(DateTime.UtcNow,
                                                                                3,
                                                                               expected,
                                                                               CommandEventContext.Create("source",
                                                                                                     "username",
                                                                                                     "token"))

        Dim cmdObjDeserialised As CommandStepCompletedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.StatusMessage

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub CommandStartedEvent_RoundTrip_Source_Serialisation_TestMethod()

        Dim expected As String = "Unit test source"
        Dim actual As String = "Not set"

        Dim testObj As CommandStepCompletedEvent = CommandStepCompletedEvent.Create(DateTime.UtcNow,
                                                                                3,
                                                                               "Uit testing step",
                                                                               CommandEventContext.Create(expected,
                                                                                                     "username",
                                                                                                     "token"))

        Dim cmdObjDeserialised As CommandStepCompletedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.Source

        Assert.AreEqual(expected, actual)
    End Sub

    <TestCase()>
    Public Sub CommandStartedEvent_RoundTrip_Username_Serialisation_TestMethod()

        Dim expected As String = "Duncan Jones"
        Dim actual As String = "Not set"

        Dim testObj As CommandStepCompletedEvent = CommandStepCompletedEvent.Create(DateTime.UtcNow,
                                                                                3,
                                                                               "Uit testing step",
                                                                               CommandEventContext.Create("source",
                                                                                                     expected,
                                                                                                     "token"))

        Dim cmdObjDeserialised As CommandStepCompletedEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.Username

        Assert.AreEqual(expected, actual)
    End Sub


End Class

<TestFixture>
Public Class CommandTransientFaultOccuredEvent_UnitTest


    <TestCase()>
    Public Sub CommandTransientFaultOccuredEvent_Constructor_TestMethod()

        Dim testObj As CommandTransientFaultOccuredEvent = CommandTransientFaultOccuredEvent.Create(DateTime.UtcNow,
                                                                                                     "Unit test fault",
                                                                                                     2,
                                                                          CommandEventContext.Create("source",
                                                                                                     "username",
                                                                                                     "token"))


        Assert.IsNotNull(testObj)

    End Sub


    <TestCase()>
    Public Sub CommandTransientFaultOccuredEvent_RoundTrip_FaultDate_Serialisation_TestMethod()

        Dim expected As DateTime = New DateTime(2017, 3, 14)
        Dim actual As DateTime = New DateTime(1988, 12, 3)

        Dim testObj As CommandTransientFaultOccuredEvent = CommandTransientFaultOccuredEvent.Create(expected,
                                                                                "Unit test fault",
                                                                                                     2,
                                                                          CommandEventContext.Create("source",
                                                                                                     "username",
                                                                                                     "token"))

        Dim cmdObjDeserialised As CommandTransientFaultOccuredEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.FaultDate

        Assert.AreEqual(expected, actual)
    End Sub


    <TestCase()>
    Public Sub CommandTransientFaultOccuredEvent_RoundTrip_StepNumber_Serialisation_TestMethod()

        Dim expected As Integer = 7
        Dim actual As Integer = 3

        Dim testObj As CommandTransientFaultOccuredEvent = CommandTransientFaultOccuredEvent.Create(DateTime.UtcNow,
                                                                                                    "Unit test fault",
                                                                                                    expected,
                                                                               CommandEventContext.Create("source",
                                                                                                     "username",
                                                                                                     "token"))

        Dim cmdObjDeserialised As CommandTransientFaultOccuredEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.StepNumber

        Assert.AreEqual(expected, actual)
    End Sub


    <TestCase()>
    Public Sub CommandTransientFaultOccuredEvent_RoundTrip_FaultMessage_Serialisation_TestMethod()

        Dim expected As String = "Unit test step fault"
        Dim actual As String = "Not set"

        Dim testObj As CommandTransientFaultOccuredEvent = CommandTransientFaultOccuredEvent.Create(DateTime.UtcNow,
                                                                                                    expected,
                                                                                                    4,
                                                                               CommandEventContext.Create("source",
                                                                                                     "username",
                                                                                                     "token"))

        Dim cmdObjDeserialised As CommandTransientFaultOccuredEvent

        Dim ser As New System.Runtime.Serialization.DataContractSerializer(testObj.GetType())
        Using ms As New System.IO.MemoryStream(5000)
            ser.WriteObject(ms, testObj)
            ms.Seek(0, IO.SeekOrigin.Begin)


            cmdObjDeserialised = ser.ReadObject(ms)
        End Using

        actual = cmdObjDeserialised.FaultMessage

        Assert.AreEqual(expected, actual)
    End Sub

End Class