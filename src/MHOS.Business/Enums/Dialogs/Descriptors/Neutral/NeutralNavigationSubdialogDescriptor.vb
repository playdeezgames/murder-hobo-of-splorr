Friend Class NeutralNavigationSubdialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.Neutral)
    End Sub

    Private Function LegacyGoBackDialog(world As IWorld) As String
        Return Nothing
    End Function
    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Dim result As New List(Of (Text As String, Mood As String))
        Dim avatar = world.Avatar
        For Each message In avatar.Messages
            result.Add(message)
        Next
        Dim location = avatar.Location
        result.Add(($"In {location.Descriptor.Name}.", Moods.Normal))
        For Each route In location.Routes
            result.Add(($"{route.RouteTypeName} going {route.DirectionName}.", Moods.Normal))
        Next
        If location.HasFeatures Then
            result.Add(($"You see:", Moods.Normal))
            For Each feature In location.Features
                result.Add(($"{feature.Descriptor.FeatureType}", Moods.Normal))
            Next
        End If
        Return result
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of IChoice)
        Dim result As New List(Of IChoice)
        result.Add(New MoveMenuChoice(Dialog, world))
        If world.Avatar.Location.HasFeatures Then
            result.Add(New InteractChoice(Dialog, world))
        End If
        result.Add(New StatusChoice(Dialog, world))
        Return result
    End Function

    Public Overrides Function GoBackDialog(world As IWorld) As IDialog
        Return New Dialog(LegacyGoBackDialog(world))
    End Function
End Class
