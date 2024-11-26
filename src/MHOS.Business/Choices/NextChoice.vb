Friend Class NextChoice
    Inherits Choice
    Private ReadOnly nextDialog As Func(Of IDialog)

    Private Sub New(nextDialog As Func(Of IDialog), world As IWorld)
        MyBase.New(world)
        Me.nextDialog = nextDialog
    End Sub

    Friend Shared Function Create(nextDialog As Func(Of IDialog), world As IWorld) As IChoice
        Return New NextChoice(nextDialog, world)
    End Function

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Next"
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        Return nextDialog()
    End Function
End Class
