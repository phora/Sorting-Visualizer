Public Class BubbleSorter
    Inherits ISorter

    Public Overrides Async Sub Sort(items As IList)
        Dim n = items.Count
        Dim swapped = False
        Do
            swapped = False
            For i As Integer = 1 To n - 1
                If rev Then
                    RaiseCmp()
                    If c.Compare(items(i - 1), items(i)) < 0 Then
                        Await swap(items, i, i - 1)
                        swapped = True
                    End If
                Else
                    RaiseCmp()
                    If c.Compare(items(i - 1), items(i)) > 0 Then
                        Await swap(items, i, i - 1)
                        swapped = True
                    End If
                End If

            Next
            n -= 1
            ' RaiseCmp()
        Loop Until Not swapped
        Me.RaiseDS()
    End Sub

    Public Overrides Function ToString() As String
        Return "Bubble Sort"
    End Function

    Public Overrides Function bestCase(itemcount As ULong) As ULong
        Return itemcount
    End Function

    Public Overrides Function averageCase(itemcount As ULong) As ULong
        Return Math.Pow(itemcount, 2)
    End Function

    Public Overrides Function worstCase(itemcount As ULong) As ULong
        Return Math.Pow(itemcount, 2)
    End Function
End Class
