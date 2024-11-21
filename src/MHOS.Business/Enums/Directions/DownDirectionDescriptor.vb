Friend Class DownDirectionDescriptor
    Inherits DirectionDescriptor

    Public Sub New()
        MyBase.New(
            Directions.Down,
            "down",
            False,
            Choices.MoveDown,
            Nothing,
            Directions.Up,
            Nothing,
            0,
            0)
    End Sub
End Class
