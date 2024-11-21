Friend Class MoveDownChoiceDescriptor
    Inherits BaseMoveChoiceDescriptor

    Public Sub New()
        MyBase.New(
            Choices.MoveDown,
            "Down",
            Directions.Down)
    End Sub
End Class
