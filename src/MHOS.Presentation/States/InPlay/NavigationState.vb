Friend Class NavigationState
    Inherits BaseGameState(Of IWorldModel)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.B, Command.Select
                If Context.Model.Avatar.CanEnterGameMenu Then
                    SetState(BoilerplateState.GameMenu)
                End If
        End Select
    End Sub

    Private Shared ReadOnly moodHues As IReadOnlyDictionary(Of String, Integer) =
        New Dictionary(Of String, Integer) From
        {
            {Moods.Normal, BoilerplateHue.LightGray}
        }

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        displayBuffer.Fill(BoilerplateHue.Black)

        Dim font = Context.Font(UIFontName)
        Dim y As Integer = 0
        For Each line In Context.Model.Avatar.Description
            font.WriteText(displayBuffer, (0, y), line.Text, moodHues(line.Mood))
            y += font.Height
        Next
    End Sub

    Public Overrides Sub OnStart()
        PlayMux("MainTheme")
        MyBase.OnStart()
    End Sub
End Class