Friend Class ClericClassDescriptor
    Inherits BaseClassDescriptor

    Public Sub New()
        MyBase.New(
            Classes.Cleric,
            "Cleric",
            Choices.Cleric,
            6,
            New List(Of ClassLevelDescriptor) From
            {
                New ClassLevelDescriptor(1, 1, 0),
                New ClassLevelDescriptor(2, 1, 0),
                New ClassLevelDescriptor(3, 1, 0),
                New ClassLevelDescriptor(4, 1, 0),
                New ClassLevelDescriptor(5, 1, 0),
                New ClassLevelDescriptor(6, 1, 0),
                New ClassLevelDescriptor(7, 1, 0),
                New ClassLevelDescriptor(8, 1, 0),
                New ClassLevelDescriptor(9, 1, 0),
                New ClassLevelDescriptor(10, 0, 1),
                New ClassLevelDescriptor(11, 0, 1),
                New ClassLevelDescriptor(12, 0, 1),
                New ClassLevelDescriptor(13, 0, 1),
                New ClassLevelDescriptor(14, 0, 1),
                New ClassLevelDescriptor(15, 0, 1),
                New ClassLevelDescriptor(16, 0, 1),
                New ClassLevelDescriptor(17, 0, 1),
                New ClassLevelDescriptor(18, 0, 1),
                New ClassLevelDescriptor(19, 0, 1),
                New ClassLevelDescriptor(20, 0, 1)
            }.ToDictionary(Function(x) x.Level, Function(x) x))
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Counter(CounterTypes.Wisdom).Value >= 9
    End Function
End Class
