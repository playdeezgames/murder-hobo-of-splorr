Friend Class InteractMenuDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(Dialogs.InteractMenu, world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Return Dialogs.Descriptors(Dialogs.InteractMenu).Description(World)
        End Get
    End Property
End Class
