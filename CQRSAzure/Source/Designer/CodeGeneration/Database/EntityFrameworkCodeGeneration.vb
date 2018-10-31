Imports System.CodeDom

''' <summary>
''' Class for the utility functions pertaining to generating code to be use dby entity framework (code first) to
''' create and use the data model 
''' </summary>
Public Class EntityFrameworkCodeGeneration


    ''' <summary>
    ''' Creates a DBSet member to prepresent the event stream of this aggregate
    ''' </summary>
    ''' <param name="aggregateName">
    ''' The name of the aggregate we are creatinmg a DBSet based event stream for 
    ''' </param>
    ''' <returns>
    ''' Public Property AggregateInstances As DBSet(Of AggregateInstance)
    ''' </returns>
    Public Shared Function CreateDBSetProperty(ByVal aggregateName As String, Optional ByVal IsPublic As Boolean = True) As CodeMemberProperty


        Dim ret As New CodeMemberProperty()
        ret.Name = MakeAggregateInstanceName(aggregateName, True)
        ret.Type = MakeDBSetTypeReference(aggregateName)
        If (IsPublic) Then
            ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Public
        End If


        'Add the getter to the private member
        Dim memberReference As New CodeFieldReferenceExpression()
        memberReference.FieldName = MakeAggregateInstanceName(aggregateName, True, True)

        ret.GetStatements.Add((New CodeMethodReturnStatement(memberReference)))

        Return ret

    End Function

    ''' <summary>
    ''' Create a type reference  for DBSet(Of AggregateInstance)
    ''' </summary>
    ''' <param name="aggregatename">
    ''' The name of the aggregate we are creatinmg a DBSet based event stream for 
    ''' </param>
    ''' <returns>
    ''' DBSet(Of AggregateInstance)
    ''' </returns>
    Public Shared Function MakeDBSetTypeReference(ByVal aggregatename As String) As CodeTypeReference

        Return New CodeTypeReference("DbSet", {New CodeTypeReference(MakeAggregateInstanceName(aggregatename))})

    End Function

    ''' <summary>
    ''' Add "Instance" to the aggregate name
    ''' </summary>
    ''' <param name="aggregateName">
    ''' The name of the aggregate type
    ''' </param>
    Private Shared Function MakeAggregateInstanceName(ByVal aggregateName As String,
                                                      Optional ByVal plural As Boolean = False,
                                                      Optional ByVal privateMember As Boolean = False) As String

        If (privateMember) Then
            aggregateName = "m_" & aggregateName.Trim()
        End If

        If (plural) Then
            Return ModelCodeGenerator.MakeValidCodeName(aggregateName & "s")
        Else
            Return ModelCodeGenerator.MakeValidCodeName(aggregateName)
        End If

    End Function


    Private Shared Function MakeAggregateCompileSwitchName(ByVal aggregateName As String) As String
        Return ModelCodeGenerator.MakeValidCodeName(aggregateName.ToUpper & "_DATABASE_BACKED")
    End Function

End Class
