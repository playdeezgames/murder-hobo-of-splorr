Friend Class AttributesState
    Inherits BaseGameState(Of IWorldModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        SetState(BoilerplateState.Neutral)
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(0)
        Dim font = Context.Font(UIFontName)
        font.WriteText(displayBuffer, (0, 0), "Attributes", 15)
    End Sub
End Class
