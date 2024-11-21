Friend Class NeutralUninitializedSubdialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.Neutral)
    End Sub

    Public Overrides Function GoBackDialog(world As IWorld) As String
        Return Dialogs.Neutral
    End Function

    Public Overrides Function LegacyAvailableChoices(world As IWorld) As IEnumerable(Of String)
        Return {
                Choices.Initialize
                }
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Return {
                ("The world is without form and void.", Moods.Normal)
                }
    End Function
End Class
