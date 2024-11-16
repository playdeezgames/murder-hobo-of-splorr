Friend Class HalflingRaceDescriptor
    Inherits BaseRaceDescriptor

    Public Sub New()
        MyBase.New(Races.Halfling, "Halfling", 8, Choices.Halfling)
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Counter(CounterTypes.Dexterity).Value >= 9 AndAlso character.Strength <= 17
    End Function
End Class
