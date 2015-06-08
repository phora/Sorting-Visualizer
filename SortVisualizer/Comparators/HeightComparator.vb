''' <summary>
''' Compares the heights of ComparaColor blocks
''' </summary>
''' <remarks></remarks>
Public Class HeightComparator
    Implements IComparer(Of ComparaColor)

    Public Function Compare(x As ComparaColor, y As ComparaColor) As Integer Implements IComparer(Of ComparaColor).Compare
        Return x.height.CompareTo(y.height)
    End Function
End Class
