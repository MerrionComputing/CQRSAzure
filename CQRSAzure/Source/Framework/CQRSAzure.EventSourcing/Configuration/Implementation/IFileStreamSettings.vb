Namespace Azure.File

    ''' <summary>
    ''' Settings for using an Azure file as the backing store for an event stream
    ''' </summary>
    Public Interface IFileStreamSettings
        Inherits Azure.IAzureStorageSettings

        ''' <summary>
        ''' The initialsize to allocate for the file when creating it
        ''' </summary>
        Property InitialSize As Long

    End Interface
End Namespace