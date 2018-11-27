Imports System.Runtime.Serialization
''' <summary>
''' A single parameter that is used to restrict the results returned for a given query definition
''' </summary>
''' <remarks>
''' This is an immutable class to allow for safe parallel/asynchronous processing
''' </remarks>
''' <typeparam name="TValue" >
''' The data type of the value held for this parameter
''' </typeparam>
<DataContract>
Public NotInheritable Class QueryParameter(Of TValue)
    Implements IQueryParameter

    <DataMember(Name:="ParameterName")>
    ReadOnly m_name As String
    <DataMember(Name:="ParameterIndex")>
    ReadOnly m_index As Integer
    <DataMember(Name:="ParameterValue")>
    Private m_value As TValue

    ''' <summary>
    ''' The name of the parameter
    ''' </summary>
    ''' <remarks>
    ''' This should be unique in any given query definition, unless there are multuiple indexed properties with the same name
    ''' </remarks>
    Public ReadOnly Property Name As String Implements IQueryParameter.Name
        Get
            Return m_name
        End Get
    End Property

    ''' <summary>
    ''' The index (zero based) of the parameter
    ''' </summary>
    ''' <remarks>
    ''' For a non-indexed parameter, this will always be zero
    ''' </remarks>
    Public ReadOnly Property Index As Integer Implements IQueryParameter.Index
        Get
            Return m_index
        End Get
    End Property

    ''' <summary>
    ''' The value of the parameter
    ''' </summary>
    Public ReadOnly Property Value As TValue
        Get
            Return m_value
        End Get
    End Property

    Private ReadOnly Property ValueAsObject As Object Implements IQueryParameter.Value
        Get
            Return Value
        End Get
    End Property

    Public Sub SetValue(value As Object) Implements IQueryParameter.SetValue
        m_value = CType(value, TValue)
    End Sub

    ''' <summary>
    ''' Creatre a new parameter instance with the given properties
    ''' </summary>
    ''' <param name="nameInit">
    ''' The name of the parameter
    ''' </param>
    ''' <param name="indexInit">
    ''' The zero-based index of the parameter
    ''' </param>
    ''' <param name="valInit">
    ''' The starting value of the parameter - this can be Nothing (null) to indicate that the parameter is not set
    ''' </param>
    ''' <remarks></remarks>
    Private Sub New(ByVal nameInit As String, ByVal indexInit As Integer, ByVal valInit As TValue)
        m_name = nameInit
        m_index = indexInit
        m_value = valInit
    End Sub

    ''' <summary>
    ''' Create a new parameter for the given properties
    ''' </summary>
    ''' <param name="name">
    ''' The name of the parameter
    ''' </param>
    ''' <param name="index">
    ''' The zero-based index of the parameter
    ''' </param>
    ''' <param name="value">
    ''' The value to use for this parameter
    ''' </param>
    Public Shared Function Create(ByVal name As String, ByVal index As Integer, ByVal value As TValue) As QueryParameter(Of TValue)
        Return New QueryParameter(Of TValue)(name, index, value)
    End Function

    ''' <summary>
    ''' Get the unique key of the given parameter as a string
    ''' </summary>
    ''' <param name="parameter">
    ''' The parameter for which to get the key
    ''' </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetParameterKey(ByVal parameter As IQueryParameter) As String
        Return GetParameterKey(parameter.Name, parameter.Index)
    End Function

    Public Shared Function GetParameterKey(ByVal parameterName As String, ByVal parameterIndex As Integer) As String
        Return parameterName & " [" & parameterIndex.ToString() & "]"
    End Function


End Class
