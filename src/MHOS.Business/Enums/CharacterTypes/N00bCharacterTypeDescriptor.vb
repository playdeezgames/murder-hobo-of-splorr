Friend Class N00bCharacterTypeDescriptor
    Inherits BaseCharacterTypeDescriptor

    Public Sub New()
        MyBase.New(CharacterTypes.N00b, "N00b")
    End Sub

    Public Overrides Sub Initialize(character As ICharacter)
        character.Counter(CounterTypes.Strength) = RNG.RollDice("3d6")
        character.Counter(CounterTypes.Intelligence) = RNG.RollDice("3d6")
        character.Counter(CounterTypes.Wisdom) = RNG.RollDice("3d6")
        character.Counter(CounterTypes.Dexterity) = RNG.RollDice("3d6")
        character.Counter(CounterTypes.Constitution) = RNG.RollDice("3d6")
        character.Counter(CounterTypes.Charisma) = RNG.RollDice("3d6")
    End Sub
End Class
