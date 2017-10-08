Imports System.Configuration

''' <summary>
''' Wrapper class for the possible configuration elements and element collections that affect how the 
''' identifier group part of the event sourcing library operates.
''' </summary>
Public Class CQRSAzureIdentifierGroupConfigurationSection
    Inherits ConfigurationSection

    <ConfigurationProperty(NameOf(ImplementationMaps), IsRequired:=False)>
    <ConfigurationCollection(GetType(CQRSAzureIdentifierGroupAggregateMapElement),
                         AddItemName:=CQRSAzureIdentifierGroupAggregateMapElementCollection.AddItemName)>
    Public ReadOnly Property ImplementationMaps As CQRSAzureIdentifierGroupAggregateMapElementCollection
        Get
            Return Me(NameOf(ImplementationMaps))
        End Get
    End Property

    <ConfigurationProperty(NameOf(ClassifierSnapshotSettings), IsRequired:=False)>
    <ConfigurationCollection(GetType(CQRSAzureIdentifierGroupClassifierSnapshotSettingsElement),
                             AddItemName:=CQRSAzureIdentifierGroupImplementationSettingsElementCollection.AddItemName)>
    Public ReadOnly Property ClassifierSnapshotSettings As CQRSAzureIdentifierGroupImplementationSettingsElementCollection
        Get
            Return Me(NameOf(ClassifierSnapshotSettings))
        End Get
    End Property

    <ConfigurationProperty(NameOf(SnapshotSettings), IsRequired:=False)>
    <ConfigurationCollection(GetType(CQRSAzureIdentifierGroupSnapshotSettingsElement),
                             AddItemName:=CQRSAzureIdentifierGroupSnapshotSettingsElementCollection.AddItemName)>
    Public ReadOnly Property SnapshotSettings As CQRSAzureIdentifierGroupSnapshotSettingsElementCollection
        Get
            Return Me(NameOf(SnapshotSettings))
        End Get
    End Property

    ''' <summary>
    ''' An empty configuration that can be used as a default if Null/Nothing is not allowed
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function DefaultConfiguration() As CQRSAzureIdentifierGroupConfigurationSection
        Return New CQRSAzureIdentifierGroupConfigurationSection()
    End Function
End Class