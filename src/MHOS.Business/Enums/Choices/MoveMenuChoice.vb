Friend Class MoveMenuChoice
    Inherits Choice

    Public Sub New(dialog As String, world As IWorld)
        MyBase.New(String.Empty, dialog, world)
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Move..."
        End Get
    End Property

    Public Overrides Function Choose() As String
        Return Dialogs.MoveMenu
    End Function
End Class
