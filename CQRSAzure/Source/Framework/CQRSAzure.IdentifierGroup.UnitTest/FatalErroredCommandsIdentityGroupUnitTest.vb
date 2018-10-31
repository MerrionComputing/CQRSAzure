Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.IdentifierGroup.Commands
Imports CQRSAzure.EventSourcing.InMemory
Imports CQRSAzure.EventSourcing.Commands
Imports CQRSAzure.EventSourcing

<TestClass()>
Public Class FatalErroredCommandsIdentityGroupUnitTest

    <TestMethod()>
    Public Sub Constructor_Empty_TestMethod()

        Dim testObj As New FatalErroredCommandsIdentityGroup()
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub IdentityGroupName_TestMethod()

        Dim expected As String = "Fatal Error Commands"
        Dim actual As String = "Not set"

        Dim testObj As New FatalErroredCommandsIdentityGroup()
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
        testEventStream.AppendEvent(CommandFatalErrorOccuredEvent.Create(DateTime.Now, "Fatal error", 1))

        'Run the projection
        Dim testReadObj As InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)

        Dim testClassifier = InMemory.InMemoryClassifier(Of CommandAggregateIdentifier, Guid, FatalErroredCommandsIdentityGroupClassifier).CreateClassifierProcessor(cmdId)
        actual = testClassifier.Classify()

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
        testEventStream.AppendEvent(CommandCompletedEvent.Create(DateTime.Now, "Command has completed"))

        'Run the projection
        Dim testReadObj As InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid) = InMemoryEventStreamReader(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)

        Dim testClassifier = InMemory.InMemoryClassifier(Of CommandAggregateIdentifier, Guid, FatalErroredCommandsIdentityGroupClassifier).CreateClassifierProcessor(cmdId)
        actual = testClassifier.Classify(forceExclude:=True)

        Assert.AreEqual(expected, actual)

    End Sub

#Region "Local file implementation"

    <TestMethod()>
    Public Sub Classifier_LocalFile_In_WithEvents_TestMethod()

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Include
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As CQRSAzure.EventSourcing.Local.File.LocalFileEventStreamWriter(Of CommandAggregateIdentifier, Guid) = CQRSAzure.EventSourcing.Local.File.LocalFileEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete"))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))
        testEventStream.AppendEvent(CommandFatalErrorOccuredEvent.Create(DateTime.Now, "Fatal error", 1))

        'Run the classifier
        Dim testReadObj As CQRSAzure.EventSourcing.Local.File.LocalFileEventStreamReader(Of CommandAggregateIdentifier, Guid) = CQRSAzure.EventSourcing.Local.File.LocalFileEventStreamReader(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)

        Dim testClassifier = Local.File.LocalFileClassifier(Of CommandAggregateIdentifier, Guid, FatalErroredCommandsIdentityGroupClassifier).CreateClassifierProcessor(cmdAgg, streamReader:=testReadObj)
        actual = testClassifier.Classify()

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub Classifier_LocalFile_Out_WithEvents_TestMethod()

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Exclude
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As CQRSAzure.EventSourcing.Local.File.LocalFileEventStreamWriter(Of CommandAggregateIdentifier, Guid) = CQRSAzure.EventSourcing.Local.File.LocalFileEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete"))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))
        testEventStream.AppendEvent(CommandCompletedEvent.Create(DateTime.Now, "Command has completed"))

        'Run the projection
        Dim testReadObj As CQRSAzure.EventSourcing.Local.File.LocalFileEventStreamReader(Of CommandAggregateIdentifier, Guid) = CQRSAzure.EventSourcing.Local.File.LocalFileEventStreamReader(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)

        Dim testClassifier = Local.File.LocalFileClassifier(Of CommandAggregateIdentifier, Guid, FatalErroredCommandsIdentityGroupClassifier).CreateClassifierProcessor(cmdAgg, streamReader:=testReadObj)
        actual = testClassifier.Classify(forceExclude:=True)

        Assert.AreEqual(expected, actual)

    End Sub
#End Region

#Region "Azure blob implementation"

    <TestMethod()>
    Public Sub Classifier_AzureBlob_In_WithEvents_TestMethod()

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Include
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        Dim cmdId As Guid = Guid.NewGuid
        Dim cmdAgg As CommandAggregateIdentifier = CommandAggregateIdentifier.Create(cmdId)

        Dim testEventStream As CQRSAzure.EventSourcing.Azure.Blob.BlobEventStreamWriter(Of CommandAggregateIdentifier, Guid) = CQRSAzure.EventSourcing.Azure.Blob.BlobEventStreamWriter(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)
        testEventStream.AppendEvent(CommandCreatedEvent.Create(Guid.NewGuid, "Test command", DateTime.Now))
        testEventStream.AppendEvent(CommandStartedEvent.Create(DateTime.Now))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 0, "Step 0 complete"))
        testEventStream.AppendEvent(CommandStepCompletedEvent.Create(DateTime.Now, 1, "Step 1 complete"))
        testEventStream.AppendEvent(CommandFatalErrorOccuredEvent.Create(DateTime.Now, "Fatal error", 1))

        'Run the classifier
        Dim testReadObj As CQRSAzure.EventSourcing.Azure.Blob.BlobEventStreamReader(Of CommandAggregateIdentifier, Guid) = CQRSAzure.EventSourcing.Azure.Blob.BlobEventStreamReader(Of CommandAggregateIdentifier, Guid).Create(cmdAgg)

        Dim testClassifier = Azure.Blob.AzureBlobClassifier(Of CommandAggregateIdentifier, Guid, FatalErroredCommandsIdentityGroupClassifier).CreateClassifierProcessor(cmdAgg, streamReader:=testReadObj)
        actual = testClassifier.Classify()


        Assert.AreEqual(expected, actual)

    End Sub

#End Region

End Class