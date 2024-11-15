Friend Class ChooseRaceDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.ChooseRace)
    End Sub

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
        Return Races.Descriptors.Values.Where(Function(x) x.IsQualified(world.Avatar)).Select(Function(x) x.Choice)
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Dim result As New List(Of (Text As String, Mood As String)) From {
            ShowCounter(world, CounterTypes.Strength),
            ShowCounter(world, CounterTypes.Intelligence),
            ShowCounter(world, CounterTypes.Wisdom),
            ShowCounter(world, CounterTypes.Dexterity),
            ShowCounter(world, CounterTypes.Constitution),
            ShowCounter(world, CounterTypes.Charisma)
        }
        result.Add(("Choose a race:", Moods.Normal))
        Return result
    End Function

    Public Overrides Function GoBackDialog(world As IWorld) As String
        Return Dialogs.ChooseRace
    End Function

    Private Function ShowCounter(world As IWorld, counterType As String) As (Text As String, Mood As String)
        Dim attributeDescriptor = CounterTypes.Descriptors(counterType)
        Dim avatar = world.Avatar
        Return ($"{attributeDescriptor.Name} {avatar.Counter(counterType).Value}", Moods.Normal)
    End Function
End Class
