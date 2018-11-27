Imports System
''' <summary>
''' Identifies a class as being a command definition that can be handled by the CommandHandler linked to it to perform
''' an action
''' </summary>
''' <remarks>
''' Each specific definition will have one matching handler
''' </remarks>
Public Interface ICommandDefinition

    ''' <summary>
    ''' Unique identifier of this command instance
    ''' </summary>
    ReadOnly Property InstanceIdentifier As Guid

    ''' <summary>
    ''' The name of the command being performed
    ''' </summary>
    ''' <remarks>
    ''' This should be unique in any given domain so that it can be used as part of an audit trail prcoess
    ''' </remarks>
    ReadOnly Property Name As String

    ''' <summary>
    ''' Add a paremeter to this command
    ''' </summary>
    ''' <param name="parameter">
    ''' The parameter to add to the command
    ''' </param>
    ''' <remarks>
    ''' This will throw an argument exception if this command already has a parameter with the same name and index.  
    ''' Use TryAddParameter to avoid this exception
    ''' </remarks>
    Sub AddParameter(Of TValueType)(ByVal parameter As CommandParameter(Of TValueType))

    ''' <summary>
    ''' Add a paremeter to this command
    ''' </summary>
    ''' <param name="parameter">
    ''' The parameter to add to the command
    ''' </param>
    ''' <remarks>
    ''' This will return true if the parameter was successfully added
    ''' </remarks>
    Function TryAddParameter(Of TValueType)(ByVal parameter As CommandParameter(Of TValueType)) As Boolean

    ''' <summary>
    ''' Get the specified parameter for this command
    ''' </summary>
    ''' <typeparam name="TParam">
    ''' The underlying data type of the parameter value
    ''' </typeparam>
    ''' <param name="ParameterName">
    ''' The name of the parameter to get
    ''' </param>
    ''' <param name="ParameterIndex">
    ''' The index of the parameter (if it is an array/list - otherwise this will be zero)
    ''' </param>
    ''' <returns>
    ''' The specified parameter (if it has been set)
    ''' </returns>
    ''' <remarks>
    ''' If this parameter does not exist then an exception is thrown.  A TryGet variant and a ParameterExists function exist and should be used in place of
    ''' this function as exception handling is an expensive process to be avoided in normal processing
    ''' </remarks>
    Function GetParameterValue(Of TParam)(ByVal ParameterName As String, ByVal ParameterIndex As Integer) As TParam

    ''' <summary>
    ''' Sets the specified parameter for this command to the gven value
    ''' </summary>
    ''' <typeparam name="TValueType">
    ''' The udnerlying data type of the parameter being set
    ''' </typeparam>
    ''' <param name="parameterName">
    ''' The name of the parameter being set
    ''' </param>
    ''' <param name="parameterIndex">
    ''' Zero-vased index of the parameter value (will always be zero if this is not an array)
    ''' </param>
    ''' <param name="value">
    ''' The specific value being set 
    ''' </param>
    Sub SetParameterValue(Of TValueType)(parameterName As String, parameterIndex As Integer, ByRef value As TValueType)

    ''' <summary>
    ''' Get the specified parameter for this command
    ''' </summary>
    ''' <typeparam name="TParam">
    ''' The underlying data type of the parameter value
    ''' </typeparam>
    ''' <param name="ParameterName">
    ''' The name of the parameter to get
    ''' </param>
    ''' <param name="ParameterIndex">
    ''' The index of the parameter (if it is an array/list - otherwise this will be zero)
    ''' </param>
    ''' <param name="Value">
    ''' The value currently set for this command parameter
    ''' </param>
    ''' <returns>
    ''' True if the parameter value exists
    ''' </returns>
    Function TryGetParameterValue(Of TParam)(ByVal ParameterName As String, ByVal ParameterIndex As Integer, ByRef Value As TParam) As Boolean

    ''' <summary>
    ''' Does the specified parameter exist for this command
    ''' </summary>
    ''' <param name="ParameterName">
    ''' The name of the parameter
    ''' </param>
    ''' <param name="ParameterIndex">
    ''' The index of the parameter (if it is an array) or 0 for non indexed parameters
    ''' </param>
    ''' <returns>
    ''' True if the parameter exists for this command
    ''' </returns>
    Function ParameterExists(ByVal ParameterName As String, ByVal ParameterIndex As Integer) As Boolean

End Interface
