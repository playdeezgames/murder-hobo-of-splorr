Friend Class NextChoiceDescriptor
    Inherits BaseChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.Next, "Next")
    End Sub

    Friend Overrides Function Choose(world As IWorld, dialog As String) As String
        Select Case dialog
            Case Dialogs.ChooseRace
                Return Dialogs.Neutral
            Case Dialogs.Initialize
                If world.InitializationStepCount = 0 Then
                    Return Dialogs.ChooseRace
                Else
                    Return Dialogs.Initialize
                End If
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Class
