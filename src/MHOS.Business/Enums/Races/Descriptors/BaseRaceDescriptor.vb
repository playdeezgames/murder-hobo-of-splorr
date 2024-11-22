Friend MustInherit Class BaseRaceDescriptor
    ReadOnly Property Race As String
    ReadOnly Property Name As String
    ReadOnly Property MaximumHitDie As Integer
    Sub New(race As String, name As String, maximumHitDie As Integer)
        Me.Race = race
        Me.Name = name
        Me.MaximumHitDie = maximumHitDie
    End Sub
    MustOverride Function IsQualified(character As ICharacter) As Boolean
End Class
