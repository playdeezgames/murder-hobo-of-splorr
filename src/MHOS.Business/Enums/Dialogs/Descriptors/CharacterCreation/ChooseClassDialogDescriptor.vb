Friend Class ChooseClassDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.ChooseClass)
    End Sub

    Public Overrides Function GoBackDialog(world As IWorld) As String
        Return Me.Dialog
    End Function

    Public Overrides Function LegacyAvailableChoices(world As IWorld) As IEnumerable(Of String)
        Return Classes.Descriptors.Values.Where(Function(x) x.IsQualified(world.Avatar)).Select(Function(x) x.Choice)
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Dim result As New List(Of (Text As String, Mood As String)) From {
            ($"Race: {world.Avatar.RaceName}", Moods.Normal)
        }
        result.AddRange(world.Avatar.DescribeAttributes)
        Return result
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of IChoice)
        Return Classes.Descriptors.Values.Where(Function(x) x.IsQualified(world.Avatar)).Select(Function(x) New Choice(x.Choice))
    End Function
End Class
