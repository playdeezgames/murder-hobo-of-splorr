Friend Class MagicUserThiefClassDescriptor
    Inherits BaseClassDescriptor

    Public Sub New()
        MyBase.New(Classes.MagicUserThief, "Magic-User/Thief", Choices.MagicUserThief)
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Counter(CounterTypes.Dexterity).Value >= 9 AndAlso character.Counter(CounterTypes.Intelligence).Value >= 9
    End Function
End Class
