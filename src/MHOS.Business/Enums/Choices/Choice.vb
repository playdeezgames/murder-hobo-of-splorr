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

    Private ReadOnly Property Choice As String

    Public Function Choose() As String Implements IChoice.Choose
        Return Choices.Descriptors(Choice).Choose(world, dialog)
    End Function
End Class
