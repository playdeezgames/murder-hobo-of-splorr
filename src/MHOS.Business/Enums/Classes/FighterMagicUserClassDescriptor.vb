Friend Class FighterMagicUserClassDescriptor
    Inherits BaseClassDescriptor

    Public Sub New()
        MyBase.New(Classes.FighterMagicUser, "Fighter/Magic-User", Choices.FighterMagicUser)
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Counter(CounterTypes.Strength).Value >= 9 AndAlso character.Counter(CounterTypes.Intelligence).Value >= 9
    End Function
End Class
