Imports System.Text
Imports NUnit.Framework

<TestFixture()>
Public Class AggregateName_UnitTest

    <TestCase()>
    Public Sub NurseAggregate_TestMethod()

        Dim expected As String = "Nurse"
        Dim actual As String = "Not set"

        actual = AggregateNameAttribute.GetAggregateName(GetType(Mocking.NurseAggregate))

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub PatientAggregate_TestMethod()

        Dim expected As String = "Patient"
        Dim actual As String = "Not set"

        actual = AggregateNameAttribute.GetAggregateName(GetType(Mocking.PatientAggregate))

        Assert.AreEqual(expected, actual)

    End Sub

    <TestCase()>
    Public Sub PatientAggregate_DomainQualifiedName_TestMethod()

        Dim expected As String = "HospitalWard.Patient"
        Dim actual As String = "Not set"

        actual = DomainNameAttribute.GetAggregateDomainQualifiedName(GetType(Mocking.PatientAggregate))

        Assert.AreEqual(expected, actual)

    End Sub

End Class