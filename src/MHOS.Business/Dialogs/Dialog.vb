Public Class Dialog
    Implements IDialog
    Sub New(dialog As String, world As IWorld)
        Me.Dialog = dialog
        Me.World = world
    End Sub

    Public Property World As IWorld Implements IDialog.World

    Private ReadOnly Property Dialog As String Implements IDialog.Dialog
End Class
