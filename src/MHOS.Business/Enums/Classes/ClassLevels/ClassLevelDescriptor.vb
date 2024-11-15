Public Class ClassLevelDescriptor
    ReadOnly Property Level As Integer
    ReadOnly Property HitDice As Integer
    ReadOnly Property HitPoints As Integer
    Sub New(level As Integer, hitDice As Integer, hitPoints As Integer)
        Me.Level = level
        Me.HitDice = hitDice
        Me.HitPoints = hitPoints
    End Sub
End Class
