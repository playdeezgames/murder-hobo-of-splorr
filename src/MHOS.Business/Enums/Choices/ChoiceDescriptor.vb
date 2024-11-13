Friend MustInherit Class ChoiceDescriptor
    ReadOnly Property Choice As String
    ReadOnly Property Text As String
    Sub New(choice As String, text As String)
        Me.Choice = choice
        Me.Text = text
    End Sub
End Class
