Friend Class DwarfRaceDescriptor
    Inherits BaseRaceDescriptor

    Public Sub New()
        MyBase.New(Races.Dwarf, "Dwarf", 8)
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return True
    End Function
End Class
