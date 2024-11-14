Friend Class HumanRaceDescriptor
    Inherits BaseRaceDescriptor

    Public Sub New()
        MyBase.New(Races.Human)
    End Sub

    Public Overrides ReadOnly Property Choice As String
        Get
            Return Choices.Human
        End Get
    End Property

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return True
    End Function
End Class
