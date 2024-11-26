Friend Class ShoppeChoice
    Inherits Choice

    Private Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Shoppe"
        End Get
    End Property

    Friend Shared Function Create(world As IWorld) As IChoice
        Return New ShoppeChoice(world)
    End Function

    Public Overrides Function Choose() As IDialog
        Return New ShoppeDialog(world)
    End Function
End Class
