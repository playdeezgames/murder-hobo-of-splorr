Friend Class ActionMenuState
    Inherits BasePickerState(Of IWorldModel, String)

    Private ReadOnly GoBackItem As String = NameOf(GoBackItem)
    Private ReadOnly AttributesItem As String = NameOf(AttributesItem)

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of IWorldModel))
        MyBase.New(parent, setState, context, Grimoire.ActionMenu, context.ControlsText("Sel", "Cancel"), GameState.Navigation)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (String, String))
        Select Case value.Item2
            Case GoBackItem
                SetState(GameState.Navigation)
            Case AttributesItem
                SetState(GameState.Attributes)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (String, String))
        Dim result As New List(Of (String, String)) From {
            ("Go Back", GoBackItem),
            ("Attributes...", AttributesItem)
        }
        Return result
    End Function
End Class
