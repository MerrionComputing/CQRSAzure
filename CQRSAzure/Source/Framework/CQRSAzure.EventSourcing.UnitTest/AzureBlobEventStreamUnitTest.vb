Imports System.Configuration
Imports CQRSAzure.EventSourcing.UnitTest.Mocking
Imports CQRSAzure.IdentifierGroup
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.Blob
Imports NUnit.Framework

<TestFixture()>
Public Class AzureBlobEventStreamUnitTest

    Public Const TEST_AGGREGATE_IDENTIFIER As String = "19121971"


    ''' <remarks>
    ''' Note that you will either need to reference an actual Azure account or to start the azure local storage emulator in order
    ''' to run this unit test
    ''' </remarks>
    <TestCase()>
    Public Sub Reader_Constructor_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As BlobEventStreamReader(Of MockAggregate, String) = CType(BlobEventStreamReader(Of MockAggregate, String).Create(testAgg), BlobEventStreamReader(Of MockAggregate, String))
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub Writer_Constructor_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As BlobEventStreamWriter(Of MockAggregate, String) = CType(BlobEventStreamWriter(Of MockAggregate, String).Create(testAgg), BlobEventStreamWriter(Of MockAggregate, String))
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Async Function Writer_WriteEvent_TestMethod() As Task

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As BlobEventStreamWriter(Of MockAggregate, String) = CType(BlobEventStreamWriter(Of MockAggregate, String).Create(testAgg), BlobEventStreamWriter(Of MockAggregate, String))
        Await testObj.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 123})

        Assert.IsTrue(testObj.RecordCount > 0)

    End Function

    <TestCase()>
    Public Async Function Writer_WriteEventTwo_TestMethod() As Task

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As BlobEventStreamWriter(Of MockAggregate, String) = CType(BlobEventStreamWriter(Of MockAggregate, String).Create(testAgg), BlobEventStreamWriter(Of MockAggregate, String))
        Await testObj.Reset()
        Await testObj.AppendEvent(New MockEventTypeTwo() With {.EventTwoNullableDateProperty = DateTime.UtcNow, .EventTwoStringProperty = "My test two", .EventTwoDecimalProperty = 123.45D})

        Assert.IsTrue(testObj.RecordCount > 0)

    End Function

    <TestCase()>
    Public Sub Reader_ReadEvents_TestMethod()


        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)

        Dim testWriter As BlobEventStreamWriter(Of MockAggregate, String) = CType(BlobEventStreamWriter(Of MockAggregate, String).Create(testAgg), BlobEventStreamWriter(Of MockAggregate, String))
        testWriter.Reset()
        'add a dummy event
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoNullableDateProperty = DateTime.UtcNow, .EventTwoStringProperty = "My test two", .EventTwoDecimalProperty = 123.45D})

        Dim testObj As BlobEventStreamReader(Of MockAggregate, String) = CType(BlobEventStreamReader(Of MockAggregate, String).Create(testAgg), BlobEventStreamReader(Of MockAggregate, String))

        Dim ret = testObj.GetEvents()

        Assert.IsNotNull(ret)

    End Sub

    <TestCase()>
    Public Sub Reader_ReadEventsWithContext_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)

        Dim testWriter As BlobEventStreamWriter(Of MockAggregate, String) = CType(BlobEventStreamWriter(Of MockAggregate, String).Create(testAgg), BlobEventStreamWriter(Of MockAggregate, String))
        testWriter.Reset()

        Dim testObj As BlobEventStreamReader(Of MockAggregate, String) = CType(BlobEventStreamReader(Of MockAggregate, String).Create(testAgg), BlobEventStreamReader(Of MockAggregate, String))
        Dim ret = testObj.GetEventsWithContext()

        Assert.IsNotNull(ret)

    End Sub


    <TestCase()>
    Public Sub MakeValidStorageFolderName_Empty_TestMethod()

        Dim expected As String = "uncategorised"
        Dim actual As String = ""

        actual = BlobEventStreamBase.MakeValidStorageFolderName("")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub MakeValidStorageFolderName_TooShort_TestMethod()

        Dim expected As String = "a-abc"
        Dim actual As String = ""

        actual = BlobEventStreamBase.MakeValidStorageFolderName("-a")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub MakeValidStorageFolderName_TooShortAfterTrim_TestMethod()

        Dim expected As String = "a-abc"
        Dim actual As String = ""

        actual = BlobEventStreamBase.MakeValidStorageFolderName("------a")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub MakeValidStorageFolderName_FixChars_TestMethod()

        Dim expected As String = "duncan-s-model"
        Dim actual As String = ""

        actual = BlobEventStreamBase.MakeValidStorageFolderName("Duncan's Model")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub MakeValidStorageFolderName_IsValid_TestMethod()

        Dim expected As String = "duncan-s-model"
        Dim actual As String = ""

        actual = BlobEventStreamBase.MakeValidStorageFolderName(expected)

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub MakeValidStorageFolderName_TooLong_TestMethod()

        Dim expected As Integer = 63
        Dim actual As Integer = 0

        actual = BlobEventStreamBase.MakeValidStorageFolderName("The quick dog jumped over the lazy dog which was suprising to them both with the display of dexterity").Length

        Assert.AreEqual(expected, actual)

    End Sub


    <TestCase()>
    Public Async Function GetAllStreamKeys_NoDate_TestMethod() As Task

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        Dim instance As New MockAggregate(AzureBlobEventStreamUnitTest.TEST_AGGREGATE_IDENTIFIER)

        Dim testObj = BlobEventStreamReader(Of MockAggregate, String).Create(New MockAggregate(AzureBlobEventStreamUnitTest.TEST_AGGREGATE_IDENTIFIER))
        Dim testProvider As IEventStreamProvider(Of MockAggregate, String) = BlobEventStreamProviderFactory.Create(Of MockAggregate, String)(instance, AzureBlobEventStreamUnitTest.TEST_AGGREGATE_IDENTIFIER)
        'TODO : Instantiate the provider instance

        Dim values As IEnumerable(Of String) = Await testProvider.GetAllStreamKeys()
        actual = values.Contains(AzureBlobEventStreamUnitTest.TEST_AGGREGATE_IDENTIFIER)

        Assert.AreEqual(expected, actual)

    End Function

    <TestCase()>
    Public Async Function GetAllStreamKeys_GUIDKey_NoDate_TestMethod() As Task

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        Dim key As Guid = New Guid("a73140ce-2204-413a-8beb-604241fd721d")

        Dim instance As New MockGuidAggregate(key)
        Dim testObj = BlobEventStreamReader(Of MockGuidAggregate, Guid).Create(instance)
        Dim testProvider As IEventStreamProvider(Of MockGuidAggregate, Guid) = BlobEventStreamProviderFactory.Create(Of MockGuidAggregate, Guid)(instance, key)

        Dim values As IEnumerable(Of Guid) = Await testProvider.GetAllStreamKeys()
        actual = values.Contains(key)

        Assert.AreEqual(expected, actual)

    End Function

    <TestCase()>
    Public Async Function GetAllStreamKeys_GUIDKey_FutureDate_TestMethod() As Task

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        Dim asOfdate As DateTime = New DateTime(2037, 1, 1)

        Dim key As Guid = New Guid("a883fb8a-97ab-4d66-85db-ec7b9ba3b423")

        Dim testObj = BlobEventStreamReader(Of MockGuidAggregate, Guid).Create(New MockGuidAggregate(key))
        Dim testProvider As IEventStreamProvider(Of MockGuidAggregate, Guid) = BlobEventStreamProviderFactory.Create(Of MockGuidAggregate, Guid)()

        Dim values As IEnumerable(Of Guid) = Await testProvider.GetAllStreamKeys()
        actual = values.Contains(key)

        Assert.AreEqual(expected, actual)

    End Function



    Private Const LARGE_STREAM_TEST_ACCOUNT As String = "Large.Stream.19121974"

