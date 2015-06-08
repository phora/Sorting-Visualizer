Public Class InsertSorter
    Inherits ISorter

    Public Overrides Async Sub Sort(items As IList)
        For i = 1 To items.Count - 1
            For j = i To 1 Step -1
                If rev Then
                    RaiseCmp()
                    If c.Compare(items(j - 1), items(j)) < 0 Then
                        Await swap(items, j, j - 1)
                    End If
                Else
                    RaiseCmp()
                    If c.Compare(items(j - 1), items(j)) > 0 Then
                        Await swap(items, j, j - 1)
                    End If
                End If

            Next
        Next
        Me.RaiseDS()
    End Sub

    Public Overrides Function ToString() As String
        Return "Insert Sort"
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
