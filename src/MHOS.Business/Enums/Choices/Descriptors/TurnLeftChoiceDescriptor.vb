Friend Class TurnLeftChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.TurnLeft, "Turn Left")
    End Sub

    Friend Overrides Function Choose(world As IWorld, choiceMode As String) As String
        Dim avatar = world.Avatar
        avatar.ClearMessages()
        avatar.AddMessage("You turn Left.", Moods.Normal)
        avatar.Facing = Directions.Descriptors(avatar.Facing).LeftDirection
        Return Dialogs.Neutral
    End Function
End Class
