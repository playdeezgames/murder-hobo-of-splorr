Friend Class ChooseRaceDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Dim result As New List(Of (Text As String, Mood As String))
            result.AddRange(World.Avatar.DescribeAttributes)
            result.Add(("Choose a race:", Moods.Normal))
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Return Races.Descriptors.Values.Where(Function(x) x.IsQualified(World.Avatar)).Select(Function(x) New RaceChoice(x.Race, World)).ToArray
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return Me
    End Function
End Class
