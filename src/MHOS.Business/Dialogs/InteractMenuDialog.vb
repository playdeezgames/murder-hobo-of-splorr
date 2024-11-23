Friend Class InteractMenuDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return New NeutralDialog(World)
    End Function
End Class
