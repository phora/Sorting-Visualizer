Public Class HeapSorter
    Inherits ISorter

    Public Overrides Async Sub Sort(items As IList)
        Dim endidx = items.Count - 1
        Await heapify(items, items.Count)

        While endidx > 0
            ' RaiseCmp()
            Await swap(items, endidx, 0)
            Await siftDown(items, 0, endidx)
            endidx -= 1
        End While
        Me.RaiseDS()
    End Sub

    ''' <summary>
    ''' Heapifies items to prepare it for heapsort
    ''' </summary>
    ''' <param name="items"></param>
    ''' <param name="count">The end index of the heaping</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Async Function heapify(items As IList, count As Integer) As Task
        Dim startidx = count \ 2 - 1
        While startidx >= 0
            ' RaiseCmp()
            Await siftDown(items, startidx, count)
            startidx -= 1
        End While
    End Function

    ''' <summary>
    ''' Pushes numbers down to propery form the heap
    ''' </summary>
    ''' <param name="items"></param>
    ''' <param name="startidx">the starting index of where it should push down items</param>
    ''' <param name="count">the ending index of the object</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Async Function siftDown(items As IList, startidx As Integer, count As Integer) As Task
        Dim root = startidx
        While root * 2 + 1 < count
            ' RaiseCmp()
            Dim childidx = root * 2 + 1
            If rev Then
                ' Workaround for visual basic
                ' not supporting logical shortcircuiting
                ' RaiseCmp()
                If childidx < count - 1 Then
                    ' RaiseCmp()
                    If c.Compare(items(childidx), items(childidx + 1)) > 0 Then
                        childidx += 1
                    End If
                End If
                RaiseCmp()
                If c.Compare(items(root), items(childidx)) > 0 Then
                    Await swap(items, root, childidx)
                    root = childidx
                Else
                    Return
                End If
            Else
                ' Workaround for visual basic
                ' not supporting logical shortcircuiting
                ' RaiseCmp()
                If childidx < count - 1 Then
                    ' RaiseCmp()
                    If c.Compare(items(childidx), items(childidx + 1)) < 0 Then
                        childidx += 1
                    End If
                End If
                RaiseCmp()
                If c.Compare(items(root), items(childidx)) < 0 Then
                    Await swap(items, root, childidx)
                    root = childidx
                Else
                    Return
                End If
            End If

        End While
    End Function

    Public Overrides Function ToString() As String
        Return "Heap Sort"
    End Function


    Public Overrides Function bestCase(itemcount As ULong) As ULong
        Return itemcount * Math.Log(itemcount, 2)
    End Function

    Public Overrides Function averageCase(itemcount As ULong) As ULong
        Return itemcount * Math.Log(itemcount, 2)
    End Function

    Public Overrides Function worstCase(itemcount As ULong) As ULong
        Return itemcount * Math.Log(itemcount, 2)
    End Function
End Class
