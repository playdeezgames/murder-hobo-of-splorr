Friend Class TurnAroundChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.TurnAround, "Turn Around")
    End Sub

    Friend Overrides Function Choose(world As IWorld, choiceMode As String) As String
        Dim avatar = world.Avatar
        avatar.ClearMessages()
        avatar.AddMessage("You turn around.", Moods.Normal)
        avatar.Facing = Directions.Descriptors(avatar.Facing).OppositeDirection
        Return Dialogs.Neutral
    End Function
End Class
