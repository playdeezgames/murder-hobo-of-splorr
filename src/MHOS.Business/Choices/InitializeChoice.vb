Friend Class InitializeChoice
    Inherits Choice

    Public Sub New(dialog As String, world As IWorld)
        MyBase.New(dialog, world)
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Initialize"
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        world.AddInitializationStep(AddressOf WorldExtensionMethods.Initialize)
        Return New Dialog(Dialogs.Initialize, world)
    End Function
End Class
