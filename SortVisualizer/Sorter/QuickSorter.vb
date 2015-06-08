Public Class QuickSorter
    Inherits ISorter

    Public Overrides Async Sub Sort(items As IList)
        Await qSort(items, 0, items.Count - 1)
        Me.RaiseDS()
    End Sub

    ' https://en.wikipedia.org/wiki/Quicksort
    ''' <summary>
    ''' Implementation of Quick Sort, recursive version
    ''' </summary>
    ''' <param name="lo">the starting index to start sorting</param>
    ''' <param name="hi">the ending index to start sorting</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Async Function qSort(items As IList, lo As Integer, hi As Integer) As Task
        Dim p As Integer
        ' RaiseCmp()
        If lo < hi Then
            p = Await QPart(items, lo, hi)
            Await qSort(items, lo, p - 1)
            Await qSort(items, p + 1, hi)
        End If
    End Function

    ' https://en.wikipedia.org/wiki/Quicksort
    ''' <summary>
    ''' Implementation of Quick Sort, partitioning helper
    ''' </summary>
    ''' <param name="lo">the starting index to start sorting</param>
    ''' <param name="hi">the ending index to start sorting</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Async Function QPart(items As IList, lo As Integer, hi As Integer) As Task(Of Integer)
        Dim r As New Random
        Dim pvtidx = r.Next(lo, hi + 1)
        Dim pvtVal = items(pvtidx)
        Await swap(items, hi, pvtidx)
        Dim storeidx = lo
        For i As Integer = lo To hi - 1
            If rev Then
                RaiseCmp()
                If c.Compare(items(i), pvtVal) > 0 Then
                    Await swap(items, i, storeidx)
                    storeidx += 1
                End If
            Else
                RaiseCmp()
                If c.Compare(items(i), pvtVal) < 0 Then
                    Await swap(items, i, storeidx)
                    storeidx += 1
                End If
            End If
        Next
        Await swap(items, storeidx, hi)
        Return storeidx
    End Function

    Public Overrides Function ToString() As String
        Return "Quick Sort"
    End Function


    Public Overrides Function bestCase(itemcount As ULong) As ULong
        Return itemcount * Math.Log(itemcount, 2)
    End Function

    Public Overrides Function averageCase(itemcount As ULong) As ULong
        Return itemcount * Math.Log(itemcount, 2)
    End Function

    Public Overrides Function worstCase(itemcount As ULong) As ULong
        Return Math.Pow(itemcount, 2)
    End Function
End Class
