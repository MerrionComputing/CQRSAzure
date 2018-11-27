Imports System
Imports System.Configuration

''' <summary>
''' The set of mapped aggregate to implementations in the application
''' </summary>
''' <remarks>
''' These are uniquely keyed by domain qualified aggregate name
''' </remarks>
<ConfigurationCollection(GetType(CQRSAzureEventSourcingAggregateMapElement), AddItemName:="Map")>
Public Class CQRSAzureEventSourcingAggregateMapElementCollection
    Inherits ConfigurationElementCollection

    Public Const AddItemName As String = "Map"

    Protected Overrides Function CreateNewElement() As ConfigurationElement
        Return New CQRSAzureEventSourcingAggregateMapElement()
    End Function

    Protected Overrides Function GetElementKey(element As ConfigurationElement) As Object
        Dim item As CQRSAzureEventSourcingAggregateMapElement = element
        If (item IsNot Nothing) Then
            Return item.AggregateDomainQualifiedName
        Else
            Throw New Exception()
        End If
    End Function

    Default Public Shadows Property Item(ByVal index As Integer) As CQRSAzureEventSourcingAggregateMapElement
        Get
            Return CType(BaseGet(index), CQRSAzureEventSourcingAggregateMapElement)
        End Get
        Set(ByVal value As CQRSAzureEventSourcingAggregateMapElement)
            If BaseGet(index) IsNot Nothing Then
                BaseRemoveAt(index)
            End If
            BaseAdd(value)
        End Set
    End Property

    Default Public Shadows ReadOnly Property Item(ByVal Name As String) As CQRSAzureEventSourcingAggregateMapElement
        Get
            Return CType(BaseGet(Name), CQRSAzureEventSourcingAggregateMapElement)
        End Get
    End Property

    Public Function IndexOf(ByVal map As CQRSAzureEventSourcingAggregateMapElement) As Integer
        Return BaseIndexOf(map)
    End Function

    Public Sub Map(ByVal map As CQRSAzureEventSourcingAggregateMapElement)
        Add(map)
    End Sub

    Private Sub Add(ByVal map As CQRSAzureEventSourcingAggregateMapElement)
        BaseAdd(map)

        ' Your custom code goes here.

    End Sub

    Protected Overloads Sub BaseAdd(ByVal element As ConfigurationElement)
        BaseAdd(element, False)

        ' Your custom code goes here.

    End Sub


    Public Sub Remove(ByVal map As CQRSAzureEventSourcingAggregateMapElement)
        If BaseIndexOf(map) >= 0 Then
            BaseRemove(map.AggregateDomainQualifiedName)
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
