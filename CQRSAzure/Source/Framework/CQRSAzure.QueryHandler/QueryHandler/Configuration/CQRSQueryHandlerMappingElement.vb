Imports System.Configuration

Public Class CQRSQueryHandlerMappingElement
    Inherits ConfigurationElement
    Implements IQueryHandlerMapping

    ''' <summary>
    ''' Unique fully qualified class name of the query definition class
    ''' </summary>
    <ConfigurationProperty(NameOf(DefinitionName), IsKey:=True, IsRequired:=True)>
    Public Property DefinitionName As String Implements IQueryHandlerMapping.DefinitionName
        Get
            Return Me(NameOf(DefinitionName))
        End Get
        Set(value As String)
            Me(NameOf(DefinitionName)) = value
        End Set
    End Property

    ''' <summary>
    ''' Fully qualified class name of the query handler class
    ''' </summary>
    <ConfigurationProperty(NameOf(HandlerName), IsRequired:=True)>
    Public Property HandlerName As String Implements IQueryHandlerMapping.HandlerName
        Get
            Return Me(NameOf(HandlerName))
        End Get
        Set(value As String)
            Me(NameOf(HandlerName)) = value
        End Set
    End Property
End Class
