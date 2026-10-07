Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports AOS.UI
Imports MHOS.Data
Imports MHOS.Presentation

' Drives the real VB game headlessly and records what it draws: one text file per scenario,
' 216 lines of 384 characters, each a hue index in hex. Usage: dotnet run -- <fonts dir> <output dir>
Module Program
    Private Const W = 384
    Private Const H = 216

    ' Same as DisplayBuffer: pixels outside the view are ignored.
    Private Class ClippedBuffer
        Inherits OffscreenBuffer
        Public Sub New()
            MyBase.New((W, H))
        End Sub
        Public Overrides Sub SetPixel(x As Integer, y As Integer, hue As Integer)
            If x < 0 OrElse y < 0 OrElse x >= W OrElse y >= H Then Return
            MyBase.SetPixel(x, y, hue)
        End Sub
    End Class

    Private Function Msg(text As String, mood As String) As MessageData
        Return New MessageData With {.Text = text, .Mood = mood}
    End Function

    Private Function World(Optional configure As Action(Of WorldData) = Nothing) As WorldData
        Dim data As New WorldData
        If configure IsNot Nothing Then configure(data)
        Return data
    End Function

    Private Class Scenario
        Public Name As String
        Public Data As WorldData
        Public Commands As String()
    End Class

    Private Sub Add(result As List(Of Scenario), name As String, data As WorldData, ParamArray commands As String())
        result.Add(New Scenario With {.Name = name, .Data = data, .Commands = commands})
    End Sub

    Private Function Scenarios() As List(Of Scenario)
        Dim result As New List(Of Scenario)
        Add(result, "neutral_fresh", World())
        Add(result, "neutral_cursor_shoppe", World(), "Right")
        Add(result, "neutral_success", World(Sub(d)
                                         d.AttemptCounter = 1 : d.MurderCounter = 1 : d.SuccessStreak = 1 : d.RecordSuccessStreak = 1 : d.ExperiencePoints = 1
                                         d.Messages.AddRange({Msg("Success!", "Success"), Msg("New Record Success Streak!", "Success"), Msg("You get 1 XP", "Success")})
                                     End Sub))
        Add(result, "neutral_failure", World(Sub(d)
                                         d.AttemptCounter = 3 : d.MurderCounter = 2 : d.SuccessStreak = 0 : d.RecordSuccessStreak = 2 : d.ExperiencePoints = 17
                                         d.MurderSkill = 2 : d.MurderDifficulty = 3
                                         d.Messages.AddRange({Msg("Failure!", "Failure"), Msg("You get 6 XP", "Success")})
                                     End Sub))
        Add(result, "neutral_streak", World(Sub(d)
                                        d.AttemptCounter = 9 : d.MurderCounter = 7 : d.SuccessStreak = 5 : d.RecordSuccessStreak = 5 : d.ExperiencePoints = 123
                                        d.Messages.AddRange({Msg("Success!", "Success"), Msg("Streak bonus 4 XP!", "Success"), Msg("New Record Success Streak!", "Success"), Msg("You get 5 XP", "Success")})
                                    End Sub))
        Add(result, "neutral_big", World(Sub(d)
                                     d.AttemptCounter = 12345678 : d.MurderCounter = 9876543 : d.SuccessStreak = 1234567 : d.ExperiencePoints = 2000000000
                                     d.MurderSkill = 99999 : d.MurderDifficulty = 88888
                                 End Sub))
        Add(result, "shoppe_poor", World(), "Right", "A")
        Add(result, "shoppe_rich", World(Sub(d) d.ExperiencePoints = 5000), "Right", "A")
        Add(result, "shoppe_some", World(Sub(d) d.ExperiencePoints = 30), "Right", "A")
        Add(result, "shoppe_cursor_last", World(Sub(d) d.ExperiencePoints = 5000), "Right", "A", "Right", "Right", "Right")
        Add(result, "shoppe_cursor_row2", World(Sub(d) d.ExperiencePoints = 5000), "Right", "A", "Down")
        ' Scenarios without a world start at the splash screen.
        Add(result, "splash", Nothing)
        Add(result, "main_menu", Nothing, "A")
        Add(result, "main_menu_item3", Nothing, "A", "Down", "Down", "Down")
        Add(result, "main_menu_wrapped", Nothing, "A", "Up")
        Add(result, "about", Nothing, "A", "Down", "Down", "Down", "Down", "A")
        Add(result, "options", Nothing, "A", "Down", "Down", "Down", "A")
        Add(result, "window_size", Nothing, "A", "Down", "Down", "Down", "A", "Down", "A")
        Add(result, "window_size_item4", Nothing, "A", "Down", "Down", "Down", "A", "Down", "A", "Down", "Down", "Down")
        Add(result, "confirm_quit", Nothing, "A", "B")
        Add(result, "game_menu", World(), "B")
        Add(result, "game_menu_wrapped", World(), "B", "Up")
        Add(result, "confirm_abandon", World(), "B", "Up", "A")
        Add(result, "confirm_abandon_yes", World(), "B", "Up", "A", "Down")
        Return result
    End Function

    Sub Main(args As String())
        Dim fontsDir = Path.GetFullPath(args(0))
        Dim outDir = Path.GetFullPath(args(1))
        Directory.SetCurrentDirectory(Path.GetTempPath())
        Directory.CreateDirectory(outDir)
        For Each scenario In Scenarios()
            Dim context As New MHOSContext(
                New Dictionary(Of String, String) From {{"UIFont", Path.Combine(fontsDir, "CyFont5x7.json")}}, (W, H))
            Dim controller As New GameController(New MHOSSettings(), context)
            controller.SetSfxHook(Sub(s) Return)
            controller.SetMuxHook(Sub(s) Return)
            If scenario.Data IsNot Nothing Then
                controller.HandleCommand(Command.A) ' splash -> main menu
                controller.HandleCommand(Command.A) ' Embark!
                Dim worldPath = Path.Combine(Path.GetTempPath(), "oracle-world.json")
                File.WriteAllText(worldPath, JsonSerializer.Serialize(scenario.Data))
                context.Model.Session.Load(worldPath)
            End If
            For Each command In scenario.Commands
                controller.HandleCommand(command)
            Next
            Dim buffer As New ClippedBuffer
            controller.Render(buffer)
            Dim text As New StringBuilder
            For y = 0 To H - 1
                For x = 0 To W - 1
                    text.Append(buffer.GetPixel(x, y).ToString("x"))
                Next
                text.Append(vbLf)
            Next
            File.WriteAllText(Path.Combine(outDir, scenario.Name & ".txt"), text.ToString())
            Console.WriteLine(scenario.Name)
        Next
    End Sub
End Module
