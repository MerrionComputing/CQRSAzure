Imports NUnit.Framework

<TestFixture()>
Public Class AggregateIdentifierAttributeUnitTest

    <TestCase()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New AggregateIdentifierAttribute(GetType(Mocking.ValidAggregateIdentifier))
        Assert.IsNotNull(testObj)

    End Sub

    <TestCase>
    Public Sub Constructor_InvalidCastException_TestMethod()

        Assert.Throws(Of InvalidCastException)(Sub()
                                                   Dim testObj As New AggregateIdentifierAttribute(GetType(Mocking.InvalidAggregateIdentifier))

                                               End Sub)

    End Sub

End Class