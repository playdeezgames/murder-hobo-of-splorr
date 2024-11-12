Friend Class ScumLoadState(Of TModel)
    Inherits BaseGameState(Of TModel)
    Private ReadOnly Property ReturnState As String
    Const ScumSlot = 0

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of TModel), returnState As String)
        MyBase.New(parent, setState, context)
        Me.ReturnState = returnState
    End Sub

    Public Overrides Sub HandleCommand(cmd As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub Render(displayBuffer As IPixelSink)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub OnStart()
        MyBase.OnStart()
        If Context.DoesSlotExist(ScumSlot) Then
            Context.LoadGame(ScumSlot)
            Context.AddMessage("Operation Complete!", "You have successfully loaded the scum slot, you dirty dog!")
            SetState(BoilerplateState.Message)
            Return
        End If
        SetState(ReturnState)
    End Sub

End Class
