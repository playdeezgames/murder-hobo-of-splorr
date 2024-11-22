Friend Class Choice
    Implements IChoice
    Private ReadOnly dialog As String
    Private ReadOnly world As IWorld
    Sub New(choice As String, dialog As String, world As IWorld)
        Me.Choice = choice
        Me.dialog = dialog
        Me.world = world
    End Sub

    Public ReadOnly Property Text As String Implements IChoice.Text
        Get
            Return Choices.Descriptors(Choice).Text
        End Get
    End Property

    Private ReadOnly Property Choice As String Implements IChoice.Choice

    Public Function LegacyChoose(dialog As String, world As IWorld) As String Implements IChoice.LegacyChoose
        Return Dialogs.
            Descriptors(dialog).
            LegacyMakeChoice(world, Choice)
    End Function

    Public Function Choose() As String Implements IChoice.Choose
        Return Dialogs.
            Descriptors(dialog).
            LegacyMakeChoice(world, Choice)
    End Function
End Class
