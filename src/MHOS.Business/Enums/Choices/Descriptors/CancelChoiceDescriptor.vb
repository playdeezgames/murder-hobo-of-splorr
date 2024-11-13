Friend Class CancelChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Cancel, "Cancel")
    End Sub

    Friend Overrides Function Choose(world As IWorld, choiceMode As String) As String
        Select Case choiceMode
            Case ChoiceModes.Status
                Return ChoiceModes.Neutral
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Class
