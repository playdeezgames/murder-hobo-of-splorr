Friend Class CancelChoice
    Inherits Choice
    ReadOnly Property nextDialog As Func(Of IDialog)

    Private Sub New(nextDialog As Func(Of IDialog), world As IWorld)
        MyBase.New(world)
        Me.nextDialog = nextDialog
    End Sub

    Public Shared Function Create(nextDialog As Func(Of IDialog), world As IWorld) As IChoice
        Return New CancelChoice(nextDialog, world)
    End Function

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Cancel"
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        Return nextDialog.Invoke()
    End Function
End Class
