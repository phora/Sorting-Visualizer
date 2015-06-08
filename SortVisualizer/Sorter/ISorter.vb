Public Class ISorter

    Protected c As IComparer(Of ComparaColor)
    Protected rev As Boolean
    Protected pauseAmt As Integer

    Public Event DoneSorting(ByVal sender)
    Public Event SwapHappened(ByVal sender)
    Public Event CompHappened(ByVal sender)

    Public Sub New()
    End Sub

    Protected Async Function swap(items As IList, i As Integer, j As Integer) As Task
        If i = j Then
            Return
        End If
        Dim tmp = items(i)
        items(i) = items(j)
        items(j) = tmp
        RaiseEvent SwapHappened(Me)
        Await Task.Delay(200 - pauseAmt)
    End Function

    Public Overridable Async Sub Sort(items As IList)
        Await Task.Delay(200 - pauseAmt)
    End Sub

    Public Sub RaiseDS()
        RaiseEvent DoneSorting(Me)
    End Sub

    Public Sub RaiseCmp()
        RaiseEvent CompHappened(Me)
    End Sub

    Public Property comparer As IComparer(Of ComparaColor)
        Get
            Return c
        End Get
        Set(value As IComparer(Of ComparaColor))
            c = value
        End Set
    End Property

    Public Property reverse As Boolean
        Get
            Return rev
        End Get
        Set(value As Boolean)
            rev = value
        End Set
    End Property

    Public Property pauseAmount As Integer
        Get
            Return pauseAmt
        End Get
        Set(value As Integer)
            Me.pauseAmt = value
        End Set
    End Property

    Public Overridable Function bestCase(itemcount As ULong) As ULong
        Return 1
    End Function

    Public Overridable Function averageCase(itemcount As ULong) As ULong
        Return 1
    End Function

    Public Overridable Function worstCase(itemcount As ULong) As ULong
        Return 1
    End Function
End Class
