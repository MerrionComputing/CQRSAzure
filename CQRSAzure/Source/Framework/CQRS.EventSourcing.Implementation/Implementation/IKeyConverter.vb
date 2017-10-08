''' <summary>
''' Because some storage mechanisms cannot use the full set of CLR types for their key we need a way to 
''' convert them to/from a string so as to store that
''' </summary>
''' <typeparam name="TAggregateKey"></typeparam>
Public Interface IKeyConverter(Of TAggregateKey)


    ''' <summary>
    ''' Convert a string to the given key type
    ''' </summary>
    ''' <param name="value">
    ''' The value stored as a string
    ''' </param>
    Function FromString(ByVal value As String) As TAggregateKey

    ''' <summary>
    ''' Convert the value to an unique string that corresponds to that value
    ''' </summary>
    ''' <param name="value">
    ''' The key value in its data type
    ''' </param>
    Function ToUniqueString(ByVal value As TAggregateKey) As String

End Interface
