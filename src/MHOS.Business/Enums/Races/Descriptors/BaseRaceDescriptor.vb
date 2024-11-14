Friend MustInherit Class BaseRaceDescriptor
    ReadOnly Property Race As String
    ReadOnly Property Name As String
    Sub New(race As String, name As String)
        Me.Race = race
        Me.Name = name
    End Sub
    MustOverride Function IsQualified(character As ICharacter) As Boolean
    MustOverride ReadOnly Property Choice As String
End Class
