Imports System.Configuration

''' <summary>
''' Configuration setting used to map a command definition type to the handler type that handles it
''' </summary>
''' <remarks>
''' This allows handlers to be changed without rebuilding an application - for example to swap in a debug version 
''' </remarks>
Public Class CQRSCommandHandlerMappingSettingsElement
    Inherits ConfigurationElement
    Implements ICommandHandlerMapping

    ''' <summary>
    ''' Unique fully qualified class name of the command definition class
    ''' </summary>
    <ConfigurationProperty(NameOf(DefinitionName), IsKey:=True, IsRequired:=True)>
    Public Property DefinitionName As String Implements ICommandHandlerMapping.DefinitionName
        Get
            Return Me(NameOf(DefinitionName))
        End Get
        Set(value As String)
            Me(NameOf(DefinitionName)) = value
        End Set
    End Property

    ''' <summary>
    ''' Fully qualified class name of the command handler class
    ''' </summary>
    <ConfigurationProperty(NameOf(HandlerName), IsRequired:=True)>
    Public Property HandlerName As String Implements ICommandHandlerMapping.HandlerName
        Get
            Return Me(NameOf(HandlerName))
        End Get
        Set(value As String)
            Me(NameOf(HandlerName)) = value
        End Set
    End Property

End Class
