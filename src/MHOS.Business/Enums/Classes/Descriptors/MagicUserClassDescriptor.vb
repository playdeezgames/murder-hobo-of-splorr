Friend Class MagicUserClassDescriptor
    Inherits BaseClassDescriptor

    Public Sub New()
        MyBase.New(
            Classes.MagicUser,
            "Magic-User",
            4,
            New List(Of ClassLevelDescriptor) From
            {
                New ClassLevelDescriptor(1, 1, 0, 0, 1),
                New ClassLevelDescriptor(2, 1, 0, 2500, 1),
                New ClassLevelDescriptor(3, 1, 0, 5000, 1),
                New ClassLevelDescriptor(4, 1, 0, 10000, 2),
                New ClassLevelDescriptor(5, 1, 0, 20000, 2),
                New ClassLevelDescriptor(6, 1, 0, 40000, 3),
                New ClassLevelDescriptor(7, 1, 0, 80000, 3),
                New ClassLevelDescriptor(8, 1, 0, 150000, 3),
                New ClassLevelDescriptor(9, 1, 0, 300000, 4),
                New ClassLevelDescriptor(10, 0, 1, 450000, 4),
                New ClassLevelDescriptor(11, 0, 1, 600000, 4),
                New ClassLevelDescriptor(12, 0, 1, 750000, 4),
                New ClassLevelDescriptor(13, 0, 1, 900000, 5),
                New ClassLevelDescriptor(14, 0, 1, 1050000, 5),
                New ClassLevelDescriptor(15, 0, 1, 1200000, 5),
                New ClassLevelDescriptor(16, 0, 1, 1350000, 6),
                New ClassLevelDescriptor(17, 0, 1, 1500000, 6),
                New ClassLevelDescriptor(18, 0, 1, 1650000, 6),
                New ClassLevelDescriptor(19, 0, 1, 1800000, 7),
                New ClassLevelDescriptor(20, 0, 1, 1950000, 7)
            }.ToDictionary(Function(x) x.Level, Function(x) x))
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Intelligence >= 9 AndAlso character.Race <> Races.Dwarf AndAlso character.Race <> Races.Halfling
    End Function
End Class
