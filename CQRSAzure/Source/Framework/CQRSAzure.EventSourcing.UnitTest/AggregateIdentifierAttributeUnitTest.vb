Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.EventSourcing

<TestClass()>
Public Class AggregateIdentifierAttributeUnitTest

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New AggregateIdentifierAttribute(GetType(Mocking.ValidAggregateIdentifier))
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    <ExpectedException(GetType(InvalidCastException), AllowDerivedTypes:=False)>
    Public Sub Constructor_InvalidCastException_TestMethod()

        Dim testObj As New AggregateIdentifierAttribute(GetType(Mocking.InvalidAggregateIdentifier))
        Assert.IsNotNull(testObj)

    End Sub

End Class