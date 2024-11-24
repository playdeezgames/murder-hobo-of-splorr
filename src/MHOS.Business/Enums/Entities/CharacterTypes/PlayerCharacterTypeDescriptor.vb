Friend Class PlayerCharacterTypeDescriptor
    Inherits BaseCharacterTypeDescriptor

    Public Sub New()
        MyBase.New(CharacterTypes.Player, "N00b")
    End Sub

    Private Shared ReadOnly attributes As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            CounterTypes.Constitution,
            CounterTypes.Charisma
        }
    Private Const AttributeDiceRoll = "3d6"

    Public Overrides Sub Initialize(character As ICharacter)
        character.Strength = RNG.RollDice(AttributeDiceRoll)
        character.Intelligence = RNG.RollDice(AttributeDiceRoll)
        character.Wisdom = RNG.RollDice(AttributeDiceRoll)
        character.Dexterity = RNG.RollDice(AttributeDiceRoll)
        For Each counterType In attributes
            character.Counter(counterType) = RNG.RollDice(AttributeDiceRoll)
        Next
    End Sub
End Class
