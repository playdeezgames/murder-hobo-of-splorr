Friend MustInherit Class BaseTurnChoiceDescriptor
    Inherits BaseChoiceDescriptor
    Private ReadOnly message As String

    Protected Sub New(choice As String, text As String, message As String)
        MyBase.New(choice, text)
        Me.message = message
    End Sub

    Protected MustOverride Function NextFacing(facing As String) As String

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        With world.Avatar
            .ClearMessages()
            .AddMessage(message, Moods.Normal)
            .Facing = NextFacing(.Facing)
        End With
        Return Dialogs.Neutral
    End Function
End Class
