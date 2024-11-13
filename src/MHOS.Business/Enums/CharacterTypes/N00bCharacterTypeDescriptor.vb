Friend Class N00bCharacterTypeDescriptor
    Inherits BaseCharacterTypeDescriptor

    Public Sub New()
        MyBase.New(CharacterTypes.N00b, "N00b")
    End Sub

    Public Overrides ReadOnly Property Attributes As IEnumerable(Of String)
        Get
            Return {CounterTypes.Strength, CounterTypes.Intelligence, CounterTypes.Wisdom, CounterTypes.Dexterity, CounterTypes.Constitution, CounterTypes.Charisma}
        End Get
    End Property

    Public Overrides Function GenerateCounter(counterType As String) As Integer
        Select Case counterType
            Case CounterTypes.Strength, CounterTypes.Intelligence, CounterTypes.Wisdom, CounterTypes.Dexterity, CounterTypes.Constitution, CounterTypes.Charisma
                Return RNG.RollDice("3d6")
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Class
