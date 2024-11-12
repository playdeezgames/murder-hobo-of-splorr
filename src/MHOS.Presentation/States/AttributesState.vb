Friend Class AttributesState
    Inherits BaseGameState(Of IWorldModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        SetState(BoilerplateState.Neutral)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(BoilerplateHue.Black)
        Dim font = Context.Font(UIFontName)
        font.WriteText(displayBuffer, (0, 0), "Attributes", BoilerplateHue.Orange)
        Dim y As Integer = font.Height
        For Each attribute In Context.Model.Avatar.Attributes
            font.WriteText(displayBuffer, (0, y), $"{attribute.Name} {attribute.Value}", BoilerplateHue.LightGray)
            y += font.Height
        Next
    End Sub
End Class
