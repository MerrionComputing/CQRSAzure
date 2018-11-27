Imports System.Text
Imports NUnit.Framework
Imports System.Configuration

<TestFixture()>
Public Class CQRSAzureHostConfigurationSectionUnitTest

    Public Const VALID_SECTION_NAME As String = "CQRSAzureHostConfigurationSection"

    <TestCase()>
    Public Sub Create_Empty_TestMethod()

        Dim testObj As New CQRSAzureHostConfigurationSection()
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub LoadConfigurationSection_Testmethod()

        Dim testObj As CQRSAzureHostConfigurationSection

        Dim config As System.Configuration.Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testObj = config.GetSection(VALID_SECTION_NAME)

        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub LoadConfigurationSection_MatchingName_Testmethod()

        Dim expected As String = "Grettel"
        Dim actual As String = "Not set"

        Dim testObj As CQRSAzureHostConfigurationSection

        Dim config As System.Configuration.Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testObj = config.GetSection(VALID_SECTION_NAME)

        actual = testObj.Name

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub LoadConfigurationSection_LoadDomains_Testmethod()

        Dim expected As Boolean = True
        Dim actual As Boolean = False

        Dim testObj As CQRSAzureHostConfigurationSection

        Dim config As System.Configuration.Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testObj = config.GetSection(VALID_SECTION_NAME)

        actual = testObj.LoadDomainsOnStartup

        Assert.AreEqual(expected, actual)

    End Sub

End Class