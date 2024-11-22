Friend Class DwarfRaceDescriptor
    Inherits BaseRaceDescriptor

    Public Sub New()
        MyBase.New(Races.Dwarf, "Dwarf", 8)
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Constitution >= 9 AndAlso character.Charisma <= 17
    End Function
End Class
