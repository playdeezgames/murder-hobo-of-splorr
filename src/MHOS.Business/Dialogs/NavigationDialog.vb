Friend Class NavigationDialog
    Inherits Dialog

    Public Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
        Get
            Dim result As New List(Of (Text As String, Mood As String))
            Dim avatar = World.Avatar
            For Each message In avatar.Messages
                result.Add(message)
            Next
            Dim location = avatar.Location
            result.Add(($"In {location.Descriptor.Name}.", Moods.Normal))
            For Each route In location.AllowedRoutes(avatar)
                result.Add(($"{route.RouteTypeName} going {route.DirectionName}.", Moods.Normal))
            Next
            If location.HasFeatures Then
                result.Add(($"You see:", Moods.Normal))
                For Each feature In location.Features
                    result.Add(($"{feature.BriefDescription()}", Moods.Normal))
                Next
            End If
            Return result
        End Get
    End Property

    Public Overrides ReadOnly Property AvailableChoices As IChoice()
        Get
            Dim result As New List(Of IChoice) From {
                New MoveMenuChoice(World)
            }
            If World.Avatar.Location.HasFeatures Then
                result.Add(New InteractChoice(World))
            End If
            result.Add(New StatusChoice(World))
            Return result.ToArray
        End Get
    End Property

    Public Overrides Function GoBack() As IDialog
        Return Nothing
    End Function
End Class
