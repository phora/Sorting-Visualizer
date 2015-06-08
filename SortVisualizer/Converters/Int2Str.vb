Public Class Int2Str
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
        Dim i As Integer = CType(value, Integer)
        Return i.ToString
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
        Dim s As String = CType(value, String)
        Return Int32.Parse(s)
    End Function
End Class
