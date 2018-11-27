Imports System.Runtime.CompilerServices

Public Module WhereAsyncExtension

    <Extension()>
    Public Function WhereAsync(Of T)(source As IEnumerable(Of T),
                                     filter As Func(Of T, Task(Of Boolean))) As Task(Of IEnumerable(Of T))

        Dim itemList = source.Select(Function(item As T)
                                         Return {item, filter.Invoke(item)}
                                     End Function
                                     ).ToList()

        If (itemList IsNot Nothing) Then
            ' Run the predicate function for all 
            Return Enumerable.Empty(Of T)
        Else
            Return Enumerable.Empty(Of T)
        End If

    End Function

End Module
