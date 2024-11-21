Friend Class ChooseRaceDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.ChooseRace)
    End Sub

    Public Overrides Function LegacyAvailableChoices(world As IWorld) As IEnumerable(Of String)
        Return Races.Descriptors.Values.Where(Function(x) x.IsQualified(world.Avatar)).Select(Function(x) x.Choice)
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Dim result As New List(Of (Text As String, Mood As String))
        result.AddRange(world.Avatar.DescribeAttributes)
        result.Add(("Choose a race:", Moods.Normal))
        Return result
    End Function

    Public Overrides Function GoBackDialog(world As IWorld) As String
        Return Dialogs.ChooseRace
    End Function
End Class
