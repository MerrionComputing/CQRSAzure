Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System.Configuration


Imports CQRSAzure.EventSourcing

<TestClass()>
Public Class CQRSAzureEventSourcingConfigurationUnitTest

    Public Const VALID_SECTION_NAME As String = "CQRSAzureEventSourcingConfiguration"

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New CQRSAzureEventSourcingConfigurationSection()
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub LoadConfigurationSection_Testmethod()

        Dim testObj As CQRSAzureEventSourcingConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testObj = config.GetSection(VALID_SECTION_NAME)

        Assert.IsNotNull(testObj)

    End Sub

End Class