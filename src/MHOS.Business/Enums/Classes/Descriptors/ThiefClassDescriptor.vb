Friend Class ThiefClassDescriptor
    Inherits BaseClassDescriptor

    Public Sub New()
        MyBase.New(
            Classes.Thief,
            "Thief",
            Choices.Thief,
            4,
            New List(Of ClassLevelDescriptor) From
            {
                New ClassLevelDescriptor(1, 1, 0, 0),
                New ClassLevelDescriptor(2, 1, 0, 1250),
                New ClassLevelDescriptor(3, 1, 0, 2500),
                New ClassLevelDescriptor(4, 1, 0, 5000),
                New ClassLevelDescriptor(5, 1, 0, 10000),
                New ClassLevelDescriptor(6, 1, 0, 20000),
                New ClassLevelDescriptor(7, 1, 0, 40000),
                New ClassLevelDescriptor(8, 1, 0, 75000),
                New ClassLevelDescriptor(9, 1, 0, 150000),
                New ClassLevelDescriptor(10, 0, 2, 225000),
                New ClassLevelDescriptor(11, 0, 2, 300000),
                New ClassLevelDescriptor(12, 0, 2, 375000),
                New ClassLevelDescriptor(13, 0, 2, 450000),
                New ClassLevelDescriptor(14, 0, 2, 525000),
                New ClassLevelDescriptor(15, 0, 2, 600000),
                New ClassLevelDescriptor(16, 0, 2, 675000),
                New ClassLevelDescriptor(17, 0, 2, 750000),
                New ClassLevelDescriptor(18, 0, 2, 825000),
                New ClassLevelDescriptor(19, 0, 2, 900000),
                New ClassLevelDescriptor(20, 0, 2, 975000)
            }.ToDictionary(Function(x) x.Level, Function(x) x))
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Dexterity >= 9
    End Function
End Class
