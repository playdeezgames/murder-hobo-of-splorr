Friend Class AutoMurderIncreaseChoice
    Inherits Choice

    Public Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Auto-murder Increase"
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        world.BuyAutoMurderIncrease()
        Return New ShoppeDialog(world)
    End Function
End Class
