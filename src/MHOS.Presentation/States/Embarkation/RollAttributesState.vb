Friend Class RollAttributesState
    Inherits BaseGameState(Of IWorldModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.A, Command.Start
                SetState(GameState.FinishEmbarkation)
        End Select
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(BoilerplateHue.Black)
        Dim font = Context.Font(UIFontName)
        font.WriteText(displayBuffer, (0, 0), "Attributes", BoilerplateHue.Orange)
        Dim y = font.Height
        For Each attribute In Context.Model.Session.Options.Attributes
            font.WriteText(displayBuffer, (0, y), $"{attribute.Name} {attribute.Value}", BoilerplateHue.LightGray)
            y += font.Height
        Next
    End Sub
End Class
