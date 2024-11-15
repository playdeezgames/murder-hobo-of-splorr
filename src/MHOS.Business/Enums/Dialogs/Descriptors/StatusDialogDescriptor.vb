Friend Class StatusDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.Status)
    End Sub

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of String)
        Return {
            Choices.Cancel
            }
    End Function

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        Dim result As New List(Of (Text As String, Mood As String)) From {
            ($"Race: {world.Avatar.RaceName}", Moods.Normal),
            ($"Class: {world.Avatar.ClassName}", Moods.Normal)
        }
        result.AddRange(world.Avatar.DescribeAttributes)
        Return result
    End Function

    Public Overrides Function GoBackDialog(world As IWorld) As String
        Return Dialogs.Neutral
    End Function
End Class
