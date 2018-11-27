Imports System
Imports CQRSAzure.QueryDefinition

''' <summary>
''' Factory methods for spinning-up instances of query handlers for given query definitions
''' </summary>
Module QueryHandlerFactory


    ''' <summary>
    ''' Perform the start-up initialisations neccessary to link query handlers up to their definitions
    ''' </summary>
    ''' <remarks>
    ''' These may be declared in a config file or may be discovered by reflection as desired
    ''' </remarks>
    Public Sub Initialise()



    End Sub

    ''' <summary>
    ''' Create a query handler to handle the specified query definition 
    ''' </summary>
    ''' <typeparam name="TQueryDefinition">
    ''' The type that defines the query inputs 
    ''' </typeparam>
    ''' <typeparam name="TResultType">
    ''' The type that defines the query expected returns
    ''' </typeparam>
    ''' <param name="queryInstance">
    ''' A specific instance of the query inputs
    ''' </param>
    ''' <returns>
    ''' An instance of a query handler class that can handle this query definition and return results
    ''' </returns>
    Public Function Create(Of TQueryDefinition As IQueryDefinition(Of TResultType), TResultType)(ByVal queryInstance As TQueryDefinition) As IQueryHandler(Of TQueryDefinition, TResultType)

        Throw New NotImplementedException("Query handler factory not implemented")

    End Function

End Module
