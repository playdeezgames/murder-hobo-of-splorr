Friend Class StatusChoice
    Inherits Choice

    Public Sub New(world As IWorld)
        MyBase.New(world)
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Status"
        End Get
    End Property

    Public Overrides Function Choose() As IDialog
        Return New StatusDialog(world)
    End Function
End Class
