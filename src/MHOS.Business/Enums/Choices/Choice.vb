Friend Class Choice
    Implements IChoice
    Sub New(choice As String)
        Me.Choice = choice
    End Sub

    Public ReadOnly Property Text As String Implements IChoice.Text
        Get
            Return Choices.Descriptors(Choice).Text
        End Get
    End Property

    Private ReadOnly Property Choice As String Implements IChoice.Choice
End Class
