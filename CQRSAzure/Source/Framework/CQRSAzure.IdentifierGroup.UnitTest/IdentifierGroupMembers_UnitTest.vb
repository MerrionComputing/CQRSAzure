Imports System.Configuration
Imports CQRSAzure.EventSourcing
Imports NUnit.Framework

<TestFixture()>
Public Class IdentifierGroupMembers_UnitTest

    <Ignore("Not complete")>
    <TestCase()>
    Public Sub CreateIdentifierGroupProcessor_Constructor_TestMethod()

        Dim testIDGProcessor As IIdentifierGroupProcessor(Of Accounts.Account.Account, String) = Nothing

        Dim testSettings As CQRSAzureEventSourcingConfigurationSection
        Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        testSettings = config.GetSection("CQRSAzureEventSourcingConfiguration")

        Dim testMapBuilder As New IdentityGroupAggregateMapBuilder(Settings:=testSettings)

        If (testMapBuilder IsNot Nothing) Then
            Dim testMap As IAggregateImplementationMap(Of Accounts.Account.Account, String) = testMapBuilder.CreateImplementationMap(GetType(Accounts.Account.Account))

            testIDGProcessor = testMap.CreateIdentifierGroupProcessor()

            Assert.IsNotNull(testIDGProcessor)
        Else

            Assert.Fail("Could not create test map builder")
        End If


    End Sub

End Class