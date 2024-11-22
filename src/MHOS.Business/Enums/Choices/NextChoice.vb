Friend Class NextChoice
    Inherits Choice
    Private ReadOnly nextDialog As String

    Public Sub New(nextDialog As String, dialog As String, world As IWorld)
        MyBase.New(String.Empty, dialog, world)
        Me.nextDialog = nextDialog
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Next"
        End Get
    End Property

    Public Overrides Function Choose() As String
        Return nextDialog
    End Function
End Class
