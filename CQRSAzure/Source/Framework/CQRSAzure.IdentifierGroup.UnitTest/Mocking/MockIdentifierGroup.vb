Imports CQRSAzure.IdentifierGroup
Imports CQRSAzure.IdentifierGroup.UnitTest

Public Class MockIdentifierGroup
    Inherits IdentityGroupBase(Of MockAggregate, String)


    Private ReadOnly m_classifier As IClassifier(Of MockAggregate, String)
    Public Overrides ReadOnly Property Classifier As IClassifier(Of MockAggregate, String)
        Get
            Return m_classifier
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Mock Identifier Group"
        End Get
    End Property


    Public Overrides Function GetMembers(Optional AsOfDate As Date = #1/1/0001 12:00:00 AM#) As IEnumerable(Of MockAggregate)

        If (m_classifier IsNot Nothing) Then
            'get all the members of the parent group
            'classify them
            'return those that are classified as in-group
        End If

    End Function

End Class
