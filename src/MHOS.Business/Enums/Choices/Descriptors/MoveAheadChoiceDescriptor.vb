Friend Class MoveAheadChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.MoveAhead, "Move Ahead")
    End Sub

    Friend Overrides Function Choose(world As IWorld, choiceMode As String) As String
        Dim avatar = world.Avatar
        Dim location = avatar.Location
        If location.HasRoute(avatar.Facing) Then
            avatar.Location = location.GetRoute(avatar.Facing).Destination
        End If
        Return ChoiceModes.Neutral
    End Function
End Class
