Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.CQRSdsl.CodeGeneration
Imports System.CodeDom

<TestClass()> Public Class InterfaceCodeGenerationUnitTest

    <TestMethod()>
    Public Sub SimplePropertyDeclaration_Constructor_TestMethod()

        Dim interfaceProperty As CodeMemberProperty = InterfaceCodeGeneration.SimplePropertyDeclaration(True,
                                                                                                                       "CommentText",
                                                                                                                       CQRSAzure.CQRSdsl.Dsl.PropertyDataType.String)
        Assert.IsNotNull(interfaceProperty)

    End Sub

    <TestMethod()>
    Public Sub InterfaceCodeGeneration_Constructor_TestMethod()

        Dim interfaceObj As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration("Duncan's Interface")

        Assert.IsNotNull(interfaceObj)

    End Sub

    <TestMethod>
    Public Sub InterfaceCodeGeneration_ToVBString_TestMethod()

        Dim interfaceObj As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration("Duncan's Interface")
        Dim interfaceHolder As New CodeCompileUnit
        Dim nsMain As New CodeNamespace("test")

        interfaceObj.StartDirectives.Add(New CodeRegionDirective(CodeRegionMode.Start,
                "Test Region"))

        interfaceObj.Comments.Add(New CodeCommentStatement("Is this in the region?"))

        interfaceObj.StartDirectives.Add(New CodeRegionDirective(CodeRegionMode.End,
                String.Empty))

        interfaceHolder.Namespaces.Add(nsMain)
        nsMain.Types.Add(interfaceObj)
        Dim vbInterface As String = CodeGenerationUtilities.ToVBCodeString(interfaceHolder)


        Assert.IsNotNull(vbInterface)
    End Sub

    <TestMethod>
    Public Sub InterfaceCodeGeneration_ToCSharpString_TestMethod()

        Dim interfaceObj As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration("Duncan's Interface")
        Dim interfaceHolder As New CodeCompileUnit
        Dim nsMain As New CodeNamespace("System")
        interfaceHolder.Namespaces.Add(nsMain)
        nsMain.Types.Add(interfaceObj)
        Dim cSharpInterface As String = CodeGenerationUtilities.ToCSharpCodeString(interfaceHolder)


        Assert.IsNotNull(cSharpInterface)
    End Sub

    <TestMethod>
    Public Sub ImplementsGenericInterfaceReference_ToVBString_TestMethod()

        Dim interfaceObj As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration("Duncan's Interface")
        Dim implementsGeneric As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("IAttribute", {New CodeTypeReference(GetType(String)), New CodeTypeReference(GetType(Integer))})
        interfaceObj.BaseTypes.Add(implementsGeneric)
        Dim interfaceHolder As CodeCompileUnit = CodeGenerationUtilities.Wrap(interfaceObj)

        Dim vbInterface As String = CodeGenerationUtilities.ToVBCodeString(interfaceHolder)

        Assert.IsNotNull(vbInterface)

    End Sub

    <TestMethod>
    Public Sub ImplementsGenericInterfaceReference_ToCSharpString_TestMethod()

        Dim interfaceObj As CodeTypeDeclaration = InterfaceCodeGeneration.InterfaceDeclaration("Duncan's Interface")
        Dim implementsGeneric As CodeTypeReference = InterfaceCodeGeneration.ImplementsGenericInterfaceReference("IAttribute", {New CodeTypeReference(GetType(String)), New CodeTypeReference(GetType(Integer))})
        interfaceObj.BaseTypes.Add(implementsGeneric)
        Dim interfaceHolder As CodeCompileUnit = CodeGenerationUtilities.Wrap(interfaceObj)

        Dim csInterface As String = CodeGenerationUtilities.ToCSharpCodeString(interfaceHolder)

        Assert.IsNotNull(csInterface)

    End Sub
End Class