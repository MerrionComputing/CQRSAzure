Imports System.Configuration

''' <summary>
''' Wrapper class for the possible configuration elements and element collections that affect how this 
''' query handler operates.
''' </summary>
Public Class CQRSQueryHandlerConfigurationSection
    Inherits ConfigurationSection

    <ConfigurationProperty(NameOf(QueryHandlerMaps), IsRequired:=False)>
    <ConfigurationCollection(GetType(CQRSQueryHandlerMappingElement),
                         AddItemName:=CQRSQueryHandlerMappingElementCollection.AddItemName)>
    Public ReadOnly Property QueryHandlerMaps As CQRSQueryHandlerMappingElementCollection
        Get
            Return Me(NameOf(QueryHandlerMaps))
        End Get
    End Property

    <ConfigurationProperty(NameOf(QueryHandlerSettings), IsRequired:=False)>
    <ConfigurationCollection(GetType(CQRSQueryHandlerSettingsElement),
                             AddItemName:=CQRSQueryHandlerSettingsElementCollection.AddItemName)>
    Public ReadOnly Property QueryHandlerSettings As CQRSQueryHandlerSettingsElementCollection
        Get
            Return Me(NameOf(QueryHandlerSettings))
        End Get
    End Property

End Class
