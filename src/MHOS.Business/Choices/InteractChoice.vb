Friend Class InteractChoice
    Inherits Choice

    Public Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Interact..."
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        Select Case world.Avatar.Location.Features.Count
            Case 0
                Return New NeutralDialog(world)
            Case 1
                Return New InteractFeatureDialog(world, world.Avatar.Location.Features.Single)
            Case Else
                Return New InteractMenuDialog(world)
        End Select
    End Function
End Class
