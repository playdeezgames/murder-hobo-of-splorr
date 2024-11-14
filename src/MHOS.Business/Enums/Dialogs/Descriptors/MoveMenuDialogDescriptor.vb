Friend Class MoveMenuDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.MoveMenu)
    End Sub

    Public Overrides Function GoBackDialog(world As IWorld) As String
        Return Dialogs.Neutral
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
        Dim result As New List(Of String) From
            {
                Choices.Cancel
            }
        Dim location = world.Avatar.Location
        If location.HasRoute(Directions.North) Then
            result.Add(Choices.MoveNorth)
        End If
        If location.HasRoute(Directions.East) Then
            result.Add(Choices.MoveEast)
        End If
        If location.HasRoute(Directions.South) Then
            result.Add(Choices.MoveSouth)
        End If
        If location.HasRoute(Directions.West) Then
            result.Add(Choices.MoveWest)
        End If
        Return result
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Return {("Move which direction?", Moods.Normal)}
    End Function
End Class
