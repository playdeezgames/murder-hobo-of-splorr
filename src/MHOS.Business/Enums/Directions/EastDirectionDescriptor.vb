Friend Class EastDirectionDescriptor
    Inherits DirectionDescriptor

    Public Sub New()
        MyBase.New(Directions.East, "east", Directions.South, Directions.West, Directions.North, 1, 0)
    End Sub
End Class
