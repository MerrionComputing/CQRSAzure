Imports CQRSAzure.EventSourcing

Namespace Commands

    ''' <summary>
    ''' A command was created (or issued) by a person or system external to the domain boundary
    ''' </summary>
    ''' <remarks>
    ''' This event provides all the context about the command
    ''' </remarks>
    Public Interface ICommandCreatedEvent
        Inherits IEvent(Of ICommandAggregateIdentifier)

        ''' <summary>
        ''' The unique identifier given to the command to identify it
        ''' </summary>
        ReadOnly Property CommandUniqueIdentifier As Guid

        ''' <summary>
        ''' The name of the type of command issued
        ''' </summary>
        ReadOnly Property CommandName As String

        ''' <summary>
        ''' The date/time the command was created
        ''' </summary>
        ReadOnly Property CreationDate As Nullable(Of DateTime)

        ''' <summary>
        ''' The name of the identity group over which to apply the command
        ''' </summary>
        ''' <remarks>
        ''' This may be blank if the command does not apply to any named identity group
        ''' </remarks>
        ReadOnly Property IdentityGroupName As String

        ''' <summary>
        ''' The URL (or other system lookup identifier maybe?) to use to retrieve the parameters passed in to this command
        ''' </summary>
        ReadOnly Property CommandParameters As String

    End Interface
End Namespace