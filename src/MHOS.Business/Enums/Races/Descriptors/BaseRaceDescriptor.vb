Friend MustInherit Class BaseRaceDescriptor
    ReadOnly Property Race As String
    Sub New(race As String)
        Me.Race = race
    End Sub
    MustOverride Function IsQualified(character As ICharacter) As Boolean
    MustOverride ReadOnly Property Choice As String
End Class
