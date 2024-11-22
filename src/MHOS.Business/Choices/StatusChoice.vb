Friend Class StatusChoice
    Inherits Choice

    Public Sub New(dialog As String, world As IWorld)
        MyBase.New(dialog, world)
    End Sub

    Public Overrides ReadOnly Property Text As String
        Get
            Return "Status"
        End Get
    End Property

    Public Overrides Function LegacyChoose() As String
        Return Dialogs.Status
    End Function
End Class
