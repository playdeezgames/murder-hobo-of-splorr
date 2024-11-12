Friend MustInherit Class CharacterTypeDescriptor
    ReadOnly Property CharacterType As String
    MustOverride ReadOnly Property Attributes As IEnumerable(Of String)
    MustOverride Function GenerateAttribute(attributeType As String) As Integer
    Sub New(characterType As String)
        Me.CharacterType = characterType
    End Sub
End Class
