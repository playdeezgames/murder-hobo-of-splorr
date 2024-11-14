Friend MustInherit Class BaseCharacterTypeDescriptor
    ReadOnly Property CharacterType As String
    ReadOnly Property Name As String
    MustOverride Sub Initialize(character As ICharacter)
    Sub New(characterType As String, name As String)
        Me.CharacterType = characterType
        Me.Name = name
    End Sub
End Class
