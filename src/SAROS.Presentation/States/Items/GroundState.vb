Friend Class GroundState
    Inherits BasePickerState(Of IWorldModel, String)
    Const GoBackItem = "GoBack"
    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context, "On the Ground", context.ControlsText("Pick Up", "Cancel"), GameState.Navigation)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (String, String))
        Select Case value.Item2
            Case GoBackItem
                SetState(GameState.Navigation)
            Case Else
                SetState(GameState.Navigation)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (String, String))
        Dim result As New List(Of (String, String)) From
            {
                ("Go Back", GoBackItem)
            }
        result.AddRange(Context.Model.GroundItems.Select(Function(x) ($"{Context.Model.GetItemTypeName(x.Key)} x{x.Value}", x.Key)))
        Return result
    End Function
End Class
