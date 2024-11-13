Friend Class TurnRightChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.TurnRight, "Turn Right")
    End Sub

    Friend Overrides Function Choose(world As IWorld) As String
        Dim avatar = world.Avatar
        avatar.Facing = Directions.Descriptors(avatar.Facing).RightDirection
        Return ChoiceModes.Navigation
    End Function
End Class
