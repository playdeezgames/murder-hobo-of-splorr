Friend Class ChooseClassDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(Dialogs.ChooseClass, world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Return Dialogs.Descriptors(Dialogs.ChooseClass).Description(World)
        End Get
    End Property
End Class
