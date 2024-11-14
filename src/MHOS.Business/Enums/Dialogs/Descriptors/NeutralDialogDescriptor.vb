Friend Class NeutralDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.Neutral)
    End Sub

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
        If world.Avatar Is Nothing Then
            Return {
                Choices.Initialize
                }
        End If
        Return {
                Choices.MoveMenu,
                Choices.MoveAhead,
                Choices.TurnMenu,
                Choices.Status
               }
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        If world.Avatar Is Nothing Then
            Return {
                ("The world is without form and void.", Moods.Normal)
                }
        End If
        Dim result As New List(Of (Text As String, Mood As String))
        Dim avatar = world.Avatar
        For Each message In avatar.Messages
            result.Add(message)
        Next
        Dim location = avatar.Location
        result.Add(($"In {location.Descriptor.Name}.", Moods.Normal))
        If location.HasRoute(Directions.North) Then
            result.Add(("Door to the north.", Moods.Normal))
        End If
        If location.HasRoute(Directions.East) Then
            result.Add(("Door to the east.", Moods.Normal))
        End If
        If location.HasRoute(Directions.South) Then
            result.Add(("Door to the south.", Moods.Normal))
        End If
        If location.HasRoute(Directions.West) Then
            result.Add(("Door to the west.", Moods.Normal))
        End If
        Return result
    End Function

    Public Overrides Function GoBackDialog(world As IWorld) As String
        If world.Avatar Is Nothing Then
            Return Dialogs.Neutral
        End If
        Return Nothing
    End Function
End Class
