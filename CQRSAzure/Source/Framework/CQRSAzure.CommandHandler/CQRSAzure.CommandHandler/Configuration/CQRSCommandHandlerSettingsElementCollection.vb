Imports System.Configuration

<ConfigurationCollection(GetType(CQRSCommandHandlerSettingsElement), AddItemName:="CommandHandler")>
Public Class CQRSCommandHandlerSettingsElementCollection
    Inherits ConfigurationElementCollection

    Public Const AddItemName As String = "CommandHandler"

    Protected Overrides Function CreateNewElement() As ConfigurationElement
        Return New CQRSCommandHandlerSettingsElement()
    End Function

    Protected Overrides Function GetElementKey(element As ConfigurationElement) As Object
        Dim item As CQRSCommandHandlerSettingsElement = element
        If (item IsNot Nothing) Then
            Return item.Name
        Else
            Throw New Exception()
        End If
    End Function

    Default Public Shadows Property Item(ByVal index As Integer) As CQRSCommandHandlerSettingsElement
        Get
            Return CType(BaseGet(index), CQRSCommandHandlerSettingsElement)
        End Get
        Set(ByVal value As CQRSCommandHandlerSettingsElement)
            If BaseGet(index) IsNot Nothing Then
                BaseRemoveAt(index)
            End If
            BaseAdd(value)
        End Set
    End Property

    Default Public Shadows ReadOnly Property Item(ByVal Name As String) As CQRSCommandHandlerSettingsElement
        Get
            Return CType(BaseGet(Name), CQRSCommandHandlerSettingsElement)
        End Get
    End Property

    Public Function IndexOf(ByVal map As CQRSCommandHandlerSettingsElement) As Integer
        Return BaseIndexOf(map)
    End Function

    Public Sub CommandHandler(ByVal handler As CQRSCommandHandlerSettingsElement)
        Add(handler)
    End Sub

    Private Sub Add(ByVal map As CQRSCommandHandlerSettingsElement)
        BaseAdd(map)

        ' Your custom code goes here.

    End Sub

    Protected Overloads Sub BaseAdd(ByVal element As ConfigurationElement)
        BaseAdd(element, False)

        ' Your custom code goes here.

    End Sub
End Class
