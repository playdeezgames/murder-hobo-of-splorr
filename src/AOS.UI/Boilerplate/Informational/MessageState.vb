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
        Dim message = Context.CurrentMessage
        Context.ShowHeader(displayBuffer, font, message.MessageTitle, BoilerplateHue.Orange, BoilerplateHue.Black)
        Dim y = displayBuffer.Height \ 2 - message.MessageLines.Count * font.HalfHeight
        For Each line In message.MessageLines
            font.WriteCenteredText(displayBuffer, y, line, BoilerplateHue.White)
            y += font.Height
        Next
        Context.ShowStatusBar(displayBuffer, font, Context.ControlsText("Continue", String.Empty), 0, 15)
    End Sub
End Class
