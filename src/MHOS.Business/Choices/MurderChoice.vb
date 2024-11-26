Friend Class MurderChoice
    Inherits Choice
    Private ReadOnly nextDialog As Func(Of IDialog)

    Private Sub New(nextDialog As Func(Of IDialog), world As IWorld)
        MyBase.New(world)
        Me.nextDialog = nextDialog
    End Sub

    Friend Shared Function Create(nextDialog As Func(Of IDialog), world As IWorld) As IChoice
        Return New MurderChoice(nextDialog, world)
    End Function

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Murder!"
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        world.Murder()
        Return nextDialog()
    End Function
End Class
