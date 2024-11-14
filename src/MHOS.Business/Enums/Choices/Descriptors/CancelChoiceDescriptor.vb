Friend Class CancelChoiceDescriptor
    Inherits BaseChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Cancel, "Cancel")
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        Select Case dialog
            Case Dialogs.Status, Dialogs.TurnMenu, Dialogs.MoveMenu
                world.Avatar.ClearMessages()
                Return Dialogs.Neutral
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Class
