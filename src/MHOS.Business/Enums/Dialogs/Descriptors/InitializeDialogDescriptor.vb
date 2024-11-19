Friend Class InitializeDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.Initialize)
    End Sub

    Public Overrides Function GoBackDialog(world As IWorld) As String
        Return Dialogs.Initialize
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
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
End Class
