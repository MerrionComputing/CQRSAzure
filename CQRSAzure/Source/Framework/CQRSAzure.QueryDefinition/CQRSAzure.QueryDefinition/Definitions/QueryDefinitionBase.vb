''' <summary>
''' Base class from which the query definitions should be build
''' </summary>
''' <remarks>
''' To avoid a fragile base class do not add any functionality to this class that is not related to the attaching of
''' parameters to a query
''' </remarks>
Public MustInherit Class QueryDefinitionBase(Of TResult)
    Implements IQueryDefinition(Of TResult)

    Public Const PARAMETER_NAME_IDENTITY_GROUP As String = "CQRSAZURE.IDENTITY_GROUP_NAME"
    Public Const PARAMETER_NAME_PROJECTION As String = "CQRSAZURE.PROJECTION_NAME"

    Private m_instanceId As Guid = Guid.NewGuid()
    Private m_parameters As Dictionary(Of String, IQueryParameter)
    Private m_multiline As Boolean = False


    ''' <summary>
    ''' The unique name for the query 
    ''' </summary>
    ''' <remarks>
    ''' This must be provided by the implementing class as each query type should have an unique name
    ''' </remarks>
    Public MustOverride ReadOnly Property QueryName As String Implements IQueryDefinition(Of TResult).QueryName

    ''' <summary>
    ''' Unique identifier of the query instance
    ''' </summary>
    ''' <remarks>
    ''' Thsi allows us to identify which query to respond to if there are multiple concurrent queries of the same type
    ''' </remarks>
    Public ReadOnly Property InstanceIdentifier As Guid Implements IQueryDefinition(Of TResult).InstanceIdentifier
        Get
            Return m_instanceId
        End Get
    End Property

    ''' <summary>
    ''' The name of the identity group over which this query should be executed
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IdentityGroupName As String Implements IQueryDefinition.IdentityGroupName
        Get
            If ParameterExists(PARAMETER_NAME_IDENTITY_GROUP, 0) Then
                Return GetParameterValue(Of String)(PARAMETER_NAME_IDENTITY_GROUP, 0)
            End If

            Return "" ' No identity group is specified for this query intsance to use
        End Get
    End Property

    Public ReadOnly Property ProjectionName As String Implements IQueryDefinition.ProjectionName
        Get
            If ParameterExists(PARAMETER_NAME_PROJECTION, 0) Then
                Return GetParameterValue(Of String)(PARAMETER_NAME_PROJECTION, 0)
            End If

            Return "" ' No projection is specified for this query intsance to use
        End Get
    End Property

    Public ReadOnly Property MultiRowResults As Boolean Implements IQueryDefinition.MultiRowResults
        Get
            Return m_multiline
        End Get
    End Property



    Protected Sub AddParameter(Of TValueType)(parameter As QueryParameter(Of TValueType)) Implements IQueryDefinition(Of TResult).AddParameter
        If (String.IsNullOrWhiteSpace(parameter.Name)) Then
            Throw New ArgumentException("Query parameter must have a valid name", "Name")
        End If

        If (m_parameters Is Nothing) Then
            m_parameters = New Dictionary(Of String, IQueryParameter)
        End If


        Dim key As String = MakeKey(parameter)

        If (m_parameters.ContainsKey(key)) Then
            Throw New ArgumentException("Query parameter with this name and index already exists", "Name")
        Else
            m_parameters.Add(key, parameter)
        End If
    End Sub

    Protected Function TryAddParameter(Of TValueType)(parameter As QueryParameter(Of TValueType)) As Boolean Implements IQueryDefinition(Of TResult).TryAddParameter

        If (ParameterExists(parameter.Name, parameter.Index)) Then
            Return False
        Else
            AddParameter(Of TValueType)(parameter)
            Return True
        End If

    End Function

    Protected Function GetParameterValue(Of TValueType)(parameterName As String, parameterIndex As Integer) As TValueType Implements IQueryDefinition(Of TResult).GetParameterValue

        If (ParameterExists(parameterName, parameterIndex)) Then
            Return CTypeDynamic(Of TValueType)(m_parameters(MakeKey(parameterName, parameterIndex)).Value)
        Else
            Throw New ArgumentException("Query parameter with this name and index does not exist", "Name")
        End If

    End Function


    Public Function ParameterExists(parameterName As String, parameterIndex As Integer) As Boolean Implements IQueryDefinition(Of TResult).ParameterExists

        If (m_parameters IsNot Nothing) Then
            Return m_parameters.ContainsKey(MakeKey(parameterName, parameterIndex))
        Else
            Return False
        End If

    End Function

    Protected Function TryGetParameter(Of TValueType)(parameterName As String, parameterIndex As Integer, ByRef value As TValueType) As Boolean Implements IQueryDefinition(Of TResult).TryGetParameter

        If (ParameterExists(parameterName, parameterIndex)) Then
            value = CTypeDynamic(Of TValueType)(m_parameters(MakeKey(parameterName, parameterIndex)).Value)
            Return True
        Else
            Return False
        End If

    End Function

    Protected Sub SetParameterValue(Of TValueType)(parameterName As String, parameterIndex As Integer, ByRef value As TValueType) Implements IQueryDefinition(Of TResult).SetParameterValue

        If (ParameterExists(parameterName, parameterIndex)) Then
            'set the value
            m_parameters(MakeKey(parameterName, parameterIndex)).SetValue(value)
        Else
            'add the parameter
            AddParameter(Of TValueType)(QueryParameter(Of TValueType).Create(parameterName, parameterIndex, value))
        End If

    End Sub


    Public Function ParametersString() As String
        Dim sbRet As System.Text.StringBuilder = New System.Text.StringBuilder
        sbRet.Append("[")
        For Each qp As IQueryParameter In m_parameters.Values
            sbRet.AppendLine()
            sbRet.Append(MakeKey(qp))
            sbRet.Append(" = ")
            sbRet.Append(qp.Value.ToString)
        Next
        sbRet.Append("]")
        Return sbRet.ToString
    End Function

    Private Shared Function MakeKey(ByVal parameter As IQueryParameter) As String

        Return MakeKey(parameter.Name, parameter.Index)

    End Function

    Private Shared Function MakeKey(ByVal parameterName As String, ByVal parameterIndex As Integer) As String

        Return parameterName & "[" & parameterIndex.ToString & "]"

    End Function

End Class
