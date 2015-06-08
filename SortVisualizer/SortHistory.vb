''' <summary>
''' Holds the sorting history
''' </summary>
''' <remarks></remarks>
Public Module SortHistory
    Private xdoc As XDocument

    ''' <summary>
    ''' Reads the history from the local state folder and if it can't find it,
    ''' it then creates the xml file so the program can write to it later.
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Sub ReadHistory()
        Dim needed_to_make = False
        Dim xmldoc As XmlDocument
        Try
            Dim sf = Await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("sort-history.xml")
            xmldoc = Await XmlDocument.LoadFromFileAsync(sf)
            xdoc = XDocument.Parse(xmldoc.GetXml())
        Catch ex As FileNotFoundException
            xmldoc = New XmlDocument()
            Dim xmlproc = xmldoc.CreateProcessingInstruction("xml", "version='1.0' encoding='utf-8'")
            xmldoc.AppendChild(xmlproc)
            xmldoc.AppendChild(xmldoc.CreateElement("sorthistory"))
            xdoc = XDocument.Parse(xmldoc.GetXml())
            needed_to_make = True
        End Try
        If needed_to_make Then
            Await SortHistory.WriteHistory()
        End If
    End Sub

    ''' <summary>
    ''' Adds an entry to the sort history.
    ''' </summary>
    ''' <param name="method">A number that represents the method used to sort</param>
    ''' <param name="swaps">The number of swaps this method took to finish sorting</param>
    ''' <param name="nitems">The number of items this sort method run sorted</param>
    ''' <remarks></remarks>
    Public Sub AddEntry(method As UInteger, swaps As ULong, cmps As ULong, nitems As ULong)
        Dim xe As New XElement("entry")
        xe.SetAttributeValue("method", method)
        xe.SetAttributeValue("swaps", swaps)
        xe.SetAttributeValue("cmps", cmps)
        xe.SetAttributeValue("nitems", nitems)
        xdoc.Root.Add(xe)
    End Sub


    ''' <summary>
    ''' Grabs ALL the entries in the history and wraps them
    ''' in objects so XAML can data bind the collection
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AllEntries() As ObservableCollection(Of SortHistWrapper)
        Dim query = From xel In xdoc.Descendants("entry") Where True

        Dim results = New ObservableCollection(Of SortHistWrapper)

        For Each item In query
            results.Add(New SortHistWrapper(item.Attribute("method").Value, item.Attribute("swaps").Value,
                                            item.Attribute("cmps").Value, item.Attribute("nitems").Value))
        Next

        Return results
    End Function

    ''' <summary>
    ''' Grabs ALL the entries for a given sort method in the history 
    ''' and wraps them in objects so XAML can data bind the collection
    ''' </summary>
    ''' <param name="method">A number that represents the method used to sort</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindEntries(method As Integer) As ObservableCollection(Of SortHistWrapper)
        Dim query = From xel In xdoc.Descendants("entry") Where xel.Attribute("method").Value = method

        Dim results = New ObservableCollection(Of SortHistWrapper)

        For Each item In query
            results.Add(New SortHistWrapper(item.Attribute("method").Value, item.Attribute("swaps").Value,
                                            item.Attribute("cmps").Value, item.Attribute("nitems").Value))
        Next

        Return results
    End Function

    ''' <summary>
    ''' Grabs ALL the entries for a given item count in the history 
    ''' and wraps them in objects so XAML can data bind the collection
    ''' </summary>
    ''' <param name="nitems">The number of items you want to see the swap results for</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindEntries(nitems As ULong) As ObservableCollection(Of SortHistWrapper)
        Dim query = From xel In xdoc.Descendants("entry") Where xel.Attribute("nitems").Value = nitems

        Dim results = New ObservableCollection(Of SortHistWrapper)

        For Each item In query
            results.Add(New SortHistWrapper(item.Attribute("method").Value, item.Attribute("swaps").Value,
                                            item.Attribute("cmps").Value, item.Attribute("nitems").Value))
        Next

        Return results
    End Function

    ''' <summary>
    ''' Grabs ALL the entries for a given sort method and item count in the 
    ''' history and wraps them in objects so XAML can data bind the collection
    ''' </summary>
    ''' <param name="method">A number that represents the method used to sort</param>
    '''  <param name="nitems">The number of items you want to see the swap results for</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindEntries(method As Integer, nitems As Long) As ObservableCollection(Of SortHistWrapper)
        Dim query = From xel In xdoc.Descendants("entry") Where (xel.Attribute("nitems").Value = nitems _
                                                                 And xel.Attribute("method").Value = method)

        Dim results = New ObservableCollection(Of SortHistWrapper)

        For Each item In query
            results.Add(New SortHistWrapper(item.Attribute("method").Value, item.Attribute("swaps").Value,
                                            item.Attribute("cmps").Value, item.Attribute("nitems").Value))
        Next

        Return results
    End Function

    ''' <summary>
    ''' Writes the history to the xml file
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function WriteHistory() As Task
        Dim sf As StorageFile = Await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("sort-history.xml", CreationCollisionOption.ReplaceExisting)
        Await FileIO.WriteTextAsync(sf, xdoc.ToString())
    End Function

End Module