#If PERFORMANCE_TESTS Then
    <TestCategory("Performance")>
    <TestCase()>
    Public Sub Write_Million_Random_Events()

        'load the serialisers
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Opened) _
            (OpenedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Closed) _
            (ClosedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Money_Deposited) _
            (DepositedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Money_Withdrawn) _
            (WithdrawnEventSerialiser.Create())

        Dim testSettings As CQRSAzureEventSourcingConfigurationSection
        'Dim config As Configuration = ConfigurationManager.GetSection(ConfigurationUserLevel.None)
        testSettings = CType(ConfigurationManager.GetSection("CQRSAzureEventSourcingConfiguration"),
            CQRSAzureEventSourcingConfigurationSection)

        Dim testMapBuilder As New AggregateMapBuilder(Settings:=testSettings)


        Dim testMap As IAggregateImplementationMap(Of Accounts.Account.Account, String) = CType(testMapBuilder.CreateImplementationMap(GetType(Accounts.Account.Account)),
            IAggregateImplementationMap(Of Accounts.Account.Account, String))

        Dim testAgg As New Accounts.Account.Account(LARGE_STREAM_TEST_ACCOUNT)
        Dim testWriter = testMap.CreateWriter(testAgg, LARGE_STREAM_TEST_ACCOUNT)


        'a date to increment as we build up the 
        Dim recordDate As Date = New Date(1971, 12, 19)

        Randomize()


        testWriter.AppendEvent(New Accounts.Account.eventDefinition.Opened(recordDate, "USD"))

        'was 1000000..
        For record As Integer = 1 To 10000 Step 1

            Dim amount As Decimal = CDec(Rnd() * 5000D)

            If ((record Mod 2) = 0) Then
                'add a deposit
                testWriter.AppendEvent(New Accounts.Account.eventDefinition.Money_Deposited(amount, recordDate.AddDays(-3), recordDate, "USD", 1D))
            Else
                If ((record Mod 7) = 0) Then
                    'add a withdrawal
                    testWriter.AppendEvent(New Accounts.Account.eventDefinition.Money_Withdrawn(amount, recordDate, "Over the counter", "Normal"))
                Else
                    testWriter.AppendEvent(New Accounts.Account.eventDefinition.Money_Withdrawn(amount, recordDate, "ATM", "TRN:" & record.ToString("00000000")))
                End If
            End If

            'Increment the date every 500 records
            If ((record Mod 500) = 0) Then
                recordDate = recordDate.AddDays(1)
            End If

        Next

        testWriter.AppendEvent(New Accounts.Account.eventDefinition.Closed(recordDate, "Client closed account - leaving jurisdiction"))

        Assert.IsTrue(testWriter.RecordCount > 0)

    End Sub
