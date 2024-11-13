Friend Class InitializeChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Initialize, "Initialize!")
    End Sub

    Friend Overrides Function Choose(world As IWorld, choiceMode As String) As String
        WorldInitializer.Initialize(world)
        Return ChoiceModes.RollAttributes
    End Function
End Class
