Imports System.Configuration

''' <summary>
''' Configuration settings controlling how projection snapshots are persisted to a backing storage mechanism
''' </summary>
''' <remarks>
''' The default (if not specified) is for the projection snapshot to use the same backing storage settings as
''' the event stream over which the projection is run
''' </remarks>
<ConfigurationCollection(GetType(CQRSAzureEventSourcingProjectionSnapshotSettingsElement), AddItemName:="SnapshotSetting")>
Public Class CQRSAzureEventSourcingProjectionSnapshotSettingsElementCollection
    Inherits ConfigurationElementCollection

    Public Const AddItemName As String = "SnapshotSetting"

    Protected Overrides Function CreateNewElement() As ConfigurationElement
        Return New CQRSAzureEventSourcingProjectionSnapshotSettingsElement()
    End Function

    Protected Overrides Function GetElementKey(element As ConfigurationElement) As Object
        Dim item As CQRSAzureEventSourcingProjectionSnapshotSettingsElement = element
        If (item IsNot Nothing) Then
            Return item.Name
        Else
            Throw New Exception()
        End If
    End Function

    Default Public Shadows Property Item(ByVal index As Integer) As CQRSAzureEventSourcingProjectionSnapshotSettingsElement
        Get
            Return CType(BaseGet(index), CQRSAzureEventSourcingProjectionSnapshotSettingsElement)
        End Get
        Set(ByVal value As CQRSAzureEventSourcingProjectionSnapshotSettingsElement)
            If BaseGet(index) IsNot Nothing Then
                BaseRemoveAt(index)
            End If
            BaseAdd(value)
        End Set
    End Property

    Public Sub SnapshotSetting(ByVal setting As CQRSAzureEventSourcingProjectionSnapshotSettingsElement)
        Add(setting)
    End Sub

    Private Sub Add(ByVal implementation As CQRSAzureEventSourcingProjectionSnapshotSettingsElement)
        BaseAdd(implementation)

        ' Your custom code goes here.

    End Sub

    Protected Overloads Sub BaseAdd(ByVal element As ConfigurationElement)
        BaseAdd(element, False)

        ' Your custom code goes here.

    End Sub


    Public Sub Remove(ByVal implementation As CQRSAzureEventSourcingProjectionSnapshotSettingsElement)
        If BaseIndexOf(implementation) >= 0 Then
            BaseRemove(implementation.Name)
            ' Your custom code goes here.
        End If
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        BaseRemoveAt(index)
        ' Your custom code goes here.
    End Sub

    Public Sub Remove(ByVal name As String)
        BaseRemove(name)
        ' Your custom code goes here.
    End Sub

    Public Sub Clear()
        BaseClear()
        ' Your custom code goes here.
    End Sub

End Class
