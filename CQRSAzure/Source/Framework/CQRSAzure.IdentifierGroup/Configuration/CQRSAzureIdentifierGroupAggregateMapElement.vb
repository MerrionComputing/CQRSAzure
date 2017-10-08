Imports System.Configuration

''' <summary>
''' Maps an aggregation class to the identifier group persistence implementation
''' </summary>
Public Class CQRSAzureIdentifierGroupAggregateMapElement
    Inherits ConfigurationElement

    Public Const DEFAULT_MAPPING_NAME As String = "Default"

    ''' <summary>
    ''' The domain-qualified name of the aggregate class that uses the given implementation 
    ''' for it's event stream backing store
    ''' </summary>
    ''' <remarks>
    ''' This must be unqiue within the CQRSAzureIdentifierGroupAggregateMapElementCollection
    ''' </remarks>
    <ConfigurationProperty(NameOf(AggregateDomainQualifiedName), IsKey:=True, IsRequired:=True)>
    Public Property AggregateDomainQualifiedName As String
        Get
            Return Me(NameOf(AggregateDomainQualifiedName))
        End Get
        Set(value As String)
            Me(NameOf(AggregateDomainQualifiedName)) = value
        End Set
    End Property

    ''' <summary>
    ''' The name of the implementation to use - this allows different configuration properties per identifier group
    ''' </summary>
    ''' <remarks>
    ''' The name relates to the XXXXSettingsElement identified by the name
    ''' </remarks>
    <ConfigurationProperty(NameOf(ImplementationName), DefaultValue:="Default")>
    Public Property ImplementationName As String
        Get
            Return Me(NameOf(ImplementationName))
        End Get
        Set(value As String)
            Me(NameOf(ImplementationName)) = value
        End Set
    End Property

    ''' <summary>
    ''' The name of the snapshot settings to use - this allows different configuration properties per identifier group
    ''' </summary>
    ''' <remarks>
    ''' The name relates to the XXXXSnapshotSettingElement identified by the name
    ''' </remarks>
    <ConfigurationProperty(NameOf(SnapshotSettingsName), DefaultValue:="Default")>
    Public Property SnapshotSettingsName As String
        Get
            Return Me(NameOf(SnapshotSettingsName))
        End Get
        Set(value As String)
            Me(NameOf(SnapshotSettingsName)) = value
        End Set
    End Property

End Class
