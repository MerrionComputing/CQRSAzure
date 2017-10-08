Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System.Configuration


<TestClass()>
Public Class CQRSAzureIdentifierGroupConfigurationSectionUnitTest

    Public Const VALID_SECTION_NAME As String = "CQRSAzureIdentifierGroupConfiguration"

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New CQRSAzureIdentifierGroupConfigurationSection()
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub LoadConfigurationSection_Testmethod()

        Dim testObj As CQRSAzureIdentifierGroupConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testObj = config.GetSection(VALID_SECTION_NAME)

        Assert.IsNotNull(testObj)

    End Sub

End Class