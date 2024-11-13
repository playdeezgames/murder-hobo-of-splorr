Friend MustInherit Class ChoiceModeDescriptor
    ReadOnly Property ChoiceMode As String
    Sub New(choiceMode As String)
        Me.ChoiceMode = choiceMode
    End Sub
    MustOverride ReadOnly Property LegacyCanEnterGameMenu As Boolean
    MustOverride Function CanEnterGameMenu(world As IWorld) As Boolean
    MustOverride Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
    MustOverride Function MakeChoice(world As IWorld, choice As String) As String
    MustOverride Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
End Class
