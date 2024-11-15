Friend Class DwarfRaceDescriptor
    Inherits BaseRaceDescriptor

    Public Sub New()
        MyBase.New(Races.Dwarf, "Dwarf", 8, Choices.Dwarf)
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Counter(CounterTypes.Constitution).Value >= 9 AndAlso character.Counter(CounterTypes.Charisma).Value <= 17
    End Function
End Class
