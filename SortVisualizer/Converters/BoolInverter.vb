Public Class BoolInverter
    Implements IValueConverter

    ''' <summary>
    ''' Inverts a boolean value for display
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="targetType"></param>
    ''' <param name="parameter"></param>
    ''' <param name="language"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
        Dim v As Boolean = CType(value, Boolean)
        Return Not v
    End Function

    ''' <summary>
    ''' Inverts a boolean value for display
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="targetType"></param>
    ''' <param name="parameter"></param>
    ''' <param name="language"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
        Dim v As Boolean = CType(value, Boolean)
        Return Not v
    End Function
End Class
