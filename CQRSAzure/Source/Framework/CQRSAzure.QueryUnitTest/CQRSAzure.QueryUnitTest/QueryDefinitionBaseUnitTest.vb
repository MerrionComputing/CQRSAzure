Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports CQRSAzure.QueryDefinition

<TestClass()>
Public Class QueryDefinitionBaseUnitTest

    <TestMethod()>
    Public Sub Constructor_String_TestMethod()

        Dim testObj As New MockQueryDefinitionClass_String()
        Assert.IsNotNull(testObj)

    End Sub

    <TestMethod()>
    Public Sub AddParameterOfString_RoundTrip_TestMethod()

        Dim expected As String = "Expected"
        Dim actual As String = "Actual"

        Dim testObj As New MockQueryDefinitionClass_String()
        testObj.MyParameter = expected
        actual = testObj.MyParameter

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub AddParameterOfInteger_RoundTrip_TestMethod()

        Dim expected As Integer = 112
        Dim actual As Integer = 789

        Dim testObj As New MockQueryDefinitionClass_String()
        testObj.MyNumber = expected
        actual = testObj.MyNumber

        Assert.AreEqual(expected, actual)

    End Sub


    <TestMethod()>
    Public Sub AddParameterOfDecimalRoundTrip_TestMethod()

        Dim expected As Decimal = 112.98
        Dim actual As Decimal = 789.09

        Dim testObj As New MockQueryDefinitionClass_String()
        testObj.MyDecimal = expected
        actual = testObj.MyDecimal

        Assert.AreEqual(expected, actual)

    End Sub

    <TestMethod()>
    Public Sub ProjectionNameRoundTrip_TestMethod()

        Dim expected As String = "My projection"
        Dim actual As String = "Not set"

        Dim testObj As New MockQueryDefinitionClass_String()
        testObj.SetSelectedProjection(expected)
        actual = testObj.ProjectionName

        Assert.AreEqual(expected, actual)

    End Sub


#Region "Mocking"

    ''' <summary>
    ''' Mock class that inherits from QueryDefinition(Of String)
    ''' </summary>
    Public Class MockQueryDefinitionClass_String
        Inherits QueryDefinitionBase(Of String)


        Public Overrides ReadOnly Property QueryName As String
            Get
                Return "Mock Query Definition (Of String)"
            End Get
        End Property


        Public Property MyParameter As String
            Get
                Return MyBase.GetParameterValue(Of String)("My parameter", 0)
            End Get
            Set(value As String)
                MyBase.SetParameterValue(Of String)("My parameter", 0, value)
            End Set
        End Property

        Public Property MySecondParameter As String
            Get
                Return MyBase.GetParameterValue(Of String)("My parameter", 1)
            End Get
            Set(value As String)
                MyBase.SetParameterValue(Of String)("My parameter", 1, value)
            End Set
        End Property

        Public Property MyNumber As Integer
            Get
                Return MyBase.GetParameterValue(Of Integer)("My parameter", 2)
            End Get
            Set(value As Integer)
                MyBase.SetParameterValue(Of Integer)("My parameter", 2, value)
            End Set
        End Property

        Public Property MyDecimal As Decimal
            Get
                Return MyBase.GetParameterValue(Of Decimal)("My parameter", 3)
            End Get
            Set(value As Decimal)
                MyBase.SetParameterValue(Of Decimal)("My parameter", 3, value)
            End Set
        End Property

        Friend Sub SetSelectedProjection(expected As String)
            MyBase.SetParameterValue(Of String)(MyBase.PARAMETER_NAME_PROJECTION, 0, expected)
        End Sub

        Friend Sub SetSelectedIdentityGroup(expected As String)
            MyBase.SetParameterValue(Of String)(MyBase.PARAMETER_NAME_IDENTITY_GROUP, 0, expected)
        End Sub
    End Class

#End Region

End Class