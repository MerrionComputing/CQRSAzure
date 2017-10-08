Imports System.CodeDom
Imports System.Text
Imports CQRSAzure.CQRSdsl.CodeGeneration
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports CQRSAzure.CQRSdsl.Dsl


<TestClass()> Public Class AggregateIdentifierCodeGenerationUnitTest

    <TestMethod()>
    Public Sub Constructor_TestMethod()

        Dim addId As AggregateIdentifier = AggregateIdentifierMock.CreateAggregateIdentifier("DuncansAggregate")
        Dim testObj As New AggregateIdentifierCodeGenerator(addId)

        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub PublicFunction_NoParams_ToVBString_TestMethod()

        Dim classObj As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration("Duncan's Class Test")
        Dim propertiesHolder As New CodeCompileUnit
        Dim nsMain As New CodeNamespace("test")

        propertiesHolder.Namespaces.Add(nsMain)
        nsMain.Types.Add(classObj)

        Dim testObj As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedFunction("MyTest", New CodeTypeReference(GetType(System.String)), Nothing)
        classObj.Members.Add(testObj)

        Dim vbProperties As String = CodeGenerationUtilities.ToVBCodeString(propertiesHolder)

        Assert.IsNotNull(vbProperties)

    End Sub

    <TestMethod>
    Public Sub PublicFunction_NoParams_ToCSharpString_TestMethod()

        Dim classObj As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration("Duncan's Class Test")
        Dim propertiesHolder As New CodeCompileUnit
        Dim nsMain As New CodeNamespace("test")

        propertiesHolder.Namespaces.Add(nsMain)
        nsMain.Types.Add(classObj)

        Dim testObj As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedFunction("MyTest", New CodeTypeReference(GetType(System.String)), Nothing)
        classObj.Members.Add(testObj)

        Dim csProperties As String = CodeGenerationUtilities.ToCSharpCodeString(propertiesHolder)

        Assert.IsNotNull(csProperties)

    End Sub


    <TestMethod>
    Public Sub PublicSub_NoParams_ToCSharpString_TestMethod()

        Dim classObj As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration("Duncan's Class Test")
        Dim propertiesHolder As New CodeCompileUnit
        Dim nsMain As New CodeNamespace("test")

        propertiesHolder.Namespaces.Add(nsMain)
        nsMain.Types.Add(classObj)

        Dim testObj As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedSub("MyTest", Nothing)
        classObj.Members.Add(testObj)

        Dim csProperties As String = CodeGenerationUtilities.ToCSharpCodeString(propertiesHolder)

        Assert.IsNotNull(csProperties)

    End Sub

    <TestMethod>
    Public Sub PublicSub_NoParams_ToVBString_TestMethod()

        Dim classObj As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration("Duncan's Class Test")
        Dim propertiesHolder As New CodeCompileUnit
        Dim nsMain As New CodeNamespace("test")

        propertiesHolder.Namespaces.Add(nsMain)
        nsMain.Types.Add(classObj)

        Dim testObj As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedSub("MyTest", Nothing)
        classObj.Members.Add(testObj)

        Dim vbProperties As String = CodeGenerationUtilities.ToVBCodeString(propertiesHolder)

        Assert.IsNotNull(vbProperties)

    End Sub


End Class