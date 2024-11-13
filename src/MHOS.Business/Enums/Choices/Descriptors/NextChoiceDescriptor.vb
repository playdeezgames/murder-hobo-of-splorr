Friend Class NextChoiceDescriptor
    Inherits BaseChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Next, "Next")
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        Select Case dialog
            Case Dialogs.RollAttributes
                Return Dialogs.Neutral
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Class
