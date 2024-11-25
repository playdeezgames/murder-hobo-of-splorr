Friend Class ClericClassDescriptor
    Inherits BaseClassDescriptor

    Public Sub New()
        MyBase.New(
            Classes.Cleric,
            "Cleric",
            6,
            New List(Of ClassLevelDescriptor) From
            {
                New ClassLevelDescriptor(1, 1, 0, 0, 1),
                New ClassLevelDescriptor(2, 1, 0, 1500, 1),
                New ClassLevelDescriptor(3, 1, 0, 3000, 2),
                New ClassLevelDescriptor(4, 1, 0, 6000, 2),
                New ClassLevelDescriptor(5, 1, 0, 12000, 3),
                New ClassLevelDescriptor(6, 1, 0, 24000, 3),
                New ClassLevelDescriptor(7, 1, 0, 48000, 4),
                New ClassLevelDescriptor(8, 1, 0, 90000, 4),
                New ClassLevelDescriptor(9, 1, 0, 180000, 5),
                New ClassLevelDescriptor(10, 0, 1, 270000, 5),
                New ClassLevelDescriptor(11, 0, 1, 360000, 5),
                New ClassLevelDescriptor(12, 0, 1, 450000, 6),
                New ClassLevelDescriptor(13, 0, 1, 540000, 6),
                New ClassLevelDescriptor(14, 0, 1, 630000, 6),
                New ClassLevelDescriptor(15, 0, 1, 720000, 7),
                New ClassLevelDescriptor(16, 0, 1, 810000, 7),
                New ClassLevelDescriptor(17, 0, 1, 900000, 7),
                New ClassLevelDescriptor(18, 0, 1, 990000, 8),
                New ClassLevelDescriptor(19, 0, 1, 1080000, 8),
                New ClassLevelDescriptor(20, 0, 1, 1170000, 8)
            }.ToDictionary(Function(x) x.Level, Function(x) x))
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return True
    End Function
End Class
