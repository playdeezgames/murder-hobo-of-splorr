Friend Class InteractMenuDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.InteractMenu)
    End Sub

    Private Function LegacyGoBackDialog(world As IWorld) As String
        Return Dialogs.Neutral
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Throw New NotImplementedException()
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of IChoice)
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GoBackDialog(world As IWorld) As IDialog
        Return New NeutralDialog(world)
    End Function
End Class
