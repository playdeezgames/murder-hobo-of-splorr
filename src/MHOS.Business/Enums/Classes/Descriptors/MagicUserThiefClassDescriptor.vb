Friend Class MagicUserThiefClassDescriptor
    Inherits BaseClassDescriptor

    Public Sub New()
        MyBase.New(
            Classes.MagicUserThief,
            "Magic-User/Thief",
            Choices.MagicUserThief,
            4,
            New List(Of ClassLevelDescriptor) From
            {
                New ClassLevelDescriptor(1, 1, 0, 0),
                New ClassLevelDescriptor(2, 1, 0, 3750),
                New ClassLevelDescriptor(3, 1, 0, 7500),
                New ClassLevelDescriptor(4, 1, 0, 15000),
                New ClassLevelDescriptor(5, 1, 0, 30000),
                New ClassLevelDescriptor(6, 1, 0, 60000),
                New ClassLevelDescriptor(7, 1, 0, 120000),
                New ClassLevelDescriptor(8, 1, 0, 225000),
                New ClassLevelDescriptor(9, 1, 0, 450000),
                New ClassLevelDescriptor(10, 0, 2, 675000),
                New ClassLevelDescriptor(11, 0, 2, 900000),
                New ClassLevelDescriptor(12, 0, 2, 1125000),
                New ClassLevelDescriptor(13, 0, 2, 1350000),
                New ClassLevelDescriptor(14, 0, 2, 1575000),
                New ClassLevelDescriptor(15, 0, 2, 1800000),
                New ClassLevelDescriptor(16, 0, 2, 2025000),
                New ClassLevelDescriptor(17, 0, 2, 2250000),
                New ClassLevelDescriptor(18, 0, 2, 2475000),
                New ClassLevelDescriptor(19, 0, 2, 2700000),
                New ClassLevelDescriptor(20, 0, 2, 2925000)
            }.ToDictionary(Function(x) x.Level, Function(x) x))
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Dexterity >= 9 AndAlso character.Intelligence >= 9 AndAlso character.Metadata(MetadataTypes.Race) <> Races.Dwarf AndAlso character.Metadata(MetadataTypes.Race) <> Races.Halfling AndAlso character.Metadata(MetadataTypes.Race) <> Races.Man
    End Function
End Class
