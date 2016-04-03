''' <summary>
''' Interface to be implemented by all query definitions 
''' </summary>
''' <remarks>
''' This allows for a separation of concerns between the definition of the query and any parameters it requires,
''' the validation(s) of that query and the execution of the query
''' </remarks>
Public Interface IQueryDefinition(Of TResult)

    ''' <summary>
    ''' Unique identifier of this query instance
    ''' </summary>
    ''' <remarks>
    ''' This allows queries to be queued and the response to a given query definition to be identified
    ''' </remarks>
    ReadOnly Property InstanceIdentifier As Guid

    ''' <summary>
    ''' The unique name of the query being requested
    ''' </summary>
    ReadOnly Property QueryName As String

    ''' <summary>
    ''' Add a paremeter to this query
    ''' </summary>
    ''' <param name="parameter">
    ''' The parameter to add to the query
    ''' </param>
    ''' <remarks>
    ''' This will throw an argument exception if this query already has a parameter with the same name and index.  
    ''' Use TryAddParameter to avoid this exception
    ''' </remarks>
    Sub AddParameter(Of TValueType)(ByVal parameter As QueryParameter(Of TValueType))

    ''' <summary>
    ''' Add a paremeter to this query
    ''' </summary>
    ''' <param name="parameter">
    ''' The parameter to add to the query
    ''' </param>
    ''' <remarks>
    ''' This will return true if the parameter was successfully added
    ''' </remarks>
    Function TryAddParameter(Of TValueType)(ByVal parameter As QueryParameter(Of TValueType)) As Boolean

    ''' <summary>
    ''' True if this query has the parameter defined for it
    ''' </summary>
    ''' <param name="parameterName">
    ''' The name of the parameter to look for
    ''' </param>
    ''' <param name="parameterIndex">
    ''' The zero-based index of the parameter
    ''' </param>
    Function ParameterExists(ByVal parameterName As String, ByVal parameterIndex As Integer) As Boolean

    ''' <summary>
    ''' Get the parameter value for the named parameter
    ''' </summary>
    ''' <param name="parameterName">
    ''' The name of the parameter to look for
    ''' </param>
    ''' <param name="parameterIndex">
    ''' The zero-based index of the parameter
    ''' </param>
    ''' <remarks>
    ''' This will throw an error if the parameter does not exists.  Use TryGetParameter instead if that is not desired
    ''' </remarks>
    Function GetParameterValue(Of TValueType)(ByVal parameterName As String, ByVal parameterIndex As Integer) As TValueType

    ''' <summary>
    ''' Get the parameter value for the named parameter
    ''' </summary>
    ''' <param name="parameterName">
    ''' The name of the parameter to look for
    ''' </param>
    ''' <param name="parameterIndex">
    ''' The zero-based index of the parameter
    ''' </param>
    ''' <param name="value">
    ''' The variable to hold the parameter value if found
    ''' </param>
    ''' <remarks>
    ''' This will return true if the parameter was retrieved
    ''' </remarks>
    Function TryGetParameter(Of TValueType)(ByVal parameterName As String, ByVal parameterIndex As Integer, ByRef value As TValueType) As Boolean

    ''' <summary>
    ''' Set the value of the indicated parameter
    ''' </summary>
    ''' <typeparam name="TValueType">
    ''' The type of the parameter we are setting
    ''' </typeparam>
    ''' <param name="parameterName">
    ''' The name of the parameter to look for
    ''' </param>
    ''' <param name="parameterIndex">
    ''' The zero-based index of the parameter
    ''' </param>
    ''' <param name="value">
    ''' The value to assign to the parameter
    ''' </param>
    ''' <remarks>
    ''' if the parameter doesn't exist, add it, otherwise update it
    ''' </remarks>
    Sub SetParameterValue(Of TValueType)(ByVal parameterName As String, ByVal parameterIndex As Integer, ByRef value As TValueType)


End Interface