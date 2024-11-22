Friend Class NorthDirectionDescriptor
    Inherits DirectionDescriptor

    Public Sub New()
        MyBase.New(
            Directions.North,
            "north",
            True,
            Directions.East,
            Directions.South,
            Directions.West,
            0,
            -1)
    End Sub
End Class
