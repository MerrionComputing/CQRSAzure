Imports CQRSAzure.EventSourcing

Public Class DoubleKeyConverter
    Implements IKeyConverter(Of Double)

    Public Function FromString(value As String) As Double Implements IKeyConverter(Of Double).FromString
        Return Double.Parse(value)
    End Function

    Public Function ToUniqueString(value As Double) As String Implements IKeyConverter(Of Double).ToUniqueString
        Return value.ToString("G17") 'Use the CPU-safe round-trip specifier
    End Function

End Class

Public Class DoubleNullableKeyConverter
    Implements IKeyConverter(Of Nullable(Of Double))

    Public Function FromString(value As String) As Double? Implements IKeyConverter(Of Double?).FromString

        If (String.IsNullOrWhiteSpace(value)) Then
            Return Nothing
        Else
            Return Double.Parse(value)
        End If

    End Function

    Public Function ToUniqueString(value As Double?) As String Implements IKeyConverter(Of Double?).ToUniqueString

        If (Not value.HasValue) Then
            Return ""
        Else
            Return value.Value.ToString("G17")
        End If

    End Function
End Class
