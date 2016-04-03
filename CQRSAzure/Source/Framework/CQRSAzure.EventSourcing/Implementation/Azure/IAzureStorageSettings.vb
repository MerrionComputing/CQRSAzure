Namespace Azure
    ''' <summary>
    ''' Settings specific to event stream implementations hosted on Windows Azure
    ''' </summary>
    Public Interface IAzureStorageSettings

        ''' <summary>
        ''' The name of the Azure storage connection string that the configuration will use to connect to the underlying event stream
        ''' </summary>
        Property ConnectionStringName As String

        ''' <summary>
        ''' The name of the Azure storage connection string that the configuration will use to read from to the underlying event stream
        ''' </summary>
        ''' <remarks>
        ''' If this is not set then the same connection is used for read and write
        ''' </remarks>
        Property ReadSideConnectionStringName As String

        ''' <summary>
        ''' The domain name to use when storing this 
        ''' </summary>
        ''' <returns></returns>
        Property DomainName As String

    End Interface
End Namespace