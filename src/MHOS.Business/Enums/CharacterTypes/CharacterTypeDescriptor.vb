Friend MustInherit Class CharacterTypeDescriptor
    ReadOnly Property CharacterType As String
    ReadOnly Property Name As String
    MustOverride ReadOnly Property Attributes As IEnumerable(Of String)
    MustOverride Function GenerateAttribute(counterType As String) As Integer
    Sub New(characterType As String, name As String)
        Me.CharacterType = characterType
        Me.Name = name
    End Sub
End Class
