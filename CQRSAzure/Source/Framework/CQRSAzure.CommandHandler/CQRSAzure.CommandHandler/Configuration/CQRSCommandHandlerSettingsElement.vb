Imports System.Configuration
Imports CQRSAzure.Hosting.Configuration

Public Class CQRSCommandHandlerSettingsElement
    Inherits ConfigurationElement
    Implements IHostCommandHandlerSettings

    ''' <summary>
    ''' Unique name by which the handler is known
    ''' </summary>
    ''' <remarks>
    ''' If this is not set then the address and port are used.  
    ''' Ideally a human readable unique name should be used to aid debugging/logging
    ''' </remarks>
    <ConfigurationProperty(NameOf(Name), IsKey:=True, IsRequired:=True)>
    Public Property Name As String Implements IHostHandlerSettings.Name
        Get
            Return Me(NameOf(Name))
        End Get
        Set(value As String)
            Me(NameOf(Name)) = value
        End Set
    End Property




End Class
