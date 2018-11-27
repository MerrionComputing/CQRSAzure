Imports System
Imports CQRSAzure.EventSourcing

Public Class GUIDKeyConverter
    Implements IKeyConverter(Of Guid)

    Public Function FromString(value As String) As Guid Implements IKeyConverter(Of Guid).FromString
        Return Guid.Parse(value)
    End Function

    Public Function ToUniqueString(value As Guid) As String Implements IKeyConverter(Of Guid).ToUniqueString
        Return value.ToString("B") ' use braces and dashes to be unambiguously not a number
    End Function
End Class
