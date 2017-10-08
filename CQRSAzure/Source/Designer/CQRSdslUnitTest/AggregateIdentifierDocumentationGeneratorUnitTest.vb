Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.CQRSdsl.DocumentationGeneration
Imports CQRSAzure.CQRSdsl.Dsl

<TestClass()>
Public Class AggregateIdentifierDocumentationGeneratorUnitTest

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As AggregateIdentifierDocumentationGenerator = Nothing
        Dim aggId As AggregateIdentifier = AggregateIdentifierMock.CreateAggregateIdentifier("My name")

        If (aggId IsNot Nothing) Then
            testObj = New AggregateIdentifierDocumentationGenerator(aggId)
        End If

        Assert.IsNotNull(testObj)

    End Sub



End Class