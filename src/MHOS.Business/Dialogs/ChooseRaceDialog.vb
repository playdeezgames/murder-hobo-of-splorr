Friend Class ChooseRaceDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(Dialogs.ChooseRace, world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Return Dialogs.Descriptors(Dialogs.ChooseRace).Description(World)
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Return Dialogs.Descriptors(Dialogs.ChooseRace).AvailableChoices(World).ToArray
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return Dialogs.Descriptors(Dialogs.ChooseRace).GoBackDialog(World)
    End Function
End Class
