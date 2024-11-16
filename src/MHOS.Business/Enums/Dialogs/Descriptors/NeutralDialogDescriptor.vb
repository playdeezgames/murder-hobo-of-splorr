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
        For Each route In location.Routes
            result.Add(($"{route.RouteTypeName} going {route.DirectionName}.", Moods.Normal))
        Next
        Return result
    End Function

    Public Overrides Function GoBackDialog(world As IWorld) As String
        If world.Avatar Is Nothing Then
            Return Dialogs.Neutral
        End If
        Return Nothing
    End Function
End Class
