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

        'If nothing is found, return an empty list
        Return Enumerable.Empty(Of MockAggregate)

    End Function

End Class

''' <summary>
''' An identifier group that works off the untyped (JSON based) event stream stored in AppendBlob
''' or Azure table storage
''' </summary>
''' <remarks>
''' This is derived from the [ALL] group 
''' </remarks>
Public Class MockIdentifierGroupUntyped
    Inherits IdentityGroupBaseUntyped

    Private ReadOnly m_classifierProcessor As IClassifierProcessorUntyped



    Public Overrides ReadOnly Property Name As String
        Get
            Return "Mock Identifier Group"
        End Get
    End Property

    Public Overrides Function GetMembers(Optional AsOfDate As Date = #1/1/0001 12:00:00 AM#) As IEnumerable(Of String)

        If (m_classifierProcessor IsNot Nothing) Then

        End If

        Throw New NotImplementedException()



    End Function


End Class
