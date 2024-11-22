Public Class Dialog
    Implements IDialog
    Sub New(dialog As String)
        Me.Dialog = dialog
    End Sub

    Private ReadOnly Property Dialog As String Implements IDialog.Dialog
End Class
