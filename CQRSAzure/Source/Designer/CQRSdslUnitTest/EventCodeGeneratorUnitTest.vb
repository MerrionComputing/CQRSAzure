Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports System.CodeDom
Imports CQRSAzure.CQRSdsl.Dsl
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSdslUnitTest.Mocking
Imports CQRSAzure.CQRSdsl.CodeGeneration

<TestClass()>
Public Class EventCodeGeneratorUnitTest

    <TestMethod()>
    Public Sub EventCodeGenerator_Constructor_TestMethod()

        Dim evtMock As EventDefinition = EventDefinitionMock.CreateEventDefinition("Duncan's event")
        Dim testObj As New EventCodeGenerator(evtMock)

        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub PrivateProperties_Constructor_TestMethod()

        Dim properties As New Collections.Generic.List(Of IEventPropertyEntity)
        properties.Add(New MockEventProperty() With {.Name = "Email", .DataType = PropertyDataType.String, .Description = "Email address of the user"})
        properties.Add(New MockEventProperty() With {.Name = "Height (cm)", .DataType = PropertyDataType.Integer, .Description = "Recorded height in cm"})

        Dim testObj As CodeTypeMember() = PropertyCodeGeneration.PrivateBackingMembers(properties)

        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod>
    Public Sub PrivateProperties_PropertyCount_TestMethod()

        Dim expected As Integer = 2
        Dim actual As Integer = 0

        Dim properties As New Collections.Generic.List(Of IEventPropertyEntity)
        properties.Add(New MockEventProperty() With {.Name = "Email", .DataType = PropertyDataType.String, .Description = "Email address of the user"})
        properties.Add(New MockEventProperty() With {.Name = "Height (cm)", .DataType = PropertyDataType.Integer, .Description = "Recorded height in cm"})

        Dim testObj As CodeTypeMember() = PropertyCodeGeneration.PrivateBackingMembers(properties)
        actual = testObj.Count

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub PrivateProperties_ToVBString_Testmethod()

        Dim properties As New Collections.Generic.List(Of IEventPropertyEntity)
        properties.Add(New MockEventProperty() With {.Name = "Email", .DataType = PropertyDataType.String, .Description = "Email address of the user"})
        properties.Add(New MockEventProperty() With {.Name = "Height (cm)", .DataType = PropertyDataType.Integer, .Description = "Recorded height in cm"})
        properties.Add(New MockEventProperty() With {.Name = "Blood/Alcohol %", .DataType = PropertyDataType.Decimal, .Description = "Recorded blood/alcohol"})


        Dim testObj As CodeTypeMember() = PropertyCodeGeneration.PrivateBackingMembers(properties)

        Dim classObj As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration("Duncan's Class Test")
        Dim propertiesHolder As New CodeCompileUnit
        Dim nsMain As New CodeNamespace("test")

        propertiesHolder.Namespaces.Add(nsMain)
        nsMain.Types.Add(classObj)

        classObj.Members.AddRange(testObj)

        Dim vbProperties As String = CodeGenerationUtilities.ToVBCodeString(propertiesHolder)

        Assert.IsNotNull(vbProperties)

    End Sub

    <TestMethod>
    Public Sub PublicProperty_ToVBString_TestMethod()

        Dim mockProperty = New MockEventProperty() With {.Name = "Email", .DataType = PropertyDataType.String, .Description = "Email address of the user"}

        Dim testObj As CodeTypeMember = PropertyCodeGeneration.PublicMember(mockProperty, "IDuncans_Interface")

        Dim classObj As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration("Duncan's Class Test")
        Dim propertiesHolder As New CodeCompileUnit
        Dim nsMain As New CodeNamespace("test")


        propertiesHolder.Namespaces.Add(nsMain)
        nsMain.Types.Add(classObj)

        classObj.Members.Add(testObj)

        Dim vbProperties As String = CodeGenerationUtilities.ToVBCodeString(propertiesHolder)

        Assert.IsNotNull(vbProperties)

    End Sub

    <TestMethod>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub ToMemberName_Empty_Testmethod()

        Dim sMember As String = EventCodeGenerator.ToMemberName("", False)

        Assert.IsNull(sMember)

    End Sub

    <TestMethod>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub ToMemberName_OnlyInvalid_Testmethod()

        Dim sMember As String = EventCodeGenerator.ToMemberName("(%!:)", False)

        Assert.IsNull(sMember)

    End Sub

    <TestMethod>
    Public Sub ToMembername_Private_TestMethod()

        Dim expected As String = "_Test_1"
        Dim actual As String

        actual = EventCodeGenerator.ToMemberName("Test%1!", True)

        Assert.AreEqual(expected, actual)

    End Sub


    <TestMethod>
    Public Sub ToMembername_Public_TestMethod()

        Dim expected As String = "Test_1"
        Dim actual As String

        actual = EventCodeGenerator.ToMemberName("Test%1!)", False)

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod>
    Public Sub ParameterisedConstructor_ToVBString_TestMethod()

        Dim properties As New Collections.Generic.List(Of IEventPropertyEntity)
        properties.Add(New MockEventProperty() With {.Name = "Email", .DataType = PropertyDataType.String, .Description = "Email address of the user"})
        properties.Add(New MockEventProperty() With {.Name = "Height (cm)", .DataType = PropertyDataType.Integer, .Description = "Recorded height in cm"})
        properties.Add(New MockEventProperty() With {.Name = "Blood/Alcohol %", .DataType = PropertyDataType.Decimal, .Description = "Recorded blood/alcohol"})


        Dim testObj As CodeTypeMember() = PropertyCodeGeneration.PrivateBackingMembers(properties)

        Dim classObj As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration("Duncan's Class Test")
        Dim propertiesHolder As New CodeCompileUnit
        Dim nsMain As New CodeNamespace("test")

        propertiesHolder.Namespaces.Add(nsMain)
        nsMain.Types.Add(classObj)

        classObj.Members.AddRange(testObj)
        classObj.Members.Add(ConstructorCodeGenerator.ParameterisedConstructor(properties))

        Dim vbProperties As String = CodeGenerationUtilities.ToVBCodeString(propertiesHolder)

        Assert.IsNotNull(vbProperties)

    End Sub

    <TestMethod>
    Public Sub InterfaceBasedConstructor_ToVBString_TestMethod()

        Dim properties As New Collections.Generic.List(Of IEventPropertyEntity)
        properties.Add(New MockEventProperty() With {.Name = "Email", .DataType = PropertyDataType.String, .Description = "Email address of the user"})
        properties.Add(New MockEventProperty() With {.Name = "Height (cm)", .DataType = PropertyDataType.Integer, .Description = "Recorded height in cm"})
        properties.Add(New MockEventProperty() With {.Name = "Blood/Alcohol %", .DataType = PropertyDataType.Decimal, .Description = "Recorded blood/alcohol"})


        Dim testObj As CodeTypeMember() = PropertyCodeGeneration.PrivateBackingMembers(properties)

        Dim classObj As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration("Duncan's Class Test")
        Dim propertiesHolder As New CodeCompileUnit
        Dim nsMain As New CodeNamespace("test")

        propertiesHolder.Namespaces.Add(nsMain)
        nsMain.Types.Add(classObj)


        classObj.Members.AddRange(testObj)
        classObj.Members.Add(ConstructorCodeGenerator.InterfaceBasedConstructor(properties, "IDuncansClass", "DuncansClassIn"))


        Dim vbProperties As String = CodeGenerationUtilities.ToVBCodeString(propertiesHolder)

        Assert.IsNotNull(vbProperties)

    End Sub

    <TestMethod>
    Public Sub BothConstructor_ToVBString_TestMethod()

        Dim properties As New Collections.Generic.List(Of IEventPropertyEntity)
        properties.Add(New MockEventProperty() With {.Name = "Email", .DataType = PropertyDataType.String, .Description = "Email address of the user"})
        properties.Add(New MockEventProperty() With {.Name = "Height (cm)", .DataType = PropertyDataType.Integer, .Description = "Recorded height in cm"})
        properties.Add(New MockEventProperty() With {.Name = "Blood/Alcohol %", .DataType = PropertyDataType.Decimal, .Description = "Recorded blood/alcohol"})


        Dim testObj As CodeTypeMember() = PropertyCodeGeneration.PrivateBackingMembers(properties)

        Dim classObj As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration("Duncan's Class Test")
        Dim propertiesHolder As New CodeCompileUnit
        Dim nsMain As New CodeNamespace("test")

        propertiesHolder.Namespaces.Add(nsMain)
        nsMain.Types.Add(classObj)



        classObj.Members.AddRange(testObj)

        For Each eventProp In properties
            classObj.Members.Add(PropertyCodeGeneration.PublicMember(eventProp))
        Next

        classObj.Members.Add(ConstructorCodeGenerator.InterfaceBasedConstructor(properties, "IDuncansClass", "DuncansClassIn"))
        classObj.Members.Add(ConstructorCodeGenerator.ParameterisedConstructor(properties))

        Dim vbProperties As String = CodeGenerationUtilities.ToVBCodeString(propertiesHolder)

        Assert.IsNotNull(vbProperties)

    End Sub


    <TestMethod>
    Public Sub SerializationConstructor_NoProperties_TestMethod()

        Dim properties As New Collections.Generic.List(Of IEventPropertyEntity)


    End Sub
End Class

Namespace Mocking

    ''' <summary>
    ''' Mock event property that does not have all the System.Modelling baggage
    ''' </summary>
    Public Class MockEventProperty
        Implements IEventPropertyEntity

        Public Property DataType As PropertyDataType Implements IEventPropertyEntity.DataType


        Public Property Description As String Implements IDocumentedEntity.Description

        Public ReadOnly Property FullyQualifiedName As String Implements INamedEntity.FullyQualifiedName
            Get
                Return Name
            End Get
        End Property

        Public Property IsEffectiveDate As Boolean Implements IEventPropertyEntity.IsEffectiveDate


        Public Property Name As String Implements INamedEntity.Name


        Public Property Notes As String Implements IDocumentedEntity.Notes

    End Class

End Namespace