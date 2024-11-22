Friend Class InitializeDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.Initialize)
    End Sub

    Public Overrides Function LegacyGoBackDialog(world As IWorld) As String
        Return Dialogs.Initialize
    End Function

    Private Function LegacyAvailableChoices(world As IWorld) As IEnumerable(Of String)
        Return {
                Choices.Next
                }
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        world.DoNextStep()
        Return {
                ($"Steps Remaining: {world.InitializationStepCount}", Moods.Normal)
            }
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of IChoice)
        Return {
            New NextChoice(If(world.InitializationStepCount = 0, Dialogs.ChooseRace, Dialogs.Initialize), Dialog, world)
            }
    End Function
End Class
