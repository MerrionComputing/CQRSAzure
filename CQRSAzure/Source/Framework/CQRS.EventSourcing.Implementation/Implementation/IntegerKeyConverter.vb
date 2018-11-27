Imports System
Imports CQRSAzure.EventSourcing

Public Class IntegerKeyConverter
    Implements IKeyConverter(Of Integer)

    Public Function FromString(value As String) As Integer Implements IKeyConverter(Of Integer).FromString
        Return Integer.Parse(value)
    End Function

    Public Function ToUniqueString(value As Integer) As String Implements IKeyConverter(Of Integer).ToUniqueString
        Return value.ToString()
    End Function

End Class

Public Class IntegerNullableKeyConverter
    Implements IKeyConverter(Of Nullable(Of Integer))

    Public Function FromString(value As String) As Integer? Implements IKeyConverter(Of Integer?).FromString

        If (String.IsNullOrWhiteSpace(value)) Then
            Return Nothing
        Else
            Return Integer.Parse(value)
        End If

    End Function

    Public Function ToUniqueString(value As Integer?) As String Implements IKeyConverter(Of Integer?).ToUniqueString

        If (value.HasValue) Then
            Return value.Value.ToString()
        Else
            Return ""
        End If

    End Function
End Class