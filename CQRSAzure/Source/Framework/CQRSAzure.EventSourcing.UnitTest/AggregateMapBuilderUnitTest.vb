Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports System.Configuration

Imports CQRSAzure.EventSourcing


<TestClass()>
Public Class AggregateMapBuilderUnitTest

    <TestMethod()>
    Public Sub Constructor_Empty_TestMethod()

        Dim testObj As New AggregateMapBuilder()
        Assert.IsNotNull(testObj)

    End Sub


    <TestMethod()>
    Public Sub Constructor_Empty_IsOnDemand_TestMethod()

        Dim actual As AggregateMapBuilder.MapCreationOption = AggregateMapBuilder.MapCreationOption.UpFront
        Dim expected As AggregateMapBuilder.MapCreationOption = AggregateMapBuilder.MapCreationOption.OnDemand

        Dim testObj As New AggregateMapBuilder()

        actual = testObj.MapCreation

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub Constructor_EmptyWithSettings_IsOnDemand_TestMethod()

        Dim actual As AggregateMapBuilder.MapCreationOption = AggregateMapBuilder.MapCreationOption.UpFront
        Dim expected As AggregateMapBuilder.MapCreationOption = AggregateMapBuilder.MapCreationOption.OnDemand

        Dim testSettings As CQRSAzureEventSourcingConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testSettings = config.GetSection("CQRSAzureEventSourcingConfiguration")

        Dim testObj As New AggregateMapBuilder(Settings:=testSettings)

        actual = testObj.MapCreation

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub Constructor_OnDemand_IsOnDemand_TestMethod()

        Dim actual As AggregateMapBuilder.MapCreationOption = AggregateMapBuilder.MapCreationOption.UpFront
        Dim expected As AggregateMapBuilder.MapCreationOption = AggregateMapBuilder.MapCreationOption.OnDemand

        Dim testObj As New AggregateMapBuilder(expected)

        actual = testObj.MapCreation

        Assert.AreEqual(expected, actual)

    End Sub


    <TestMethod()>
    Public Sub Constructor_UpFront_IsUpFront_TestMethod()

        Dim actual As AggregateMapBuilder.MapCreationOption = AggregateMapBuilder.MapCreationOption.OnDemand
        Dim expected As AggregateMapBuilder.MapCreationOption = AggregateMapBuilder.MapCreationOption.UpFront

        Dim testObj As New AggregateMapBuilder(expected)

        actual = testObj.MapCreation

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub Factory_CreateDefaultAggregateMaps_NotNull_TestMethod()

        Dim testObj = AggregateMapBuilderFactory.CreateDefaultAggregateMaps()


        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub GetImplementationMap_NurseAggregate_TestMethod()

        Dim testSettings As CQRSAzureEventSourcingConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testSettings = config.GetSection("CQRSAzureEventSourcingConfiguration")

        Dim testMapBuilder As New AggregateMapBuilder(Settings:=testSettings)


        Dim testMap = testMapBuilder.CreateImplementationMap(GetType(Mocking.NurseAggregate))
        Assert.IsNotNull(testMap)

    End Sub

    <TestMethod>
    Public Sub GetImplementationMap_PatientAggregate_TestMethod()

        Dim testSettings As CQRSAzureEventSourcingConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testSettings = config.GetSection("CQRSAzureEventSourcingConfiguration")

        Dim testMapBuilder As New AggregateMapBuilder(Settings:=testSettings)


        Dim testMap = testMapBuilder.CreateImplementationMap(GetType(Mocking.PatientAggregate))
        Assert.IsNotNull(testMap)

    End Sub

    <TestMethod>
    Public Sub GetImplementationMap_PatientAggregate_Reader_TestMethod()

        Dim testSettings As CQRSAzureEventSourcingConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testSettings = config.GetSection("CQRSAzureEventSourcingConfiguration")

        Dim testMapBuilder As New AggregateMapBuilder(Settings:=testSettings)


        Dim testMap As IAggregateImplementationMap(Of Mocking.PatientAggregate, String) = testMapBuilder.CreateImplementationMap(GetType(Mocking.PatientAggregate))
        Assert.IsNotNull(testMap.CreateReader(New Mocking.PatientAggregate("123"), "123"))

    End Sub

    <TestMethod>
    Public Sub GetImplementationMap_PatientAggregate_Writer_TestMethod()

        Dim testSettings As CQRSAzureEventSourcingConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testSettings = config.GetSection("CQRSAzureEventSourcingConfiguration")

        Dim testMapBuilder As New AggregateMapBuilder(Settings:=testSettings)


        Dim testMap As IAggregateImplementationMap(Of Mocking.PatientAggregate, String) = testMapBuilder.CreateImplementationMap(GetType(Mocking.PatientAggregate))
        Assert.IsNotNull(testMap.CreateWriter(New Mocking.PatientAggregate("123"), "123"))

    End Sub

    <TestMethod>
    Public Sub GetImplementationMap_BedAggregate_Writer_TestMethod()

        Dim testSettings As CQRSAzureEventSourcingConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testSettings = config.GetSection("CQRSAzureEventSourcingConfiguration")

        Dim testMapBuilder As New AggregateMapBuilder(Settings:=testSettings)


        Dim testMap As IAggregateImplementationMap(Of Mocking.BedAggregate, Integer) = testMapBuilder.CreateImplementationMap(GetType(Mocking.BedAggregate))
        Assert.IsNotNull(testMap.CreateWriter(New Mocking.BedAggregate(123), 123))

    End Sub

    ' Test that this also works for aggregate identifiers imported from other projects that were code generated
    <TestMethod>
    Public Sub GetImplementationMap_BankAccountAggregate_Reader_TestMethod()

        Dim testSettings As CQRSAzureEventSourcingConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testSettings = config.GetSection("CQRSAzureEventSourcingConfiguration")

        Dim testMapBuilder As New AggregateMapBuilder(Settings:=testSettings)


        Dim testMap As IAggregateImplementationMap(Of Accounts.Account.Account, String) = testMapBuilder.CreateImplementationMap(GetType(Accounts.Account.Account))
        Assert.IsNotNull(testMap.CreateReader(New Accounts.Account.Account("123"), "123"))

    End Sub

    <TestMethod>
    Public Sub GetImplementationMap_BankAccountAggregate_Writer_TestMethod()

        Dim testSettings As CQRSAzureEventSourcingConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testSettings = config.GetSection("CQRSAzureEventSourcingConfiguration")

        Dim testMapBuilder As New AggregateMapBuilder(Settings:=testSettings)


        Dim testMap As IAggregateImplementationMap(Of Accounts.Account.Account, String) = testMapBuilder.CreateImplementationMap(GetType(Accounts.Account.Account))
        Assert.IsNotNull(testMap.CreateWriter(New Accounts.Account.Account("123"), "123"))

    End Sub

    <TestMethod>
    Public Sub UseImplementationMap_BankAccountAggregate_TestMethod()

        'load the serialisers
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Opened) _
            (OpenedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Closed) _
            (ClosedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Money_Deposited) _
            (DepositedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Money_Withdrawn) _
            (WithdrawnEventSerialiser.Create())

        Const MY_ACCOUNT_NUMBER As String = "1185888"
        Dim testSettings As CQRSAzureEventSourcingConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testSettings = config.GetSection("CQRSAzureEventSourcingConfiguration")

        Dim testMapBuilder As New AggregateMapBuilder(Settings:=testSettings)


        Dim testMap As IAggregateImplementationMap(Of Accounts.Account.Account, String) = testMapBuilder.CreateImplementationMap(GetType(Accounts.Account.Account))

        Dim writer As IEventStreamWriter(Of Accounts.Account.Account, String) = testMap.CreateWriter(New Accounts.Account.Account(MY_ACCOUNT_NUMBER), MY_ACCOUNT_NUMBER)
        'Account was opened
        writer.AppendEvent(New Accounts.Account.eventDefinition.Opened(New DateTime(2016, 12, 19, 12, 22, 7),
                                                                       "EUR"))
        'Initial deposit
        writer.AppendEvent(New Accounts.Account.eventDefinition.Money_Deposited(2000.0,
                                                                                New DateTime(2016, 12, 20),
                                                                                New Date(2016, 12, 27),
                                                                                "EUR",
                                                                                1.0))

        'Cross currency deposit
        writer.AppendEvent(New Accounts.Account.eventDefinition.Money_Deposited(1200.0,
                                                                             New DateTime(2017, 1, 12),
                                                                             New DateTime(2017, 1, 16),
                                                                             "USD",
                                                                             1.2907))

        writer.AppendEvent(New Accounts.Account.eventDefinition.Money_Withdrawn(100.0,
                                                                                New DateTime(2017, 1, 19),
                                                                                "Over The Counter",
                                                                                "Hagley Road Branch, Birmingham"))

        writer.AppendEvent(New Accounts.Account.eventDefinition.Money_Withdrawn(400.0,
                                                                                New DateTime(2017, 1, 20),
                                                                                "Cash Machine",
                                                                                "Machine 208AC23"))

        writer.AppendEvent(New Accounts.Account.eventDefinition.Closed(New DateTime(2017, 6, 12),
                                                                       "Moving to alternative jurisdiction"))


        Dim projection As IProjectionProcessor(Of Accounts.Account.Account, String)
        projection = testMap.CreateProjectionProcessor(New Accounts.Account.Account(MY_ACCOUNT_NUMBER), MY_ACCOUNT_NUMBER)

        Dim getBalance As New Accounts.Account.projection.Running_Balance()
        projection.Process(getBalance)

        Assert.IsTrue(getBalance.Balance > 0.00)

    End Sub

End Class