Imports System
Imports System.Configuration
Imports CQRSAzure.Hosting

<ConfigurationCollection(GetType(CQRSQueryHandlerSettingsElement), AddItemName:="QueryHandler")>
Public Class CQRSQueryHandlerSettingsElementCollection
    Inherits ConfigurationElementCollection

    Public Const AddItemName As String = "QueryHandler"

    Protected Overrides Function CreateNewElement() As ConfigurationElement
        Return New CQRSQueryHandlerSettingsElement()
    End Function

    Protected Overrides Function GetElementKey(element As ConfigurationElement) As Object
        Dim item As CQRSQueryHandlerSettingsElement = element
        If (item IsNot Nothing) Then
            Return item.Name
        Else
            Throw New Exception()
        End If
    End Function

    Default Public Shadows Property Item(ByVal index As Integer) As CQRSQueryHandlerSettingsElement
        Get
            Return CType(BaseGet(index), CQRSQueryHandlerSettingsElement)
        End Get
        Set(ByVal value As CQRSQueryHandlerSettingsElement)
            If BaseGet(index) IsNot Nothing Then
                BaseRemoveAt(index)
            End If
            BaseAdd(value)
        End Set
    End Property

    Default Public Shadows ReadOnly Property Item(ByVal Name As String) As CQRSQueryHandlerSettingsElement
        Get
            Return CType(BaseGet(Name), CQRSQueryHandlerSettingsElement)
        End Get
    End Property

    Public Function IndexOf(ByVal map As CQRSQueryHandlerSettingsElement) As Integer
        Return BaseIndexOf(map)
    End Function

    Public Sub CommandHandler(ByVal handler As CQRSQueryHandlerSettingsElement)
        Add(handler)
    End Sub

    Private Sub Add(ByVal map As CQRSQueryHandlerSettingsElement)
        BaseAdd(map)
        ' Your custom code goes here.
    End Sub

    Protected Overloads Sub BaseAdd(ByVal element As ConfigurationElement)
        BaseAdd(element, False)
        ' Your custom code goes here.
    End Sub
End Class
