Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CodeGeneration
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces

<TestClass()>
Public Class ModelCodeGenerationOptionsUnitTest

    <TestMethod>
    Public Sub Constructor_Default_TestMethod()

        Dim testObj As IModelCodeGenerationOptions = ModelCodeGenerationOptions.Default()

        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub Create_VBNet_TestMethod()

        Dim expected As ModelCodegenerationOptionsBase.SupportedLanguages = ModelCodegenerationOptionsBase.SupportedLanguages.VBNet
        Dim actual As ModelCodegenerationOptionsBase.SupportedLanguages = ModelCodegenerationOptionsBase.SupportedLanguages.CSharp

        Dim testObj As IModelCodeGenerationOptions = ModelCodeGenerationOptions.Create(ModelCodegenerationOptionsBase.SupportedLanguages.VBNet,
                                                                                       ModelCodegenerationOptionsBase.ConstructorPreferenceSetting.GenerateBoth,
                                                                                       New System.IO.DirectoryInfo("C:\temp"),
                                                                                       False,
                                                                                       False,
                                                                                       False)

        actual = testObj.CodeLanguage

        Assert.AreEqual(expected, actual)


    End Sub

End Class