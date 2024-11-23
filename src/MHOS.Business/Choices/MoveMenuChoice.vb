Friend Class MoveMenuChoice
    Inherits Choice

    Public Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Move..."
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        Return New MoveMenuDialog(world)
    End Function
End Class
