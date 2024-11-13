Friend Class TurnRightChoiceDescriptor
    Inherits BaseTurnChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.TurnRight, "Turn Right", "You turn right.")
    End Sub

    Protected Overrides Function NextFacing(facing As String) As String
        Return Directions.Descriptors(facing).RightDirection
    End Function
End Class
