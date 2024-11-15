Friend Class ManRaceDescriptor
    Inherits BaseRaceDescriptor

    Public Sub New()
        MyBase.New(Races.Man, "Man", 8, Choices.Man)
    End Sub

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return True
    End Function
End Class
