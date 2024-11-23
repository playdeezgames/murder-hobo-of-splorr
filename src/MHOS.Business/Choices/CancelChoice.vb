Friend Class CancelChoice
    Inherits Choice
    Private ReadOnly cancelDialog As String

    Public Sub New(cancelDialog As String, dialog As String, world As IWorld)
        MyBase.New(dialog, world)
        Me.cancelDialog = cancelDialog
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Cancel"
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        world.Avatar.ClearMessages()
        Return New Dialog(cancelDialog, world)
    End Function
End Class
