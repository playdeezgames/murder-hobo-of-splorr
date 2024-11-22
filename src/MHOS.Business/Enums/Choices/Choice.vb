Friend MustInherit Class Choice
    Implements IChoice
    Protected ReadOnly dialog As String
    Protected ReadOnly world As IWorld
    Sub New(choice As String, dialog As String, world As IWorld)
        Me.dialog = dialog
        Me.world = world
    End Sub

    Public MustOverride ReadOnly Property Text As String Implements IChoice.Text

    Public MustOverride Function Choose() As String Implements IChoice.Choose
End Class
