Imports System.Text
Imports NUnit.Framework
Imports System.Configuration


Imports CQRSAzure.EventSourcing

<TestFixture()>
Public Class CQRSAzureEventSourcingConfigurationUnitTest

    Public Const VALID_SECTION_NAME As String = "CQRSAzureEventSourcingConfiguration"

    <TestCase()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New CQRSAzureEventSourcingConfigurationSection()
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase()>
    Public Sub LoadConfigurationSection_Testmethod()

        Dim testObj As CQRSAzureEventSourcingConfigurationSection

        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testObj = config.GetSection(VALID_SECTION_NAME)

        Assert.IsNotNull(testObj)

    End Sub

End Class