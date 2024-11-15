Friend MustInherit Class BaseRaceDescriptor
    ReadOnly Property Race As String
    ReadOnly Property Name As String
    ReadOnly Property MaximumHitDie As Integer
    Sub New(race As String, name As String, maximumHitDie As Integer, choice As String)
        Me.Race = race
        Me.Name = name
        Me.MaximumHitDie = maximumHitDie
        Me.Choice = choice
    End Sub
    MustOverride Function IsQualified(character As ICharacter) As Boolean
    ReadOnly Property Choice As String
End Class
