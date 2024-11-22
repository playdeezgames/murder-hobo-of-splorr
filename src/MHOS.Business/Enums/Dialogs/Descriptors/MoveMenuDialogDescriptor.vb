Friend Class MoveMenuDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.MoveMenu)
    End Sub

    Public Overrides Function LegacyGoBackDialog(world As IWorld) As String
        Return Dialogs.Neutral
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Return {("Move which direction?", Moods.Normal)}
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of IChoice)
        Dim result As New List(Of IChoice) From
            {
                New CancelChoice(Dialogs.Neutral, Dialog, world)
            }
        Dim location = world.Avatar.Location
        For Each entry In Directions.Descriptors
            If location.HasRoute(entry.Key) Then
                result.Add(New DirectionChoice(entry.Key, Dialog, world))
            End If
        Next
        Return result
    End Function
End Class
