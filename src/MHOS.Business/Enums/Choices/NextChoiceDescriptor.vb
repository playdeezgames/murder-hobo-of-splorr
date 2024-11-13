Friend Class NextChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Next, "Next")
    End Sub

    Friend Overrides Function Choose(world As IWorld) As String
        Return ChoiceModes.Neutral
    End Function
End Class
