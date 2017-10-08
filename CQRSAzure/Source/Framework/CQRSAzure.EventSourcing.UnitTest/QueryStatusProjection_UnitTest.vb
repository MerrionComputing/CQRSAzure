Imports System.Text
Imports CQRSAzure.EventSourcing.Queries
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class QueryStatusProjection_UnitTest

    <TestMethod()>
    Public Sub QueryAggregate_Constructor_TestMethod()

        Dim testObj As New QueryAggregate(Guid.Empty)
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub QueryCreatedEvent_Constructor_TestMethod()

        Dim testObj As QueryCreatedEvent = QueryCreatedEvent.Create(Guid.Empty,
                                                                    "Test Query",
                                                                    DateTime.UtcNow,
                                                                    "Unit testing",
                                                                    "Duncan",
                                                                    "Identity group",
                                                                    "Parameters")

        Assert.IsNotNull(testObj)

    End Sub



End Class