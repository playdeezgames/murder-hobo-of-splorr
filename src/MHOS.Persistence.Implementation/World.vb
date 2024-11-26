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

    Public Sub AttemptMurder() Implements IWorld.AttemptMurder
        WorldData.AttemptCounter += 1
        WorldData.Messages.Clear()
        If RNG.FromRange(1, WorldData.MurderSkill + WorldData.MurderDifficulty) <= WorldData.MurderSkill Then
            WorldData.ExperiencePoints += 1
            WorldData.MurderCounter += 1
            WorldData.Messages.Add(New MessageData With {.Text = "Success!", .Mood = Moods.Success})
        Else
            WorldData.ExperiencePoints += 2
            WorldData.Messages.Add(New MessageData With {.Text = "Failure!", .Mood = Moods.Failure})
        End If
    End Sub
End Class
