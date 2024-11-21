Friend Class InteractMenuDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.InteractMenu)
    End Sub

    Public Overrides Function GoBackDialog(world As IWorld) As String
        Return Dialogs.Neutral
    End Function

    Public Overrides Function LegacyAvailableChoices(world As IWorld) As IEnumerable(Of String)
        Throw New NotImplementedException()
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Throw New NotImplementedException()
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of IChoice)
        Return LegacyAvailableChoices(world).Select(Function(x) New Choice(x))
    End Function
End Class
