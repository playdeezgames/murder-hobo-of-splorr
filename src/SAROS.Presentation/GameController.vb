Public Class GameController
    Inherits BaseGameController(Of IWorldModel)

    Public Sub New(settings As ISettings, context As IUIContext(Of IWorldModel))
        MyBase.New(settings, context)
        SetBoilerplateStates(context)
        SetCurrentState(BoilerplateState.Splash, True)
    End Sub

    Private Sub SetBoilerplateStates(context As IUIContext(Of IWorldModel))
        SetState(BoilerplateState.Embark, New EmbarkState(Me, AddressOf SetCurrentState, context))
        SetState(BoilerplateState.Neutral, New NeutralState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Navigation, New NavigationState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Lose, New LoseState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Combat, New CombatState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.CombatResult, New CombatResultState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.ActionMenu, New ActionMenuState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Ground, New GroundState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Win, New WinState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Status, New StatusState(Me, AddressOf SetCurrentState, context))
    End Sub
End Class
