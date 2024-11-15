Friend MustInherit Class BaseClassDescriptor
    ReadOnly Property [Class] As String
    ReadOnly Property Name As String
    ReadOnly Property Choice As String
    Sub New([class] As String, name As String, choice As String)
        Me.Class = [class]
        Me.Name = name
        Me.Choice = choice
    End Sub
    MustOverride Function IsQualified(character As ICharacter) As Boolean
End Class
