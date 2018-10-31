Namespace Queries

    ''' <summary>
    ''' A new query instance was created
    ''' </summary>
    ''' <remarks>
    ''' The initial parameters passed to the query are attached at this point
    ''' </remarks>
    Public Interface IQueryCreatedEvent
        Inherits IEvent(Of IQueryAggregateIdentifier)

        ''' <summary>
        ''' The unique identifier given to the query to identify it
        ''' </summary>
        ReadOnly Property QueryUniqueIdentifier As Guid

        ''' <summary>
        ''' The name of the type of query issued
        ''' </summary>
        ReadOnly Property QueryName As String

        ''' <summary>
        ''' The date/time the query was created
        ''' </summary>
        ReadOnly Property CreationDate As Nullable(Of DateTime)

        ''' <summary>
        ''' Where was the query request sent from
        ''' </summary>
        ReadOnly Property Source As String

        ''' <summary>
        ''' The user who requested a query
        ''' </summary>
        ReadOnly Property Username As String

        ''' <summary>
        ''' The target identity group over which the query is to be run
        ''' </summary>
        ''' <remarks>
        ''' This may be blank if the query type only applies to one identity group or if the identity group 
        ''' information is passed as parameters
        ''' </remarks>
        ReadOnly Property IdentityGroupName As String

        ''' <summary>
        ''' The additional Query Parameters for the query 
        ''' </summary>
        ''' <remarks>
        ''' This could be the raw parameters (e.g. in JSON) or a URI to a location containing the parameters - 
        ''' depends on implementation.
        ''' </remarks>
        ReadOnly Property QueryParameters As String

    End Interface

End Namespace
