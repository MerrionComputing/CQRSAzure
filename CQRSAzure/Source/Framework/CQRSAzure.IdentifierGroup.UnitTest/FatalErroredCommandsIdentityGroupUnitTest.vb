
Imports CQRSAzure.EventSourcing.Commands
Imports CQRSAzure.IdentifierGroup.Commands
Imports CQRSAzure.EventSourcing.InMemory
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.Blob
Imports CQRSAzure.EventSourcing.Local.File
Imports NUnit.Framework

<TestFixture()>
Public Class FatalErroredCommandsIdentityGroupUnitTest

    <TestCase()>
    Public Sub Constructor_Empty_TestMethod()

        Dim testObj As New FatalErroredCommandsIdentityGroup()
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub IdentityGroupName_TestMethod()

        Dim expected As String = "Fatal Error Commands"
        Dim actual As String = "Not set"

        Dim testObj As New FatalErroredCommandsIdentityGroup()
        actual = testObj.Name

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Async Function Classifier_In_WithEvents_TestMethod() As Task

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Include
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        Await testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        Await testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        Await testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete"))
        Await testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))
        Await testEventStream.AppendEvent(CommandFatalErrorOccuredEvent.Create(DateTime.Now, "Fatal error", 1))

        'Run the projection
        Dim testReadObj As InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)

        Dim testClassifier = InMemory.InMemoryClassifier(Of CommandAggregateIdentifier, Guid, FatalErroredCommandsIdentityGroupClassifier).CreateClassifierProcessor(cmdId)
        actual = Await testClassifier.Classify()

        Assert.AreEqual(expected, actual)

    End Function

    <TestCase()>
    Public Async Function Classifier_Out_WithEvents_TestMethod() As Task

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Exclude
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        Task.WaitAll(
            testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now)),
            testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now)),
            testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete")),
            testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))
        )
        Await testEventStream.AppendEvent(CommandCompletedEvent.Create(DateTime.Now, "Command has completed"))

        'Run the projection
        Dim testReadObj As InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)

        Dim testClassifier = InMemory.InMemoryClassifier(Of CommandAggregateIdentifier, Guid, FatalErroredCommandsIdentityGroupClassifier).CreateClassifierProcessor(cmdId)
        actual = Await testClassifier.Classify(forceExclude:=True)

        Assert.AreEqual(expected, actual)

    End Function

#Region "Local file implementation"

    <TestCase()>
    Public Async Function Classifier_LocalFile_In_WithEvents_TestMethod() As Task

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Include
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As LocalFileEventStreamWriter(Of CommandAggregateIdentifier, Guid) = LocalFileEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        Await testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        Await testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        Await testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete"))
        Await testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))
        Await testEventStream.AppendEvent(CommandFatalErrorOccuredEvent.Create(DateTime.Now, "Fatal error", 1))

        'Run the classifier
        Dim testReadObj As LocalFileEventStreamReader(Of CommandAggregateIdentifier, Guid) = LocalFileEventStreamReader(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)

        Dim testClassifier = Local.File.LocalFileClassifier(Of CommandAggregateIdentifier, Guid, FatalErroredCommandsIdentityGroupClassifier).CreateClassifierProcessor(cmdAgg, streamReader:=testReadObj)
        actual = Await testClassifier.Classify()

        Assert.AreEqual(expected, actual)

    End Function

    <TestCase()>
    Public Async Function Classifier_LocalFile_Out_WithEvents_TestMethod() As Task

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Exclude
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As LocalFileEventStreamWriter(Of CommandAggregateIdentifier, Guid) = LocalFileEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        Await testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        Await testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        Task.WaitAll(
            testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete")),
            testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete")),
            testEventStream.AppendEvent(CommandCompletedEvent.Create(DateTime.Now, "Command has completed"))
        )

        'Run the projection
        Dim testReadObj As LocalFileEventStreamReader(Of CommandAggregateIdentifier, Guid) = LocalFileEventStreamReader(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)

        Dim testClassifier = Local.File.LocalFileClassifier(Of CommandAggregateIdentifier, Guid, FatalErroredCommandsIdentityGroupClassifier).CreateClassifierProcessor(cmdAgg, streamReader:=testReadObj)
        actual = Await testClassifier.Classify(forceExclude:=True)

        Assert.AreEqual(expected, actual)

    End Function
#End Region

#Region "Azure blob implementation"

    <TestCase()>
    Public Async Function Classifier_AzureBlob_In_WithEvents_TestMethod() As Task

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Include
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As BlobEventStreamWriter(Of CommandAggregateIdentifier, Guid) = BlobEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        Await testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        Await testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        Await testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete"))
        Await testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))
        Await testEventStream.AppendEvent(CommandFatalErrorOccuredEvent.Create(DateTime.Now, "Fatal error", 1))

        'Run the classifier
        Dim testReadObj As BlobEventStreamReader(Of CommandAggregateIdentifier, Guid) = BlobEventStreamReader(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)

        Dim testClassifier = Azure.Blob.AzureBlobClassifier(Of CommandAggregateIdentifier, Guid, FatalErroredCommandsIdentityGroupClassifier).CreateClassifierProcessor(cmdAgg, streamReader:=testReadObj)
        actual = Await testClassifier.Classify()


        Assert.AreEqual(expected, actual)

    End Function

#End Region

End Class