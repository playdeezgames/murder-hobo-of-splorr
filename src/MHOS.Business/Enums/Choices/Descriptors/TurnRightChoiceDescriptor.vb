Friend Class TurnRightChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.TurnRight, "Turn Right")
    End Sub

    Friend Overrides Function Choose(world As IWorld, choiceMode As String) As String
        Dim avatar = world.Avatar
        avatar.ClearMessages()
        avatar.AddMessage("You turn right.", Moods.Normal)
        avatar.Facing = Directions.Descriptors(avatar.Facing).RightDirection
        Return Dialogs.Neutral
    End Function
End Class
