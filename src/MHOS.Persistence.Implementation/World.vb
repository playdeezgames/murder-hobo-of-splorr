Imports System.Text.Json
Imports MHOS.Data
Imports SPLORR.Game

Public Class World
    Implements IWorld
    Protected ReadOnly WorldData As WorldData
    Sub New(worldData As WorldData)
        Me.WorldData = worldData
    End Sub

    Public ReadOnly Property Serialized As String Implements IWorld.Serialized
        Get
            Return JsonSerializer.Serialize(WorldData)
        End Get
    End Property

    Public ReadOnly Property MurderCounter As Integer Implements IWorld.MurderCounter
        Get
            Return WorldData.MurderCounter
        End Get
    End Property

    Public ReadOnly Property AttemptCounter As Integer Implements IWorld.AttemptCounter
        Get
            Return WorldData.AttemptCounter
        End Get
    End Property

    Public ReadOnly Property SuccessRate As Integer? Implements IWorld.SuccessRate
        Get
            If AttemptCounter = 0 Then
                Return Nothing
            End If
            Return 100 * MurderCounter \ AttemptCounter
        End Get
    End Property

    Public ReadOnly Property Messages As IEnumerable(Of (Text As String, Mood As String)) Implements IWorld.Messages
        Get
            Return WorldData.Messages.Select(Function(x) (x.Text, x.Mood))
        End Get
    End Property

    Public ReadOnly Property ExperiencePoints As Integer Implements IWorld.ExperiencePoints
        Get
            Return WorldData.ExperiencePoints
        End Get
    End Property

    Public ReadOnly Property MurderSkill As Integer Implements IWorld.MurderSkill
        Get
            Return WorldData.MurderSkill
        End Get
    End Property

    Public ReadOnly Property MurderDifficulty As Integer Implements IWorld.MurderDifficulty
        Get
            Return WorldData.MurderDifficulty
        End Get
    End Property

    Public ReadOnly Property CanBuySkillIncrease As Boolean Implements IWorld.CanBuySkillIncrease
        Get
            Return ExperiencePoints >= SkillIncreaseCost
        End Get
    End Property

    Public ReadOnly Property SkillIncreaseCost As Integer Implements IWorld.SkillIncreaseCost
        Get
            Return WorldData.SkillIncreaseCost
        End Get
    End Property

    Public ReadOnly Property CanBuyDifficultyIncrease As Boolean Implements IWorld.CanBuyDifficultyIncrease
        Get
            Return ExperiencePoints >= DifficultyIncreaseCost
        End Get
    End Property

    Public ReadOnly Property DifficultyIncreaseCost As Integer Implements IWorld.DifficultyIncreaseCost
        Get
            Return WorldData.DifficultyIncreaseCost
        End Get
    End Property

    Public Sub AttemptMurder() Implements IWorld.AttemptMurder
        WorldData.AttemptCounter += 1
        WorldData.Messages.Clear()
        If RNG.FromRange(1, WorldData.MurderSkill + WorldData.MurderDifficulty) <= WorldData.MurderSkill Then
            WorldData.ExperiencePoints += WorldData.MurderDifficulty
            WorldData.MurderCounter += 1
            WorldData.Messages.Add(New MessageData With {.Text = "Success!", .Mood = Moods.Success})
        Else
            WorldData.ExperiencePoints += WorldData.MurderDifficulty * 2
            WorldData.Messages.Add(New MessageData With {.Text = "Failure!", .Mood = Moods.Failure})
        End If
    End Sub

    Public Sub BuySkillIncrease() Implements IWorld.BuySkillIncrease
        If Not CanBuySkillIncrease Then
            Return
        End If
        WorldData.ExperiencePoints -= SkillIncreaseCost
        WorldData.MurderSkill += 1
        WorldData.SkillIncreaseCost *= 2
    End Sub

    Public Sub BuyDifficultyIncrease() Implements IWorld.BuyDifficultyIncrease
        If Not CanBuyDifficultyIncrease Then
            Return
        End If
        WorldData.ExperiencePoints -= DifficultyIncreaseCost
        WorldData.MurderDifficulty += 1
        WorldData.DifficultyIncreaseCost *= 2
    End Sub
End Class
