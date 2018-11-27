
Imports CQRSAzure.CommandDefinition
Imports NUnit.Framework

<TestFixture()>
Public Class CommandhandlerbaseUnitTest

    <TestCase()>
    Public Sub Constructor_TestMethod()

        Dim testObj As New MockCommandDefinitionClass()
        Assert.IsNotNull(testObj)

    End Sub


    <TestCase()>
    Public Sub AddParameterOfString_RoundTrip_TestMethod()

        Dim expected As String = "Expected"
        Dim actual As String = "Actual"

        Dim testObj As New MockCommandDefinitionClass()
        testObj.MyParameter = expected
        actual = testObj.MyParameter

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub AddParameterOfInteger_RoundTrip_TestMethod()

        Dim expected As Integer = 112
        Dim actual As Integer = 789

        Dim testObj As New MockCommandDefinitionClass()
        testObj.MyNumber = expected
        actual = testObj.MyNumber

        Assert.AreEqual(expected, actual)

    End Sub


    <TestCase()>
    Public Sub AddParameterOfDecimalRoundTrip_TestMethod()

        Dim expected As Decimal = 112.98
        Dim actual As Decimal = 789.09

        Dim testObj As New MockCommandDefinitionClass()
        testObj.MyDecimal = expected
        actual = testObj.MyDecimal

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub GetBeforeSet_ThrowsError_TestMethod()


        Assert.Throws(Of ArgumentException)(Sub()
                                                Dim expected As String = "Expected"
                                                Dim actual As String = "Actual"

                                                Dim testObj As New MockCommandDefinitionClass()
                                                'testObj.MyParameter = expected ..if the parameter is not set it should throw a not found...
                                                actual = testObj.MyParameter

                                                Assert.IsNotNull(testObj)

                                            End Sub
            )
    End Sub


    <TestCase()>
    Public Sub MockCommandHandlerOne_Constructor_TestMethod()

        Dim testObj As New MockCommandHandlerOne()
        Assert.IsNotNull(testObj)

    End Sub


    <TestCase()>
    Public Sub MockCommandhandler_HandleEvent_UnitTest()

        Dim expected As String = "Duncan Jones"
        Dim actual As String = "Not set"

        Dim testHandler As New MockCommandHandlerOne()
        Dim testDefinition As New MockCommandDefinitionOne() With {.FirstNamesParameter = "Duncan", .SurnameParameter = "Jones"}
        testHandler.HandleCommand(testDefinition)

        actual = testHandler.MostRecentName

        Assert.AreEqual(expected, actual)

    End Sub

#Region "Mocking"



    Public Class MockCommandDefinitionClass
        Inherits CommandDefinitionBase

        Public Overrides ReadOnly Property CommandName As String
            Get
                Return "Unit testing command"
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

    End Class

#End Region
End Class