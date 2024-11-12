Friend Class NavigationState
    Inherits BaseGameState(Of IWorldModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.A, Command.Start
                SetState(GameState.ActionMenu)
            Case Command.B, Command.Select
                SetState(BoilerplateState.GameMenu)
            Case Command.Left
                Context.Model.Avatar.TurnLeft()
            Case Command.Right
                Context.Model.Avatar.TurnRight()
            Case Command.Up
                Context.Model.Avatar.MoveAhead()
                SetState(BoilerplateState.Neutral)
            Case Command.Down
                Context.Model.Avatar.TurnAround()
        End Select
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(BoilerplateHue.Black)
        DrawRoomFrame(displayBuffer)

        'draw section and facing
        Dim uifont = Context.Font(UIFontName)
        'draw gutter
        Context.ShowStatusBar(displayBuffer, uifont, Context.ControlsText(Grimoire.ActionMenu, Grimoire.GameMenu), 0, 7)
    End Sub

    Private Sub DrawRoomFrame(displayBuffer As IPixelSink)
        Dim roomFont = Context.Font(RoomFontName)
        roomFont.WriteText(displayBuffer, (0, 8), Context.Model.Avatar.RoomString, BoilerplateHue.DarkGray)
    End Sub

    Public Overrides Sub OnStart()
        PlayMux("MainTheme")
        MyBase.OnStart()
    End Sub
End Class