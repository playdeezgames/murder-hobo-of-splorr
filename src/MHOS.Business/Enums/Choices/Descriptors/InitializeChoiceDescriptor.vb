Friend Class InitializeChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Initialize, "Initialize!")
    End Sub

    Friend Overrides Function Choose(world As IWorld) As String
        WorldInitializer.Initialize(world)
        Return ChoiceModes.Neutral
    End Function
End Class
