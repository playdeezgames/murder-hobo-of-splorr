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
    End Sub
End Class
