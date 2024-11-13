Friend Class NavigationChoiceModeDescriptor
    Inherits ChoiceModeDescriptor

    Public Sub New()
        MyBase.New(ChoiceModes.Navigation)
    End Sub

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
        If world.Avatar Is Nothing Then
            Return {
                Choices.Initialize
                }
        End If
        Return {
                Choices.MoveAhead,
                Choices.TurnRight,
                Choices.TurnLeft,
                Choices.TurnAround,
                Choices.Status
               }
    End Function

    Public Overrides Function MakeChoice(world As IWorld, choice As String) As String
        Return Choices.Descriptors(choice).Choose(world)
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        If world.Avatar Is Nothing Then
            Return {
                ("The world is without form and void.", Moods.Normal)
                }
        End If
        Dim result As New List(Of (Text As String, Mood As String))
        Dim avatar = world.Avatar
        Dim location = avatar.Location
        result.Add(($"In {location.Descriptor.Name}.", Moods.Normal))
        If location.HasRoute(avatar.Facing) Then
            result.Add(("Door ahead.", Moods.Normal))
        End If
        If location.HasRoute(Directions.Descriptors(avatar.Facing).LeftDirection) Then
            result.Add(("Door to yer left.", Moods.Normal))
        End If
        If location.HasRoute(Directions.Descriptors(avatar.Facing).RightDirection) Then
            result.Add(("Door to yer right.", Moods.Normal))
        End If
        If location.HasRoute(Directions.Descriptors(avatar.Facing).OppositeDirection) Then
            result.Add(("Door behind you.", Moods.Normal))
        End If
        Return result
    End Function

    Public Overrides Function CanEnterGameMenu(world As IWorld) As Boolean
        Return world.Avatar IsNot Nothing
    End Function
End Class
