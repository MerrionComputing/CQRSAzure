Namespace Azure.Table

    ''' <summary>
    ''' Additional settings required for using an Azure table as backing store for an event stream
    ''' </summary>
    Public Interface ITableSettings
        Inherits Azure.IAzureStorageSettings

        ''' <summary>
        ''' If set, use this format string to turn the sequence number into a row key of the record
        ''' </summary>
        ''' <returns>
        ''' Default is "0000000000000000000" 
        ''' </returns>
        Property SequenceNumberFormat As String

        ''' <summary>
        ''' Should the generated table name include the domain name
        ''' </summary>
        Property IncludeDomainInTableName As Boolean

        ''' <summary>
        ''' If set, use this format string to turn the row number into a row key of the snapshot record
        ''' </summary>
        ''' <returns>
        ''' Default is "0000000000000000000" 
        ''' </returns>
        Property RowNumberFormat As String

    End Interface
End Namespace