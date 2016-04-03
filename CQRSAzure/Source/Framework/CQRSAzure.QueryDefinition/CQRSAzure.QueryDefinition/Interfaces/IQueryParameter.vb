''' <summary>
''' identifies a single parameter that is used to restrict the results returned for a given query definition
''' </summary>
Public Interface IQueryParameter

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
    ''' For a non-indexed parameter, this will always be zero
    ''' </remarks>
    ReadOnly Property Index As Integer

    ''' <summary>
    ''' The value attached to the query parameter
    ''' </summary>
    ''' <returns>
    ''' </returns>
    ReadOnly Property Value As Object

    Sub SetValue(ByVal value As Object)

End Interface
