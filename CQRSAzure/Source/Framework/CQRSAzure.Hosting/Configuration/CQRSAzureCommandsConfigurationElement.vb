Imports System.Configuration

''' <summary>
''' Configuration settings for where a domain's commands come from
''' </summary>
Public Class CQRSAzureCommandsConfigurationElement
    Inherits ConfigurationElement

    <ConfigurationProperty(NameOf(Definitions), IsRequired:=False)>
    Public Property Definitions As CQRSAzureDefinitionsConfigurationElement
        Get
            Return Me(NameOf(Definitions))
        End Get
        Set(value As CQRSAzureDefinitionsConfigurationElement)
            Me(NameOf(Definitions)) = value
        End Set
    End Property

    <ConfigurationProperty(NameOf(Handlers), IsRequired:=False)>
    Public Property Handlers As CQRSAzureHandlersConfigurationElement
        Get
            Return Me(NameOf(Handlers))
        End Get
        Set(value As CQRSAzureHandlersConfigurationElement)
            Me(NameOf(Handlers)) = value
        End Set
    End Property

End Class
