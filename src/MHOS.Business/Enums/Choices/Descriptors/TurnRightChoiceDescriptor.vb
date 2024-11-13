Friend Class TurnRightChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.TurnRight, "Turn Right")
    End Sub

    Friend Overrides Function Choose(world As IWorld, choiceMode As String) As String
        Dim avatar = world.Avatar
        avatar.Facing = Directions.Descriptors(avatar.Facing).RightDirection
        Return ChoiceModes.Neutral
    End Function
End Class
