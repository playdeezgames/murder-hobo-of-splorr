Friend Class NextChoiceDescriptor
    Inherits ChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Next, "Next")
    End Sub

    Friend Overrides Function Choose(world As IWorld, choiceMode As String) As String
        Select Case choiceMode
            Case Dialogs.RollAttributes
                Return Dialogs.Neutral
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Class
