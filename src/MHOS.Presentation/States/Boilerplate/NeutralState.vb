Friend Class NeutralState
    Inherits BaseGameState(Of IWorldModel)

    Const ChoiceColumns = 3
    Private currentChoice As Integer = 0

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
                Dim Choices = Context.Model.AvailableChoices
                Context.Model.MakeChoice(Choices(currentChoice))
                SetState(BoilerplateState.Neutral)
            Case Command.Right
                currentChoice = Math.Min(currentChoice + 1, Context.Model.AvailableChoices.Length - 1)
            Case Command.Left
                currentChoice = Math.Max(currentChoice - 1, 0)
            Case Command.Up
                currentChoice = Math.Max(currentChoice - ChoiceColumns, 0)
            Case Command.Down
                currentChoice = Math.Min(currentChoice + ChoiceColumns, Context.Model.AvailableChoices.Length - 1)
        End Select
    End Sub

    Private Shared ReadOnly moodHues As IReadOnlyDictionary(Of String, Integer) =
        New Dictionary(Of String, Integer) From
        {
            {Moods.Normal, BoilerplateHue.LightGray}
        }

    Private Function RenderLine(displayBuffer As IPixelSink, font As Font, y As Integer, text As String, mood As String) As Integer
        font.WriteText(displayBuffer, (0, y), text, moodHues(mood))
        Return y + font.Height
    End Function

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        Dim Choices = Context.Model.AvailableChoices
        If currentChoice >= Choices.Length Then
            currentChoice = 0
        End If
        displayBuffer.Fill(BoilerplateHue.Black)

        Dim font = Context.Font(UIFontName)
        Dim y As Integer = RenderLine(displayBuffer, font, 0, "(Escape -> Game Menu)", Moods.Normal)
        For Each line In Context.Model.Description
            y = RenderLine(displayBuffer, font, y, line.Text, line.Mood)
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
        PlayMux("MainTheme")
        MyBase.OnStart()
    End Sub
End Class
