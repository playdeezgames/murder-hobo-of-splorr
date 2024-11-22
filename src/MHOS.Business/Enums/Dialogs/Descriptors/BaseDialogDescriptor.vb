Friend MustInherit Class BaseDialogDescriptor
    ReadOnly Property Dialog As String
    Sub New(dialog As String)
        Me.Dialog = dialog
    End Sub
    MustOverride Function LegacyGoBackDialog(world As IWorld) As String
    MustOverride Function AvailableChoices(world As IWorld) As IEnumerable(Of IChoice)
    MustOverride Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
    Public Function LegacyMakeChoice(world As IWorld, choice As String) As String
        Return Choices.Descriptors(choice).Choose(world, Dialog)
    End Function
End Class
