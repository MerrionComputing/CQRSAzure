Imports System.Configuration

''' <summary>
''' The set of hosted domains known to a CQRS Azure host
''' </summary>
<ConfigurationCollection(GetType(CQRSAzureHostedDomainElement), AddItemName:="HostedDomain")>
Public Class CQRSAzureHostedDomainElementCollection
    Inherits ConfigurationElementCollection

    Public Const AddItemName As String = "HostedDomain"


    Protected Overrides Function CreateNewElement() As ConfigurationElement
        Return New CQRSAzureHostedDomainElement()
    End Function

    Protected Overrides Function GetElementKey(element As ConfigurationElement) As Object
        Dim item As CQRSAzureHostedDomainElement = element
        If (item IsNot Nothing) Then
            Return item.Name
        Else
            Throw New Exception()
        End If
    End Function

    Default Public Shadows Property Item(ByVal index As Integer) As CQRSAzureHostedDomainElement
        Get
            Return CType(BaseGet(index), CQRSAzureHostedDomainElement)
        End Get
        Set(ByVal value As CQRSAzureHostedDomainElement)
            If BaseGet(index) IsNot Nothing Then
                BaseRemoveAt(index)
            End If
            BaseAdd(value)
        End Set
    End Property

    Default Public Shadows ReadOnly Property Item(ByVal Name As String) As CQRSAzureHostedDomainElement
        Get
            Return CType(BaseGet(Name), CQRSAzureHostedDomainElement)
        End Get
    End Property

    Public Function IndexOf(ByVal map As CQRSAzureHostedDomainElement) As Integer
        Return BaseIndexOf(map)
    End Function

    Public Sub Map(ByVal map As CQRSAzureHostedDomainElement)
        Add(map)
    End Sub

    Private Sub Add(ByVal map As CQRSAzureHostedDomainElement)
        BaseAdd(map)

        ' Your custom code goes here.

    End Sub

    Protected Overloads Sub BaseAdd(ByVal element As ConfigurationElement)
        BaseAdd(element, False)

        ' Your custom code goes here.

    End Sub


    Public Sub Remove(ByVal map As CQRSAzureHostedDomainElement)
        If BaseIndexOf(map) >= 0 Then
            BaseRemove(map.Name)
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
