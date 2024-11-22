Friend Class DirectionChoice
    Inherits Choice
    Private ReadOnly direction As String

    Public Sub New(direction As String, dialog As String, world As IWorld)
        MyBase.New(String.Empty, dialog, world)
        Me.direction = direction
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return Directions.Descriptors(direction).Name
        End Get
    End Property

    Public Overrides Function Choose() As String
        Dim avatar = world.Avatar
        Dim location = avatar.Location
        avatar.ClearMessages()
        If location.HasRoute(direction) Then
            avatar.AddMessage($"You go {Directions.Descriptors(direction).Name}.", Moods.Normal)
            avatar.Location = location.GetRoute(direction).Destination
        Else
            avatar.AddMessage("You cannot go that way!", Moods.Normal)
        End If
        Return Dialogs.Neutral
    End Function
End Class
