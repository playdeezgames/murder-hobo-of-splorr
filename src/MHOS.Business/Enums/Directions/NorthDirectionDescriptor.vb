Friend Class NorthDirectionDescriptor
    Inherits DirectionDescriptor

    Public Sub New()
        MyBase.New(
            Directions.North,
            "north",
            True,
            Choices.MoveNorth,
            Directions.East,
            Directions.South,
            Directions.West,
            0,
            -1)
    End Sub
End Class
