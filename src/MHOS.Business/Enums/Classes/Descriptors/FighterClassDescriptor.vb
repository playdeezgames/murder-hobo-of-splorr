Friend Class FighterClassDescriptor
    Inherits BaseClassDescriptor

    Public Sub New()
        MyBase.New(
            Classes.Fighter,
            "Fighter",
            Choices.Fighter,
            8,
            New List(Of ClassLevelDescriptor) From
            {
                New ClassLevelDescriptor(1, 1, 0, 0),
                New ClassLevelDescriptor(2, 1, 0, 2000),
                New ClassLevelDescriptor(3, 1, 0, 4000),
                New ClassLevelDescriptor(4, 1, 0, 8000),
                New ClassLevelDescriptor(5, 1, 0, 16000),
                New ClassLevelDescriptor(6, 1, 0, 32000),
                New ClassLevelDescriptor(7, 1, 0, 64000),
                New ClassLevelDescriptor(8, 1, 0, 120000),
                New ClassLevelDescriptor(9, 1, 0, 240000),
                New ClassLevelDescriptor(10, 0, 2, 360000),
                New ClassLevelDescriptor(11, 0, 2, 480000),
                New ClassLevelDescriptor(12, 0, 2, 600000),
                New ClassLevelDescriptor(13, 0, 2, 720000),
                New ClassLevelDescriptor(14, 0, 2, 840000),
                New ClassLevelDescriptor(15, 0, 2, 960000),
                New ClassLevelDescriptor(16, 0, 2, 1080000),
                New ClassLevelDescriptor(17, 0, 2, 1200000),
                New ClassLevelDescriptor(18, 0, 2, 1320000),
                New ClassLevelDescriptor(19, 0, 2, 1440000),
                New ClassLevelDescriptor(20, 0, 2, 1560000)
            }.ToDictionary(Function(x) x.Level, Function(x) x))
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Strength >= 9
    End Function
End Class
