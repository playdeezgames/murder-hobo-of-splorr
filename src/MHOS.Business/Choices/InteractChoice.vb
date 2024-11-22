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

    Public Overrides Function LegacyChoose() As String
        Select Case world.Avatar.Location.Features.Count
            Case 0
                Return Dialogs.Neutral
            Case 1
                'TODO: if there is only one feature, interact with the one feature
                Return Dialogs.InteractFeature
            Case Else
                Return Dialogs.InteractMenu
        End Select
    End Function

    Public Overrides Function Choose() As IDialog
        Return New Dialog(LegacyChoose())
    End Function
End Class
