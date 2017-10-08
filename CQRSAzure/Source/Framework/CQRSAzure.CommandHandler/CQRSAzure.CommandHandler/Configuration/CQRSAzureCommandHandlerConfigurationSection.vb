Imports System.Configuration

''' <summary>
''' Wrapper class for the possible configuration elements and element collections that affect how the 
''' command handler operates.
''' </summary>
Public Class CQRSAzureCommandHandlerConfigurationSection
    Inherits ConfigurationSection

    <ConfigurationProperty(NameOf(CommandHandlerMaps), IsRequired:=False)>
    <ConfigurationCollection(GetType(CQRSCommandHandlerMappingSettingsElement),
                         AddItemName:=CQRSCommandHandlerMappingSettingsElementCollection.AddItemName)>
    Public ReadOnly Property CommandHandlerMaps As CQRSCommandHandlerMappingSettingsElementCollection
        Get
            Return Me(NameOf(CommandHandlerMaps))
        End Get
    End Property

    <ConfigurationProperty(NameOf(CommandHandlerSettings), IsRequired:=False)>
    <ConfigurationCollection(GetType(CQRSCommandHandlerSettingsElement),
                             AddItemName:=CQRSCommandHandlerSettingsElementCollection.AddItemName)>
    Public ReadOnly Property CommandHandlerSettings As CQRSCommandHandlerSettingsElementCollection
        Get
            Return Me(NameOf(CommandHandlerSettings))
        End Get
    End Property

End Class
