Friend Class PlayerCharacterTypeDescriptor
    Inherits BaseCharacterTypeDescriptor

    Public Sub New()
        MyBase.New(CharacterTypes.Player, "N00b")
    End Sub

    Private Const AttributeDiceRoll = "3d6"

    Public Overrides Sub Initialize(character As ICharacter)
    End Sub
End Class
