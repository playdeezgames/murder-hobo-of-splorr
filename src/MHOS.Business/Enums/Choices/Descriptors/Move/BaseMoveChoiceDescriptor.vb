Friend MustInherit Class BaseMoveChoiceDescriptor
    Inherits BaseChoiceDescriptor
    Private ReadOnly Direction As String

    Protected Sub New(choice As String, text As String, direction As String)
        MyBase.New(choice, text)
        Me.Direction = direction
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        Dim avatar = world.Avatar
        Dim location = avatar.Location
        avatar.ClearMessages()
        If location.HasRoute(Direction) Then
            avatar.AddMessage($"You go {Directions.Descriptors(Direction).Name}.", Moods.Normal)
            avatar.Location = location.GetRoute(Direction).Destination
        Else
            avatar.AddMessage("You cannot go that way!", Moods.Normal)
        End If
        Return Dialogs.Neutral
    End Function
End Class
