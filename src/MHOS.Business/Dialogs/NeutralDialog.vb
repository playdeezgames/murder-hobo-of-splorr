Friend Class NeutralDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(Dialogs.Neutral, world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Return Dialogs.Descriptors(Dialogs.Neutral).Description(World)
        End Get
    End Property

    Public Overrides ReadOnly Property CanEnterGameMenu As Boolean
        Get
            Return GoBack() Is Nothing
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Return Dialogs.Descriptors(Dialogs.Neutral).AvailableChoices(World).ToArray
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return Dialogs.Descriptors(Dialogs.Neutral).GoBackDialog(World)
    End Function
End Class
