Imports System.Configuration

''' <summary>
''' Base class for all event store implementations that sit on top of a 
''' </summary>
Public MustInherit Class CQRSAzureEventSourcingAzureStorageSettingsBase
    Inherits ConfigurationElement
    Implements Azure.IAzureStorageSettings

    ''' <summary>
    ''' The name of the Azure storage connection string that the configuration will use to connect to the underlying event stream
    ''' </summary>
    <ConfigurationProperty(NameOf(ConnectionStringName), IsKey:=False, IsRequired:=True)>
    Public Property ConnectionStringName As String Implements Azure.IAzureStorageSettings.ConnectionStringName
        Get
            Return Me(NameOf(ConnectionStringName))
        End Get
        Set(value As String)
            Me(NameOf(ConnectionStringName)) = value
        End Set
    End Property

    ''' <summary>
    ''' The name of the Azure storage connection string that the configuration will use to read from to the underlying event stream
    ''' </summary>
    ''' <remarks>
    ''' If this is not set then the same connection is used for read and write
    ''' </remarks>
    <ConfigurationProperty(NameOf(ReadSideConnectionStringName), IsKey:=False, IsRequired:=False)>
    Public Property ReadSideConnectionStringName As String Implements Azure.IAzureStorageSettings.ReadSideConnectionStringName
        Get
            Return Me(NameOf(ReadSideConnectionStringName))
        End Get
        Set(value As String)
            Me(NameOf(ReadSideConnectionStringName)) = value
        End Set
    End Property

    ''' <summary>
    ''' The domain name to use when storing / readin gthe events of this aggregate
    ''' </summary>
    ''' <remarks>
    ''' If this is not set the aggregate may be tagged with a domain attribute
    ''' </remarks>
    <ConfigurationProperty(NameOf(DomainName), IsKey:=False, IsRequired:=False)>
    Public Property DomainName As String Implements Azure.IAzureStorageSettings.DomainName
        Get
            Return Me(NameOf(DomainName))
        End Get
        Set(value As String)
            Me(NameOf(DomainName)) = value
        End Set
    End Property
End Class
