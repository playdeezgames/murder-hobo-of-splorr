Friend Class ElfRaceDescriptor
    Inherits BaseRaceDescriptor

    Public Sub New()
        MyBase.New(Races.Elf, "Elf", 6, Choices.Elf)
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Intelligence >= 9 AndAlso character.Constitution <= 17
    End Function
End Class
