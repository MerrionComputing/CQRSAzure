Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.CQRSdsl.CommandLine

<TestClass()>
Public Class CommandLineUnitTest

    <TestMethod()>
    Public Sub Constructor_Empty_TestMethod()

        Dim testObj As New CommandLineParser()
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub UnknownCommandLine_TestMethod()

        Dim expected As ModelModificationCommandBase.CommandActionType = ModelModificationCommandBase.CommandActionType.NoAction
        Dim actual As ModelModificationCommandBase.CommandActionType = ModelModificationCommandBase.CommandActionType.Depreciate

        Dim testObj As New CommandLineParser
        Dim testCommand = testObj.ParseCommand("What is this command?")
        actual = testCommand.ActionType

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub InvalidModelName_Quote_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        actual = CreateModelCommand.IsValidModelName("Test'model")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub InvalidModelName_Period_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        actual = CreateModelCommand.IsValidModelName("Test.model")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub InvalidModelName_Empty_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        actual = CreateModelCommand.IsValidModelName("  ")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub InvalidModelName_SemiColon_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        actual = CreateModelCommand.IsValidModelName("Test_model;")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub ValidModelName_TestMethod()

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        actual = CreateModelCommand.IsValidModelName("Test_model_name")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub ValidCreateModelCommand_TestMethod()

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        actual = CreateModelCommand.IsValidCreateModelCommand("CREATE MODEL foo_bar")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub InvalidCreateModelCommand_CommandType_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        actual = CreateModelCommand.IsValidCreateModelCommand("MODIFY MODEL foo_bar")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub InvalidCreateModelCommand_EntityType_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        actual = CreateModelCommand.IsValidCreateModelCommand("CREATE EVENT foo_bar")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub InvalidCreateModelCommand_ModelName_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        actual = CreateModelCommand.IsValidCreateModelCommand("CREATE EVENT foo:bar;")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub ValidUsingModelCommand_TestMethod()

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        actual = UsingModelCommand.IsValidUsingModelCommand("USING CQRS_TEST_Model")

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod>
    Public Sub InvalidUsingModelCommand_CommandName_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        actual = UsingModelCommand.IsValidUsingModelCommand("UTILISANT CQRS_TEST_Model")

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod>
    Public Sub InvalidUsingModelCommand_Empty_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        actual = UsingModelCommand.IsValidUsingModelCommand("  ")

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod>
    Public Sub InvalidUsingModelCommand_EmptyModel_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        actual = UsingModelCommand.IsValidUsingModelCommand("USING  ")

        Assert.AreEqual(expected, actual)
    End Sub

    <TestMethod>
    Public Sub InvalidUsingModelCommand_ModelName_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        actual = UsingModelCommand.IsValidUsingModelCommand("USING Foo.BAR!  ")

        Assert.AreEqual(expected, actual)
    End Sub

End Class

<TestClass>
Public Class DocumentObjectCommandUnitTest

    <TestMethod>
    Public Sub DocumentAggregateCommand_Constructor()

        Dim testObj As New DocumentAggregateCommand("modelName",
                                                    "aggregateName",
                                                    "Aggregate description text",
                                                    "Additional aggregate notes")

        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub DocumentAggregateCommand_ObjectType_TestMethod()

        Dim expected As ModelModificationCommandBase.CommandTargetType = ModelModificationCommandBase.CommandTargetType.Aggregate
        Dim actual As ModelModificationCommandBase.CommandTargetType = ModelModificationCommandBase.CommandTargetType.EventProperty

        Dim testObj As New DocumentAggregateCommand("modelName",
                                                    "aggregateName",
                                                    "Aggregate description text",
                                                    "Additional aggregate notes")

        actual = testObj.Target

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub DocumentCommandDefinitionCommand_ObjectType_TestMethod()

        Dim expected As ModelModificationCommandBase.CommandTargetType = ModelModificationCommandBase.CommandTargetType.CommandDefinition
        Dim actual As ModelModificationCommandBase.CommandTargetType = ModelModificationCommandBase.CommandTargetType.EventProperty

        Dim testObj As New DocumentCommandDefinitionCommand("modelName",
                                                    "aggregateName.commandName",
                                                    "Command description text",
                                                    "Command aggregate notes")

        actual = testObj.Target

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub DocumentEventDefinitionCommand_ObjectType_TestMethod()

        Dim expected As ModelModificationCommandBase.CommandTargetType = ModelModificationCommandBase.CommandTargetType.Event
        Dim actual As ModelModificationCommandBase.CommandTargetType = ModelModificationCommandBase.CommandTargetType.Aggregate

        Dim testObj As New DocumentEventDefinitionCommand("modelName",
                                                    "aggregateName.eventName",
                                                    "Event description text",
                                                    "Event aggregate notes")

        actual = testObj.Target

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub DocumentProjectionDefinitionCommand_ObjectType_TestMethod()

        Dim expected As ModelModificationCommandBase.CommandTargetType = ModelModificationCommandBase.CommandTargetType.Projection
        Dim actual As ModelModificationCommandBase.CommandTargetType = ModelModificationCommandBase.CommandTargetType.Aggregate

        Dim testObj As New DocumentProjectionDefinitionCommand("modelName",
                                                    "aggregateName.projectionName",
                                                    "Projection description text",
                                                    "Projection aggregate notes")

        actual = testObj.Target

        Assert.AreEqual(expected, actual)

    End Sub

End Class