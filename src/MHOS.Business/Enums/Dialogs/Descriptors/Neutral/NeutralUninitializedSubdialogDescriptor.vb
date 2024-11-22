Friend Class NeutralUninitializedSubdialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.Neutral)
    End Sub

    Public Overrides Function LegacyGoBackDialog(world As IWorld) As String
        Return Dialogs.Neutral
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Return {
                ("The world is without form and void.", Moods.Normal)
                }
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of IChoice)
        Return {
                New InitializeChoice(Dialog, world)
                }
    End Function
End Class
