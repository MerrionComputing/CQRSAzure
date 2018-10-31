Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.QueryDefinition
Imports CQRSAzure.QueryHandler
Imports System.Configuration

<TestClass()>
Public Class QueryHandlerConfigurationSectionUnitTest

    Public Const VALID_SECTION_NAME As String = "CQRSAzureQueryHandlerConfiguration"

    <TestMethod()>
    Public Sub Constructor_Empty_TestMethod()

        Dim testObj As New CQRSQueryHandlerConfigurationSection()
        Assert.IsNotNull(testObj)

    End Sub


    <TestMethod()>
    Public Sub LoadConfigurationSection_Testmethod()

        Dim testObj As CQRSQueryHandlerConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testObj = config.GetSection(VALID_SECTION_NAME)

        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
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