Friend Class InitializeDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            World.DoNextStep()
            Return {
                ($"Steps Remaining: {World.InitializationStepCount}", Moods.Normal)
            }
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Return {
            New NextChoice(
            If(
                World.InitializationStepCount = 0,
                CType(New ChooseRaceDialog(World), IDialog),
                New InitializeDialog(World)),
            World)
            }
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return Me
    End Function
End Class
