Friend Class N00bCharacterTypeDescriptor
    Inherits CharacterTypeDescriptor

    Public Sub New()
        MyBase.New(CharacterTypes.N00b, "N00b")
    End Sub

    Public Overrides ReadOnly Property Attributes As IEnumerable(Of String)
        Get
            Return {AttributeTypes.Strength, AttributeTypes.Intelligence, AttributeTypes.Wisdom, AttributeTypes.Dexterity, AttributeTypes.Constitution, AttributeTypes.Charisma}
        End Get
    End Property

    Public Overrides Function GenerateAttribute(attributeType As String) As Integer
        Select Case attributeType
            Case AttributeTypes.Strength, AttributeTypes.Intelligence, AttributeTypes.Wisdom, AttributeTypes.Dexterity, AttributeTypes.Constitution, AttributeTypes.Charisma
                Return 10
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Class
