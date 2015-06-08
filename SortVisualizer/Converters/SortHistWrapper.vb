''' <summary>
''' Workaround for not being able to databind xml elements
''' directly to search results
''' </summary>
''' <remarks></remarks>
Public Class SortHistWrapper
    Private method As UInteger
    Private swps As ULong
    Private nitms As ULong
    Private cmps As ULong

    Public Sub New(method As UInteger, swps As ULong, cmps As ULong, nitms As ULong)
        Me.method = method
        Me.swps = swps
        Me.nitms = nitms
        Me.cmps = cmps
    End Sub

    Public ReadOnly Property SortMethod
        Get
            Return method
        End Get
    End Property

    Public ReadOnly Property swaps
        Get
            Return swps
        End Get
    End Property

    Public ReadOnly Property nitems
        Get
            Return nitms
        End Get
    End Property

    Public ReadOnly Property comparisons
        Get
            Return cmps
        End Get
    End Property
End Class
