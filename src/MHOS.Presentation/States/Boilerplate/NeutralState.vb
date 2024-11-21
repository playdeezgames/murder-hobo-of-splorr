Friend Class NeutralState
    Inherits BaseGameState(Of IWorldModel)

    Const ChoiceColumns = 3
    Private currentChoice As Integer = 0
    Private choices As (Text As String, Choice As String)()

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context)
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Select Case cmd
            Case Command.B, Command.Select
                If Context.Model.CanEnterGameMenu Then
                    SetState(BoilerplateState.GameMenu)
                Else
                    Context.Model.GoBack()
                    SetState(BoilerplateState.Neutral)
                End If
            Case Command.A, Command.Start
                Context.Model.MakeChoice(choices(currentChoice).Choice)
                SetState(BoilerplateState.Neutral)
            Case Command.Right
                currentChoice = Math.Min(currentChoice + 1, Context.Model.LegacyAvailableChoices.Length - 1)
            Case Command.Left
                currentChoice = Math.Max(currentChoice - 1, 0)
            Case Command.Up
                currentChoice = Math.Max(currentChoice - ChoiceColumns, 0)
            Case Command.Down
                currentChoice = Math.Min(currentChoice + ChoiceColumns, Context.Model.LegacyAvailableChoices.Length - 1)
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
        For Each line In Context.Model.Description
            font.WriteText(displayBuffer, (0, y), line.Text, moodHues(line.Mood))
            y += font.Height
        Next

        Dim rows = (choices.Length + ChoiceColumns - 1) \ ChoiceColumns
        y = displayBuffer.Height - font.Height * rows
        Dim index = 0
        Dim columnWidth = displayBuffer.Width \ ChoiceColumns
        For Each row In Enumerable.Range(0, rows)
            For Each column In Enumerable.Range(0, ChoiceColumns)
                If index < choices.Length Then
                    If index = currentChoice Then
                        displayBuffer.Fill((column * columnWidth, y), (columnWidth, font.Height), BoilerplateHue.White)
                        font.WriteText(displayBuffer, (column * columnWidth, y), choices(index).Text, BoilerplateHue.Black)
                    Else
                        displayBuffer.Fill((column * columnWidth, y), (columnWidth, font.Height), BoilerplateHue.Black)
                        font.WriteText(displayBuffer, (column * columnWidth, y), choices(index).Text, BoilerplateHue.White)
                    End If
                End If
                index += 1
            Next
            y += font.Height
        Next
    End Sub

    Public Overrides Sub OnStart()
        choices = Context.Model.LegacyAvailableChoices
        If currentChoice >= choices.Length Then
            currentChoice = 0
        End If
        PlayMux("MainTheme")
        MyBase.OnStart()
    End Sub
End Class
