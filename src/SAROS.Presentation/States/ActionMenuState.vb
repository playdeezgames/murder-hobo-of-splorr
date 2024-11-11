Friend Class ActionMenuState
    Inherits BasePickerState(Of IWorldModel, String)

    Private Const GoBackItem As String = "GoBack"
    Private Const FaceMemoryItem As String = "FaceMemory"
    Private Const GroundItem As String = "Ground"

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context, "Action Menu", context.ControlsText("Select", "Cancel"), GameState.Navigation)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (String, String))
        Select Case value.Item2
            Case GoBackItem
                SetState(GameState.Navigation)
            Case FaceMemoryItem
                SetState(BoilerplateState.Neutral)
            Case GroundItem
                SetState(GameState.Ground)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (String, String))
        Dim result As New List(Of (String, String)) From {
            ("Go Back", GoBackItem)
        }
        If Context.Model.HasGroundItems Then
            result.Add(("Ground...", GroundItem))
        End If
        Return result
    End Function
End Class
