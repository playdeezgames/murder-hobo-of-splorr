Friend Class TurnAroundChoiceDescriptor
    Inherits BaseTurnChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.TurnAround, "Turn Around", "You turn around.")
    End Sub

    Protected Overrides Function NextFacing(facing As String) As String
        Return Directions.Descriptors(facing).OppositeDirection
    End Function
End Class
