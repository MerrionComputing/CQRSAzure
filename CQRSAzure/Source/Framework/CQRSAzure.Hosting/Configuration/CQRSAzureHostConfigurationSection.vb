Imports System.Configuration

''' <summary>
''' Top level configuration settings for a CQRS Azure host
''' </summary>
Public Class CQRSAzureHostConfigurationSection
    Inherits ConfigurationSection

    ''' <summary>
    ''' Unique name of the host
    ''' </summary>
    ''' <remarks>
    ''' This is conceptually similar to a server name in SQL server 
    ''' </remarks>
    <ConfigurationProperty(NameOf(Name), IsKey:=True, IsRequired:=True)>
    Public Property Name As String
        Get
            Return Me(NameOf(Name))
        End Get
        Set(value As String)
            Me(NameOf(Name)) = value
        End Set
    End Property

    ''' <summary>
    ''' Should this host load all its known domains on startup?
    ''' </summary>
    <ConfigurationProperty(NameOf(LoadDomainsOnStartup), IsKey:=False, IsRequired:=False, DefaultValue:=False)>
    Public Property LoadDomainsOnStartup As Boolean
        Get
            Return Me(NameOf(LoadDomainsOnStartup))
        End Get
        Set(value As Boolean)
            Me(NameOf(LoadDomainsOnStartup)) = value
        End Set
    End Property

    <ConfigurationProperty(NameOf(HostedDomains), IsRequired:=False)>
    <ConfigurationCollection(GetType(CQRSAzureHostedDomainElement),
                         AddItemName:=CQRSAzureHostedDomainElementCollection.AddItemName)>
    Public ReadOnly Property HostedDomains As CQRSAzureHostedDomainElementCollection
        Get
            Return Me(NameOf(HostedDomains))
        End Get
    End Property

End Class
