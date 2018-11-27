Imports System

Public Module KeyConverterFactory

    Public Function CreateKeyConverter(Of TAggregateKey)() As IKeyConverter(Of TAggregateKey)

        If (GetType(TAggregateKey) Is GetType(String)) Then
            Return New StringKeyConverter()
        End If

        If (GetType(TAggregateKey) Is GetType(Integer)) Then
            Return New IntegerKeyConverter()
        End If

        If (GetType(TAggregateKey) Is GetType(Nullable(Of Integer))) Then
            Return New IntegerNullableKeyConverter()
        End If

        If (GetType(TAggregateKey) Is GetType(Decimal)) Then
            Return New DecimalKeyConverter()
        End If

        If (GetType(TAggregateKey) Is GetType(Nullable(Of Decimal))) Then
            Return New DecimalNullableKeyConverter()
        End If

        If (GetType(TAggregateKey) Is GetType(Double)) Then
            Return New DoubleKeyConverter()
        End If

        If (GetType(TAggregateKey) Is GetType(Nullable(Of Double))) Then
            Return New DoubleNullableKeyConverter()
        End If

        If (GetType(TAggregateKey) Is GetType(Guid)) Then
            Return New GUIDKeyConverter()
        End If

        Throw New NotSupportedException("No key converter class available for specified type - " & GetType(TAggregateKey).Name)

    End Function

End Module
