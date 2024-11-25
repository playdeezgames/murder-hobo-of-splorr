Friend Class FighterMagicUserClassDescriptor
    Inherits BaseClassDescriptor

    Public Sub New()
        MyBase.New(
            Classes.FighterMagicUser,
            "Fighter/Magic-User",
            6,
            New List(Of ClassLevelDescriptor) From
            {
                New ClassLevelDescriptor(1, 1, 0, 0, 1),
                New ClassLevelDescriptor(2, 1, 0, 4500, 2),
                New ClassLevelDescriptor(3, 1, 0, 9000, 2),
                New ClassLevelDescriptor(4, 1, 0, 18000, 3),
                New ClassLevelDescriptor(5, 1, 0, 36000, 4),
                New ClassLevelDescriptor(6, 1, 0, 72000, 4),
                New ClassLevelDescriptor(7, 1, 0, 144000, 5),
                New ClassLevelDescriptor(8, 1, 0, 270000, 6),
                New ClassLevelDescriptor(9, 1, 0, 540000, 6),
                New ClassLevelDescriptor(10, 0, 2, 810000, 6),
                New ClassLevelDescriptor(11, 0, 2, 1080000, 7),
                New ClassLevelDescriptor(12, 0, 2, 1350000, 7),
                New ClassLevelDescriptor(13, 0, 2, 1620000, 8),
                New ClassLevelDescriptor(14, 0, 2, 1890000, 8),
                New ClassLevelDescriptor(15, 0, 2, 2160000, 8),
                New ClassLevelDescriptor(16, 0, 2, 2430000, 9),
                New ClassLevelDescriptor(17, 0, 2, 2700000, 9),
                New ClassLevelDescriptor(18, 0, 2, 2970000, 10),
                New ClassLevelDescriptor(19, 0, 2, 3240000, 10),
                New ClassLevelDescriptor(20, 0, 2, 3510000, 10)
            }.ToDictionary(Function(x) x.Level, Function(x) x))
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return True
    End Function
End Class
