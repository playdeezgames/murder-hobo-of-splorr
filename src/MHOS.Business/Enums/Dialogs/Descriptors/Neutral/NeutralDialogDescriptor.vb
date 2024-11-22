Friend Class NeutralDialogDescriptor
    Inherits BaseDialogDescriptor
    Private Enum Subdialogs
        Uninitialized
        Navigation
    End Enum
    Private subdialogTable As IReadOnlyDictionary(Of Subdialogs, BaseDialogDescriptor) =
        New Dictionary(Of Subdialogs, BaseDialogDescriptor) From
        {
            {Subdialogs.Navigation, New NeutralNavigationSubdialogDescriptor()},
            {Subdialogs.Uninitialized, New NeutralUninitializedSubdialogDescriptor()}
        }

    Public Sub New()
        MyBase.New(Dialogs.Neutral)
    End Sub

    Private Function GetSubdialog(world As IWorld) As BaseDialogDescriptor
        If world.Avatar Is Nothing Then
            Return subdialogTable(Subdialogs.Uninitialized)
        End If
        Return subdialogTable(Subdialogs.Navigation)
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Return GetSubdialog(world).Description(world)
    End Function

    Public Overrides Function LegacyGoBackDialog(world As IWorld) As String
        Return GetSubdialog(world).LegacyGoBackDialog(world)
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of IChoice)
        Return GetSubdialog(world).AvailableChoices(world)
    End Function
End Class
