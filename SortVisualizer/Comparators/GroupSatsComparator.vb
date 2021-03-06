﻿
Class GroupSatsComparator
    Implements IComparer(Of ComparaColor)

    Public Function Compare(x As ComparaColor, y As ComparaColor) As Integer Implements IComparer(Of ComparaColor).Compare
        Dim c = x.saturation.CompareTo(y.saturation)
        If c = 0 Then
            c = x.hue.CompareTo(y.hue)
            If c = 0 Then
                Return x.value.CompareTo(y.value)
            Else
                Return c
            End If
        Else
            Return c
        End If
    End Function
End Class
