Friend Class NeutralDialog
    Inherits Dialog

    Private Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Friend Shared Function Create(world As IWorld) As IDialog
        Return New NeutralDialog(world)
    End Function


    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Dim result As New List(Of (Text As String, Mood As String)) From
                {
                    ("Yer Playing the Game!", Moods.Normal),
                    ($"Move Counter: {World.MoveCounter}", Moods.Normal)
                }
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Dim result As New List(Of IChoice) From {
                NextChoice.Create(Function() Me, World)
            }
            Return result.ToArray
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return Nothing
    End Function
End Class
