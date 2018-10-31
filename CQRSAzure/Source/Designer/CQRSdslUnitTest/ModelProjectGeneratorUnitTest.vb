Imports CQRSAzure.CQRSdsl.CodeGeneration
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class ModelProjectGeneratorUnitTest

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New ModelProjectGenerator(ModelProjectGenerator.ProjectClassification.CommandDefinition, "Herd")
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub Filename_VBNet_Command_TestMethod()

        Dim expected As String = "Herd.CommandDefinition.vbproj"
        Dim actual As String = "Not set"

        Dim testObj As New ModelProjectGenerator(ModelProjectGenerator.ProjectClassification.CommandDefinition, "Herd")
        actual = testObj.GetProjectFilename(CQRSAzure.CQRSdsl.CustomCode.Interfaces.ModelCodegenerationOptionsBase.SupportedLanguages.VBNet)

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub Filename_CSharp_Command_TestMethod()

        Dim expected As String = "Herd.CommandDefinition.csproj"
        Dim actual As String = "Not set"

        Dim testObj As New ModelProjectGenerator(ModelProjectGenerator.ProjectClassification.CommandDefinition, "Herd")
        actual = testObj.GetProjectFilename(CQRSAzure.CQRSdsl.CustomCode.Interfaces.ModelCodegenerationOptionsBase.SupportedLanguages.CSharp)

        Assert.AreEqual(expected, actual)


    End Sub


    <TestMethod()>
    Public Sub AddTwoItems_Testmethod()

        Dim testObj As New ModelProjectGenerator(ModelProjectGenerator.ProjectClassification.CommandDefinition, "Herd")
        testObj.AddProjectItem(ProjectItem.ValidCompileActions.Compile, "MyFile.vb")
        testObj.AddProjectItem(ProjectItem.ValidCompileActions.None, "Reference.cqrsx")

        Assert.IsNotNull(testObj)


    End Sub

End Class

<TestClass()>
Public Class ProjectItemGroupTestClass

    <TestMethod()>
    Public Sub Constructor_NotNull_TestMethod()

        Dim testObj As New ProjectItemGroup()
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub ToString_NoItems_TestMethod()

        Dim expected As String = "<ItemGroup>" & vbNewLine &
            "</ItemGroup>" & vbNewLine

        Dim actual As String = "test"
        Dim testObj As New ProjectItemGroup()

        actual = testObj.ToString()

        Assert.AreEqual(expected, actual)
    End Sub


    <TestMethod()>
    Public Sub ToString_TwoItems_TestMethod()

        Dim expected As String = "<ItemGroup>" & vbNewLine &
            "   <Compile Include=""MyFile.vb"" />" & vbNewLine &
            "   <None Include=""Reference.cqrsx"" />" & vbNewLine &
            "</ItemGroup>" & vbNewLine

        Dim actual As String = "test"
        Dim testObj As New ProjectItemGroup()
        testObj.AddItem(New ProjectItem(ProjectItem.ValidCompileActions.Compile, "MyFile.vb"))
        testObj.AddItem(New ProjectItem(ProjectItem.ValidCompileActions.None, "Reference.cqrsx"))

        actual = testObj.ToString()

        Assert.AreEqual(expected, actual)
    End Sub

End Class

<TestClass()>
Public Class ProjectItemTestClass

    <TestMethod()>
    Public Sub Constructor_NotNull_testmethod()

        Dim testObj As New ProjectItem(ProjectItem.ValidCompileActions.Compile, "MyCode.cs")
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub ToString_TestMethod()

        Dim expected As String = "<Reference Include=""System.Xml.Linq"" />"
        Dim actual As String = "test"

        Dim testObj As New ProjectItem(ProjectItem.ValidCompileActions.Reference, "System.Xml.Linq")
        actual = testObj.ToString()

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub ToString_Depencency_TestMethod()

        Dim expected As String = "<Compile Include=""Properties\Settings.Designer.cs"" >" & vbNewLine &
            "   <DependentUpon>Settings.settings</DependentUpon>" & vbNewLine &
            "</Compile>" & vbNewLine

        Dim actual As String = "test"

        Dim testObj As New ProjectItem(ProjectItem.ValidCompileActions.Compile, "Properties\Settings.Designer.cs", "Settings.settings")
        actual = testObj.ToString()

        Assert.AreEqual(expected, actual)

    End Sub

End Class