Friend Class ManRaceDescriptor
    Inherits BaseRaceDescriptor

    Public Sub New()
        MyBase.New(Races.Man, "Man")
    End Sub

    Public Overrides ReadOnly Property Choice As String
        Get
            Return Choices.Man
        End Get
    End Property

    Public Overrides Function IsQualified(character As ICharacter) As Boolean
        Return True
    End Function
End Class
