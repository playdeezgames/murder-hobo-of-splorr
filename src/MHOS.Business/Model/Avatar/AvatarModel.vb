Friend Class AvatarModel
    Implements IAvatarModel

    Private ReadOnly world As IWorld
    Private Shared choiceMode As String = ChoiceModes.Navigation

    Public Sub New(world As IWorld)
        Me.world = world
    End Sub

    Public ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String)) Implements IAvatarModel.Description
        Get
            Return ChoiceModes.Descriptors(choiceMode).Description(world)
        End Get
    End Property

    Public ReadOnly Property CanEnterGameMenu As Boolean Implements IAvatarModel.CanEnterGameMenu
        Get
            Return ChoiceModes.Descriptors(choiceMode).CanEnterGameMenu
        End Get
    End Property

    Public ReadOnly Property AvailableChoices As (Text As String, Choice As String)() Implements IAvatarModel.AvailableChoices
        Get
            Return ChoiceModes.
                Descriptors(choiceMode).
                AvailableChoices(world).
                Select(Function(x) (Choices.Descriptors(x).Text, x)).ToArray
        End Get
    End Property

    Public Sub MakeChoice(choice As String) Implements IAvatarModel.MakeChoice
        choiceMode = ChoiceModes.
            Descriptors(choiceMode).
            MakeChoice(world, choice)
    End Sub
End Class
