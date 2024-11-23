Friend Class MoveMenuChoice
    Inherits Choice

    Public Sub New(dialog As String, world As IWorld)
        MyBase.New(dialog, world)
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Move..."
        End Get
    End Property

    Private Function LegacyChoose() As String
        Return Dialogs.MoveMenu
    End Function

    Public Overrides Function Choose() As IDialog
        Return New Dialog(LegacyChoose(), world)
    End Function
End Class
