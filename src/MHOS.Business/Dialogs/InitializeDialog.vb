Friend Class InitializeDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(Dialogs.Initialize, world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Return Dialogs.Descriptors(Dialogs.Initialize).Description(World)
        End Get
    End Property
End Class
