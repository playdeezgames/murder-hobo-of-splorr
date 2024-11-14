Friend Class MoveMenuChoiceDescriptor
    Inherits BaseChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.MoveMenu, "Move...")
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        Return Dialogs.MoveMenu
    End Function
End Class
