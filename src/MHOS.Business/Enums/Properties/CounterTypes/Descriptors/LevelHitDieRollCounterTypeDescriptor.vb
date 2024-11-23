Friend Class LevelHitDieRollCounterTypeDescriptor
    Inherits BaseCounterTypeDescriptor

    Private ReadOnly level As Integer

    Public Sub New(level As Integer)
        MyBase.New(CounterTypes.LevelHitDieRoll(level), $"Level {level} Hit Die Roll")
        Me.level = level
    End Sub
End Class
