Namespace Projections

    ''' <summary>
    ''' The identifier by which a single unique instance of a projection is known
    ''' </summary>
    ''' <typeparam name="TAggregateIdentifier">
    ''' The data type that is used to uniquely identify the event stream instance over which the projection is run
    ''' </typeparam>
    ''' <remarks>
    ''' This is used to identify the event stream underlying the projection
    ''' </remarks>
    Public Interface IProjectionInstanceIdentifier(Of TAggregateIdentifier)
        Inherits IProjectionInstanceIdentifier

        ''' <summary>
        ''' The unique identifier of the aggregate over whose event stream the projection will run
        ''' </summary>
        ReadOnly Property AggregateIdentifier As TAggregateIdentifier



    End Interface

    Public Interface IProjectionInstanceIdentifier
        Inherits IAggregationIdentifier

        ''' <summary>
        ''' The name of the domain (per DDD) containing this projection
        ''' </summary>
        ReadOnly Property DomainName As String

        ''' <summary>
        ''' The unique name of this projection (unique within the domain)
        ''' </summary>
        ReadOnly Property ProjectionName As String

    End Interface
End Namespace