Friend Class NextChoice
    Inherits Choice
    Private ReadOnly nextDialog As IDialog

    Public Sub New(nextDialog As IDialog, world As IWorld)
        MyBase.New(world)
        Me.nextDialog = nextDialog
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Next"
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        Return nextDialog
    End Function
End Class
