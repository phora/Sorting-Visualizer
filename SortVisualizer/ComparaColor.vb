''' <summary>
''' A class that represents the colors displayed through the sorting GUI.
''' Holds both height and color.
''' </summary>
''' <remarks></remarks>
Public Class ComparaColor
    Implements INotifyPropertyChanged
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private _col As Windows.UI.Color
    Private _h As Long

    ''' <summary>
    ''' Makes a new ComparaColor
    ''' </summary>
    ''' <param name="r">The red channel represented as a number from 0 to 255</param>
    ''' <param name="g">The green channel represented as a number from 0 to 255</param>
    ''' <param name="b">The blue channel represented as a number from 0 to 255</param>
    ''' <remarks></remarks>
    Public Sub New(r As Byte, g As Byte, b As Byte)
        _col = Windows.UI.Color.FromArgb(255, r, g, b)
        _h = 100
    End Sub

    ''' <summary>
    ''' Makes a new ComparaColor
    ''' </summary>
    ''' <param name="r">The red channel represented as a number from 0 to 255</param>
    ''' <param name="g">The green channel represented as a number from 0 to 255</param>
    ''' <param name="b">The blue channel represented as a number from 0 to 255</param>
    ''' <param name="h">The height represented as a number from 1 to 100</param>
    ''' <remarks></remarks>
    Public Sub New(r As Byte, g As Byte, b As Byte, h As Long)
        _col = Windows.UI.Color.FromArgb(255, r, g, b)
        _h = Math.Max(1, Math.Min(100, h))
    End Sub

    ''' <summary>
    ''' The color for the block
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property color As Windows.UI.Color
        Get
            Return Me._col
        End Get
        Set(value As Windows.UI.Color)
            Me._col = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("color"))
        End Set
    End Property

    ''' <summary>
    ''' The height of the block
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property height As Long
        Get
            Return Me._h
        End Get
        Set(value As Long)
            Me._h = Math.Max(1, Math.Min(100, value))
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("height"))
        End Set
    End Property

    ''' <summary>
    ''' The hue of the block, calculated from RGB components
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property hue As Double
        Get
            Dim h As Double
            Dim r As Double = _col.R / 255.0
            Dim g As Double = _col.G / 255.0
            Dim b As Double = _col.B / 255.0

            Dim min As Double = Math.Min(Math.Min(r, g), b)
            Dim max As Double = Math.Max(Math.Max(r, g), b)
            Dim maxmindiff As Double = (max - min)

            If maxmindiff = 0 Then
                ' return a dedicated hue for grayscale vals
                Return -1
            End If

            If r = max Then
                h = (g - b) / maxmindiff
            End If
            If g = max Then
                h = 2.0 + (b - r) / maxmindiff
            End If
            If b = max Then
                h = 4.0 + (r - g) / maxmindiff
            End If

            h *= 60.0

            If h < 0 Then
                h += 360.0
            End If

            Return h
        End Get
    End Property

    Public ReadOnly Property saturation As Double
        Get
            If Me.hue = -1 Then
                Return 0
            Else
                Dim r As Double = _col.R / 255.0
                Dim g As Double = _col.G / 255.0
                Dim b As Double = _col.B / 255.0

                Dim min As Double = Math.Min(Math.Min(r, g), b)
                Dim max As Double = Math.Max(Math.Max(r, g), b)
                Dim maxmindiff As Double = (max - min)

                Return maxmindiff / max * 100
            End If
        End Get
    End Property

    Public ReadOnly Property value As Double
        Get
            Dim r As Double = _col.R / 255.0
            Dim g As Double = _col.G / 255.0
            Dim b As Double = _col.B / 255.0

            Return Math.Max(Math.Max(r, g), b) * 100
        End Get
    End Property

End Class
