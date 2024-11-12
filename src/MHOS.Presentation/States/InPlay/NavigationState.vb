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

        Dim font = Context.Font(UIFontName)
        Dim y As Integer = 0
        If Model.Avatar.HasDoorAhead Then
            font.WriteText(displayBuffer, (0, y), "There is a door ahead.", BoilerplateHue.White)
            y += font.Height
        End If
        If Model.Avatar.HasDoorToLeft Then
            font.WriteText(displayBuffer, (0, y), "There is a door to yer left.", BoilerplateHue.White)
            y += font.Height
        End If
        If Model.Avatar.HasDoorToRight Then
            font.WriteText(displayBuffer, (0, y), "There is a door to yer right.", BoilerplateHue.White)
            y += font.Height
        End If
        If Model.Avatar.HasDoorBehind Then
            font.WriteText(displayBuffer, (0, y), "There is a door behind you.", BoilerplateHue.White)
            y += font.Height
        End If

        'draw gutter
        Context.ShowStatusBar(displayBuffer, font, Context.ControlsText(Grimoire.ActionMenu, Grimoire.GameMenu), 0, 7)
    End Sub

    Public Overrides Sub OnStart()
        PlayMux("MainTheme")
        MyBase.OnStart()
    End Sub
End Class