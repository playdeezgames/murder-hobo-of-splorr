Public Class WorldModel
    Implements IWorldModel

    Private _world As IWorld
    Private dialog As String = Dialogs.Neutral
    Sub New()
    End Sub

    Private Sub LegacyMakeChoice(choice As String)
        dialog = Dialogs.
            Descriptors(dialog).
            MakeChoice(World, choice)
    End Sub

    Public Sub GoBack() Implements IWorldModel.GoBack
        dialog = Dialogs.Descriptors(dialog).GoBackDialog(World)
    End Sub

    Public Sub MakeChoice(choice As IChoice) Implements IWorldModel.MakeChoice
        LegacyMakeChoice(choice.Choice)
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
            Return Dialogs.Descriptors(dialog).Description(World)
        End Get
    End Property

    Public ReadOnly Property CanEnterGameMenu As Boolean Implements IWorldModel.CanEnterGameMenu
        Get
            Return String.IsNullOrEmpty(Dialogs.Descriptors(dialog).GoBackDialog(World))
        End Get
    End Property

    Public ReadOnly Property AvailableChoices As IChoice() Implements IWorldModel.AvailableChoices
        Get
            Return Dialogs.
                Descriptors(dialog).
                AvailableChoices(World).
                ToArray
        End Get
    End Property
End Class
