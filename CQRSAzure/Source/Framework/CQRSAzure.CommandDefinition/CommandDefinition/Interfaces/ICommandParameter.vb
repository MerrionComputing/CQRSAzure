''' <summary>
''' Identifies a single parameter that is used to affect how (and on what) a command operates
''' </summary>
Public Interface ICommandParameter

    ''' <summary>
    ''' The name of the parameter
    ''' </summary>
    ''' <remarks>
    ''' This should be unique in any given query definition, unless there are multuiple indexed properties with the same name
    ''' </remarks>
    ReadOnly Property Name As String

    ''' <summary>
    ''' The index (zero based) of the parameter
    ''' </summary>
    ''' <remarks>
    ''' For a non-indexed parameter, this wuill always be zero
    ''' </remarks>
    ReadOnly Property Index As Integer

    ''' <summary>
    ''' The value attached to the command parameter
    ''' </summary>
    ''' <returns>
    ''' </returns>
    ReadOnly Property Value As Object

    ''' <summary>
    ''' Set the actual value part of the command parameter
    ''' </summary>
    ''' <param name="value">
    ''' The value to be set
    ''' </param>
    Sub SetValue(ByVal value As Object)

End Interface
