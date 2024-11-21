Friend Class InteractChoiceDescriptor
    Inherits BaseChoiceDescriptor

    Public Sub New()
        MyBase.New(
            Choices.Interact,
            "Interact...")
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        Select Case world.Avatar.Location.Features.Count
            Case 0
                Return Dialogs.Neutral
            Case 1
                'TODO: if there is only one feature, interact with the one feature
                Return Dialogs.InteractFeature
            Case Else
                Return Dialogs.InteractMenu
        End Select
    End Function
End Class
