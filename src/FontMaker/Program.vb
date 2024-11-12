Imports System.Drawing
Imports System.IO
Imports System.Text.Json
Imports AOS.UI

Module Program
    Sub Main(args As String())
        'MakeFont("source.png", 384, 192, "..\..\..\..\MHOS\Content\Fonts\Room.json")
        'MakeFont("mapsource.png", 25, 25, "..\..\..\..\MHOS\Content\Fonts\Map.json")
        MakeFont("itemsource.png", 12, 12, "..\..\..\..\MHOS\Content\Fonts\Item.json")
    End Sub
    Private Sub MakeFont(inputFilename As String, cellWidth As Integer, cellHeight As Integer, outputFilename As String)
#Disable Warning CA1416 ' Validate platform compatibility
        Dim bmp = New Bitmap(inputFilename)
        Dim rows = (bmp.Height + 1) \ (cellHeight + 1)
        Dim columns = (bmp.Width + 1) \ (cellWidth + 1)
        Dim glyph = ChrW(0)
        Dim fontData As New FontData With {
            .Height = cellHeight,
            .Glyphs = New Dictionary(Of Char, GlyphData)
        }
        For row = 0 To rows - 1
            For column = 0 To columns - 1
                Dim glyphData As New GlyphData With {.Width = cellWidth, .Lines = New Dictionary(Of Integer, IEnumerable(Of Integer))}
                fontData.Glyphs(glyph) = glyphData
                glyph = ChrW(AscW(glyph) + 1)
                Console.WriteLine(AscW(glyph))
                For y = 0 To cellHeight - 1
                    Dim line As New List(Of Integer)
                    For x = 0 To cellWidth - 1
                        Dim color = bmp.GetPixel(column * (cellWidth + 1) + x, row * (cellHeight + 1) + y)
                        If color.R = 0 AndAlso color.G = 0 AndAlso color.B = 0 Then
                            Console.Write(" ")
                        Else
                            Console.Write("#")
                            line.Add(x)
                        End If
                    Next
                    If line.Any Then
                        glyphData.Lines(y) = line
                    End If
                    Console.WriteLine()
                Next
            Next
        Next
        File.WriteAllText(Path.GetFullPath(outputFilename), JsonSerializer.Serialize(fontData))
#Enable Warning CA1416 ' Validate platform compatibility
    End Sub
End Module
