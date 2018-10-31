Imports CQRSAzure.CommandDefinition
''' <summary>
''' Base class from which the command definitions should be build
''' </summary>
''' <remarks>
''' To avoid a fragile base class do not add any functionality to this class that is not related to the attaching of
''' parameters to a command
''' </remarks>
Public MustInherit Class CommandDefinitionBase
    Implements ICommandDefinition


    Private m_instanceId As Guid = Guid.NewGuid()
    Private m_parameters As Dictionary(Of String, ICommandParameter)

    ''' <summary>
    ''' The unique name for the command 
    ''' </summary>
    ''' <remarks>
    ''' This must be provided by the implementing class as each command should have an unique name
    ''' </remarks>
    Public MustOverride ReadOnly Property CommandName As String Implements ICommandDefinition.Name

    Public ReadOnly Property InstanceIdentifier As Guid Implements ICommandDefinition.InstanceIdentifier
        Get
            Return m_instanceId
        End Get
    End Property


    Protected Sub AddParameter(Of TValueType)(parameter As CommandParameter(Of TValueType)) Implements ICommandDefinition.AddParameter

        If (String.IsNullOrWhiteSpace(parameter.Name)) Then
            Throw New ArgumentException("Command parameter must have a valid name", "Name")
        End If

        If (m_parameters Is Nothing) Then
            m_parameters = New Dictionary(Of String, ICommandParameter)
        End If


        Dim key As String = MakeKey(parameter)

        If (m_parameters.ContainsKey(key)) Then
            Throw New ArgumentException("Command parameter with this name and index already exists", "Name")
        Else
            m_parameters.Add(key, parameter)
        End If

    End Sub

    Protected Function TryAddParameter(Of TValueType)(parameter As CommandParameter(Of TValueType)) As Boolean Implements ICommandDefinition.TryAddParameter

        If (ParameterExists(parameter.Name, parameter.Index)) Then
            Return False
        Else
            AddParameter(Of TValueType)(parameter)
            Return True
        End If

    End Function

    Public Function GetParameterValue(Of TParam)(ParameterName As String, ParameterIndex As Integer) As TParam Implements ICommandDefinition.GetParameterValue

        If (ParameterExists(ParameterName, ParameterIndex)) Then
            Return CType((m_parameters(MakeKey(ParameterName, ParameterIndex)).Value), TParam)
        Else
            Throw New ArgumentException("Command parameter with this name and index does not exist", "Name")
        End If

    End Function

    Public Function ParameterExists(ParameterName As String, ParameterIndex As Integer) As Boolean Implements ICommandDefinition.ParameterExists

        If (m_parameters IsNot Nothing) Then
            Return m_parameters.ContainsKey(MakeKey(ParameterName, ParameterIndex))
        Else
            Return False
        End If

    End Function

    Public Function TryGetParameterValue(Of TParam)(ParameterName As String, ParameterIndex As Integer, ByRef Value As TParam) As Boolean Implements ICommandDefinition.TryGetParameterValue

        If (ParameterExists(ParameterName, ParameterIndex)) Then
            Value = CType((m_parameters(MakeKey(ParameterName, ParameterIndex)).Value), TParam)
            Return True
        Else
            Return False
        End If

    End Function

    Public Function ParametersString() As String
        Dim sbRet As System.Text.StringBuilder = New System.Text.StringBuilder
        sbRet.Append("[")
        For Each cp As ICommandParameter In m_parameters.Values
            sbRet.AppendLine()
            sbRet.Append(MakeKey(cp.Name, cp.Index))
            sbRet.Append(" = ")
            sbRet.Append(cp.Value.ToString)
        Next
        sbRet.Append("]")
        Return sbRet.ToString
    End Function

    Private Shared Function MakeKey(ByVal parameter As ICommandParameter) As String

        Return MakeKey(parameter.Name, parameter.Index)

    End Function

    Private Shared Function MakeKey(ByVal parameterName As String, ByVal parameterIndex As Integer) As String

        Return parameterName & "[" & parameterIndex.ToString & "]"

    End Function

    Public Sub SetParameterValue(Of TValueType)(parameterName As String, parameterIndex As Integer, ByRef value As TValueType) Implements ICommandDefinition.SetParameterValue

        If (ParameterExists(parameterName, parameterIndex)) Then
            'set the value
            m_parameters(MakeKey(parameterName, parameterIndex)).SetValue(value)
        Else
            'add the parameter
            AddParameter(Of TValueType)(CommandParameter(Of TValueType).Create(Of TValueType)(parameterName, parameterIndex, value))
        End If

    End Sub

End Class
