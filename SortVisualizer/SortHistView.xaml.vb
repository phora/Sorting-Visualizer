' The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

''' <summary>
''' A basic page that provides characteristics common to most applications.
''' </summary>
Public NotInheritable Class SortHistView
    Inherits Page
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private qresults As New ObservableCollection(Of SortHistWrapper)
    Private dummy_sorter As ISorter = Nothing

    ''' <summary>
    ''' NavigationHelper is used on each page to aid in navigation and 
    ''' process lifetime management
    ''' </summary>
    Public ReadOnly Property NavigationHelper As Common.NavigationHelper
        Get
            Return Me._navigationHelper
        End Get
    End Property
    Private _navigationHelper As Common.NavigationHelper

    ''' <summary>
    ''' This can be changed to a strongly typed view model.
    ''' </summary>
    Public ReadOnly Property DefaultViewModel As Common.ObservableDictionary
        Get
            Return Me._defaultViewModel
        End Get
    End Property
    Private _defaultViewModel As New Common.ObservableDictionary()

    Public Sub New()

        InitializeComponent()
        Me._navigationHelper = New Common.NavigationHelper(Me)
        AddHandler Me._navigationHelper.LoadState, AddressOf NavigationHelper_LoadState
        AddHandler Me._navigationHelper.SaveState, AddressOf NavigationHelper_SaveState
        Me.query_results = SortHistory.AllEntries()
    End Sub

    Public Property query_results As ObservableCollection(Of SortHistWrapper)
        Get
            Return qresults
        End Get
        Set(value As ObservableCollection(Of SortHistWrapper))
            Me.qresults = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("query_results"))
        End Set
    End Property

    Public Property sorter_stats As ISorter
        Get
            Return dummy_sorter
        End Get
        Set(value As ISorter)
            Me.dummy_sorter = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("best_cmps"))
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("worst_cmps"))
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("avg_cmps"))
        End Set
    End Property

    Public ReadOnly Property best_cmps
        Get
            If Me.dummy_sorter Is Nothing Then
                Return 0
            Else
                Try
                    Return Me.dummy_sorter.bestCase(Convert.ToUInt64(nItems.Text))
                Catch ex As FormatException
                    Return 0
                End Try
            End If
        End Get
    End Property

    Public ReadOnly Property worst_cmps
        Get
            If Me.dummy_sorter Is Nothing Then
                Return 0
            Else
                Try
                    Return Me.dummy_sorter.worstCase(Convert.ToUInt64(nItems.Text))
                Catch ex As FormatException
                    Return 0
                End Try
            End If
        End Get
    End Property

    Public ReadOnly Property avg_cmps
        Get
            If Me.dummy_sorter Is Nothing Then
                Return 0
            Else
                Try
                    Return Me.dummy_sorter.averageCase(Convert.ToUInt64(nItems.Text))
                Catch ex As FormatException
                    Return 0
                End Try
            End If
        End Get
    End Property

    ''' <summary>
    ''' Locks a textbox into suporting only number values
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TextBox_KeyDown(sender As Object, e As KeyRoutedEventArgs)
        If Not ((Windows.System.VirtualKey.Number0 <= e.Key And e.Key <= Windows.System.VirtualKey.Number9) Or
                (Windows.System.VirtualKey.NumberPad0 <= e.Key And e.Key <= Windows.System.VirtualKey.NumberPad9)) Then
            e.Handled = True
        End If
    End Sub

    ''' <summary>
    ''' Populates the page with content passed during navigation.  Any saved state is also
    ''' provided when recreating a page from a prior session.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event; typically <see cref="NavigationHelper"/>
    ''' </param>
    ''' <param name="e">Event data that provides both the navigation parameter passed to
    ''' <see cref="Frame.Navigate"/> when this page was initially requested and
    ''' a dictionary of state preserved by this page during an earlier
    ''' session.  The state will be null the first time a page is visited.</param>
    Private Sub NavigationHelper_LoadState(sender As Object, e As Common.LoadStateEventArgs)

    End Sub

    ''' <summary>
    ''' Preserves state associated with this page in case the application is suspended or the
    ''' page is discarded from the navigation cache.  Values must conform to the serialization
    ''' requirements of <see cref="Common.SuspensionManager.SessionState"/>.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event; typically <see cref="NavigationHelper"/>
    ''' </param>
    ''' <param name="e">Event data that provides an empty dictionary to be populated with 
    ''' serializable state.</param>
    Private Sub NavigationHelper_SaveState(sender As Object, e As Common.SaveStateEventArgs)

    End Sub

#Region "NavigationHelper registration"

    ''' The methods provided in this section are simply used to allow
    ''' NavigationHelper to respond to the page's navigation methods.
    ''' 
    ''' Page specific logic should be placed in event handlers for the  
    ''' <see cref="Common.NavigationHelper.LoadState"/>
    ''' and <see cref="Common.NavigationHelper.SaveState"/>.
    ''' The navigation parameter is available in the LoadState method 
    ''' in addition to page state preserved during an earlier session.

    Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
        _navigationHelper.OnNavigatedTo(e)
    End Sub

    Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
        _navigationHelper.OnNavigatedFrom(e)
    End Sub

#End Region


    Private Sub SortMethod_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If sender Is Nothing Or nItems Is Nothing Then
            Return
        End If

        ApplyFilters()
    End Sub

    Private Sub nItems_TextChanged(sender As Object, e As TextChangedEventArgs)
        If sender Is Nothing Or SortMethod Is Nothing Then
            Return
        End If

        ApplyFilters()
    End Sub

    Private Sub ApplyFilters()

        If SortMethod.SelectedIndex = 0 Then
            If Not (nItems.Text = "") AndAlso Not (nItems.Text = "0") Then
                Dim int_items As ULong = Convert.ToInt64(nItems.Text)
                Me.query_results = SortHistory.FindEntries(int_items)
            Else
                Me.query_results = SortHistory.AllEntries()
            End If
            Me.sorter_stats = Nothing

        Else
            If Not (nItems.Text = "") AndAlso Not (nItems.Text = "0") Then
                Dim int_items As ULong = Convert.ToInt64(nItems.Text)
                Me.query_results = SortHistory.FindEntries(SortMethod.SelectedIndex - 1, int_items)
            Else
                Me.query_results = SortHistory.FindEntries(SortMethod.SelectedIndex - 1)
            End If
            Select Case SortMethod.SelectedIndex
                Case 1
                    Me.sorter_stats = New BubbleSorter()
                Case 2
                    Me.sorter_stats = New HeapSorter()
                Case 3
                    Me.sorter_stats = New QuickSorter()
                Case 4
                    Me.sorter_stats = New InsertSorter()
                Case Else
                    Me.sorter_stats = Nothing
            End Select
        End If
    End Sub

End Class
