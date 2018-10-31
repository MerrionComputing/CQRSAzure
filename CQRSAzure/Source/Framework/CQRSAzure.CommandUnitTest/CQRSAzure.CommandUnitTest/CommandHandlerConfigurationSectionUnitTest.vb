Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.CommandDefinition
Imports CQRSAzure.CommandHandler
Imports System.Configuration

<TestClass()>
Public Class CommandHandlerConfigurationSectionUnitTest

    Public Const VALID_SECTION_NAME As String = "CQRSAzureCommandHandlerConfiguration"

    <TestMethod()>
    Public Sub Constructor_Empty_TestMethod()

        Dim testObj As New CQRSAzureCommandHandlerConfigurationSection()
        Assert.IsNotNull(testObj)

    End Sub


    <TestMethod()>
    Public Sub LoadConfigurationSection_Testmethod()

        Dim testObj As CQRSAzureCommandHandlerConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testObj = config.GetSection(VALID_SECTION_NAME)

        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub QueryHandlerMaps_NonZero_TestMethod()

        Dim actual As Integer = 0
        Dim notExpected As Integer = 0

        Dim testObj As CQRSAzureCommandHandlerConfigurationSection
        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testObj = config.GetSection(VALID_SECTION_NAME)

        actual = testObj.CommandHandlerMaps.Count()

        Assert.AreNotEqual(notExpected, actual)

    End Sub

End Class