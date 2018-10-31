Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.CQRSdsl.CustomCode.Settings

<TestClass()>
Public Class CodeGenerationSettingsUnitTest

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As CQRSAzure.CQRSdsl.CustomCode.Interfaces.IModelCodeGenerationOptions = CQRSAzure.CQRSdsl.Dsl.ModelCodeGenerationOptions.Default()
        Assert.IsNotNull(testObj)

    End Sub


End Class