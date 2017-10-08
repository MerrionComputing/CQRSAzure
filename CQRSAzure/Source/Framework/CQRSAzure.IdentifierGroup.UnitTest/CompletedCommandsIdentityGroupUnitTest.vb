Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.IdentifierGroup.Commands
Imports CQRSAzure.EventSourcing.InMemory
Imports CQRSAzure.EventSourcing.Commands
Imports CQRSAzure.EventSourcing

<TestClass()> Public Class CompletedCommandsIdentityGroupUnitTest

    <TestMethod()>
    Public Sub Constructor_Empty_TestMethod()

        Dim testObj As New CompletedCommandsIdentityGroup()
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub IdentityGroupName_TestMethod()

        Dim expected As String = "Commands Completed"
        Dim actual As String = "Not set"

        Dim testObj As New CompletedCommandsIdentityGroup()
        actual = testObj.Name

        Assert.AreEqual(expected, actual)

    End Sub


    <TestMethod()>
    Public Sub Classifier_In_WithEvents_TestMethod()

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Include
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete"))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))
        testEventStream.AppendEvent(CommandCompletedEvent.Create(DateTime.Now, "Command complete"))

        'Run the projection
        Dim testReadObj As ProjectionProcessor(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid).CreateProjectionProcessor(cmdAgg)

        Dim projObj As New CommandStatusProjection()
        testReadObj.Process(projObj)

        Dim testObj As New CompletedCommandsIdentityGroupClassifier()
        actual = testObj.EvaluateCommandStatusProjection(projObj)

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub Classifier_Out_WithEvents_TestMethod()

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Exclude
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete"))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))
        testEventStream.AppendEvent(CommandFatalErrorOccuredEvent.Create(DateTime.Now, "Command had a fatal error", 1))

        'Run the projection
        Dim testReadObj As ProjectionProcessor(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid).CreateProjectionProcessor(cmdAgg)

        Dim projObj As New CommandStatusProjection()
        testReadObj.Process(projObj)

        Dim testObj As New CompletedCommandsIdentityGroupClassifier()
        actual = testObj.EvaluateCommandStatusProjection(projObj)

        Assert.AreEqual(expected, actual)

    End Sub


End Class