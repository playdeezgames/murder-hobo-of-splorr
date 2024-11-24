Friend Class PlayerCharacterTypeDescriptor
    Inherits BaseCharacterTypeDescriptor

    Public Sub New()
        MyBase.New(CharacterTypes.Player, "N00b")
    End Sub

    Private Const AttributeDiceRoll = "3d6"

    Public Overrides Sub Initialize(character As ICharacter)
        character.Strength = RNG.RollDice(AttributeDiceRoll)
        character.Intelligence = RNG.RollDice(AttributeDiceRoll)
        character.Wisdom = RNG.RollDice(AttributeDiceRoll)
        character.Dexterity = RNG.RollDice(AttributeDiceRoll)
        character.Constitution = RNG.RollDice(AttributeDiceRoll)
        character.Charisma = RNG.RollDice(AttributeDiceRoll)
    End Sub
End Class
