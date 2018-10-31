''' <summary>
''' A single CQRS domain model hosted by this host
''' </summary>
''' <remarks>
''' A single host may host multiple domains (in a conceptually similar manner to a single 
''' database server might host multiple databases) but equally a domain may be hosted by 
''' more than one host instance so as to allow for scaling across machines 
''' </remarks>
Public Interface IHostedDomainModel

    ''' <summary>
    ''' The unique name fo the domain model
    ''' </summary>
    ReadOnly Property Name As String

    ''' <summary>
    ''' The namespace to load the model's event definitions from
    ''' </summary>
    ''' <remarks>
    ''' This is the event definitions and projections
    ''' </remarks>
    ReadOnly Property EventSourceingNamespace As String

    ''' <summary>
    ''' The namespace to load the identifier group implementations from
    ''' </summary>
    ReadOnly Property IdentifierGroupNamespace As String

    ''' <summary>
    ''' The namespace to load the command definitions for this model from
    ''' </summary>
    ReadOnly Property CommandDefinitionNamespace As String

    ''' <summary>
    ''' The namespace containing the command handlers that handle the commands for this domain
    ''' </summary>
    ReadOnly Property CommandHandlerNamespace As String

    ''' <summary>
    ''' The namespace containing the query definitions for this domain model
    ''' </summary>
    ReadOnly Property QueryDefinitionNamespace As String

    ''' <summary>
    ''' The namespace containing the query handlers for this domain model
    ''' </summary>
    ReadOnly Property QueryHandlerNamespace As String



End Interface
