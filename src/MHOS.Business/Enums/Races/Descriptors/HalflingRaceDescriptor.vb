Friend Class HalflingRaceDescriptor
    Inherits BaseRaceDescriptor

    Public Sub New()
        MyBase.New(Races.Halfling, "Halfling")
    End Sub

    Public Overrides ReadOnly Property Choice As String
        Get
            Return Choices.Halfling
        End Get
    End Property

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Counter(CounterTypes.Dexterity).Value >= 9 AndAlso character.Counter(CounterTypes.Strength).Value <= 17
    End Function
End Class
