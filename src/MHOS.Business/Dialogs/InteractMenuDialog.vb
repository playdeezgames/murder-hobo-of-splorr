Friend Class InteractMenuDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Return {
                ("Interact with?", Moods.Normal)
                }
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Dim result As New List(Of IChoice) From {
                New CancelChoice(New NeutralDialog(World), World)
            }
            For Each feature In World.Avatar.Location.Features
                result.Add(New InteractFeatureChoice(World, feature))
            Next
            Return result.ToArray
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return New NeutralDialog(World)
    End Function
End Class
