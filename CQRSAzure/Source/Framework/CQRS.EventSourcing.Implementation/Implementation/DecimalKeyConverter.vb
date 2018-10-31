Imports CQRSAzure.EventSourcing

Public Class DecimalKeyConverter
    Implements IKeyConverter(Of Decimal)

    Public Function FromString(value As String) As Decimal Implements IKeyConverter(Of Decimal).FromString
        Return Decimal.Parse(value)
    End Function

    Public Function ToUniqueString(value As Decimal) As String Implements IKeyConverter(Of Decimal).ToUniqueString

        Return value.ToString("G29")

    End Function
End Class

Public Class DecimalNullableKeyConverter
    Implements IKeyConverter(Of Nullable(Of Decimal))

    Public Function FromString(value As String) As Decimal? Implements IKeyConverter(Of Decimal?).FromString

        If String.IsNullOrWhiteSpace(value) Then
            Return Nothing
        Else
            Return Decimal.Parse(value)
        End If

    End Function

    Public Function ToUniqueString(value As Decimal?) As String Implements IKeyConverter(Of Decimal?).ToUniqueString

        If (Not value.HasValue) Then
            Return ""
        Else
            Return value.Value.ToString("G29")
        End If

    End Function
End Class