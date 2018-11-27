Imports System
Imports System.Configuration

<ConfigurationCollection(GetType(CQRSQueryHandlerMappingElement), AddItemName:="QueryHandlerMapping")>
Public Class CQRSQueryHandlerMappingElementCollection
    Inherits ConfigurationElementCollection

    Public Const AddItemName As String = "QueryHandlerMapping"

    Protected Overrides Function CreateNewElement() As ConfigurationElement
        Return New CQRSQueryHandlerMappingElement()
    End Function

    ''' <summary>
    ''' The command definition is the unique key
    ''' </summary>
    ''' <param name="element">
    ''' The full configuration element for which to retireve the key
    ''' </param>
    ''' <returns></returns>
    Protected Overrides Function GetElementKey(element As ConfigurationElement) As Object
        Dim item As CQRSQueryHandlerMappingElement = element
        If (item IsNot Nothing) Then
            Return item.DefinitionName
        Else
            Throw New Exception()
        End If
    End Function

    Default Public Shadows Property Item(ByVal index As Integer) As CQRSQueryHandlerMappingElement
        Get
            Return CType(BaseGet(index), CQRSQueryHandlerMappingElement)
        End Get
        Set(ByVal value As CQRSQueryHandlerMappingElement)
            If BaseGet(index) IsNot Nothing Then
                BaseRemoveAt(index)
            End If
            BaseAdd(value)
        End Set
    End Property

    Default Public Shadows ReadOnly Property Item(ByVal Name As String) As CQRSQueryHandlerMappingElement
        Get
            Return CType(BaseGet(Name), CQRSQueryHandlerMappingElement)
        End Get
    End Property

    Public Function IndexOf(ByVal map As CQRSQueryHandlerMappingElement) As Integer
        Return BaseIndexOf(map)
    End Function

    Public Sub CommandHandlerMapping(ByVal handlerMap As CQRSQueryHandlerMappingElement)
        Add(handlerMap)
    End Sub

    Private Sub Add(ByVal map As CQRSQueryHandlerMappingElement)
        BaseAdd(map)

        ' Your custom code goes here.

    End Sub

    Protected Overloads Sub BaseAdd(ByVal element As ConfigurationElement)
        BaseAdd(element, False)

        ' Your custom code goes here.

    End Sub
End Class
