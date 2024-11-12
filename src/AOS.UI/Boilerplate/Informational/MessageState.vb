Friend Class MessageState(Of TModel)
    Inherits BaseGameState(Of TModel)
    Private ReadOnly Property nextGameState As String
    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of TModel),
                  nextGameState As String)
        MyBase.New(parent, setState, context)
        Me.nextGameState = nextGameState
    End Sub
    Public Overrides Sub HandleCommand(cmd As String)
        Context.DismissMessage()
        If Not Context.HasMessage Then
            SetState(nextGameState)
        End If
    End Sub
    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(BoilerplateHue.Black)
        Dim font = Context.Font(UIFont)
        Dim y = 0
        Dim message = Context.CurrentMessage
        font.WriteText(displayBuffer, (0, y), message.MessageTitle, BoilerplateHue.Orange)
        y += font.Height
        For Each line In message.MessageLines
            font.WriteText(displayBuffer, (0, y), line, BoilerplateHue.White)
            y += font.Height
        Next
        Context.ShowStatusBar(displayBuffer, font, Context.ControlsText("Continue", String.Empty), 0, 15)
    End Sub
End Class
