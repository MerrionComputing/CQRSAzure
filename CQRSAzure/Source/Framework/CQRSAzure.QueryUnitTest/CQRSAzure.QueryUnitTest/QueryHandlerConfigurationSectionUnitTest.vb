Imports System.Text
Imports NUnit.Framework

Imports CQRSAzure.QueryDefinition
Imports CQRSAzure.QueryHandler
Imports System.Configuration

<TestFixture()>
Public Class QueryHandlerConfigurationSectionUnitTest

    Public Const VALID_SECTION_NAME As String = "CQRSAzureQueryHandlerConfiguration"

    <TestCase()>
    Public Sub Constructor_Empty_TestMethod()

        Dim testObj As New CQRSQueryHandlerConfigurationSection()
        Assert.IsNotNull(testObj)

    End Sub


    <TestCase()>
    Public Sub LoadConfigurationSection_Testmethod()

        Dim testObj As CQRSQueryHandlerConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testObj = config.GetSection(VALID_SECTION_NAME)

        Assert.IsNotNull(testObj)

    End Sub

    <TestCase>
    Public Sub QueryHandlerMaps_NonZero_TestMethod()

        Dim actual As Integer = 0
        Dim notExpected As Integer = 0

        Dim testObj As CQRSQueryHandlerConfigurationSection
        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testObj = config.GetSection(VALID_SECTION_NAME)

        actual = testObj.QueryHandlerMaps.Count()

        Assert.AreNotEqual(notExpected, actual)

    End Sub

End Class