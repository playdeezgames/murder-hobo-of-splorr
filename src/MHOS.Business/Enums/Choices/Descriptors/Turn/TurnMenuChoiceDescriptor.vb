Friend Class TurnMenuChoiceDescriptor
    Inherits BaseChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.TurnMenu, "Turn...")
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        Return Dialogs.TurnMenu
    End Function
End Class
