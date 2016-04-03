Imports CQRSAzure.QueryDefinition

''' <summary>
''' Base class for any specific query handling class
''' </summary>
''' <typeparam name="TQueryDefinition"></typeparam>
''' <typeparam name="TResultType"></typeparam>
''' <remarks>
''' This is to hold common cross-cutting functionality that applies to all query handlers (such as routing, error notifications, logging etc.)
''' </remarks>
Public MustInherit Class QueryHandlerBase(Of TQueryDefinition As IQueryDefinition(Of TResultType), TResultType)
    Implements IQueryHandler(Of TQueryDefinition, TResultType)

    ''' <summary>
    ''' Handle the specific query instance and return the results
    ''' </summary>
    ''' <param name="qryToHandle">
    ''' The specific instance of the query and its input parameters
    ''' </param>
    ''' <returns>
    ''' A populated data type as required by the query definition
    ''' </returns>
    Public MustOverride Function HandleQuery(ByVal qryToHandle As TQueryDefinition) As TResultType Implements IQueryHandler(Of TQueryDefinition, TResultType).HandleQuery

End Class
