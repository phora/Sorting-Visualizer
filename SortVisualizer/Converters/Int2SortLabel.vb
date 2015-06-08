''' <summary>
''' Converts an int to the appropriate label for the sort. This is used
''' in the sort history viewer.
''' </summary>
''' <remarks></remarks>

Public Class Int2SortLabel
    Implements IValueConverter

    Private labels As List(Of String) = New List(Of String)
    Public Sub New()
        labels.Add("Bubble Sort")
        labels.Add("Heap Sort")
        labels.Add("Quick Sort")
        labels.Add("Insert Sort")
    End Sub

    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
        Return labels(value)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
        Return labels.IndexOf(value)
    End Function
End Class
