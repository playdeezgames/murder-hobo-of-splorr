Friend MustInherit Class BaseDialogDescriptor
    ReadOnly Property Dialog As String
    Sub New(dialog As String)
        Me.Dialog = dialog
    End Sub
    MustOverride Function GoBackDialog(world As IWorld) As IDialog
    MustOverride Function AvailableChoices(world As IWorld) As IEnumerable(Of IChoice)
    MustOverride Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
End Class
