Friend Class InteractFeatureDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.InteractFeature)
    End Sub

    Public Overrides Function GoBackDialog(world As IWorld) As String
        Return Dialogs.InteractMenu
    End Function
    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Throw New NotImplementedException()
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of IChoice)
        Throw New NotImplementedException()
    End Function
End Class
