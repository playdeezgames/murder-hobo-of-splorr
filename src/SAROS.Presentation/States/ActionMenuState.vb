Friend Class ActionMenuState
    Inherits BasePickerState(Of IWorldModel, String)

    Private Const GoBackItem As String = "GoBack"
    Private Const FaceMemoryItem As String = "FaceMemory"
    Private Const GroundItem As String = "Ground"
    Private Const StatusItem As String = "Status"

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
            Case StatusItem
                SetState(GameState.Status)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (String, String))
        Dim result As New List(Of (String, String)) From {
            ("Go Back", GoBackItem),
            ("Status...", StatusItem)
        }
        If Context.Model.TriggerLevel > 0 Then
            result.Add(("Face memory...", FaceMemoryItem))
        End If
        If Context.Model.HasGroundItems Then
            result.Add(("Ground...", GroundItem))
        End If
        Return result
    End Function
End Class
