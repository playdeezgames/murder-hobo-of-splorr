Friend Class TurnMenuChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.TurnMenu, "Turn...")
    End Sub

    Friend Overrides Function Choose(world As IWorld, choiceMode As String) As String
        Return Dialogs.TurnMenu
    End Function
End Class
