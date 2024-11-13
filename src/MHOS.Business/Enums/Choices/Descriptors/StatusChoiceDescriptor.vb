Friend Class StatusChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Status, "Status")
    End Sub

    Friend Overrides Function Choose(world As IWorld, choiceMode As String) As String
        Return ChoiceModes.Status
    End Function
End Class
