Friend Class ShoppeDialog
    Inherits Dialog

    Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Dim result As New List(Of (Text As String, Mood As String))
            result.Add(($"Shoppe:", Moods.Heading))
            If World.CanBuySkillIncrease Then
                result.Add(($"Skill Increase: {World.SkillIncreaseCost} XP", Moods.Success))
            Else
                result.Add(($"Skill Increase: {World.SkillIncreaseCost} XP", Moods.Failure))
            End If
            result.Add(($"Experience Points: {World.ExperiencePoints}", Moods.Normal))
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Dim result As New List(Of IChoice)
            result.Add(CancelChoice.Create(Function() NeutralDialog.Create(World), World))
            Return result.ToArray
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return NeutralDialog.Create(World)
    End Function
End Class
