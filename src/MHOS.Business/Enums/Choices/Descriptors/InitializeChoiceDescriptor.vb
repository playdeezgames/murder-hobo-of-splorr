Friend Class InitializeChoiceDescriptor
    Inherits BaseChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Initialize, "Initialize!")
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        world.AddInitializationStep(AddressOf WorldExtensionMethods.Initialize)
        Return Dialogs.Initialize
    End Function
End Class
