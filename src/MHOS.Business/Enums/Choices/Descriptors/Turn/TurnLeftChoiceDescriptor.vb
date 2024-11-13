Friend Class TurnLeftChoiceDescriptor
    Inherits BaseTurnChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.TurnLeft, "Turn Left", "You turn left.")
    End Sub

    Protected Overrides Function NextFacing(facing As String) As String
        Return Directions.Descriptors(facing).LeftDirection
    End Function
End Class
