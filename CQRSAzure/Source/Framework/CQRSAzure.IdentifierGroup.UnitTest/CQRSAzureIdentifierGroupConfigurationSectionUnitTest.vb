Imports System.Text
Imports NUnit.Framework
Imports System.Configuration


<TestFixture()>
Public Class CQRSAzureIdentifierGroupConfigurationSectionUnitTest

    Public Const VALID_SECTION_NAME As String = "CQRSAzureIdentifierGroupConfiguration"

    <TestCase()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New CQRSAzureIdentifierGroupConfigurationSection()
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub LoadConfigurationSection_Testmethod()

        Dim testObj As CQRSAzureIdentifierGroupConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testObj = config.GetSection(VALID_SECTION_NAME)

        Assert.IsNotNull(testObj)

    End Sub

End Class