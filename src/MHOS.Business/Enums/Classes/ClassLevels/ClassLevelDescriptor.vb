Public Class ClassLevelDescriptor
    ReadOnly Property Level As Integer
    ReadOnly Property HitDice As Integer
    ReadOnly Property HitPoints As Integer
    ReadOnly Property ExperiencePoints As Integer
    ReadOnly Property AttackBonus As Integer
    ReadOnly Property HitDieRollCounterType As String
        Get
            Return CounterTypes.LevelHitDieRoll(Level)
        End Get
    End Property
    ReadOnly Property HasConstitutionBonus As Boolean
        Get
            Return HitDice > 0
        End Get
    End Property
    Sub New(
           level As Integer,
           hitDice As Integer,
           hitPoints As Integer,
           experiencePoints As Integer,
           attackBonus As Integer)
        Me.Level = level
        Me.HitDice = hitDice
        Me.HitPoints = hitPoints
        Me.ExperiencePoints = experiencePoints
        Me.AttackBonus = attackBonus
    End Sub
End Class
