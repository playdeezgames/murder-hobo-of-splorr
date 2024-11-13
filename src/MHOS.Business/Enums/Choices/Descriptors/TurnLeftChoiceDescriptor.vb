Friend Class TurnLeftChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.TurnLeft, "Turn Left")
    End Sub

    Friend Overrides Function Choose(world As IWorld) As String
        Dim avatar = world.Avatar
        avatar.Facing = Directions.Descriptors(avatar.Facing).LeftDirection
        Return ChoiceModes.Neutral
    End Function
End Class
