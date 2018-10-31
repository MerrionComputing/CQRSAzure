Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.Blob
Imports CQRSAzure.EventSourcing.UnitTest.Mocking

<TestClass()>
Public Class AzureBlobEventStreamUnitTest

    Public Const TEST_AGGREGATE_IDENTIFIER As String = "19121971"


    ''' <remarks>
    ''' Note that you will either need to reference an actual Azure account or to start the azure local storage emulator in order
    ''' to run this unit test
    ''' </remarks>
    <TestMethod()>
    Public Sub Reader_Constructor_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As BlobEventStreamReader(Of MockAggregate, String) = BlobEventStreamReader(Of MockAggregate, String).Create(testAgg)
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub Writer_Constructor_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As BlobEventStreamWriter(Of MockAggregate, String) = BlobEventStreamWriter(Of MockAggregate, String).Create(testAgg)
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub Writer_WriteEvent_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As BlobEventStreamWriter(Of MockAggregate, String) = BlobEventStreamWriter(Of MockAggregate, String).Create(testAgg)
        testObj.AppendEvent(New MockEventTypeOne() With {.EventOneStringProperty = "My test", .EventOneIntegerProperty = 123})

        Assert.IsTrue(testObj.RecordCount > 0)

    End Sub

    <TestMethod()>
    Public Sub Writer_WriteEventTwo_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)
        Dim testObj As BlobEventStreamWriter(Of MockAggregate, String) = BlobEventStreamWriter(Of MockAggregate, String).Create(testAgg)
        testObj.Reset()
        testObj.AppendEvent(New MockEventTypeTwo() With {.EventTwoNullableDateProperty = DateTime.UtcNow, .EventTwoStringProperty = "My test two", .EventTwoDecimalProperty = 123.45})

        Assert.IsTrue(testObj.RecordCount > 0)

    End Sub

    <TestMethod()>
    Public Sub Reader_ReadEvents_TestMethod()


        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)

        Dim testWriter As BlobEventStreamWriter(Of MockAggregate, String) = BlobEventStreamWriter(Of MockAggregate, String).Create(testAgg)
        testWriter.Reset()
        'add a dummy event
        testWriter.AppendEvent(New MockEventTypeTwo() With {.EventTwoNullableDateProperty = DateTime.UtcNow, .EventTwoStringProperty = "My test two", .EventTwoDecimalProperty = 123.45})

        Dim testObj As BlobEventStreamReader(Of MockAggregate, String) = BlobEventStreamReader(Of MockAggregate, String).Create(testAgg)

        Dim ret = testObj.GetEvents()

        Assert.IsNotNull(ret)

    End Sub

    <TestMethod()>
    Public Sub Reader_ReadEventsWithContext_TestMethod()

        Dim testAgg As New MockAggregate(TEST_AGGREGATE_IDENTIFIER)

        Dim testWriter As BlobEventStreamWriter(Of MockAggregate, String) = BlobEventStreamWriter(Of MockAggregate, String).Create(testAgg)
        testWriter.Reset()

        Dim testObj As BlobEventStreamReader(Of MockAggregate, String) = BlobEventStreamReader(Of MockAggregate, String).Create(testAgg)
        Dim ret = testObj.GetEventsWithContext()

        Assert.IsNotNull(ret)

    End Sub


    <TestMethod>
    Public Sub MakeValidStorageFolderName_Empty_TestMethod()

        Dim expected As String = "uncategorised"
        Dim actual As String = ""

        actual = BlobEventStreamBase.MakeValidStorageFolderName("")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub MakeValidStorageFolderName_TooShort_TestMethod()

        Dim expected As String = "a-abc"
        Dim actual As String = ""

        actual = BlobEventStreamBase.MakeValidStorageFolderName("-a")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub MakeValidStorageFolderName_TooShortAfterTrim_TestMethod()

        Dim expected As String = "a-abc"
        Dim actual As String = ""

        actual = BlobEventStreamBase.MakeValidStorageFolderName("------a")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub MakeValidStorageFolderName_FixChars_TestMethod()

        Dim expected As String = "duncan-s-model"
        Dim actual As String = ""

        actual = BlobEventStreamBase.MakeValidStorageFolderName("Duncan's Model")

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub MakeValidStorageFolderName_IsValid_TestMethod()

        Dim expected As String = "duncan-s-model"
        Dim actual As String = ""

        actual = BlobEventStreamBase.MakeValidStorageFolderName(expected)

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub MakeValidStorageFolderName_TooLong_TestMethod()

        Dim expected As Integer = 63
        Dim actual As Integer = 0

        actual = BlobEventStreamBase.MakeValidStorageFolderName("The quick dog jumped over the lazy dog which was suprising to them both with the display of dexterity").Length

        Assert.AreEqual(expected, actual)

    End Sub


    <TestMethod()>
    Public Sub GetAllStreamKeys_NoDate_TestMethod()

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        Dim instance As New MockAggregate(AzureBlobEventStreamUnitTest.TEST_AGGREGATE_IDENTIFIER)

        Dim testObj = Azure.Blob.BlobEventStreamReader(Of MockAggregate, String).Create(New MockAggregate(AzureBlobEventStreamUnitTest.TEST_AGGREGATE_IDENTIFIER))
        Dim testProvider As IEventStreamProvider(Of MockAggregate, String) = BlobEventStreamProviderFactory.Create(Of MockAggregate, String)(instance, AzureBlobEventStreamUnitTest.TEST_AGGREGATE_IDENTIFIER)
        'TODO : Instantiate the provider instance

        actual = testProvider.GetAllStreamKeys().Contains(AzureBlobEventStreamUnitTest.TEST_AGGREGATE_IDENTIFIER)

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub GetAllStreamKeys_GUIDKey_NoDate_TestMethod()

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        Dim key As Guid = New Guid("a73140ce-2204-413a-8beb-604241fd721d")

        Dim instance As New MockGuidAggregate(key)
        Dim testObj = Azure.Blob.BlobEventStreamReader(Of MockGuidAggregate, Guid).Create(instance)
        Dim testProvider As IEventStreamProvider(Of MockGuidAggregate, Guid) = BlobEventStreamProviderFactory.Create(Of MockGuidAggregate, Guid)(instance, key)

        actual = testProvider.GetAllStreamKeys().Contains(key)

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub GetAllStreamKeys_GUIDKey_FutureDate_TestMethod()

        Dim expected As Boolean = False
        Dim actual As Boolean = True

        Dim asOfdate As DateTime = New DateTime(2037, 1, 1)

        Dim key As Guid = New Guid("a883fb8a-97ab-4d66-85db-ec7b9ba3b423")

        Dim testObj = Azure.Blob.BlobEventStreamReader(Of MockGuidAggregate, Guid).Create(New MockGuidAggregate(key))
        Dim testProvider As IEventStreamProvider(Of MockGuidAggregate, Guid) = BlobEventStreamProviderFactory.Create(Of MockGuidAggregate, Guid)()

        actual = testProvider.GetAllStreamKeys(asOfdate).Contains(key)

        Assert.AreEqual(expected, actual)

    End Sub

End Class