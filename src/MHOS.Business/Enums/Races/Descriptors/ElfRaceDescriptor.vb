Friend Class ElfRaceDescriptor
    Inherits BaseRaceDescriptor

    Public Sub New()
        MyBase.New(Races.Elf)
    End Sub

    Public Overrides ReadOnly Property Choice As String
        Get
            Return Choices.Elf
        End Get
    End Property

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Counter(CounterTypes.Intelligence).Value >= 9 AndAlso character.Counter(CounterTypes.Constitution).Value <= 17
    End Function
End Class
