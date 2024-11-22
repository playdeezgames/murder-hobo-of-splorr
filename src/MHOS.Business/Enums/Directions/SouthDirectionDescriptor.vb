Friend Class SouthDirectionDescriptor
    Inherits DirectionDescriptor

    Public Sub New()
        MyBase.New(
            Directions.South,
            "south",
            True,
            Directions.West,
            Directions.North,
            Directions.East,
            0,
            1)
    End Sub
End Class
