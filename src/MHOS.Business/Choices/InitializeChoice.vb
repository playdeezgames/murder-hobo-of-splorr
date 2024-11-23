Friend Class InitializeChoice
    Inherits Choice

    Public Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Initialize"
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        Dim newWorld As IWorld = New World(New WorldData)
        newWorld.AddInitializationStep(AddressOf WorldExtensionMethods.Initialize)
        Return New InitializeDialog(newWorld)
    End Function
End Class
