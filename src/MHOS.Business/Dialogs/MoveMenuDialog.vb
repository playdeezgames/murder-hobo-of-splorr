Friend Class MoveMenuDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(Dialogs.MoveMenu, world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Return Dialogs.Descriptors(Dialogs.MoveMenu).Description(World)
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Return Dialogs.Descriptors(Dialogs.MoveMenu).AvailableChoices(World).ToArray
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return Dialogs.Descriptors(Dialogs.MoveMenu).GoBackDialog(World)
    End Function
End Class
