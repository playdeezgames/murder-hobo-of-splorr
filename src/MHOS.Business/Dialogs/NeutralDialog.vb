Friend Class NeutralDialog
    Inherits Dialog

    Private Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Friend Shared Function Create(world As IWorld) As IDialog
        Return New NeutralDialog(world)
    End Function


    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Dim result =World.Messages.ToList()
            result.AddRange(
                {
                    ($"Murder Skill: {World.MurderSkill}", Moods.Normal),
                    ($"Murder Difficulty: {World.MurderDifficulty}", Moods.Normal),
                    ($"Murder Counter: {World.MurderCounter}", Moods.Normal),
                    ($"Attempt Counter: {World.AttemptCounter}", Moods.Normal),
                    ($"Success Rate: {If(World.SuccessRate.HasValue, World.SuccessRate.Value.ToString() + "%", "?") }", Moods.Normal),
                    ($"Experience Points: {World.ExperiencePoints}", Moods.Normal)
                })
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Dim result As New List(Of IChoice) From {
                MurderChoice.Create(Function() Me, World)
            }
            Return result.ToArray
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return Nothing
    End Function
End Class
