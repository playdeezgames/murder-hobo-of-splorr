Friend Class MoveAheadChoiceDescriptor
    Inherits BaseChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.MoveAhead, "Move Ahead")
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        Dim avatar = world.Avatar
        Dim location = avatar.Location
        avatar.ClearMessages()
        If location.HasRoute(avatar.Facing) Then
            avatar.AddMessage("You move ahead.", Moods.Normal)
            avatar.Location = location.GetRoute(avatar.Facing).Destination
        Else
            avatar.AddMessage("You cannot go that way.", Moods.Normal)
        End If
        Return Dialogs.Neutral
    End Function
End Class
