Friend Class InteractFeatureDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(Dialogs.InteractFeature, world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Return Dialogs.Descriptors(Dialogs.InteractFeature).Description(World)
        End Get
    End Property
End Class
