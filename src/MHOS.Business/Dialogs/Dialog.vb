Public MustInherit Class Dialog
    Implements IDialog
    Protected Sub New(world As IWorld)
        Me.World = world
    End Sub
    Public Property World As IWorld Implements IDialog.World
    Public MustOverride ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String)) Implements IDialog.Description
    Public MustOverride ReadOnly Property AvailableChoices As IChoice() Implements IDialog.AvailableChoices
    Public MustOverride Function GoBack() As IDialog Implements IDialog.GoBack
End Class
