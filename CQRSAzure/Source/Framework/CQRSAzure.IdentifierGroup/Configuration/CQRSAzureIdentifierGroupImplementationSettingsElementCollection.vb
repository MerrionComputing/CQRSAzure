Imports System.Configuration

''' <summary>
''' The set of implementation settings properties in the &lt;Implementations&gt; section
''' </summary>
<ConfigurationCollection(GetType(CQRSAzureIdentifierGroupClassifierSnapshotSettingsElement), AddItemName:="Implementation")>
Public Class CQRSAzureIdentifierGroupImplementationSettingsElementCollection
    Inherits ConfigurationElementCollection

    Public Const AddItemName As String = "Implementation"

    Protected Overrides Function CreateNewElement() As ConfigurationElement
        Return New CQRSAzureIdentifierGroupClassifierSnapshotSettingsElement()
    End Function

    Protected Overrides Function GetElementKey(element As ConfigurationElement) As Object
        Dim item As CQRSAzureIdentifierGroupClassifierSnapshotSettingsElement = element
        If (item IsNot Nothing) Then
            Return item.Name
        Else
            Throw New Exception()
        End If
    End Function


    Default Public Shadows Property Item(ByVal index As Integer) As CQRSAzureIdentifierGroupClassifierSnapshotSettingsElement
        Get
            Return CType(BaseGet(index), CQRSAzureIdentifierGroupClassifierSnapshotSettingsElement)
        End Get
        Set(ByVal value As CQRSAzureIdentifierGroupClassifierSnapshotSettingsElement)
            If BaseGet(index) IsNot Nothing Then
                BaseRemoveAt(index)
            End If
            BaseAdd(value)
        End Set
    End Property

    Default Public Shadows ReadOnly Property Item(ByVal Name As String) As CQRSAzureIdentifierGroupClassifierSnapshotSettingsElement
        Get
            Return CType(BaseGet(Name), CQRSAzureIdentifierGroupClassifierSnapshotSettingsElement)
        End Get
    End Property

    Public Function IndexOf(ByVal implementation As CQRSAzureIdentifierGroupClassifierSnapshotSettingsElement) As Integer
        Return BaseIndexOf(implementation)
    End Function

    Public Sub Implementation(ByVal implementation As CQRSAzureIdentifierGroupClassifierSnapshotSettingsElement)
        Add(implementation)
    End Sub

    Private Sub Add(ByVal implementation As CQRSAzureIdentifierGroupClassifierSnapshotSettingsElement)
        BaseAdd(implementation)

        ' Your custom code goes here.

    End Sub

    Protected Overloads Sub BaseAdd(ByVal element As ConfigurationElement)
        BaseAdd(element, False)

        ' Your custom code goes here.

    End Sub


    Public Sub Remove(ByVal implementation As CQRSAzureIdentifierGroupClassifierSnapshotSettingsElement)
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
