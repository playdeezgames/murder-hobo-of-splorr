Friend MustInherit Class ChoiceDescriptor
    ReadOnly Property Choice As String
    ReadOnly Property Text As String
    Sub New(choice As String, text As String)
        Me.Choice = choice
        Me.Text = text
    End Sub

    Friend MustOverride Function Choose(world As IWorld, choiceMode As String) As String
End Class
