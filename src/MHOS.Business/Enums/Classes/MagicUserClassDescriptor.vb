Friend Class MagicUserClassDescriptor
    Inherits BaseClassDescriptor

    Public Sub New()
        MyBase.New(Classes.MagicUser, "Magic-User", Choices.MagicUser)
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return character.Counter(CounterTypes.Intelligence).Value >= 9
    End Function
End Class
