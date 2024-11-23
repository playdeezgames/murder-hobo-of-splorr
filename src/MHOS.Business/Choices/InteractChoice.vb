Friend Class InteractChoice
    Inherits Choice

    Public Sub New(dialog As String, world As IWorld)
        MyBase.New(dialog, world)
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Interact..."
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        Select Case world.Avatar.Location.Features.Count
            Case 0
                Return New Dialog(Dialogs.Neutral, world)
            Case 1
                'TODO: if there is only one feature, interact with the one feature
                Return New Dialog(Dialogs.InteractFeature, world)
            Case Else
                Return New Dialog(Dialogs.InteractMenu, world)
        End Select
    End Function
End Class