#End If

    '<TestCategory("Performance")>
    <TestCase()>
    Public Async Function Read_Million_Random_Events() As Task

        Dim actual As Decimal = 0D
        Dim expected As Decimal = 0.00D

        'load the serialisers
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Opened) _
            (OpenedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Closed) _
            (ClosedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Money_Deposited) _
            (DepositedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Money_Withdrawn) _
            (WithdrawnEventSerialiser.Create())

        Dim testSettings As CQRSAzureEventSourcingConfigurationSection
        'Dim config As Configuration = ConfigurationManager.GetSection(ConfigurationUserLevel.None)
        testSettings = CType(ConfigurationManager.GetSection("CQRSAzureEventSourcingConfiguration"),
            CQRSAzureEventSourcingConfigurationSection)

        Dim testMapBuilder As New AggregateMapBuilder(Settings:=testSettings)


        Dim testMap As IAggregateImplementationMap(Of Accounts.Account.Account, String) = CType(testMapBuilder.CreateImplementationMap(GetType(Accounts.Account.Account)),
            IAggregateImplementationMap(Of Accounts.Account.Account, String))



        Dim testAgg As New Accounts.Account.Account(LARGE_STREAM_TEST_ACCOUNT)

        'add a couple of events
        Dim writer = testMap.CreateWriter(testAgg, testAgg.GetKey())
        Await writer.AppendEvent(New Accounts.Account.eventDefinition.Money_Deposited(1234.56, DateTime.Today.AddDays(-3), DateTime.Today, "USD", 1D))

        Dim testProcessor = testMap.CreateProjectionProcessor(testAgg, LARGE_STREAM_TEST_ACCOUNT)

        Dim projection As New Accounts.Account.projection.Running_Balance()
        If (testProcessor IsNot Nothing) Then
            Await testProcessor.Process(projection)

            System.Diagnostics.Debug.WriteLine(projection.Last_Transaction_Date.ToLongDateString() & " value was " & projection.Balance.ToString())
        End If

        System.Diagnostics.Debug.WriteLine(" Last sequence " & projection.CurrentSequenceNumber.ToString())

        actual = projection.Balance

        Assert.AreNotEqual(expected, actual)


    End Function

    '<TestCategory("Performance")>
    <TestCase()>
    Public Async Function Categorise_Million_Random_Events() As Task

        Dim expected As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Include
        Dim actual As IClassifierDataSourceHandler.EvaluationResult = IClassifierDataSourceHandler.EvaluationResult.Unchanged

        'load the serialisers
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Opened) _
            (OpenedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Closed) _
            (ClosedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Money_Deposited) _
            (DepositedEventSerialiser.Create())
        EventSerializerFactory.AddOrSetSerialiser(Of Accounts.Account.eventDefinition.Money_Withdrawn) _
            (WithdrawnEventSerialiser.Create())

        Dim testSettings As CQRSAzureEventSourcingConfigurationSection
        'Dim config As Configuration = ConfigurationManager.GetSection(ConfigurationUserLevel.None)
        testSettings = CType(ConfigurationManager.GetSection("CQRSAzureEventSourcingConfiguration"),
            CQRSAzureEventSourcingConfigurationSection)

        Dim testMapBuilder As New AggregateMapBuilder(Settings:=testSettings)


        Dim testMap As IAggregateImplementationMap(Of Accounts.Account.Account, String) = CType(testMapBuilder.CreateImplementationMap(GetType(Accounts.Account.Account)),
            IAggregateImplementationMap(Of Accounts.Account.Account, String))

        Dim testAgg As New Accounts.Account.Account(LARGE_STREAM_TEST_ACCOUNT)

        'add a couple of events
        Dim writer = testMap.CreateWriter(testAgg, testAgg.GetKey())
        Await writer.AppendEvent(New Accounts.Account.eventDefinition.Money_Deposited(321.09, DateTime.Today.AddDays(-3), DateTime.Today, "USD", 1D))

        'Create a classifier
        Dim classifier As New Accounts.Account.classifier.Accounts_In_Credit_Classifier()
        'run the projection

        Dim testProcessor = testMap.CreateProjectionProcessor(testAgg, LARGE_STREAM_TEST_ACCOUNT)
        Dim projection As New Accounts.Account.projection.Running_Balance()
        If (testProcessor IsNot Nothing) Then
            Await testProcessor.Process(projection)

            System.Diagnostics.Debug.WriteLine(projection.Last_Transaction_Date.ToLongDateString() & " value was " & projection.Balance.ToString())
        End If

        'and evaluate it
        actual = classifier.EvaluateProjection(projection)

        Assert.AreEqual(expected, actual)

    End Function



End Class