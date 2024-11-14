Friend Class DwarfRaceDescriptor
    Inherits BaseRaceDescriptor

    Public Sub New()
        MyBase.New(Races.Dwarf)
    End Sub

    Public Overrides ReadOnly Property Choice As String
        Get
            Return Choices.Dwarf
        End Get
    End Property

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Counter(CounterTypes.Constitution).Value >= 9 AndAlso character.Counter(CounterTypes.Charisma).Value <= 17
    End Function
End Class
