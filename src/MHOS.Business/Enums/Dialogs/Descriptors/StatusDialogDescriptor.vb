Friend Class StatusDialogDescriptor
    Inherits BaseDialogDescriptor

    Public Sub New()
        MyBase.New(Dialogs.Status)
    End Sub

    Public Overrides Function Description(world As IWorld) As IEnumerable(Of (Text As String, Mood As String))
        With world.Avatar
            Dim result As New List(Of (Text As String, Mood As String)) From {
                ($"Race: { .RaceName}", Moods.Normal),
                ($"Class: { .ClassName}", Moods.Normal),
                ($"Experience Level: { .ExperienceLevel}", Moods.Normal),
                ($"Experience Points: { .ExperiencePoints}", Moods.Normal),
                ($"Hit Points: { .HitPoints}/{ .MaximumHitPoints}", Moods.Normal),
                ($"Attack Bonus: { .AttackBonus}", Moods.Normal)
            }
            result.AddRange(.DescribeAttributes)
            Return result
        End With
    End Function

    Private Function LegacyGoBackDialog(world As IWorld) As String
        Return Dialogs.Neutral
    End Function

    Public Overrides Function AvailableChoices(world As IWorld) As IEnumerable(Of IChoice)
        Return {
            New CancelChoice(Dialogs.Neutral, Dialog, world)
            }
    End Function

    Public Overrides Function GoBackDialog(world As IWorld) As IDialog
        Return New Dialog(LegacyGoBackDialog(world))
    End Function
End Class
