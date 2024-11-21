Friend Class Choice
    Implements IChoice
    Sub New(choice As String)
        Me.Choice = choice
    End Sub

    Private ReadOnly Property Choice As String Implements IChoice.Choice
End Class
