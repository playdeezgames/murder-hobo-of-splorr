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

    Public Overrides Function LegacyChoose() As String
        world.AddInitializationStep(AddressOf WorldExtensionMethods.Initialize)
        Return Dialogs.Initialize
    End Function

    Public Overrides Function Choose() As IDialog
        Return New Dialog(LegacyChoose())
    End Function
End Class
