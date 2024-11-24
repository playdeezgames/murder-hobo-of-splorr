Friend Class InteractFeatureDialog
    Inherits Dialog
    Private ReadOnly feature As IFeature

    Public Sub New(world As IWorld, feature As IFeature)
        MyBase.New(world)
        Me.feature = feature
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Dim result As New List(Of (Text As String, Mood As String))
            For Each line In feature.DescriptionLines
                result.Add((line, Moods.Normal))
            Next
            For Each verb In feature.AllowedVerbs(World.Avatar)
                result.Add((verb.Metadata(MetadataTypes.Name), Moods.Normal))
            Next
            Return result.ToArray
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Dim result As New List(Of IChoice) From {
                New CancelChoice(
                    If(
                        World.Avatar.Location.Features.Count = 1,
                        CType(New NeutralDialog(World), IDialog),
                        New InteractMenuDialog(World)),
                        World)
                }
            For Each verb In feature.AllowedVerbs(World.Avatar)
                result.Add(New VerbChoice(World, verb))
            Next
            Return result.ToArray
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return New InteractMenuDialog(World)
    End Function
End Class
