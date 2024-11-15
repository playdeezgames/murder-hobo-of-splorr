Friend Class ClericClassDescriptor
    Inherits BaseClassDescriptor

    Public Sub New()
        MyBase.New(Classes.Cleric, "Cleric", Choices.Cleric)
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Counter(CounterTypes.Wisdom).Value >= 9
    End Function
End Class
