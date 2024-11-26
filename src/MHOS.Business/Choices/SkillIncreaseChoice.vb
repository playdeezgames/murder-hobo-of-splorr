Friend Class SkillIncreaseChoice
    Inherits Choice

    Public Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Skill Increase"
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        world.BuySkillIncrease()
        Return New ShoppeDialog(world)
    End Function
End Class
