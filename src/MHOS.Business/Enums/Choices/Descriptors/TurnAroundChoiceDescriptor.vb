Friend Class TurnAroundChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.TurnAround, "Turn Around")
    End Sub

    Friend Overrides Function Choose(world As IWorld) As String
        Dim avatar = world.Avatar
        avatar.Facing = Directions.Descriptors(avatar.Facing).OppositeDirection
        Return ChoiceModes.Navigation
    End Function
End Class
