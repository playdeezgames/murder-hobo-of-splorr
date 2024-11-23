Friend Class MoveMenuDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Return {("Move which direction?", Moods.Normal)}
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Dim result As New List(Of IChoice) From
            {
                New CancelChoice(New NeutralDialog(World), World)
            }
            Dim location = World.Avatar.Location
            For Each entry In Directions.Descriptors
                If location.HasRoute(entry.Key) Then
                    result.Add(New DirectionChoice(entry.Key, World))
                End If
            Next
            Return result.ToArray
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return New NeutralDialog(World)
    End Function
End Class
