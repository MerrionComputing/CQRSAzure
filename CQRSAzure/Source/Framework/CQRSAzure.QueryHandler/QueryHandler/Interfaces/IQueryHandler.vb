Imports CQRSAzure.QueryDefinition

''' <summary>
''' Base interface for any class which handles a specific query and returns the required data
''' </summary>
''' <typeparam name="TQueryDefinition">
''' The type that defines the query
''' </typeparam>
''' <typeparam name="TResultType">
''' The type that defines the data returned from the query
''' </typeparam>
Public Interface IQueryHandler(Of TQueryDefinition As IQueryDefinition(Of TResultType), TResultType)

    ''' <summary>
    ''' Perform the function for the underlying query and return the required results
    ''' </summary>
    ''' <param name="qryToHandle">
    ''' The definition of the query (including input parameters)
    ''' </param>
    ''' <returns>
    ''' A populated result set per the query definition
    ''' </returns>
    Function HandleQuery(ByVal qryToHandle As TQueryDefinition) As TResultType

End Interface
