Imports System.IO

Public Class MHOSContext
    Inherits UIContext(Of IWorldModel)

    Public Sub New(fontFilenames As IReadOnlyDictionary(Of String, String), viewSize As (Integer, Integer))
        MyBase.New(New WorldModel, fontFilenames, viewSize)
    End Sub

    Private Const ScumSlotFilename As String = "scum.json"
    Private Const Slot1Filename As String = "slot1.json"
    Private Const Slot2Filename As String = "slot2.json"
    Private Const Slot3Filename As String = "slot3.json"
    Private Const Slot4Filename As String = "slot4.json"
    Private Const Slot5Filename As String = "slot5.json"
    Private ReadOnly multipliers As IReadOnlyList(Of Integer) =
        New List(Of Integer) From
        {
            3,
            4,
            5,
            9,
            10,
            14,
            15,
            19,
            20
        }

    Public Overrides ReadOnly Property AvailableWindowSizes As IEnumerable(Of (Integer, Integer))
        Get
            Return multipliers.Select(Function(multiplier) (ViewWidth * multiplier, ViewHeight * multiplier))
        End Get
    End Property
    Private ReadOnly DeltasAndColor As IReadOnlyList(Of (deltaX As Integer, deltaY As Integer, hue As Integer)) =
        New List(Of (Integer, Integer, Integer)) From
        {
            (-1, -1, 13),
            (0, -1, 13),
            (1, -1, 13),
            (-1, 0, 13),
            (1, 0, 13),
            (-1, 1, 13),
            (0, 1, 13),
            (1, 1, 13),
            (0, 0, 6)
        }
    Private Sub ShowTitle(displayBuffer As IPixelSink, font As Font)
        Dim text = GameTitle
        Dim x = ViewWidth \ 2 - font.TextWidth(text) \ 2
        Dim y = ViewHeight \ 2 - font.Height * 3 \ 2
        With font
            For Each deltaAndColor In DeltasAndColor
                .WriteText(
                    displayBuffer,
                    (x + deltaAndColor.deltaX, y + deltaAndColor.deltaY),
                    text,
                    deltaAndColor.hue)
            Next
        End With
    End Sub
    Public Overrides Sub ShowSplashContent(displayBuffer As IPixelSink, font As Font)
        Me.Font(RoomFontName).WriteText(displayBuffer, (0, 0), ChrW(7), BoilerplateHue.White)
        ShowTitle(displayBuffer, font)
        ShowSubtitle(displayBuffer, font)
        ShowStatusBar(displayBuffer, font, ControlsText(ContinueText, Nothing), 0, BoilerplateHue.LightGray)
    End Sub

    Private Sub ShowSubtitle(displayBuffer As IPixelSink, font As Font)
        Dim text = GameSubtitle
        Dim x = ViewWidth \ 2 - font.TextWidth(text) \ 2
        Dim y = ViewHeight \ 2 + font.Height * 3 \ 2
        With font
            .WriteText(
                    displayBuffer,
                    (x, y),
                    text,
                    BoilerplateHue.DarkGray)
        End With
    End Sub

    Private ReadOnly aboutLines As IReadOnlyDictionary(Of Integer, (String, Integer)) =
        New Dictionary(Of Integer, (String, Integer)) From
        {
            {0, ("About Murder Hobo of SPLORR!!", BoilerplateHue.Orange)},
            {1, ("A Production of TheGrumpyGameDev", BoilerplateHue.White)}
        }

    Public Overrides Sub ShowAboutContent(displayBuffer As IPixelSink, font As Font)
        With font
            For Each aboutLine In aboutLines
                .WriteText(displayBuffer, (0, font.Height * aboutLine.Key), aboutLine.Value.Item1, aboutLine.Value.Item2)
            Next
        End With
    End Sub

    Public Overrides Sub AbandonGame()
        Model.Abandon()
    End Sub

    Public Overrides Sub LoadGame(slot As Integer)
        Model.Load(SlotFilename(slot))
    End Sub

    Public Overrides Sub SaveGame(slot As Integer)
        Model.Save(SlotFilename(slot))
    End Sub

    Public Overrides Function DoesSlotExist(slot As Integer) As Boolean
        Return File.Exists(SlotFilename(slot))
    End Function

    Private ReadOnly SlotFilename As IReadOnlyDictionary(Of Integer, String) =
        New Dictionary(Of Integer, String) From
        {
            {0, ScumSlotFilename},
            {1, Slot1Filename},
            {2, Slot2Filename},
            {3, Slot3Filename},
            {4, Slot4Filename},
            {5, Slot5Filename}
        }
End Class
