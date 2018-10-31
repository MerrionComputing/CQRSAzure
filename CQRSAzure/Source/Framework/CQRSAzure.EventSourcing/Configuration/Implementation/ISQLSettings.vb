Namespace Azure.SQL

    ''' <summary>
    ''' Configuration settings for running an event stream over an Azure SQL database
    ''' </summary>
    Public Interface ISQLSettings
        Inherits Azure.IAzureStorageSettings

        ''' <summary>
        ''' What is the field name of the unique identifier of the aggregate in the table
        ''' </summary>
        Property AggregateIdentifierField As String

        ''' <summary>
        ''' What is the name of the field holding the sequence number
        ''' </summary>
        Property SequenceField As String

        ''' <summary>
        ''' What is the name of the field holding the event type
        ''' </summary>
        Property EventTypeField As String

        ''' <summary>
        ''' What field holds the event version number
        ''' </summary>
        Property EventVersionField As String

        ''' <summary>
        ''' What to use as the name of the base table for the event stream
        ''' </summary>
        ''' <remarks>
        ''' if this is blank then an name made from the aggregate name + Events] is used
        ''' </remarks>
        Property EventStreamTableName As String

    End Interface
End Namespace