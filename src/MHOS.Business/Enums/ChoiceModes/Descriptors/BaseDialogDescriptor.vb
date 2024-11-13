Friend MustInherit Class BaseDialogDescriptor
    ReadOnly Property ChoiceMode As String
    Sub New(choiceMode As String)
        Me.ChoiceMode = choiceMode
    End Sub
    MustOverride Function GoBackDialog(world As IWorld) As String
    MustOverride Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
    MustOverride Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
    Public Function MakeChoice(world As IWorld, choice As String) As String
        Return Choices.Descriptors(choice).Choose(world, ChoiceMode)
    End Function
End Class
