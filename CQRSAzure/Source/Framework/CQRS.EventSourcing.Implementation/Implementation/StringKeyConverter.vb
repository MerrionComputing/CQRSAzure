Imports CQRSAzure.EventSourcing

Public Class StringKeyConverter
    Implements IKeyConverter(Of String)

    Public Function FromString(value As String) As String Implements IKeyConverter(Of String).FromString
        Return value
    End Function

    Public Function ToUniqueString(value As String) As String Implements IKeyConverter(Of String).ToUniqueString
        Return value
    End Function

End Class
