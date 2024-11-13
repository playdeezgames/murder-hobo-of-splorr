Public Class WorldModel
    Implements IWorldModel

    Private _world As IWorld
    Private choiceMode As String = Dialogs.Neutral
    Sub New()
    End Sub

    Public Sub MakeChoice(choice As String) Implements IWorldModel.MakeChoice
        choiceMode = Dialogs.
            Descriptors(choiceMode).
            MakeChoice(World, choice)
    End Sub

    Public Sub GoBack() Implements IWorldModel.GoBack
        choiceMode = Dialogs.Descriptors(choiceMode).GoBackDialog(World)
    End Sub

    Private Property World As IWorld
        Get
            Return _world
        End Get
        Set(value As IWorld)
            _world = value
        End Set
    End Property
    Public ReadOnly Property Session As IWorldSessionModel Implements IWorldModel.Session
        Get
            Return New WorldSessionModel(Sub(w) World = w, Function() World)
        End Get
    End Property

    Public ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String)) Implements IWorldModel.Description
        Get
            Return Dialogs.Descriptors(choiceMode).Description(World)
        End Get
    End Property

    Public ReadOnly Property CanEnterGameMenu As Boolean Implements IWorldModel.CanEnterGameMenu
        Get
            Return String.IsNullOrEmpty(Dialogs.Descriptors(choiceMode).GoBackDialog(World))
        End Get
    End Property

    Public ReadOnly Property AvailableChoices As (Text As String, Choice As String)() Implements IWorldModel.AvailableChoices
        Get
            Return Dialogs.
                Descriptors(choiceMode).
                AvailableChoices(World).
                Select(Function(x) (Choices.Descriptors(x).Text, x)).ToArray
        End Get
    End Property
End Class
