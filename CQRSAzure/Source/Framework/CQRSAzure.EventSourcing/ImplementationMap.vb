
Imports System
''' <summary>
''' Map between an aggregate and the actual implementation class for the events stream of instances of that aggregate
''' </summary>
''' <remarks>
''' This can be set on a per-aggregate-type basis or on a global basis in the application .config files
''' </remarks>
Public Module ImplementationMap

    ''' <summary>
    ''' The different backing technologies that can be used for an event stream in this library
    ''' </summary>
    Public Enum SupportedEventStreamImplementations
        ''' <summary>
        ''' Use whatever is set as the system default
        ''' </summary>
        [Default] = 0
        ''' <summary>
        ''' In-memory event stream for unit testing and proof-of-concept work
        ''' </summary>
        InMemory = 1
        ''' <summary>
        ''' Azure append-only blobs
        ''' </summary>
        ''' <remarks>
        ''' This is currentlythe most performant option
        '''</remarks>
        AzureBlob = 10
        ''' <summary>
        ''' Azure storage files
        ''' </summary>
        AzureFile = 11
        ''' <summary>
        ''' Azure hosted SQL Server database 
        ''' </summary>
        <Obsolete("Not implemented")>
        AzureSQL = 12
        ''' <summary>
        ''' Azure Table
        ''' </summary>
        AzureTable = 13
        ''' <summary>
        ''' Local machine file system
        ''' </summary>
        LocalFileSettings = 20
    End Enum

    ''' <summary>
    ''' If no implementation is specified we fall back to the default
    ''' </summary>
    Public Const DefaultImplementation As SupportedEventStreamImplementations = SupportedEventStreamImplementations.AzureBlob




End Module
