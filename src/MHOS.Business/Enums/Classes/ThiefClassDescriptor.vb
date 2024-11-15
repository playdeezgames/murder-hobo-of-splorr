Friend Class ThiefClassDescriptor
    Inherits BaseClassDescriptor

    Public Sub New()
        MyBase.New(Classes.Thief, "Thief", Choices.Thief)
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Counter(CounterTypes.Dexterity).Value >= 9
    End Function
End Class
