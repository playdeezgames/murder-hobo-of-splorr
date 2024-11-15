Friend Class FighterClassDescriptor
    Inherits BaseClassDescriptor

    Public Sub New()
        MyBase.New(Classes.Fighter, "Fighter", Choices.Fighter)
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Counter(CounterTypes.Strength).Value >= 9
    End Function
End Class
