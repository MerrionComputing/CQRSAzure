Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports CQRSAzure.CQRSdsl.CodeGeneration

<TestClass()> Public Class EntityFrameworkCodeGenerationUnitTest

    <TestMethod()>
    Public Sub CreateDBSetProperty_TestMethod()

        Dim testObj As CodeDom.CodeMemberProperty = EntityFrameworkCodeGeneration.CreateDBSetProperty("Cow")
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub CreateDBSetProperty_ToVBCode_TestMethod()

        Dim expected As String = "As DbSet(Of Cow)"
        Dim actual As String = "Not set"

        Dim testObj As CodeDom.CodeMemberProperty = EntityFrameworkCodeGeneration.CreateDBSetProperty("Cow")

        actual = CodeGenerationUtilities.ToVBCodeString(CodeGenerationUtilities.Wrap(testObj))

        Assert.IsTrue(actual.Contains(expected))

    End Sub

End Class