Public Class WorldModel
    Implements IWorldModel

    Private dialog As IDialog = New NeutralDialog(Nothing)
    Sub New()
    End Sub

    Public Sub GoBack() Implements IWorldModel.GoBack
        dialog = dialog.LegacyGoBack()
    End Sub

    Public Sub MakeChoice(choice As IChoice) Implements IWorldModel.MakeChoice
        dialog = choice.Choose()
    End Sub

    Private Property World As IWorld
        Get
            Return dialog.World
        End Get
        Set(value As IWorld)
            dialog.World = value
        End Set
    End Property
    Public ReadOnly Property Session As IWorldSessionModel Implements IWorldModel.Session
        Get
            Return New WorldSessionModel(Sub(w) World = w, Function() World)
        End Get
    End Property

    Public ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String)) Implements IWorldModel.Description
        Get
            Return dialog.Description
        End Get
    End Property

    Public ReadOnly Property CanEnterGameMenu As Boolean Implements IWorldModel.CanEnterGameMenu
        Get
            Return dialog.LegacyCanEnterGameMenu
        End Get
    End Property

    Public ReadOnly Property AvailableChoices As IChoice() Implements IWorldModel.AvailableChoices
        Get
            Return dialog.LegacyAvailableChoices
        End Get
    End Property
End Class
