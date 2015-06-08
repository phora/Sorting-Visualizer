' The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private orig_data As New ObservableCollection(Of ComparaColor)
    Private sorted_data As New ObservableCollection(Of ComparaColor)
    Private swap_counter As New ULong
    Private comp_counter As New ULong
    Private item_count As ULong = 50
    Private speed_mod As Integer = 0

    Private rndmh As Boolean = True
    Private fixedhval As Integer = 100
    Private invsort As Boolean = False

    Private comparer As IComparer(Of ComparaColor) = New GroupShadesComparator()
    Private avail_sorts As New ObservableCollection(Of ISorter)

    ' https://msdn.microsoft.com/en-us/library/ms172877.aspx
    Private Sub AddSorters()
        Dim b As ISorter = New BubbleSorter()
        AddHandler b.DoneSorting, AddressOf Me.SortFinish
        AddHandler b.SwapHappened, AddressOf Me.IncSwaps
        AddHandler b.CompHappened, AddressOf Me.IncComps
        b.pauseAmount = speedMod
        b.comparer = comparer
        avail_sorts.Add(b)

        b = New HeapSorter()
        AddHandler b.DoneSorting, AddressOf Me.SortFinish
        AddHandler b.SwapHappened, AddressOf Me.IncSwaps
        AddHandler b.CompHappened, AddressOf Me.IncComps
        b.pauseAmount = speedMod
        b.comparer = comparer
        avail_sorts.Add(b)

        b = New QuickSorter()
        AddHandler b.DoneSorting, AddressOf Me.SortFinish
        AddHandler b.SwapHappened, AddressOf Me.IncSwaps
        AddHandler b.CompHappened, AddressOf Me.IncComps
        b.pauseAmount = speedMod
        b.comparer = comparer
        avail_sorts.Add(b)

        b = New InsertSorter()
        AddHandler b.DoneSorting, AddressOf Me.SortFinish
        AddHandler b.SwapHappened, AddressOf Me.IncSwaps
        AddHandler b.CompHappened, AddressOf Me.IncComps
        b.pauseAmount = speedMod
        b.comparer = comparer
        avail_sorts.Add(b)
    End Sub

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddGibberish()
        AddSorters()
    End Sub

    ''' <summary>
    ''' Adds random data to the original data and sorted data views.
    ''' Whether this also generates random heights is determined by the rndmh variable
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AddGibberish()
        Dim rndm As New Random
        orig_data.Clear()
        sorted_data.Clear()

        For i As Integer = 1 To item_count
            Dim r = rndm.Next(0, 256)
            Dim g = rndm.Next(0, 256)
            Dim b = rndm.Next(0, 256)
            Dim c As ComparaColor

            If rndmh Then
                Dim h = rndm.Next(1, 101)
                c = New ComparaColor(r, g, b, h)
            Else
                c = New ComparaColor(r, g, b, fixedhval)
            End If
            orig_data.Add(c)
            sorted_data.Add(c)
        Next
    End Sub

    ''' <summary>
    ''' Returns whether we should sort from greatest to least
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property invertSort As Boolean
        Get
            Return invsort
        End Get
        Set(value As Boolean)
            Me.invsort = value
            For Each sorter As ISorter In avail_sorts
                sorter.reverse = value
            Next
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("invertSort"))
        End Set
    End Property

    ''' <summary>
    ''' Returns the fixed height the blocks should be generated with should
    ''' isRndmHeight be toggled off.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property fixedHeightVal As Integer
        Get
            Return Me.fixedhval
        End Get
        Set(value As Integer)
            Me.fixedhval = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("fixedHeightVal"))
        End Set
    End Property

    ''' <summary>
    ''' Determines whether heights of blocks are randomly generated.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property isRndmHeight As Boolean
        Get
            Return Me.rndmh
        End Get
        Set(value As Boolean)
            Me.rndmh = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("isRndmHeight"))
        End Set
    End Property

    ''' <summary>
    ''' The number of random items to generate
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property itemCount
        Get
            Return Me.item_count
        End Get
        Set(value)
            Me.item_count = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("itemCount"))
        End Set
    End Property

    ''' <summary>
    ''' The original copy of the input data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property origData
        Get
            Return Me.orig_data
        End Get
        Set(value)

        End Set
    End Property

    ''' <summary>
    ''' The number of swaps for the given sorting method
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property swapCounter
        Get
            Return Me.swap_counter
        End Get
        Set(value)
            Me.swap_counter = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("swapCounter"))
        End Set
    End Property

    ''' <summary>
    ''' The number of swaps for the given sorting method
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property compCounter
        Get
            Return Me.comp_counter
        End Get
        Set(value)
            Me.comp_counter = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("compCounter"))
        End Set
    End Property

    ''' <summary>
    ''' The sorted copy of the original data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property sortedData
        Get
            Return Me.sorted_data
        End Get
        Set(value)

        End Set
    End Property

    ''' <summary>
    ''' Modifies how many milliseconds from 200 ms should be added/subtracted to 
    ''' it to change the animation of the speed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property speedMod
        Get
            Return Me.speed_mod
        End Get
        Set(value)
            Me.speed_mod = value
            For Each sorter As ISorter In avail_sorts
                sorter.pauseAmount = value
            Next
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("speedMod"))
        End Set
    End Property


    Private Sub IncComps(sender As Object)
        compCounter += 1
    End Sub

    Private Sub IncSwaps(sender As Object)
        swapCounter += 1
    End Sub

    Private Async Sub SortFinish(sender As Object)
        isDoingSomething.IsActive = False

        SortHistory.AddEntry(SortMethod.SelectedIndex, swap_counter, comp_counter, orig_data.Count)
        Await SortHistory.WriteHistory()
        Dim notixml = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastImageAndText01)
        Dim toastels = notixml.GetElementsByTagName("text")
        Dim noti As Windows.UI.Notifications.ToastNotification

        toastels(0).AppendChild(notixml.CreateTextNode("Saved recent sort to history!"))
        noti = New Windows.UI.Notifications.ToastNotification(notixml)
        Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(noti)

    End Sub

    ''' <summary>
    ''' Determines which method to run for sorting and starts sorting the data
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim mTodo = SortMethod.SelectedIndex
        Me.swapCounter = 0
        Me.compCounter = 0
        isDoingSomething.IsActive = True
        If mTodo >= 0 AndAlso mTodo < avail_sorts.Count Then
            avail_sorts(mTodo).Sort(sorted_data)
        Else
            isDoingSomething.IsActive = False
        End If
    End Sub

    ''' <summary>
    ''' Reapplies height generation when the rndmHeights checkbox is checked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rndmHeights_Checked(sender As Object, e As RoutedEventArgs)
        If sender Is Nothing Then
            Return
        End If
        applyHeights()
    End Sub

    ''' <summary>
    ''' Reapplies height generation when the rndmHeights checkbox is unchecked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rndmHeights_Unchecked(sender As Object, e As RoutedEventArgs)
        If sender Is Nothing Then
            Return
        End If
        applyHeights()
    End Sub

    ''' <summary>
    ''' Changes the numerical view for the ComparaColors and the Comparator 
    ''' used to sort them
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SortBy_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If sender Is Nothing Or origView Is Nothing Or sortedView Is Nothing Then
            Return
        End If
        If sender.SelectedIndex = 0 Then
            comparer = New GroupShadesComparator()
        ElseIf sender.SelectedIndex = 1 Then
            comparer = New GroupSatsComparator()
        Else
            comparer = New HeightComparator()
        End If
        For Each sorter As ISorter In avail_sorts
            sorter.comparer = comparer
        Next
    End Sub

    ''' <summary>
    ''' Generates random data after the number of randomly generated items is changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub nItems_ValueChanged(sender As Object, e As RangeBaseValueChangedEventArgs)
        AddGibberish()
    End Sub

    ''' <summary>
    ''' Generates random data after user requests more random data
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub moreRndm_Click(sender As Object, e As RoutedEventArgs)
        AddGibberish()
    End Sub

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
    ''' Recopies the original data into the sorted data pane after button click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub resetRndm_Click(sender As Object, e As RoutedEventArgs)
        copyBack()
    End Sub

    ''' <summary>
    ''' Helper method for recopying the original data into the sorted data pane 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub copyBack()
        sorted_data.Clear()
        For Each el As ComparaColor In orig_data
            sorted_data.Add(el)
        Next
    End Sub

    ''' <summary>
    ''' Helper method for reapplying height generation
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub applyHeights()
        Dim r As New Random()

        For x As Integer = 0 To orig_data.Count - 1
            Dim h
            If rndmh Then
                h = r.Next(1, 101)
            Else
                h = fixedhval
            End If
            orig_data(x).height = h
            sorted_data(x).height = h
        Next
    End Sub

    ''' <summary>
    ''' Generates ComparaColor blocks from an image
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Async Sub PicButton_Click(sender As Object, e As RoutedEventArgs)
        Dim notixml = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastImageAndText01)
        Dim toastels = notixml.GetElementsByTagName("text")
        Dim noti As Windows.UI.Notifications.ToastNotification


        Dim fop = New Windows.Storage.Pickers.FileOpenPicker()
        fop.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
        fop.FileTypeFilter.Add(".png")

        Dim sf As StorageFile = Await fop.PickSingleFileAsync()
        If sf Is Nothing Then
            toastels(0).AppendChild(notixml.CreateTextNode("Didn't retrieve colors from image."))
            noti = New Windows.UI.Notifications.ToastNotification(notixml)
            Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(noti)
            Return
        End If

        isDoingSomething.IsActive = True
        Dim imgstream As Windows.Storage.Streams.IRandomAccessStream = Await sf.OpenAsync(FileAccessMode.Read)
        Dim png_reader = Await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(Windows.Graphics.Imaging.BitmapDecoder.PngDecoderId, imgstream)

        Dim pixel_data = Await png_reader.GetPixelDataAsync()
        Dim pd = pixel_data.DetachPixelData()

        orig_data.Clear()
        sorted_data.Clear()

        If png_reader.BitmapPixelFormat = Windows.Graphics.Imaging.BitmapPixelFormat.Rgba16 Then
            toastels(0).AppendChild(notixml.CreateTextNode("Cannot accept 16-bit precision in colors!"))
            noti = New Windows.UI.Notifications.ToastNotification(notixml)
            Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(noti)
            isDoingSomething.IsActive = False
            Return
        ElseIf png_reader.BitmapPixelFormat = Windows.Graphics.Imaging.BitmapPixelFormat.Unknown Then
            toastels(0).AppendChild(notixml.CreateTextNode("I have no idea what the pixel format is."))
            noti = New Windows.UI.Notifications.ToastNotification(notixml)
            Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(noti)
            isDoingSomething.IsActive = False
            Return
        End If

        Dim rndm = New Random()
        Dim colset = New HashSet(Of Long)
        For i = 0 To pd.Count - 1 Step 4
            Dim r As Byte
            Dim g As Byte
            Dim b As Byte

            If png_reader.BitmapPixelFormat = Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8 Then
                b = pd(i)
                g = pd(i + 1)
                r = pd(i + 2)
            Else
                r = pd(i)
                g = pd(i + 1)
                b = pd(i + 2)
            End If

            Dim a As Byte = pd(i + 3)
            If a = 0 Then
                Continue For
            End If

            Dim hash As Long
            hash = (CType(r, Long) << 16) Or (CType(g, Long) << 8) Or CType(b, Long)

            If Not colset.Contains(hash) Then
                colset.Add(hash)
                Dim c As ComparaColor
                If rndmh Then
                    c = New ComparaColor(r, g, b, rndm.Next(1, 101))
                Else
                    c = New ComparaColor(r, g, b, fixedhval)
                End If
                orig_data.Add(c)
                sorted_data.Add(c)
            End If
        Next

        toastels(0).AppendChild(notixml.CreateTextNode("Retrieved colors from image!"))
        noti = New Windows.UI.Notifications.ToastNotification(notixml)
        Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(noti)
        isDoingSomething.IsActive = False
    End Sub

    ''' <summary>
    ''' Reapplies height if fixedHeight value is changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub fixedHeight_ValueChanged(sender As Object, e As RangeBaseValueChangedEventArgs)
        applyHeights()
    End Sub

    Private Sub ViewBy_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If sender Is Nothing Or origView Is Nothing Or sortedView Is Nothing Then
            Return
        End If

        Select Case sender.SelectedIndex
            Case 0
                origView.ItemTemplate = Application.Current.Resources("HuePeeker")
                sortedView.ItemTemplate = Application.Current.Resources("HuePeeker")
                origView.ItemContainerStyle = Application.Current.Resources("NumberStyle")
                sortedView.ItemContainerStyle = Application.Current.Resources("NumberStyle")

            Case 1
                origView.ItemTemplate = Application.Current.Resources("SaturationPeeker")
                sortedView.ItemTemplate = Application.Current.Resources("SaturationPeeker")
                origView.ItemContainerStyle = Application.Current.Resources("NumberStyle")
                sortedView.ItemContainerStyle = Application.Current.Resources("NumberStyle")

            Case 2
                origView.ItemTemplate = Application.Current.Resources("ValuePeeker")
                sortedView.ItemTemplate = Application.Current.Resources("ValuePeeker")
                origView.ItemContainerStyle = Application.Current.Resources("NumberStyle")
                sortedView.ItemContainerStyle = Application.Current.Resources("NumberStyle")

            Case 3
                origView.ItemTemplate = Application.Current.Resources("HeightPeeker")
                sortedView.ItemTemplate = Application.Current.Resources("HeightPeeker")
                origView.ItemContainerStyle = Application.Current.Resources("NumberStyle")
                sortedView.ItemContainerStyle = Application.Current.Resources("NumberStyle")

            Case 4
                origView.ItemTemplate = Application.Current.Resources("ItemVisualizer")
                sortedView.ItemTemplate = Application.Current.Resources("ItemVisualizer")
                origView.ItemContainerStyle = Application.Current.Resources("ItemVisualizerStyle")
                sortedView.ItemContainerStyle = Application.Current.Resources("ItemVisualizerStyle")

            Case Else
                origView.ItemTemplate = Application.Current.Resources("ItemVisualizer")
                sortedView.ItemTemplate = Application.Current.Resources("ItemVisualizer")
                origView.ItemContainerStyle = Application.Current.Resources("ItemVisualizerStyle")
                sortedView.ItemContainerStyle = Application.Current.Resources("ItemVisualizerStyle")
        End Select
    End Sub

    Private Async Sub XmlButton_Click(sender As Object, e As RoutedEventArgs)
        Dim notixml = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastImageAndText01)
        Dim toastels = notixml.GetElementsByTagName("text")
        Dim noti As Windows.UI.Notifications.ToastNotification


        Dim fop = New Windows.Storage.Pickers.FileOpenPicker()
        fop.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
        fop.FileTypeFilter.Add(".sample")

        Dim sf As StorageFile = Await fop.PickSingleFileAsync()
        If sf Is Nothing Then
            toastels(0).AppendChild(notixml.CreateTextNode("Didn't retrieve colors from *.sample xml."))
            noti = New Windows.UI.Notifications.ToastNotification(notixml)
            Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(noti)
            Return
        End If

        isDoingSomething.IsActive = True

        sorted_data.Clear()
        orig_data.Clear()
        Dim xmldoc As XmlDocument = Await XmlDocument.LoadFromFileAsync(sf)
        Dim xdoc As XDocument = XDocument.Parse(xmldoc.GetXml())
        For Each el In xdoc.Descendants("colorblock")
            Dim h As Long = Convert.ToInt64(el.Attribute("height").Value)
            Dim r As Byte = Convert.ToByte(el.Attribute("r").Value)
            Dim g As Byte = Convert.ToByte(el.Attribute("g").Value)
            Dim b As Byte = Convert.ToByte(el.Attribute("b").Value)

            Dim c = New ComparaColor(r, g, b, h)
            orig_data.Add(c)
            sorted_data.Add(c)
        Next

        toastels(0).AppendChild(notixml.CreateTextNode("Retrieved colors from *.sample!"))
        noti = New Windows.UI.Notifications.ToastNotification(notixml)
        Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(noti)
        isDoingSomething.IsActive = False
    End Sub

    Private Async Sub ToXmlButton_Click(sender As Object, e As RoutedEventArgs)
        Dim notixml = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastImageAndText01)
        Dim toastels = notixml.GetElementsByTagName("text")
        Dim noti As Windows.UI.Notifications.ToastNotification

        Dim fop = New Windows.Storage.Pickers.FileSavePicker()
        fop.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
        fop.FileTypeChoices.Add("Imported data", New String() {".sample"})
        fop.SuggestedFileName = System.DateTime.UtcNow.ToString("yyyy-MM-dd_-_HH-mm-ss\.\s\a\m\p\l\e")

        Dim sf As StorageFile = Await fop.PickSaveFileAsync()
        If sf Is Nothing Then
            toastels(0).AppendChild(notixml.CreateTextNode("Didn't export data"))
            noti = New Windows.UI.Notifications.ToastNotification(notixml)
            Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(noti)
            Return
        End If

        CachedFileManager.DeferUpdates(sf)

        isDoingSomething.IsActive = True
        Dim xmlwrs As New Xml.XmlWriterSettings
        xmlwrs.Indent = True
        xmlwrs.IndentChars = vbTab
        xmlwrs.Encoding = Text.Encoding.UTF8
        Dim builder = New MemoryStream()

        Dim xmlwr As Xml.XmlWriter = Xml.XmlWriter.Create(builder, xmlwrs)

        xmlwr.WriteStartElement("data")
        For Each color As ComparaColor In orig_data
            Dim xe = New XElement("colorblock")
            xe.SetAttributeValue("height", color.height)
            xe.SetAttributeValue("r", color.color.R)
            xe.SetAttributeValue("g", color.color.G)
            xe.SetAttributeValue("b", color.color.B)
            xe.WriteTo(xmlwr)
        Next
        xmlwr.WriteEndElement()
        xmlwr.Flush()

        Await FileIO.WriteBytesAsync(sf, builder.ToArray())
        toastels(0).AppendChild(notixml.CreateTextNode("Exported data!"))
        noti = New Windows.UI.Notifications.ToastNotification(notixml)
        Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(noti)
        isDoingSomething.IsActive = False
    End Sub

    Private Sub GoToSHist_Click(sender As Object, e As RoutedEventArgs)
        Dim rootFrame As Frame = TryCast(Window.Current.Content, Frame)
        rootFrame.Navigate(GetType(SortHistView), Nothing)
    End Sub
End Class
