Imports System.Configuration

''' <summary>
''' The set of configuration setting used to map a command definition type to the handler type that handles it
''' </summary>
<ConfigurationCollection(GetType(CQRSCommandHandlerMappingSettingsElement), AddItemName:="CommandHandlerMapping")>
Public Class CQRSCommandHandlerMappingSettingsElementCollection
    Inherits ConfigurationElementCollection

    Public Const AddItemName As String = "CommandHandlerMapping"

    Protected Overrides Function CreateNewElement() As ConfigurationElement
        Return New CQRSCommandHandlerMappingSettingsElement()
    End Function

    ''' <summary>
    ''' The command definition is the unique key
    ''' </summary>
    ''' <param name="element">
    ''' The full configuration element for which to retireve the key
    ''' </param>
    ''' <returns></returns>
    Protected Overrides Function GetElementKey(element As ConfigurationElement) As Object
        Dim item As CQRSCommandHandlerMappingSettingsElement = element
        If (item IsNot Nothing) Then
            Return item.DefinitionName
        Else
            Throw New Exception()
        End If
    End Function

    Default Public Shadows Property Item(ByVal index As Integer) As CQRSCommandHandlerMappingSettingsElement
        Get
            Return CType(BaseGet(index), CQRSCommandHandlerMappingSettingsElement)
        End Get
        Set(ByVal value As CQRSCommandHandlerMappingSettingsElement)
            If BaseGet(index) IsNot Nothing Then
                BaseRemoveAt(index)
            End If
            BaseAdd(value)
        End Set
    End Property

    Default Public Shadows ReadOnly Property Item(ByVal Name As String) As CQRSCommandHandlerMappingSettingsElement
        Get
            Return CType(BaseGet(Name), CQRSCommandHandlerMappingSettingsElement)
        End Get
    End Property

    Public Function IndexOf(ByVal map As CQRSCommandHandlerMappingSettingsElement) As Integer
        Return BaseIndexOf(map)
    End Function

    Public Sub CommandHandlerMapping(ByVal handlerMap As CQRSCommandHandlerMappingSettingsElement)
        Add(handlerMap)
    End Sub

    Private Sub Add(ByVal map As CQRSCommandHandlerMappingSettingsElement)
        BaseAdd(map)

        ' Your custom code goes here.

    End Sub

    Protected Overloads Sub BaseAdd(ByVal element As ConfigurationElement)
        BaseAdd(element, False)

        ' Your custom code goes here.

    End Sub

End Class
