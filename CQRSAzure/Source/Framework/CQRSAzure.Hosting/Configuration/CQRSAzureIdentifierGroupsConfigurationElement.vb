Imports System.Configuration

''' <summary>
''' Configuration settings for where a domain's identifier groups come from
''' </summary>
Public Class CQRSAzureIdentifierGroupsConfigurationElement
    Inherits ConfigurationElement

    ''' <summary>
    ''' The full name of the assembly that contains the identifier groups definitions for this domain
    ''' </summary>
    <ConfigurationProperty(NameOf(FullName), IsRequired:=True)>
    Public Property FullName As String
        Get
            Return Me(NameOf(FullName))
        End Get
        Set(value As String)
            Me(NameOf(FullName)) = value
        End Set
    End Property

End Class
