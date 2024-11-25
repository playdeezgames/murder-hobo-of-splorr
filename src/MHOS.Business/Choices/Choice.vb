Friend MustInherit Class Choice
    Implements IChoice
    Protected ReadOnly world As IWorld
    Protected Sub New(world As IWorld)
        Me.world = world
    End Sub
    Public MustOverride ReadOnly Property Text As String Implements IChoice.Text
    Public MustOverride Function Choose() As IDialog Implements IChoice.Choose
End Class
