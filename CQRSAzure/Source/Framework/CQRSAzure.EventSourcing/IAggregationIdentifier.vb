''' <summary>
''' A marker interface to state that a class can uniquely identify an aggregation
''' </summary>
''' <remarks>
''' This is used to allow us to make the compiler enforce rules that "x must be an aggregation"
''' </remarks>
Public Interface IAggregationIdentifier


    ''' <summary>
    ''' The identifier of this aggregate, as a string
    ''' </summary>
    ''' <remarks>
    ''' This is used if the event source is backed by Azure tables to set the partition key to use for it
    ''' </remarks>
    Function GetAggregateIdentifier() As String


End Interface

''' <summary>
''' A typed interface to set the key to use to identify an aggregation
''' </summary>
''' <typeparam name="TAggregationKey">
''' The base type that uniquely identifies an individual of the aggregation
''' </typeparam>
Public Interface IAggregationIdentifier(Of TAggregationKey)
    Inherits IAggregationIdentifier

    ''' <summary>
    ''' Set the key to use to create the aggregation identifier for this type of thing
    ''' </summary>
    ''' <param name="key"></param>
    ''' <remarks></remarks>
    Sub SetKey(ByVal key As TAggregationKey)

    ''' <summary>
    ''' Get the key used to uniquely identify the aggregate
    ''' </summary>
    Function GetKey() As TAggregationKey

End Interface
