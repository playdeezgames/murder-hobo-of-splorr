Friend Class CancelChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Cancel, "Cancel")
    End Sub

    Friend Overrides Function Choose(world As IWorld, choiceMode As String) As String
        Return ChoiceModes.Neutral
    End Function
End Class
