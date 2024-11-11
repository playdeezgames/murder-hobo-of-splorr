Friend Class NavigationState
    Inherits BaseGameState(Of IWorldModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.A
                SetState(GameState.ActionMenu)
            Case Command.B
                SetState(BoilerplateState.GameMenu)
            Case Command.Left
                Context.Model.TurnLeft()
            Case Command.Right
                Context.Model.TurnRight()
            Case Command.Up
                Context.Model.MoveAhead()
                SetState(BoilerplateState.Neutral)
            Case Command.Down
                Context.Model.TurnAround()
        End Select
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(0)
        DrawRoomFrame(displayBuffer)
        DrawItems(displayBuffer)

        'draw section and facing
        Dim uifont = Context.Font(UIFontName)
        Dim text = $"Section {Context.Model.SectionName} Facing {Context.Model.Facing.ToUpper}"
        uifont.WriteText(displayBuffer, ((ViewWidth - uifont.TextWidth(text)) \ 2, 0), text, 9)

        'draw sanity
        text = $"Sanity: {Context.Model.Sanity}/{Context.Model.MaximumSanity}"
        Dim hue = 2
        If Context.Model.Sanity <= 33 Then
            hue = 4
        ElseIf Context.Model.Sanity <= 66 Then
            hue = 14
        End If
        uifont.WriteText(displayBuffer, ((ViewWidth - uifont.TextWidth(text)) \ 2, 200), text, hue)

        'draw gutter
        Context.ShowStatusBar(displayBuffer, uifont, Context.ControlsText("Action Menu", "Game Menu"), 0, 7)
    End Sub

    Private Sub DrawItems(displayBuffer As IPixelSink)
        Dim itemFont = Context.Font(ItemFontName)
        If Context.Model.HasGroundItems Then
            For Each itemGlyph In Context.Model.ItemGlyphs
                itemFont.WriteText(displayBuffer, itemGlyph.Position, itemGlyph.Text, itemGlyph.Hue)
            Next
        End If
    End Sub

    Private Sub DrawRoomFrame(displayBuffer As IPixelSink)
        Dim roomFont = Context.Font(RoomFontName)
        roomFont.WriteText(displayBuffer, (0, 8), Context.Model.RoomString, 8)
    End Sub

    Public Overrides Sub OnStart()
        PlayMux("MainTheme")
        MyBase.OnStart()
    End Sub
End Class