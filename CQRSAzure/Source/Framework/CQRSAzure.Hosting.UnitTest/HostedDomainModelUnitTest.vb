Imports System.Text
Imports NUnit.Framework

<TestFixture()>
Public Class HostedDomainModelUnitTest

    <TestCase()>
    Public Sub HostedDomainModel_Constructor_TestMethod()

        Dim testOb As HostedDomainModel = HostedDomainModel.Create("Unit test hosted domain",
                                                                    "Unit Test ES ",
                                                                    "Unit test IG",
                                                                    "Unit test cmd def",
                                                                    "Unit test implementation",
                                                                    "Unit test qry def",
                                                                    "Unit test qry implementation")

        Assert.IsNotNull(testOb)

    End Sub

End Class